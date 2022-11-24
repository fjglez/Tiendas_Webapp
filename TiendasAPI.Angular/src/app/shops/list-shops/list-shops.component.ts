import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { IShop } from '../../shared/ishop';
import { ShopsService } from '../../shared/shops.service';

@Component({
  selector: 'app-list-shops',
  templateUrl: './list-shops.component.html',
  styleUrls: ['./list-shops.component.css']
})
export class ListShopsComponent implements OnInit, OnDestroy {

  constructor(private shopsService: ShopsService) { }
  elementsPerRow = 4;

  sub!: Subscription;
  shops: IShop[] = [];
  rows: number[] = [0];

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  ngOnInit(): void {
    this.sub = this.shopsService.getShops().subscribe({
      next: shops => {
        this.shops = shops;
        let nRows = Math.ceil(shops.length / this.elementsPerRow);
        this.rows = Array(nRows).fill(0).map((_, i) => i);
      }
    })
  }

}
