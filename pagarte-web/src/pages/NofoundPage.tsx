import { Link } from "react-router-dom";
import { FaExclamationTriangle } from "react-icons/fa";

const NotFoundPage = () => {
  return (
    <section className="flex justify-center min-h-screen w-full text-center  flex-col  items-center">
      <FaExclamationTriangle className="text-yellow-400 text-6xl mb-4" />
      <h1 className="text-6xl font-bold mb-4">404 Not Found</h1>
      <p className="text-xl mb-5">This page does not exist</p>
      <Link
        to="/Home"
        className="text-white bg-blue-900 hover:bg-blue-700 rounded-md px-3 py-2 mt-4"
      >
        Go Back
      </Link>
    </section>
  );
};
export default NotFoundPage;
