import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../shared/models/product';
import { MatCardModule } from '@angular/material/card';
import { MatButton } from '@angular/material/button';
import { ProductItemComponent } from "./product-item/product-item.component";
import { MatDialog } from '@angular/material/dialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { FormsModule } from '@angular/forms';
import { shopParams } from '../../shared/models/shopParams';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../shared/models/pagination';
@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    MatCardModule,
    MatButton,
    ProductItemComponent,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator,
    FormsModule
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit {
  private shopSerivce= inject(ShopService);
  private dialogService = inject(MatDialog);
  products?: Pagination<Product>;
  sortOptions=[
    {name:'Alphabetical',value:'name'},
    {name:'Price: Low-High',value:'priceAsc'},
    {name:'Price: High-Low',value:'priceDesc'},
   
  ]
  title = 'BuyBright';

  shopParam =new shopParams();
  pageSizeOptions = [5,10,15,20]
  ngOnInit(): void {
   this.initializeShop();
  }

  initializeShop(){
    this.shopSerivce.getBrands();
    this.shopSerivce.getTypes();
    this.getProducts();
  }

  openFiltersDialog(){
    const dialogRef = this.dialogService.open(FiltersDialogComponent,
      {
        minWidth:'500px',
        data:{
          selectedBrands: this.shopParam.brands,
          selectedTypes: this.shopParam.types
        }
      }
    )
    dialogRef.afterClosed().subscribe({
      next:result=>{
        if(result){
         // console.log(result);
          this.shopParam.brands =result.selectedBrands;
          this.shopParam.types  =result.selectedTypes;
          this.shopParam.pageNumber=1;
          //apply filters
          // this.shopSerivce.getProducts(this.selectedBrands,this.selectedTypes).subscribe({
          //   next:response=> this.products=response.data,
          //   error:error=>console.log(error)
          // })
          this.getProducts();
        }
      }
    })
  }

  getProducts(){
    this.shopSerivce.getProducts(this.shopParam).subscribe({
      next: response => this.products = response,
      error: error => console.log(error),
      complete: () => console.log('complete')
    })
  }
  onSortChange(event:  MatSelectionListChange){
    const selectedOption = event.options[0];
    if(selectedOption){
    
      this.shopParam.sort = selectedOption.value;
      this.shopParam.pageNumber=1;
      console.log(this.shopParam.sort);
      this.getProducts();
    }
  }
  
  handlePageEvent(event: PageEvent){
    this.shopParam.pageNumber = event.pageIndex + 1;
    this.shopParam.pageSize = event.pageSize;
    this.getProducts();
  }

  onSearchChange(){
    this.shopParam.pageNumber=1;
    this.getProducts();
  }
}
