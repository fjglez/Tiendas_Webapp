import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap, catchError, throwError} from 'rxjs';
import { INewProduct } from '../shops/create-product/INewProduct';
import { IProduct } from './iproduct';
import { IShop } from './ishop';

@Injectable({
  providedIn: 'root'
})
export class ShopsService {
  baseUrl = "https://localhost:7202/api"

  constructor(private http: HttpClient) {
  }

  getShop(shopId: string): Observable<IShop> {
    return this.http.get<IShop>(this.baseUrl + "/shops/" + shopId).pipe(
      tap(data => console.log('Shop:', JSON.stringify(data))),
      catchError(this.handleError));
  }

  getShops(): Observable<IShop[]> {
    return this.http.get<IShop[]>(this.baseUrl+"/shops?pagination=false").pipe(
      tap(data => console.log('Shops:', JSON.stringify(data))),
      catchError(this.handleError));
  }
  getProducts(shopId: string): Observable<IProduct[]>  {
    return this.http.get<IProduct[]>(this.baseUrl + "/shops/"+shopId+"/products?pagination=false").pipe(
      tap(data => console.log('Products:', JSON.stringify(data))),
      catchError(this.handleError));
  }
  getProduct(shopId: string, productId: string): Observable<IProduct> {
    return this.http.get<IProduct>(this.baseUrl + "/shops/" + shopId + "/products/" + productId).pipe(
      tap(data => console.log('Product:', JSON.stringify(data))),
      catchError(this.handleError));
  }

  postProduct(newProduct: INewProduct, shopId: string): Observable<any> {
    console.log("Creando nuevo producto");
    return this.http.post(this.baseUrl + "/shops/" + shopId + "/products", newProduct).pipe(
      tap(data => console.log('Producto creado:', JSON.stringify(data))),
      catchError(this.handleError));
  }

  putProduct(newProduct: INewProduct, shopId: string, productId: string): Observable<any> {
    console.log("Editando producto");
    return this.http.put(this.baseUrl + "/shops/" + shopId + "/products/" + productId, newProduct).pipe(
      tap(_ => console.log('Producto modificado con éxito.')),
      catchError(this.handleError));
  }

  deleteProduct(shopId: string, productId: number): Observable<any> {
    console.log("Eliminando un producto");
    return this.http.delete(this.baseUrl + "/shops/" + shopId + "/products/" + productId).pipe(
      tap(_ => console.log('Producto eliminado con éxito.')),
      catchError(this.handleError));
  }
  private handleError(err: HttpErrorResponse) {
    let errorMessage = err.error.message;
    console.error(errorMessage);
    return throwError(() => errorMessage)
  }

}
