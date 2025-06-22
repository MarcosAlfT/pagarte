import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import MainLayout from "../layouts/MainLayout";
import Home from "../pages/Home";
import NotFoundPage from "../pages/NofoundPage";
import AddPaymentMethod from "../pages/AddPaymentMethod";
import LoginPage from "../pages/LoginPage";
import MakePayment from "../pages/MakePayment";
import FlowForm from "../components/forms/GenericFlowForm";
// import IconReferencePage from "../pages/IconReferencePage";
import RegisterPage from "../pages/RegisterPage";
import RegistrationSuccessPage from "../pages/RegistrationSuccessPage";
import LogoutPage from "../pages/LogoutPage";

interface AppRouterProps {
  isLoggedIn: boolean;
  onLogin: () => void;
  onLogout: () => void;
}

const AppRouter: React.FC<AppRouterProps> = ({
  isLoggedIn,
  onLogin,
  onLogout,
}) => (
  <Routes>
    {/* Public login route */}
    <Route path="/login" element={<LoginPage onLogin={onLogin} />} />
    <Route path="/register" element={<RegisterPage />} />
    <Route path="/register-success" element={<RegistrationSuccessPage />} />
    {/* Protected routes (with MainLayout) */}
    <Route
      path="/"
      element={
        isLoggedIn ? (
          <MainLayout isLoggedIn={isLoggedIn} />
        ) : (
          <Navigate to="/login" replace />
        )
      }
    >
      <Route path="home" element={<Home />} />
      <Route path="makepayment" element={<MakePayment />} />
      <Route path="payments/new" element={<FlowForm />} />
      <Route path="addpaymentmethod" element={<AddPaymentMethod />} />
      {/* <Route path="icon-reference" element={<IconReferencePage />} /> */}
      {/* <Route path="settings" element={<Settings />} /> */}
      <Route path="logout" element={<LogoutPage onLogout={onLogout} />} />
      <Route index element={<Navigate to="/home" replace />} />
      <Route path="*" element={<NotFoundPage />} />
    </Route>
    {/* Catch-all: If not logged in, redirect any unknown route to login */}
    <Route
      path="*"
      element={<Navigate to={isLoggedIn ? "/home" : "/login"} replace />}
    />
  </Routes>
);

export default AppRouter;
