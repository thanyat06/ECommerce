
import { Routes } from '@angular/router';

import { Home } from './features/home/home';
import { CategoryManagement } from './features/categories/category-management/category-management';

export const routes: Routes = [

  // Home
  {
    path: '',
    component: Home
  },

  // Category Management
  {
    path: 'categories',
    component: CategoryManagement
  },

  // Product Listing
  {
    path: 'products',
    loadComponent: () =>
      import('./features/products/product-list/product-list')
        .then(m => m.ProductList)
  },

  // Product Details
  {
    path: 'products/:id',
    loadComponent: () =>
      import('./features/products/product-details/product-details')
        .then(m => m.ProductDetails)
  },

  // Invalid Routes
  {
    path: '**',
    redirectTo: ''
  }

];
