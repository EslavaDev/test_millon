import { create } from 'zustand';
import { devtools, persist } from 'zustand/middleware';
import type { PropertyFilter } from '../types/property.types';

export interface PropertyStoreState {
  filters: PropertyFilter;
  setFilters: (filters: Partial<PropertyFilter>) => void;
  resetFilters: () => void;
  setCurrentPage: (page: number) => void;
  setSorting: (sortBy: PropertyFilter['sortBy'], sortDescending?: boolean) => void;
  setPageSize: (pageSize: number) => void;
}

const defaultFilters: PropertyFilter = {
  name: undefined,
  address: undefined,
  minPrice: undefined,
  maxPrice: undefined,
  year: undefined,
  sortBy: 'name',
  sortDescending: false,
  pageNumber: 1,
  pageSize: 10,
};

export const usePropertyStore = create<PropertyStoreState>()(
  devtools(
    persist(
      (set) => ({
        filters: { ...defaultFilters },

        setFilters: (newFilters) =>
          set(
            (state) => ({
              filters: {
                ...state.filters,
                ...newFilters,
                pageNumber: newFilters.pageNumber !== undefined
                  ? newFilters.pageNumber
                  : 1,
              },
            }),
            false,
            'setFilters'
          ),

        resetFilters: () =>
          set(
            {
              filters: { ...defaultFilters },
            },
            false,
            'resetFilters'
          ),

        setCurrentPage: (page) =>
          set(
            (state) => ({
              filters: {
                ...state.filters,
                pageNumber: page,
              },
            }),
            false,
            'setCurrentPage'
          ),

        setSorting: (sortBy, sortDescending = false) =>
          set(
            (state) => ({
              filters: {
                ...state.filters,
                sortBy,
                sortDescending,
                pageNumber: 1,
              },
            }),
            false,
            'setSorting'
          ),

        setPageSize: (pageSize) =>
          set(
            (state) => ({
              filters: {
                ...state.filters,
                pageSize,
                pageNumber: 1,
              },
            }),
            false,
            'setPageSize'
          ),
      }),
      {
        name: 'property-filters',
        partialize: (state) => ({
          filters: {
            sortBy: state.filters.sortBy,
            sortDescending: state.filters.sortDescending,
            pageSize: state.filters.pageSize,
          },
        }),
      }
    ),
    {
      name: 'PropertyStore',
      enabled: import.meta.env.DEV,
    }
  )
);
