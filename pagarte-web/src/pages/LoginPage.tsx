import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useLogin } from "../hooks/useLogin";

import AuthLayout from "../layouts/AuthLayout";
import GenericForm from "../components/forms/GenericForm";
import EmailInput from "../components/controls/EmailInput";
import PasswordInput from "../components/controls/PasswordInput";
import GenericButton from "../components/controls/GenericButton";
import GenericLink from "../components/controls/GenericLink";
import GenericCheckbox from "../components/controls/GenericCheckbox";

interface LoginPageProps {
  // This prop is for updating a global state
  onLogin: () => void;
}

const LoginPage: React.FC<LoginPageProps> = ({ onLogin }) => {
  const navigate = useNavigate();
  const {
    execute: performLogin,
    isLoading,
    error: apiError,
    isSuccess,
  } = useLogin();

  // It's cleaner to manage form state in a single object
  const [form, setForm] = useState({ email: "", password: "" });
  const [rememberMe, setRememberMe] = useState(false);

  // This side-effect runs when the login is successful
  useEffect(() => {
    if (isSuccess) {
      onLogin(); // Tell the parent App that the user is authenticated
      navigate("/home"); // Redirect to the main application page
    }
  }, [isSuccess, onLogin, navigate]);

  const handleFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await performLogin({
      usernameOrEmail: form.email,
      password: form.password,
    });
  };

  return (
    <AuthLayout>
      <GenericForm
        onSubmit={handleSubmit}
        error={apiError ?? undefined}
        title="Welcome"
        subtitle="Sign in to your account to continue"
      >
        <EmailInput
          name="email" // Add name for the handler
          value={form.email}
          onChange={handleFormChange}
          autoFocus
        />
        <PasswordInput
          name="password" // Add name for the handler
          value={form.password}
          onChange={handleFormChange}
          validate={false}
        />

        <div className="flex items-center justify-between w-full mb-1 mt-1">
          <GenericCheckbox
            id="rememberMe"
            checked={rememberMe}
            onChange={(e) => setRememberMe(e.target.checked)}
            label="Remember me"
          />
          <GenericLink to="/forgotpassword" text="Forgot password?" />
        </div>

        <GenericButton
          type="submit"
          text={isLoading ? "Signing In..." : "Sign In"}
          disabled={isLoading}
          className="mt-2 w-full rounded-xl"
        />

        <div className="border-t border-border_default w-full my-6"></div>

        <div className="text-center text-base">
          <span className="text-gray-500">Don't have an account? </span>
          <GenericLink
            to="/register"
            text="Sign up here"
            className="font-medium"
          />
        </div>
      </GenericForm>
    </AuthLayout>
  );
};

export default LoginPage;
