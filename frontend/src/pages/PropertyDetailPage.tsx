import React, { useEffect } from 'react';
import { useParams, Navigate } from 'react-router-dom';
import { PropertyDetail } from '../components/properties/PropertyDetail';
import { usePropertyDetail } from '../lib/hooks/usePropertyDetail';

export const PropertyDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const { property, isLoading, error } = usePropertyDetail(id);

  useEffect(() => {
    if (property) {
      document.title = `${property.name} - Real Estate Management`;
    } else {
      document.title = 'Property Details - Real Estate Management';
    }
  }, [property]);

  if (!id) {
    return <Navigate to="/" replace />;
  }

  if (isLoading) {
    return (
      <div className="space-y-6">
        <div className="animate-pulse">
          <div className="h-9 w-40 bg-gray-200 rounded mb-6"></div>

          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            <div className="space-y-4">
              <div className="h-96 bg-gray-200 rounded-lg"></div>
              <div className="grid grid-cols-4 gap-2">
                <div className="h-20 bg-gray-200 rounded"></div>
                <div className="h-20 bg-gray-200 rounded"></div>
                <div className="h-20 bg-gray-200 rounded"></div>
                <div className="h-20 bg-gray-200 rounded"></div>
              </div>
            </div>

            <div className="space-y-6">
              <div>
                <div className="h-9 bg-gray-200 rounded w-3/4 mb-2"></div>
                <div className="h-9 bg-gray-200 rounded w-1/2"></div>
              </div>
              <div className="h-64 bg-gray-200 rounded"></div>
              <div className="h-48 bg-gray-200 rounded"></div>
            </div>
          </div>
        </div>
      </div>
    );
  }

  if (error) {
    const errorMessage =
      typeof error === 'string'
        ? error
        : (error && typeof error === 'object' && 'detail' in error && typeof error.detail === 'string')
          ? error.detail
          : (error && typeof error === 'object' && 'message' in error && typeof error.message === 'string')
            ? error.message
            : 'Failed to load property';

    return (
      <div className="text-center py-12">
        <div className="inline-flex items-center justify-center w-16 h-16 rounded-full bg-red-100 mb-4">
          <svg
            className="w-8 h-8 text-red-600"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
            />
          </svg>
        </div>
        <h2 className="text-2xl font-bold text-gray-900 mb-2">Error Loading Property</h2>
        <p className="text-gray-600 mb-6">{errorMessage}</p>
        <a
          href="/"
          className="inline-flex items-center text-primary-600 hover:text-primary-700 font-medium"
        >
          <svg
            className="w-5 h-5 mr-2"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M10 19l-7-7m0 0l7-7m-7 7h18"
            />
          </svg>
          Back to Properties
        </a>
      </div>
    );
  }

  if (!property) {
    return (
      <div className="text-center py-12">
        <div className="inline-flex items-center justify-center w-16 h-16 rounded-full bg-gray-100 mb-4">
          <svg
            className="w-8 h-8 text-gray-400"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"
            />
          </svg>
        </div>
        <h2 className="text-2xl font-bold text-gray-900 mb-2">Property Not Found</h2>
        <p className="text-gray-600 mb-6">
          The property you're looking for doesn't exist or has been removed.
        </p>
        <a
          href="/"
          className="inline-flex items-center text-primary-600 hover:text-primary-700 font-medium"
        >
          <svg
            className="w-5 h-5 mr-2"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M10 19l-7-7m0 0l7-7m-7 7h18"
            />
          </svg>
          Back to Properties
        </a>
      </div>
    );
  }

  return <PropertyDetail property={property} />;
};
