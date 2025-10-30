import React from "react";

export interface ButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: "primary" | "secondary" | "outline" | "ghost";
  size?: "sm" | "md" | "lg";
  disabled?: boolean;
  children: React.ReactNode;
}

export const Button: React.FC<ButtonProps> = ({
  variant = "primary",
  size = "md",
  disabled = false,
  children,
  className = "",
  ...props
}) => {
  const baseStyles =
    "inline-flex items-center justify-center font-medium rounded-md transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed";

  const variantStyles = {
    primary:
      "bg-primary-600 text-black/500 hover:bg-primary-700 focus:ring-primary-500 border border-black-500",
    secondary:
      "bg-secondary-600 text-secondary hover:bg-secondary-700 focus:ring-secondary-500",
    outline:
      "border-2 border-primary-600 text-primary-600 hover:bg-primary-50 focus:ring-primary-500",
    ghost: "text-gray-700 hover:bg-gray-100 focus:ring-gray-500",
  };

  const sizeStyles = {
    sm: "px-3 py-1.5 text-sm",
    md: "px-4 py-2 text-base",
    lg: "px-6 py-3 text-lg",
  };

  const combinedClassName = `${baseStyles} ${variantStyles[variant]} ${sizeStyles[size]} ${className}`;

  return (
    <button className={combinedClassName} disabled={disabled} {...props}>
      {children}
    </button>
  );
};
