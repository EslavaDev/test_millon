import React from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Input } from '../ui/Input';
import { Button } from '../ui/Button';
import type { PropertyFilter } from '../../lib/types/property.types';

interface PropertyFilterFormData {
  name?: string;
  address?: string;
  minPrice?: number;
  maxPrice?: number;
}

const propertyFilterSchema = z.object({
  name: z.string().optional(),
  address: z.string().optional(),
  minPrice: z.union([z.number(), z.nan()]).optional().transform(val =>
    val === undefined || (typeof val === 'number' && isNaN(val)) ? undefined : val
  ),
  maxPrice: z.union([z.number(), z.nan()]).optional().transform(val =>
    val === undefined || (typeof val === 'number' && isNaN(val)) ? undefined : val
  ),
}).refine(
  (data) => {
    if (data.minPrice !== undefined && data.minPrice < 0) {
      return false;
    }
    return true;
  },
  {
    message: 'Minimum price must be 0 or greater',
    path: ['minPrice'],
  }
).refine(
  (data) => {
    if (data.maxPrice !== undefined && data.maxPrice < 0) {
      return false;
    }
    return true;
  },
  {
    message: 'Maximum price must be 0 or greater',
    path: ['maxPrice'],
  }
).refine(
  (data) => {
    if (
      data.minPrice !== undefined &&
      data.maxPrice !== undefined &&
      data.minPrice > data.maxPrice
    ) {
      return false;
    }
    return true;
  },
  {
    message: 'Minimum price must be less than or equal to maximum price',
    path: ['minPrice'],
  }
);

export interface PropertyFiltersProps {
  filters: PropertyFilter;
  onFilterChange: (filters: PropertyFilter) => void;
  onReset: () => void;
}

export const PropertyFilters: React.FC<PropertyFiltersProps> = ({
  filters,
  onFilterChange,
  onReset,
}) => {
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<PropertyFilterFormData>({
    resolver: zodResolver(propertyFilterSchema),
    defaultValues: {
      name: filters.name || '',
      address: filters.address || '',
      minPrice: filters.minPrice,
      maxPrice: filters.maxPrice,
    },
  });

  const onSubmit = (data: PropertyFilterFormData) => {
    const filterData: PropertyFilter = {
      name: data.name || undefined,
      address: data.address || undefined,
      minPrice: data.minPrice !== undefined && !isNaN(data.minPrice) ? data.minPrice : undefined,
      maxPrice: data.maxPrice !== undefined && !isNaN(data.maxPrice) ? data.maxPrice : undefined,
    };

    onFilterChange(filterData);
  };

  const handleReset = () => {
    reset({
      name: '',
      address: '',
      minPrice: undefined,
      maxPrice: undefined,
    });
    onReset();
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="bg-gradient-to-br from-white to-gray-50 p-4 sm:p-6 lg:p-8 rounded-2xl shadow-xl border border-gray-200">
      <div className="flex items-center gap-2 sm:gap-3 mb-4 sm:mb-6">
        <div className="p-2 bg-primary-100 rounded-lg">
          <svg
            className="w-5 h-5 sm:w-6 sm:h-6 text-primary-600"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M3 4a1 1 0 011-1h16a1 1 0 011 1v2.586a1 1 0 01-.293.707l-6.414 6.414a1 1 0 00-.293.707V17l-4 4v-6.586a1 1 0 00-.293-.707L3.293 7.293A1 1 0 013 6.586V4z"
            />
          </svg>
        </div>
        <div>
          <h2 className="text-lg sm:text-xl lg:text-2xl font-bold text-gray-900">Filter Properties</h2>
          <p className="text-xs sm:text-sm text-gray-500 hidden sm:block">Refine your search to find the perfect property</p>
        </div>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-3 sm:gap-4 lg:gap-5">
        <Input
          label="Property Name"
          placeholder="e.g., Beach House..."
          {...register('name')}
          error={errors.name?.message}
        />

        <Input
          label="Address"
          placeholder="e.g., Miami Beach..."
          {...register('address')}
          error={errors.address?.message}
        />

        <Input
          label="Minimum Price"
          type="number"
          placeholder="$ 0"
          {...register('minPrice', { valueAsNumber: true })}
          error={errors.minPrice?.message}
        />

        <Input
          label="Maximum Price"
          type="number"
          placeholder="$ 0"
          {...register('maxPrice', { valueAsNumber: true })}
          error={errors.maxPrice?.message}
        />
      </div>

      <div className="flex flex-col sm:flex-row gap-3 sm:gap-4 mt-6 sm:mt-8">
        <Button
          type="submit"
          variant="primary"
          className="flex-1 shadow-md hover:shadow-xl transition-all duration-300 transform hover:-translate-y-0.5 py-3 text-sm sm:text-base font-semibold"
        >
          <svg
            className="w-4 h-4 sm:w-5 sm:h-5 mr-2"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
            />
          </svg>
          <span className="hidden xs:inline">Apply Filters</span>
          <span className="xs:hidden">Apply</span>
        </Button>
        <Button
          type="button"
          variant="outline"
          onClick={handleReset}
          className="flex-1 py-3 text-sm sm:text-base font-semibold hover:bg-gray-50 transition-all duration-300"
        >
          <svg
            className="w-4 h-4 sm:w-5 sm:h-5 mr-2"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15"
            />
          </svg>
          <span className="hidden xs:inline">Reset Filters</span>
          <span className="xs:hidden">Reset</span>
        </Button>
      </div>
    </form>
  );
};
