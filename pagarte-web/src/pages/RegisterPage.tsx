import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useRegistration } from "../hooks/useRegistration";

import AuthLayout from "../layouts/AuthLayout";
import GenericForm from "../components/forms/GenericForm";
import GenericInput from "../components/controls/GenericInput";
import EmailInput from "../components/controls/EmailInput";
import PasswordInput from "../components/controls/PasswordInput";
import GenericButton from "../components/controls/GenericButton";
import GenericLink from "../components/controls/GenericLink";

const RegisterPage: React.FC = () => {
  // --- Hooks and State ---
  const navigate = useNavigate();
  const {
    execute: performRegistration,
    isLoading,
    error: apiError,
    isSuccess,
  } = useRegistration();
  const [form, setForm] = useState({
    userName: "",
    email: "",
    userPassword: "",
  });
  const [captcha, setCaptcha] = useState("");

  // State for client-side validation errors
  const [clientErrors, setClientErrors] = useState({
    email: undefined as string | undefined,
    password: undefined as string | undefined,
    captcha: undefined as string | undefined,
  });

  // --- Side Effects ---
  // Navigate to the success page when the API call succeeds
  useEffect(() => {
    if (isSuccess) {
      navigate("/register-success");
    }
  }, [isSuccess, navigate]);

  // --- Event Handlers ---
  const handleFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleCaptchaChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setCaptcha(e.target.value);
    // Clear captcha error when user starts typing
    if (clientErrors.captcha) {
      setClientErrors({ ...clientErrors, captcha: undefined });
    }
  };

  const handleEmailValidation = (isValid: boolean) => {
    setClientErrors((prev) => ({
      ...prev,
      email: isValid ? undefined : "Email format is invalid.",
    }));
  };

  const handlePasswordValidation = (isValid: boolean) => {
    setClientErrors((prev) => ({
      ...prev,
      password: isValid
        ? undefined
        : "Password must be 8-20 chars, with uppercase, number, and symbol.",
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    // --- Client-Side Validation ---
    const correctCaptcha = "7";
    if (captcha !== correctCaptcha) {
      setClientErrors({
        ...clientErrors,
        captcha: "CAPTCHA answer is incorrect.",
      });
      return;
    }

    if (clientErrors.email || clientErrors.password) {
      return;
    }

    // Call the registration hook
    await performRegistration(form);
  };

  // Determine if the submit button should be disabled for a better UX
  const isSubmitDisabled =
    isLoading || // 1. While the API call is in progress
    !form.userName ||
    !form.email ||
    !form.userPassword ||
    !captcha || // 2. If any field is empty
    !!clientErrors.email ||
    !!clientErrors.password; // 3. If there are known validation errors

  return (
    <AuthLayout>
      <GenericForm
        onSubmit={handleSubmit}
        title="Create your account"
        subtitle="Sign up to get started"
        // The error prop now only shows errors from the API
        error={apiError ?? undefined}
      >
        <GenericInput
          name="userName"
          label="Username"
          value={form.userName}
          onChange={handleFormChange}
          autoFocus
          placeholder="Enter your username"
        />
        <EmailInput
          value={form.email}
          onChange={handleFormChange}
          externalError={clientErrors.email}
          onValidation={handleEmailValidation}
        />
        <PasswordInput
          name="userPassword"
          value={form.userPassword}
          onChange={handleFormChange}
          validate
          externalError={clientErrors.password}
          onValidation={handlePasswordValidation}
        />

        {/* Simple CAPTCHA Section */}
        <div className="space-y-1">
          <div className="flex items-center space-x-2">
            <label htmlFor="captcha" className="text-base">
              What is 5 + 2?
            </label>
            <GenericInput
              id="captcha"
              name="captcha"
              value={captcha}
              onChange={handleCaptchaChange}
              placeholder="Answer"
              className="w-24"
            />
          </div>
          {/* Display CAPTCHA-specific error message */}
          {clientErrors.captcha && (
            <p className="text-error text-xs">{clientErrors.captcha}</p>
          )}
        </div>

        <GenericButton
          type="submit"
          text={isLoading ? "Registering..." : "Register"}
          className="w-full bg-primary text-secondary hover:bg-comp_hover rounded-2xl shadow p-2 font-semibold"
          disabled={isSubmitDisabled}
        />

        <div className="text-center text-base">
          <span className="text-gray-500">Already have an account? </span>
          <GenericLink
            to="/login"
            text="Sign in here"
            className="font-medium"
          />
        </div>
      </GenericForm>
    </AuthLayout>
  );
};

export default RegisterPage;
