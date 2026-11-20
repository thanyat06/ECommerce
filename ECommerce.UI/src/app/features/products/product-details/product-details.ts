import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../models/product';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './product-details.html',
  styleUrl: './product-details.css'
})
export class ProductDetails implements OnInit {

  product = signal<Product | null>(null);
  loading = signal(false);
  error = signal('');

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!id || id < 1) {
      this.error.set('Invalid product ID.');
      return;
    }

    this.loadProduct(id);
  }

  loadProduct(id: number): void {
    this.loading.set(true);
    this.error.set('');

    this.productService.getProductById(id).subscribe({

      next: (data) => {
        this.product.set(data);
        this.loading.set(false);
      },

      error: () => {
        this.error.set('Product not found or unable to load product.');
        this.loading.set(false);
      }

    });
  }
}