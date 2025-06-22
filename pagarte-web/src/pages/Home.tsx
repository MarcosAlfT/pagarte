import React from "react";
import {
  FaPlane,
  FaTooth,
  FaTshirt,
  FaBuilding,
  FaFlask,
  FaRunning,
  FaBurn,
  FaBriefcaseMedical,
  FaGraduationCap,
  FaFaucet,
  FaConciergeBell,
  FaBolt,
} from "react-icons/fa";
import GenericButton from "../components/controls/GenericButton";
import { useNavigate } from "react-router-dom";
import ContentLayout from "../layouts/ContentLayout";

const industryIcons = [
  { icon: <FaBolt />, label: "Electricity" },
  { icon: <FaPlane />, label: "Travel" },
  { icon: <FaTooth />, label: "Dentist" },
  { icon: <FaTshirt />, label: "Retail" },
  { icon: <FaBuilding />, label: "Corporate" },
  { icon: <FaFlask />, label: "Beauty" },
  { icon: <FaRunning />, label: "Sports" },
  { icon: <FaBurn />, label: "Water" },
  { icon: <FaBriefcaseMedical />, label: "Wellness" },
  { icon: <FaGraduationCap />, label: "Education" },
  { icon: <FaFaucet />, label: "Water" },
  { icon: <FaConciergeBell />, label: "Hospitality" },
];

const Home: React.FC = () => {
  const navigate = useNavigate();

  return (
    <ContentLayout title="Tailored to your industry">
      <div className="flex flex-col md:flex-row items-center gap-12 mx-auto max-w-6xl">
        {/* Left Side: Text & Actions */}
        <div className="flex-1 max-w-2xl">
          <p className="text-lg text-primary/80 mb-8">
            We understand your business. From college campuses to complex
            corporations, we designed our payments solutions to help your
            business needs.
          </p>
          <div className="flex gap-4 mb-8 flex-wrap">
            <GenericButton
              text="Make a Payment"
              variant="primary"
              className="px-6"
              onClick={() => navigate("/makepayment")}
            />
            <GenericButton
              text="Register Payment Method"
              variant="primary"
              className="px-6"
              onClick={() => navigate("/addpaymentmethod")}
            />
          </div>
        </div>

        {/* Right Side: Icon Grid */}
        <div className="flex-1 grid grid-cols-4 gap-10 max-w-2xl">
          {industryIcons.map(({ icon, label }) => (
            <div
              key={label}
              className="flex flex-col items-center group"
              title={label}
            >
              <div className="text-[3.5rem] text-primary group-hover:text-comp_hover transition">
                {icon}
              </div>
              <span className="text-sm text-primary/70 mt-3">{label}</span>
            </div>
          ))}
        </div>
      </div>
    </ContentLayout>
  );
};

export default Home;
