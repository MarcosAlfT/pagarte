import React from "react";

/**
 * Define the props for the GenericInput component.
 * We extend React.InputHTMLAttributes to inherit all standard <input> props
 * like `disabled`, `required`, `maxLength`, etc., making the component very flexible.
 */
interface GenericInputProps
  extends React.InputHTMLAttributes<HTMLInputElement> {
  // --- Required props ---
  name: string; // Used for the `name` attribute, `id`, and the label's `htmlFor`
  value: string | number;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;

  // --- Optional props ---
  label?: string;
  error?: string; // An error message to display below the input
}

/**
 * A reusable and styled text input component for forms.
 * It handles its own label, error display, and consistent styling.
 */
const GenericInput: React.FC<GenericInputProps> = ({
  name,
  label,
  value,
  onChange,
  error,
  type = "text", // Default to a text input if no type is specified
  className = "", // Allow passing extra classes for custom styling
  ...rest // Collect all other standard input props (e.g., placeholder, autoFocus, disabled)
}) => {
  return (
    <div className="w-full space-y-2">
      {/* Conditionally render the label only if it's provided */}
      {label && (
        <label
          htmlFor={name}
          className="block text-sm font-medium text-primary"
        >
          {label}
        </label>
      )}

      <input
        // --- Core Attributes ---
        id={name}
        name={name}
        type={type}
        value={value}
        onChange={onChange}
        // --- Accessibility ---
        // Links the input to its error message for screen readers
        aria-describedby={`${name}-error`}
        // Tells screen readers if the input is in an invalid state
        aria-invalid={!!error}
        // --- Styling ---
        // Combines base styles, conditional error styles, and custom parent styles
        className={`
          w-full px-3 py-2 border rounded-md text-sm transition-colors duration-200
          focus:outline-none focus:ring-2 focus:ring-primary/50
          ${error ? "border-error" : "border-border_default"}
          ${className}
        `}
        // --- Pass all other props through ---
        // This is how props like `placeholder`, `autoFocus`, `required`, etc., get applied
        {...rest}
      />

      {/* 
        Error message container.
        It has a minimum height to prevent the layout from "jumping" when an error appears.
      */}
      <div id={`${name}-error`} className="text-error text-xs min-h-[1.25em]">
        {/* Render the error message if it exists, otherwise render a non-breaking space */}
        {error || "\u00A0"}
      </div>
    </div>
  );
};

export default GenericInput;
