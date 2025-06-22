import React, { type FormEvent, type RefObject } from "react";

interface GenericFormFormProps {
  onSubmit: (e: FormEvent) => void;
  formRef?: RefObject<HTMLFormElement | null>;
  title?: string;
  subtitle?: string;
  children: React.ReactNode;
  error?: string;
}

const GenericForm: React.FC<GenericFormFormProps> = ({
  onSubmit,
  formRef,
  children,
  error,
  title,
  subtitle,
}) => (
  <form
    ref={formRef}
    onSubmit={onSubmit}
    className={`
      w-full max-w-xs bg-white rounded-xl shadow-lg px-6 py-8 flex flex-col justify-center space-y-4
    `}
    autoComplete="off"
    style={{ minHeight: "420px" }}
  >
    {title && (
      <div className="flex flex-col items-center mb-2">
        <h2 className="text-2xl font-bold text-primary text-center mb-1">
          {title}
        </h2>
        {subtitle && (
          <div className="text-base text-gray-500 text-center">{subtitle}</div>
        )}
      </div>
    )}

    {children}
    {error && (
      <div className="text-error text-xs text-center mt-1 animate-fade-in">
        {error}
      </div>
    )}
  </form>
);

export default GenericForm;
