import React from "react";
import ContentLayout from "../layouts/ContentLayout";
import GenericFlowForm from "../components/forms/GenericFlowForm";
import { FaMoneyBillWave } from "react-icons/fa"; // A fitting icon from your sidebar

const MakePayment: React.FC = () => {
  return (
    <ContentLayout title="Make a Payment" icon={<FaMoneyBillWave />}>
      <GenericFlowForm />
    </ContentLayout>
  );
};

export default MakePayment;
