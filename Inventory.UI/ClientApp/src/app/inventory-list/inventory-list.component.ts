import { Component, OnInit } from '@angular/core';
import { InventoryApiClient, Stock } from '../../../../Connected Services/InventoryApi/InventoryApi';

@Component({
  selector: 'app-inventory-list',
  templateUrl: './inventory-list.component.html',
  styleUrls: ['./inventory-list.component.css']
})
export class InventoryListComponent implements OnInit {

  stock[]: Stock;

  constructor(private inventoryApiClient: InventoryApiClient) { }

  ngOnInit(): void {
  }

  getStock() {
    this.stock = this.inventoryApiClient.stocksAll();
  }
}
