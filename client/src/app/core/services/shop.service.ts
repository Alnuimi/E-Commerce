import { HttpClient, HttpParams } from '@angular/common/http';
import { inject,Injectable } from '@angular/core';
import { Pagination } from '../../shared/models/pagination';
import { Product } from '../../shared/models/product';
import { shopParams } from '../../shared/models/shopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/api/'
  private http = inject(HttpClient);
  types: string[]  = [];
  brands: string[] = [];


  getProducts(shopParams:shopParams){
    let param = new HttpParams();

    if(shopParams.brands && shopParams.brands.length>0){
   
     param= param.append('brands', shopParams.brands.join(','));
    }
    if(shopParams.types && shopParams.types.length > 0){
     param= param.append('types', shopParams.types.join(','));
    }
    if(shopParams.sort){
      param = param.append("sort" , shopParams.sort);
    }

    if(shopParams.search){
      param = param.append("search" , shopParams.search);
    }
    param = param.append('pageSize' , shopParams.pageSize);
    param = param.append('pageIndex' , shopParams.pageNumber);
   
   return  this.http.get<Pagination<Product>>(this.baseUrl + 'product' ,{params:param})
  }
  getProductByID(id:number){
   return this.http.get<Product>(this.baseUrl + 'product/' + id);
  }
  getBrands(){
    if(this.brands.length>0) return;
    return this.http.get<string[]>(this.baseUrl + 'product/brands').subscribe({
      next: response => this.brands = response
    })
  }

  getTypes(){
    if(this.types.length>0) return;
    return this.http.get<string[]>(this.baseUrl + 'product/types').subscribe({
      next: response => this.types = response
    })
  }
}
