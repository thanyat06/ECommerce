
import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { CategoryService } from '../../../core/services/category.service';
import { Category, CategoryRequest } from '../../../models/category';

@Component({
  selector: 'app-category-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './category-management.html',
  styleUrl: './category-management.css'
})
export class CategoryManagement implements OnInit {

  private categoryService = inject(CategoryService);

  // Signals
  categories = signal<Category[]>([]);
  successMessage = signal('');
  errorMessage = signal('');
  loading = signal(false);

  // Computed Signal
  categoryCount = computed(() => this.categories().length);

  categoryName = '';
  description = '';

  editingCategoryId: number | null = null;

  ngOnInit(): void {
    this.loadCategories();
  }

  // Load categories automatically
  loadCategories(): void {

    this.loading.set(true);
    this.errorMessage.set('');

    this.categoryService.getAllCategories().subscribe({

      next: (data) => {
        this.categories.set(data);
        this.loading.set(false);
      },

      error: (error) => {
        this.errorMessage.set(
          this.getErrorMessage(error, 'Unable to load categories.')
        );
        this.loading.set(false);
      }

    });
  }

  // Add or Update category
  saveCategory(): void {

    this.clearMessages();

    if (!this.categoryName.trim()) {
      this.errorMessage.set('Category name is required.');
      return;
    }

    const category: CategoryRequest = {
      categoryName: this.categoryName.trim(),
      description: this.description.trim()
    };

    // Update category
    if (this.editingCategoryId !== null) {

      const id = this.editingCategoryId;

      this.categoryService.updateCategory(id, category).subscribe({

        next: () => {

          this.resetForm();
          this.successMessage.set('Category updated successfully.');

          // Fetch updated categories
          this.loadCategories();

        },

        error: (error) => {
          this.errorMessage.set(
            this.getErrorMessage(error, 'Unable to update category.')
          );
        }

      });

    } else {

      // Add category
      this.categoryService.addCategory(category).subscribe({

        next: () => {

          this.resetForm();
          this.successMessage.set('Category added successfully.');

          // Fetch updated categories
          this.loadCategories();

        },

        error: (error) => {
          this.errorMessage.set(
            this.getErrorMessage(error, 'Unable to add category.')
          );
        }

      });

    }
  }

  // Edit category
  editCategory(category: Category): void {

    this.categoryName = category.categoryName;
    this.description = category.description || '';

    this.editingCategoryId = category.categoryId;

    this.clearMessages();
  }

  // Delete category
  deleteCategory(id: number): void {

    const confirmed = confirm(
      'Are you sure you want to delete this category?'
    );

    if (!confirmed) {
      return;
    }

    this.clearMessages();

    this.categoryService.deleteCategory(id).subscribe({

      next: () => {

        // Update Signal immediately
        this.categories.update(categories =>
          categories.filter(category => category.categoryId !== id)
        );

        this.successMessage.set('Category deleted successfully.');

      },

      error: (error) => {
        this.errorMessage.set(
          this.getErrorMessage(
            error,
            'Unable to delete category. It may contain products.'
          )
        );
      }

    });
  }

  // Extract backend error message
  private getErrorMessage(error: any, fallback: string): string {

    if (error.status === 409) {
      return error.error?.message
        || error.error?.detail
        || error.error?.title
        || 'A category with this name already exists.';
    }

    return error.error?.message
      || error.error?.detail
      || error.error?.title
      || fallback;
  }

  // Reset form
  resetForm(): void {
    this.categoryName = '';
    this.description = '';
    this.editingCategoryId = null;
  }

  // Clear messages
  clearMessages(): void {
    this.successMessage.set('');
    this.errorMessage.set('');
  }
}
