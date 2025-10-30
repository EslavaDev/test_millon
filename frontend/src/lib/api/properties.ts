import apiClient, { buildQueryString } from './client';
import type {
  PropertyListItem,
  PropertyDetail,
  PropertyFilter,
  PagedResult,
} from '../types/property.types';

export async function getProperties(
  filter: PropertyFilter = {}
): Promise<PagedResult<PropertyListItem>> {
  const queryString = buildQueryString(filter as Record<string, unknown>);
  const response = await apiClient.get<PagedResult<PropertyListItem>>(
    `/properties${queryString}`
  );
  return response.data;
}

export async function getPropertyById(id: string): Promise<PropertyDetail> {
  const response = await apiClient.get<PropertyDetail>(`/properties/${id}`);
  return response.data;
}

export const propertyApi = {
  getProperties,
  getPropertyById,
};
