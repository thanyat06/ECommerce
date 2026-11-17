import { Routes } from '@angular/router';
import { Home } from './features/home/home';
import { CategoryManagement } from './features/categories/category-management/category-management';

export const routes: Routes = [
  {
    path: '',
    component: Home
  },
  {
    path: 'categories',
    component: CategoryManagement
  },
  {
    path: '**',
    redirectTo: ''
  }
];