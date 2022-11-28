import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';
import { AuthenticationService } from '../../shared/authentication/authentication.service';
import { IProduct } from '../../shared/iproduct';
import { IShop } from '../../shared/ishop';
import { MessagesService } from '../../shared/messages.service';
import { ShopsService } from '../../shared/shops.service';

@Component({
  selector: 'app-list-products',
  templateUrl: './list-products.component.html',
  styleUrls: ['./list-products.component.css']
})
export class ListProductsComponent implements OnInit {

  constructor(private route: ActivatedRoute, private router: Router, private authenticationService: AuthenticationService,
    private shopsService: ShopsService, private messages: MessagesService) { }

  shopId?: string | null;
  allProducts: IProduct[] = [];
  products: IProduct[] = [];
  shop?: IShop;
  successMessage?: string;
  errorMessage?: string;

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => this.shopId = params.get('id'));
    this.loadProducts();
    this.loadShopDetails();

    if (this.messages.successMessage != undefined) {
      this.successMessage = this.messages.successMessage;
      this.messages.successMessage = undefined;
    }
  }

  loadProducts(): void {
    if (this.shopId != null) {
      this.shopsService.getProducts(this.shopId).subscribe({
        next: products => {
          this.allProducts = products;
          this.products = this.allProducts;
        }
      });
    }
  }

  loadShopDetails(): void {
    if (this.shopId != null) {
      this.shopsService.getShop(this.shopId).subscribe({
        next: shop => {
          this.shop = shop;
        }
      });
    }
  }

  
  // Botón para volver a la lista de tiendas
  returnToShopList(): void {
    this.router.navigate(['/shops']);
  }

  // Boton para añadir producto
  newProduct(): void {
    if (this.shop != null) {
      this.router.navigate(['/shops/'+this.shop.id+"/new"])
    }
  }

  // Botón para editar producto
  editProduct(product: IProduct) {
    if (this.shopId != null) {
      this.router.navigate(['/shops/' + this.shopId + '/edit/' + product.id]);
    }
  }

  // Botón para eliminar producto
  confirmDeletion(product: IProduct) {
    if (!this.authenticationService.isLogged()) {
      this.router.navigate(['login']);
      return;
    }
    Swal.fire({
      title: 'Confirmar eliminación',
      text: "El producto '" + product.name + "' será eliminado para siempre.",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Confirmar',
      cancelButtonText: 'Cancelar'
    }).then((response: any) => {
      if (response.value && this.shopId != null) {
        this.shopsService.deleteProduct(this.shopId, product.id).subscribe(
          result => this.onHttpSuccess(),
          error => this.onHttpError(error)
        );
      } else if (response.dismiss === Swal.DismissReason.cancel) {
      }
    })
  }

  onHttpError(errorResponse: any) {
    console.error('error: ', errorResponse);
    this.errorMessage = "Ha ocurrido un error";
    this.successMessage = undefined;
  }

  onHttpSuccess() {
    if (this.shopId != null) {
      this.successMessage = "Producto eliminado con éxito";
      this.errorMessage = undefined;
      this.loadProducts();
    }
  }

  // Búsqueda por filtrado
  private _listFilter: string = "";

  get listFilter(): string {
    return this._listFilter;
  }

  set listFilter(value: string) {
    this._listFilter = value;
    this.products = this.performFilter(value);
  }

  performFilter(query: string): IProduct[] {
    query = query.toLowerCase();
    return this.allProducts.filter((product: IProduct) =>
      product.name.toLowerCase().includes(query));
  }

}
