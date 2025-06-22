import React from 'react';

interface GenericButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  type?: 'button' | 'submit' | 'reset';
  variant?: 'primary' | 'secondary' | 'outline';
  text?: string;
  onClick?: () => void;
  className?: string;
  children?: React.ReactNode;
  icon?: React.ReactNode;
  disabled?: boolean;
  loading?: boolean;
}

const buttonBase =
  'transition font-medium rounded focus:outline-none focus:ring-2 focus:ring-offset-2 flex items-center justify-center';
const variants: Record<string, string> = {
  primary: 'bg-primary text-white hover:bg-comp_hover py-2',
  secondary: 'bg-secondary text-primary border border-primary hover:bg-primary hover:text-white',
  outline: 'bg-secondary text-primary hover:text-comp_hover',
};

const GenericButton: React.FC<GenericButtonProps> = ({
  type = 'button',
  variant = 'primary',
  text,
  icon,
  onClick,
  className = '',
  children,
  disabled = false,
  loading = false,
  ...rest
}) => {
  return (
    <button
      type={type}
      className={`
        ${buttonBase} ${variants[variant]} ${className}
        ${disabled || loading ? 'opacity-50 cursor-not-allowed pointer-events-none' : ''}
      `}
      onClick={onClick}
      disabled={disabled || loading}
      {...rest}
    >
      {icon}
      {loading && (
        <svg className="animate-spin mr-2 h-4 w-4 text-white" viewBox="0 0 24 24">
          <circle
            className="opacity-25"
            cx="12"
            cy="12"
            r="10"
            stroke="currentColor"
            strokeWidth="4"
            fill="none"
          />
          <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v8z" />
        </svg>
      )}
      {text}
      {children}
    </button>
  );
};

export default GenericButton;
