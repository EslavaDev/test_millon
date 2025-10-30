import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { Button } from '../ui/Button';
import { formatCurrency, formatDate } from '../../lib/utils/format';
import type { PropertyDetail as PropertyDetailType } from '../../lib/types/property.types';

export interface PropertyDetailProps {
  property: PropertyDetailType;
}

export const PropertyDetail: React.FC<PropertyDetailProps> = ({ property }) => {
  const [selectedImageIndex, setSelectedImageIndex] = useState(0);

  const images = property.images?.filter((img) => img.enabled) || [];
  const currentImage = images[selectedImageIndex] || null;
  const imageUrl = currentImage?.file || '/placeholder-property.jpg';

  const owner = property.owner;
  const ownerName = owner?.name || 'Unknown Owner';

  const traces = property.traces || [];
  const hasSaleHistory = traces.length > 0;

  return (
    <div className="space-y-6">
      <div>
        <Link to="/">
          <Button variant="outline" size="sm">
            <svg
              className="w-4 h-4 mr-2"
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
          </Button>
        </Link>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="space-y-4">
          <div className="relative w-full h-96 bg-gray-200 rounded-lg overflow-hidden">
            <img
              src={imageUrl}
              alt={property.name}
              className="w-full h-full object-cover"
            />
          </div>

          {/* Image Thumbnails */}
          {images.length > 1 && (
            <div className="grid grid-cols-4 gap-2">
              {images.map((image, index) => (
                <button
                  key={image.idPropertyImage}
                  onClick={() => setSelectedImageIndex(index)}
                  className={`relative h-20 rounded-md overflow-hidden border-2 transition-all ${
                    index === selectedImageIndex
                      ? 'border-primary-600'
                      : 'border-gray-200 hover:border-gray-300'
                  }`}
                >
                  <img
                    src={image.file}
                    alt={`${property.name} - Image ${index + 1}`}
                    className="w-full h-full object-cover"
                  />
                </button>
              ))}
            </div>
          )}
        </div>

        {/* Right Column - Details */}
        <div className="space-y-6">
          {/* Property Name and Price */}
          <div>
            <h1 className="text-3xl font-bold text-gray-900 mb-2">
              {property.name}
            </h1>
            <p className="text-3xl font-bold text-primary-600">
              {formatCurrency(property.price)}
            </p>
          </div>

          {/* Property Information */}
          <div className="bg-gray-50 rounded-lg p-6 space-y-4">
            <h2 className="text-lg font-semibold text-gray-900">
              Property Information
            </h2>

            <div className="grid grid-cols-2 gap-4 text-sm">
              <div>
                <span className="text-gray-600">Year Built:</span>
                <p className="font-medium text-gray-900">{property.year}</p>
              </div>

              <div>
                <span className="text-gray-600">Code Internal:</span>
                <p className="font-medium text-gray-900">{property.codeInternal}</p>
              </div>

              <div className="col-span-2">
                <span className="text-gray-600">Address:</span>
                <p className="font-medium text-gray-900">{property.address}</p>
              </div>
            </div>
          </div>

          {/* Owner Information */}
          {owner && (
            <div className="bg-gray-50 rounded-lg p-6 space-y-4">
              <h2 className="text-lg font-semibold text-gray-900">
                Owner Information
              </h2>

              <div className="space-y-3 text-sm">
                <div>
                  <span className="text-gray-600">Name:</span>
                  <p className="font-medium text-gray-900">{ownerName}</p>
                </div>

                <div>
                  <span className="text-gray-600">Address:</span>
                  <p className="font-medium text-gray-900">{owner.address}</p>
                </div>

                {owner.photo && (
                  <div>
                    <span className="text-gray-600">Photo:</span>
                    <div className="mt-2">
                      <img
                        src={owner.photo}
                        alt={ownerName}
                        className="w-16 h-16 rounded-full object-cover"
                      />
                    </div>
                  </div>
                )}

                <div>
                  <span className="text-gray-600">Birthday:</span>
                  <p className="font-medium text-gray-900">
                    {formatDate(new Date(owner.birthday), 'long')}
                  </p>
                </div>
              </div>
            </div>
          )}
        </div>
      </div>

      {/* Sale History */}
      {hasSaleHistory && (
        <div className="bg-white rounded-lg shadow-md p-6">
          <h2 className="text-lg font-semibold text-gray-900 mb-4">
            Sale History
          </h2>

          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Date
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Sale Price
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Tax
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Name
                  </th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {traces.map((trace) => (
                  <tr key={trace.idPropertyTrace}>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                      {formatDate(new Date(trace.dateSale), 'short')}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                      {formatCurrency(trace.value)}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                      {formatCurrency(trace.tax)}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                      {trace.name}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
};
