export interface OwnerBasic {
  idOwner: string;
  name: string;
}

export interface Owner {
  idOwner: string;
  name: string;
  address: string;
  photo?: string | null;
  birthday: string;
}

export interface PropertyImage {
  idPropertyImage: string;
  idProperty: string;
  file: string;
  enabled: boolean;
}

export interface PropertyTrace {
  idPropertyTrace: string;
  idProperty: string;
  dateSale: string;
  name: string;
  value: number;
  tax: number;
}

export interface PropertyListItem {
  idProperty: string;
  name: string;
  address: string;
  price: number;
  codeInternal?: string | null;
  year: number;
  imageUrl?: string | null;
  owner?: OwnerBasic | null;
}

export interface PropertyDetail {
  idProperty: string;
  name: string;
  address: string;
  price: number;
  codeInternal?: string | null;
  year: number;
  idOwner: string;
  owner?: Owner | null;
  images: PropertyImage[];
  traces: PropertyTrace[];
  createdAt: string;
  updatedAt: string;
}

export interface PropertyFilter {
  name?: string;
  address?: string;
  minPrice?: number;
  maxPrice?: number;
  year?: number;
  sortBy?: 'name' | 'address' | 'price' | 'year';
  sortDescending?: boolean;
  pageNumber?: number;
  pageSize?: number;
}

export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface ApiError {
  title: string;
  status: number;
  detail: string;
  traceId?: string;
  errors?: Record<string, string[]>;
}

export interface FormError {
  field: string;
  message: string;
}
