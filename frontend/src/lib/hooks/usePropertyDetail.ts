import useSWR from 'swr';
import { getPropertyById } from '../api/properties';
import type { PropertyDetail } from '../types/property.types';

function getPropertyDetailKey(id: string | null | undefined): string | null {
  if (!id) {
    return null;
  }
  return `property-${id}`;
}

export function usePropertyDetail(id: string | null | undefined) {
  const { data, error, isLoading, mutate } = useSWR<PropertyDetail>(
    getPropertyDetailKey(id),
    () => getPropertyById(id!),
    {
      dedupingInterval: 300000,
      revalidateOnFocus: false,
      revalidateOnReconnect: true,
      revalidateIfStale: true,
    }
  );

  return {
    data,
    property: data,
    error,
    isLoading,
    mutate,
  };
}
