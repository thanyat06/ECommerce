export interface Product {
  productId: number;
  productName: string;
  description: string;
  price: number;
  stock: number;
  imageUrl?: string;
  categoryId: number;
  categoryName: string;
  isActive: boolean;
}

export interface ProductListResponse {
  products: Product[];
  pageNumber: number;
  pageSize: number;
  totalRecords: number;
  totalPages: number;
}