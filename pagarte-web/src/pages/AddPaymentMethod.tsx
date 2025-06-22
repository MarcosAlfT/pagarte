import { Link } from 'react-router-dom';
import { FaWallet } from 'react-icons/fa';
import ContentLayout from '../layouts/ContentLayout';

const AddPaymentMethod = () => {
  return (
    <ContentLayout title="Add Payment Method">
      <div className="flex flex-col items-center justify-center min-h-[60vh] py-8">
        <FaWallet className="text-red-600 text-5xl mb-4" />
        <h1 className="text-6xl font-bold mb-4">Add payment method</h1>
        <p className="text-xl mb-5">Why not show this page</p>
        <Link to="/Home" className="text-white bg-blue-900 hover:bg-blue-700 rounded-md px-3 py-2 mt-4">
          Go Back
        </Link>
      </div>
    </ContentLayout>
  );
};
export default AddPaymentMethod;
