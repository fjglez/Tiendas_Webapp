import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IShop } from '../../shared/ishop';
import { ShopsService } from '../../shared/shops.service';
import { MessagesService } from '../../shared/messages.service';
import { INewProduct } from './INewProduct';

@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.css']
})
export class CreateProductComponent implements OnInit {


  templateNewProduct: INewProduct = {
    name: undefined,
    price: undefined,
    description: undefined
  }
  newProduct: INewProduct = { ...this.templateNewProduct }

  errorMessage?: string;

  shopId?: string | null;
  shop?: IShop;

  // Edición
  productToEditId?: string | null;

  constructor(private route: ActivatedRoute, private router: Router,
    private shopsService: ShopsService, private messages: MessagesService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => this.shopId = params.get('id'));
    if (this.shopId != null) {
      this.shopsService.getShop(this.shopId).subscribe({
        next: shop => {
          this.shop = shop;
        }
      });
    }

    // Edición
    this.route.paramMap.subscribe(params => this.productToEditId = params.get('idProduct'));
    if (this.shopId != null && this.productToEditId != null) {
      this.shopsService.getProduct(this.shopId, this.productToEditId).subscribe({
        next: product => {
        this.newProduct = product;
        } 
      });
    }
  }

  onSubmit(form: NgForm) {
    // Edita el producto
    if (form.valid && this.shopId != null && this.productToEditId != null) {
      this.shopsService.putProduct(this.newProduct, this.shopId, this.productToEditId).subscribe(
        result => this.onHttpSuccess("Producto editado con éxito"),
        error => this.onHttpError(error)
      )
    }
    // Crea un nuevo producto
    else if (form.valid && this.shopId != null) {
      this.shopsService.postProduct(this.newProduct, this.shopId).subscribe(
        result => this.onHttpSuccess("Producto creado con éxito"),
        error => this.onHttpError(error)
      )
    }
  }


  onHttpError(errorResponse: any) {
    console.error('error: ', errorResponse);
    this.errorMessage = "Ha ocurrido un error";
  }

  onHttpSuccess(message: string) {
    if (this.shopId != null) { 
      this.messages.successMessage = message;
      this.router.navigate(['/shops/' + this.shopId]);
    }

  }

  onCancel() {
    this.router.navigate(['/shops/' + this.shopId]);

  }
}
