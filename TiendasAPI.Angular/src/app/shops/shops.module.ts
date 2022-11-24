import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListShopsComponent } from './list-shops/list-shops.component';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ListProductsComponent } from './list-products/list-products.component';
import { CreateProductComponent } from './create-product/create-product.component';



@NgModule({
  declarations: [
    ListShopsComponent,
    ListProductsComponent,
    CreateProductComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild([
      { path: 'shops/:id/new', component: CreateProductComponent },
      { path: 'shops/:id/edit/:idProduct', component: CreateProductComponent },
      { path: 'shops/:id', component: ListProductsComponent },
      { path: 'shops', component: ListShopsComponent }
    ])
  ]
})
export class ShopsModule {
}
