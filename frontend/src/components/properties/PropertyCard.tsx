import React, { useState } from "react";
import { Link } from "react-router-dom";
import { Card } from "../ui/Card";
import { formatCurrency } from "../../lib/utils/format";
import type { PropertyListItem } from "../../lib/types/property.types";

export interface PropertyCardProps {
  property: PropertyListItem;
}

export const PropertyCard: React.FC<PropertyCardProps> = ({ property }) => {
  const [imageError, setImageError] = useState(false);
  const placeholderImage = "/placeholder-property.jpg";
  const imageUrl =
    imageError || !property.imageUrl ? placeholderImage : property.imageUrl;
  const ownerName = property.owner?.name || "Unknown Owner";

  const handleImageError = () => {
    setImageError(true);
  };

  return (
    <Link to={`/properties/${property.idProperty}`} className="group">
      <Card
        hoverable
        className="h-full overflow-hidden transition-all duration-300 hover:scale-[1.02]"
      >
        <div className="relative h-48 sm:h-56 md:h-64 w-full overflow-hidden bg-gray-200">
          <img
            src={imageUrl}
            alt={property.name}
            className="w-full h-full object-cover transition-transform duration-300 group-hover:scale-110"
            loading="lazy"
            onError={handleImageError}
          />
          <div className="absolute top-4 right-4 bg-white px-3 py-1.5 rounded-full shadow-lg">
            <p className="text-lg font-bold text-primary-600">
              {formatCurrency(property.price)}
            </p>
          </div>
        </div>

        <div className="p-4 sm:p-5 space-y-2 sm:space-y-3 bg-white">
          <h3 className="text-lg sm:text-xl font-bold text-gray-900 truncate group-hover:text-primary-600 transition-colors">
            {property.name}
          </h3>

          <div className="flex items-start gap-2">
            <svg
              className="w-4 h-4 sm:w-5 sm:h-5 text-gray-400 shrink-0 mt-0.5"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z"
              />
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M15 11a3 3 0 11-6 0 3 3 0 016 0z"
              />
            </svg>
            <p
              className="text-xs sm:text-sm text-gray-600 line-clamp-2"
              title={property.address}
            >
              {property.address}
            </p>
          </div>

          <div className="flex flex-col xs:flex-row xs:justify-between xs:items-center gap-2 xs:gap-0 pt-2 sm:pt-3 border-t border-gray-100">
            <div className="flex items-center gap-1.5 text-xs sm:text-sm text-gray-600">
              <svg
                className="w-3.5 h-3.5 sm:w-4 sm:h-4"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"
                />
              </svg>
              <span className="font-medium">{property.year}</span>
            </div>
            <div className="flex items-center gap-1.5 text-xs sm:text-sm text-gray-600">
              <svg
                className="w-3.5 h-3.5 sm:w-4 sm:h-4"
                fill="none"
                viewBox="0 0 24 24"
                stroke="currentColor"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth={2}
                  d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"
                />
              </svg>
              <span
                className="truncate max-w-[120px] sm:max-w-[100px]"
                title={ownerName}
              >
                {ownerName}
              </span>
            </div>
          </div>
        </div>
      </Card>
    </Link>
  );
};
