import React from "react";
import { useNavigate } from "react-router-dom";
import ContentLayout from "../layouts/ContentLayout";
import GenericButton from "../components/controls/GenericButton";
import { FaExclamationTriangle, FaSignOutAlt } from "react-icons/fa";

interface LogoutPageProps {
  onLogout: () => void;
}

const LogoutPage: React.FC<LogoutPageProps> = ({ onLogout }) => {
  const navigate = useNavigate();

  const handleConfirmLogout = () => {
    onLogout();
    navigate("/login", { replace: true });
  };

  const handleCancel = () => {
    navigate("/home");
  };

  return (
    <ContentLayout title="Confirm Logout" icon={<FaSignOutAlt />}>
      <div className="max-w-md w-full mx-auto bg-white p-8 rounded-xl shadow-lg text-center flex flex-col items-center mt-10">
        <FaExclamationTriangle className="text-yellow-500 h-12 w-12 mb-5" />
        <h2 className="text-2xl font-bold text-primary mb-4">Are you sure?</h2>

        <p className="text-gray-600 mb-8">
          You will be logged out of your account and will need to sign in again.
        </p>

        <div className="flex justify-center gap-4 w-full">
          {/* Destructive Action Button (Red) */}
          <GenericButton
            text="Yes, Log Out"
            onClick={handleConfirmLogout}
            className="flex-1 bg-error text-white hover:bg-red-700"
          />
          <GenericButton
            text="Cancel"
            onClick={handleCancel}
            variant="secondary"
            className="flex-1"
          />
        </div>
      </div>
    </ContentLayout>
  );
};

export default LogoutPage;
