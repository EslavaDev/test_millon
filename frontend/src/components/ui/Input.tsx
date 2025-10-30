import React from 'react';

export interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label?: string;
  error?: string;
  hasError?: boolean;
}

export const Input = React.forwardRef<HTMLInputElement, InputProps>(
  ({ label, error, hasError, className = '', id, ...props }, ref) => {
    const inputId = id || label?.toLowerCase().replace(/\s+/g, '-');
    const showError = hasError || !!error;

    const baseStyles =
      'block w-full rounded-md border-gray-300 shadow-sm focus:border-primary-500 focus:ring-primary-500 sm:text-sm disabled:bg-gray-100 disabled:cursor-not-allowed';

    const errorStyles = showError
      ? 'border-red-500 focus:border-red-500 focus:ring-red-500'
      : '';

    const combinedClassName = `${baseStyles} ${errorStyles} ${className}`;

    return (
      <div className="w-full">
        {label && (
          <label
            htmlFor={inputId}
            className="block text-sm font-medium text-gray-700 mb-1"
          >
            {label}
          </label>
        )}
        <input
          ref={ref}
          id={inputId}
          className={combinedClassName}
          aria-invalid={showError}
          aria-describedby={showError ? `${inputId}-error` : undefined}
          {...props}
        />
        {error && (
          <p
            id={`${inputId}-error`}
            className="mt-1 text-sm text-red-600"
            role="alert"
          >
            {error}
          </p>
        )}
      </div>
    );
  }
);

Input.displayName = 'Input';
