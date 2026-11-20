import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Product, ProductListResponse } from '../../models/product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private http = inject(HttpClient);
  private apiUrl = `${environment.apiBaseUrl}/products`;

  getAllProducts(
    search?: string,
    categoryId?: number,
    pageNumber: number = 1,
    pageSize: number = 12
  ): Observable<ProductListResponse> {

    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (search?.trim()) {
      params = params.set('search', search.trim());
    }

    if (categoryId) {
      params = params.set('categoryId', categoryId);
    }

    return this.http.get<ProductListResponse>(
      this.apiUrl,
      { params }
    );
  }

  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`);
  }
}