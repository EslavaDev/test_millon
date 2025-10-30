import React from "react";
import { Link } from "react-router-dom";

export const Header: React.FC = () => {
  return (
    <header className="bg-white shadow-sm border-b border-gray-200 sticky top-0 z-10">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 justify-center items-center flex">
        <div className="flex justify-between items-center h-16">
          <Link
            to="/"
            className="flex items-center space-x-2 text-primary-600 hover:text-primary-700 transition-colors"
          >
            <svg
              className="h-8 w-8"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
              aria-hidden="true"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"
              />
            </svg>
            <span className="text-xl font-bold">Real Estate</span>
          </Link>
        </div>
      </div>
    </header>
  );
};
