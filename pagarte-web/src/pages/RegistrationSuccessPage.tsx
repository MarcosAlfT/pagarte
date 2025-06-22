import React from 'react';
import { Link } from 'react-router-dom';
import AuthLayout from '../layouts/AuthLayout';
import { FaCheckCircle } from 'react-icons/fa'; // A nice icon for success

const RegistrationSuccessPage: React.FC = () => {
  return (
    <AuthLayout>
      <div className="w-full max-w-md bg-secondary p-8 rounded-lg shadow-md text-center">
        <FaCheckCircle className="text-success h-16 w-16 mx-auto mb-6" />
        <h2 className="text-2xl font-bold text-primary mb-4">Registration Successful!</h2>

        <p className="text-gray-600 mb-8">
          We've sent a confirmation link to your email address. Please click the link to activate your
          account.
        </p>

        <Link
          to="/login"
          className="w-full inline-block bg-primary text-secondary hover:bg-comp_hover rounded-2xl shadow p-3 font-semibold transition-colors duration-200"
        >
          Proceed to Login
        </Link>
      </div>
    </AuthLayout>
  );
};

export default RegistrationSuccessPage;
