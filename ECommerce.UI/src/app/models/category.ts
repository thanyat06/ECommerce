export interface Category {
  categoryId: number;
  categoryName: string;
  description?: string;
}

export interface CategoryRequest {
  categoryName: string;
  description?: string;
}