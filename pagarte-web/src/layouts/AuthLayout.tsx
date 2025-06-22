import React, { useState } from "react";
import ChatbotButton from "../components/controls/ChatBotButton";
import ChatbotPopover from "../components/controls/ChatbotPopover";

interface AuthLayoutProps {
  children: React.ReactNode;
}

const AuthLayout: React.FC<AuthLayoutProps> = ({ children }) => {
  // State for managing the visibility of the chatbot popover
  const [chatbotOpen, setChatbotOpen] = useState(false);

  return (
    // This is the main container for the entire page
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-r from-primary/10 via-secondary relative p-4">
      <ChatbotButton onClick={() => setChatbotOpen(true)} />
      <ChatbotPopover
        open={chatbotOpen}
        onClose={() => setChatbotOpen(false)}
      />
      <div className="w-full flex items-center justify-center">{children}</div>
    </div>
  );
};

export default AuthLayout;
