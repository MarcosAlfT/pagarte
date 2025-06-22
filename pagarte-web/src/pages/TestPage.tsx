import React from "react";
import GenericButton from "../components/controls/GenericButton";

const TestPage: React.FC = () => {
  // you could do other work here...
  console.log("Rendering TestPage");

  return (
    <div className="p-4 bg-blue-500 min-h-screen">
      <GenericButton type="button" text="Hello" className="p-5" />
      <p className="text-white font-bold text-2xl">Hello Test</p>
      <p className="font-medium text-yellow-300">
        Testing font-medium directly
      </p>
    </div>
  );
};

export default TestPage;
