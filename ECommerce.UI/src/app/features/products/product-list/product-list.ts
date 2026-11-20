import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductCard } from '../../../shared/components/product-card/product-card';
import { ProductService } from '../../../core/services/product.service';
import { CategoryService } from '../../../core/services/category.service';
import { Product } from '../../../models/product';
import { Category } from '../../../models/category';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, ProductCard],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css'
})
export class ProductList implements OnInit {

  products = signal<Product[]>([]);
  categories = signal<Category[]>([]);

  searchTerm = signal('');
  selectedCategoryId = signal<number | null>(null);

  loading = signal(false);
  error = signal('');

  currentPage = signal(1);
  pageSize = 12;
  totalPages = signal(0);
  totalRecords = signal(0);

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService
  ) {}

  ngOnInit(): void {
    this.loadCategories();
    this.loadProducts();
  }

  loadCategories(): void {
    this.categoryService.getAllCategories().subscribe({
      next: (data) => {
        this.categories.set(data);
      },
      error: () => {
        console.error('Unable to load categories.');
      }
    });
  }

  loadProducts(): void {

    this.loading.set(true);
    this.error.set('');

    this.productService.getAllProducts(
      this.searchTerm(),
      this.selectedCategoryId() ?? undefined,
      this.currentPage(),
      this.pageSize
    ).subscribe({

      next: (response) => {
        this.products.set(response.products);
        this.totalPages.set(response.totalPages);
        this.totalRecords.set(response.totalRecords);
        this.loading.set(false);
      },

      error: (err) => {
        this.products.set([]);

        this.error.set(
          typeof err.error === 'string'
            ? err.error
            : err.error?.message || 'Unable to load products.'
        );

        this.loading.set(false);
      }

    });
  }

  onSearchChange(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchTerm.set(value);
  }

  searchProducts(): void {
    this.currentPage.set(1);
    this.loadProducts();
  }

  onCategoryChange(event: Event): void {
    const value = (event.target as HTMLSelectElement).value;

    this.selectedCategoryId.set(
      value ? Number(value) : null
    );

    this.currentPage.set(1);
    this.loadProducts();
  }

  clearFilters(): void {
    this.searchTerm.set('');
    this.selectedCategoryId.set(null);
    this.currentPage.set(1);

    this.loadProducts();
  }

  changePage(page: number): void {
    if (page < 1 || page > this.totalPages()) {
      return;
    }

    this.currentPage.set(page);
    this.loadProducts();
  }
}