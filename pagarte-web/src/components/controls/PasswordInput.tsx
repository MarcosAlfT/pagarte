import React, { useState } from "react";
import { validatePassword } from "../../utils/validators";
import { FaEye, FaEyeSlash } from "react-icons/fa"; // <-- Add this line

interface PasswordInputProps
  extends React.InputHTMLAttributes<HTMLInputElement> {
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  autoFocus?: boolean;
  externalError?: string;
  validate?: boolean;
  onValidation?: (isValid: boolean) => void;
  label?: string;
  name?: string;
}

const PasswordInput: React.FC<PasswordInputProps> = ({
  value,
  onChange,
  autoFocus = false,
  externalError,
  validate = false,
  onValidation,
  label = "Password",
  name = "password",
  className = "",
  ...rest
}) => {
  const [touched, setTouched] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const isValid = validate ? validatePassword(value) : true;
  const showError = touched && !isValid;
  const errorMessage =
    externalError ||
    (showError
      ? "Password must be 8-20 chars, include uppercase, number, and symbol."
      : "");

  const handleBlur = () => {
    setTouched(true);
    onValidation?.(isValid);
  };

  return (
    <div className="space-y-2">
      <label htmlFor={name} className="block text-xs font-medium text-primary">
        {label}
      </label>
      <div className="relative">
        <input
          id={name}
          name={name}
          type={showPassword ? "text" : "password"}
          autoFocus={autoFocus}
          className={`
            w-full px-3 py-2 border rounded text-sm focus:outline-none
            focus:ring-2 focus:ring-primary pr-10
            ${errorMessage ? "border-error" : "border-border_default"}
            ${className}
          `}
          value={value}
          onChange={onChange}
          onBlur={handleBlur}
          required
          placeholder="Password"
          aria-invalid={!!errorMessage}
          aria-describedby={`${name}-error`}
          {...rest}
        />
        <button
          type="button"
          tabIndex={-1}
          className="absolute right-2 top-1/2 transform -translate-y-1/2 text-primary"
          onClick={() => setShowPassword((v) => !v)}
          aria-label={showPassword ? "Hide password" : "Show password"}
        >
          {showPassword ? (
            <FaEye className="h-5 w-5" />
          ) : (
            <FaEyeSlash className="h-5 w-5" />
          )}
        </button>
      </div>
      <div
        id={`${name}-error`}
        className="text-error text-xs min-h-[1.25em] transition-all"
      >
        {errorMessage || "\u00A0"}
      </div>
    </div>
  );
};

export default PasswordInput;
