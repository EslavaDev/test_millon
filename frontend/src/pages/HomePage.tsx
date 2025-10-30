import React, { useEffect } from "react";
import { PropertyList } from "../components/properties/PropertyList";
import { PropertyFilters } from "../components/properties/PropertyFilters";
import { Button } from "../components/ui/Button";
import { Select } from "../components/ui/Select";
import { usePropertyStore } from "../lib/store/propertyStore";
import { useProperties } from "../lib/hooks/useProperties";
import type { PropertyFilter } from "../lib/types/property.types";

export const HomePage: React.FC = () => {
  const {
    filters,
    setFilters,
    resetFilters,
    setCurrentPage,
    setSorting,
    setPageSize,
  } = usePropertyStore();

  const {
    properties,
    totalPages,
    hasNextPage,
    hasPreviousPage,
    isLoading,
    error,
  } = useProperties(filters);

  useEffect(() => {
    document.title = "Properties - Real Estate Management";
  }, []);

  useEffect(() => {
    window.scrollTo({ top: 0, behavior: "smooth" });
  }, [filters.pageNumber]);

  const handleFilterChange = (newFilters: PropertyFilter) => {
    setFilters(newFilters);
  };

  const handleFilterReset = () => {
    resetFilters();
  };

  const handlePreviousPage = () => {
    if (hasPreviousPage && filters.pageNumber) {
      setCurrentPage(filters.pageNumber - 1);
    }
  };

  const handleNextPage = () => {
    if (hasNextPage && filters.pageNumber) {
      setCurrentPage(filters.pageNumber + 1);
    }
  };

  const handleSortChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const value = event.target.value;
    const [sortBy, order] = value.split("-") as [
      PropertyFilter["sortBy"],
      "asc" | "desc"
    ];
    setSorting(sortBy, order === "desc");
  };

  const handlePageSizeChange = (
    event: React.ChangeEvent<HTMLSelectElement>
  ) => {
    const newPageSize = parseInt(event.target.value, 10);
    setPageSize(newPageSize);
  };

  const currentSort = `${filters.sortBy}-${
    filters.sortDescending ? "desc" : "asc"
  }`;

  const errorMessage = error
    ? typeof error === "string"
      ? error
      : error &&
        typeof error === "object" &&
        "detail" in error &&
        typeof error.detail === "string"
      ? error.detail
      : error &&
        typeof error === "object" &&
        "message" in error &&
        typeof error.message === "string"
      ? error.message
      : "An error occurred"
    : null;

  return (
    <div className="space-y-8">
      <PropertyFilters
        filters={filters}
        onFilterChange={handleFilterChange}
        onReset={handleFilterReset}
      />

      <PropertyList
        properties={properties}
        isLoading={isLoading}
        error={errorMessage}
      />

      {!isLoading && !errorMessage && properties.length > 0 && (
        <div className="flex flex-col sm:flex-row justify-between items-center gap-3 sm:gap-4 pt-6 sm:pt-8 pb-4">
          <div className="text-xs sm:text-sm font-medium text-black/500 bg-linear-to-r from-gray-100 to-gray-50 px-4 py-2 rounded-full shadow-sm border border-black-500">
            Page {filters.pageNumber || 1} of {totalPages || 1}
          </div>

          <div className="flex flex-row gap-2 sm:gap-3 w-fit items-center justify-end">
            <div className="flex flex-row  gap-2 sm:gap-3 w-full lg:w-auto bg-white p-3 sm:p-4 rounded-lg items-end justify-end">
              <Select
                label="Sort By"
                value={currentSort}
                onChange={handleSortChange}
                options={[
                  { value: "name-asc", label: "Name (A-Z)" },
                  { value: "name-desc", label: "Name (Z-A)" },
                  { value: "price-asc", label: "Price (Low-High)" },
                  { value: "price-desc", label: "Price (High-Low)" },
                  { value: "year-asc", label: "Year (Old-New)" },
                  { value: "year-desc", label: "Year (New-Old)" },
                ]}
              />

              <Select
                label="Per Page"
                value={filters.pageSize?.toString() || "10"}
                onChange={handlePageSizeChange}
                options={[
                  { value: "5", label: "5" },
                  { value: "10", label: "10" },
                  { value: "20", label: "20" },
                  { value: "50", label: "50" },
                ]}
              />
            </div>
            <div className="flex gap-2 sm:gap-3">
              <Button
                variant="outline"
                onClick={handlePreviousPage}
                disabled={!hasPreviousPage}
                className="shadow-sm hover:shadow-md transition-all duration-300 text-sm sm:text-base"
              >
                <svg
                  className="w-4 h-4 sm:w-5 sm:h-5 mr-1 sm:mr-2"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth={2}
                    d="M15 19l-7-7 7-7"
                  />
                </svg>
                <span className="xs:inline">Previous</span>
              </Button>

              <Button
                variant="primary"
                onClick={handleNextPage}
                disabled={!hasNextPage}
                className="shadow-sm hover:shadow-md transition-all duration-300 text-sm sm:text-base"
              >
                <span className="xs:inline">Next</span>
                <svg
                  className="w-4 h-4 sm:w-5 sm:h-5 ml-1 sm:ml-2"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth={2}
                    d="M9 5l7 7-7 7"
                  />
                </svg>
              </Button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
