import useSWR from 'swr';
import { getProperties } from '../api/properties';
import type { PropertyFilter, PropertyListItem, PagedResult } from '../types/property.types';

function getPropertiesKey(filters: PropertyFilter): string | null {
  if (filters.pageNumber && filters.pageNumber < 1) {
    return null;
  }

  return ['properties', JSON.stringify(filters)].join('-');
}

export function useProperties(filters: PropertyFilter = {}) {
  const { data, error, isLoading, mutate } = useSWR<PagedResult<PropertyListItem>>(
    getPropertiesKey(filters),
    () => getProperties(filters),
    {
      dedupingInterval: 2000,
      revalidateOnFocus: true,
      revalidateOnReconnect: true,
      keepPreviousData: true,
    }
  );

  return {
    data,
    error,
    isLoading,
    mutate,
    properties: data?.items || [],
    totalCount: data?.totalCount || 0,
    totalPages: data?.totalPages || 0,
    hasNextPage: data?.hasNextPage || false,
    hasPreviousPage: data?.hasPreviousPage || false,
  };
}
