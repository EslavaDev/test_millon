import React from 'react';

export const Footer: React.FC = () => {
  const currentYear = new Date().getFullYear();

  return (
    <footer className="bg-gray-50 border-t border-gray-200 mt-auto">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="flex flex-col sm:flex-row justify-between items-center space-y-4 sm:space-y-0">
          <div className="text-gray-600 text-sm">
            &copy; {currentYear} Real Estate Property Management. All rights reserved.
          </div>

          <div className="flex space-x-6 text-sm">
            <a
              href="#"
              className="text-gray-600 hover:text-primary-600 transition-colors"
            >
              Privacy Policy
            </a>
            <a
              href="#"
              className="text-gray-600 hover:text-primary-600 transition-colors"
            >
              Terms of Service
            </a>
            <a
              href="#"
              className="text-gray-600 hover:text-primary-600 transition-colors"
            >
              Contact
            </a>
          </div>
        </div>
      </div>
    </footer>
  );
};
