import React from 'react';

export interface CardProps {
  children: React.ReactNode;
  className?: string;
  hoverable?: boolean;
  onClick?: () => void;
}

export const Card: React.FC<CardProps> = ({
  children,
  className = '',
  hoverable = false,
  onClick,
}) => {
  const baseStyles =
    'bg-white rounded-lg shadow-md overflow-hidden transition-shadow';

  const hoverStyles = hoverable
    ? 'hover:shadow-lg cursor-pointer'
    : '';

  const clickStyles = onClick && !hoverable
    ? 'cursor-pointer'
    : '';

  const combinedClassName = `${baseStyles} ${hoverStyles} ${clickStyles} ${className}`;

  return (
    <div
      className={combinedClassName}
      onClick={onClick}
      role={onClick ? 'button' : undefined}
      tabIndex={onClick ? 0 : undefined}
      onKeyDown={
        onClick
          ? (e) => {
              if (e.key === 'Enter' || e.key === ' ') {
                e.preventDefault();
                onClick();
              }
            }
          : undefined
      }
    >
      {children}
    </div>
  );
};
