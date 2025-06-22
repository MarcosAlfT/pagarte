import React, { useState } from "react";
import { validateEmail } from "../../utils/validators";

interface EmailInputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  autoFocus?: boolean;
  externalError?: string;
  onValidation?: (isValid: boolean) => void;
  name?: string;
}

const EmailInput: React.FC<EmailInputProps> = ({
  value,
  onChange,
  autoFocus = false,
  externalError,
  onValidation,
  name = "email",
  className = "",
  ...rest
}) => {
  const [touched, setTouched] = useState(false);
  const isValid = validateEmail(value);
  const showError = touched && !isValid;
  const errorMessage =
    externalError || (showError ? "Email format is invalid" : "");

  const handleBlur = () => {
    setTouched(value.length > 0);
    onValidation?.(isValid);
  };

  return (
    <div className="space-y-2">
      <label htmlFor={name} className="block text-xs font-medium text-primary">
        Email
      </label>
      <input
        id={name}
        name={name}
        type="email"
        autoFocus={autoFocus}
        className={`
          w-full px-3 py-2 border rounded text-sm focus:outline-none
          focus:ring-2 focus:ring-primary
          ${errorMessage ? "border-error" : "border-border_default"}
          ${className}
        `}
        value={value}
        onChange={onChange}
        onBlur={handleBlur}
        required
        placeholder="your@email.com"
        aria-invalid={!!errorMessage}
        aria-describedby={`${name}-error`}
        {...rest}
      />
      <div
        id={`${name}-error`}
        className="text-error text-xs min-h-[1.25em] transition-all"
      >
        {errorMessage || "\u00A0"}
      </div>
    </div>
  );
};

export default EmailInput;
