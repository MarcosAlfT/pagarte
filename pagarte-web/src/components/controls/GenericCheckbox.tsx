// import React from 'react';

interface GenericCheckboxProps {
  id?: string;
  checked: boolean;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  label: string;
  className?: string;
}

const GenericCheckbox: React.FC<GenericCheckboxProps> = ({
  id,
  checked,
  onChange,
  label,
  className = '',
}) => (
  <label className={`flex items-center space-x-2 text-primary text-xs cursor-pointer ${className}`}>
    <input
      type="checkbox"
      id={id}
      checked={checked}
      onChange={onChange}
      className="accent-primary h-4 w-4 rounded border-border_default focus:ring-primary transition"
    />
    <span>{label}</span>
  </label>
);

export default GenericCheckbox;
