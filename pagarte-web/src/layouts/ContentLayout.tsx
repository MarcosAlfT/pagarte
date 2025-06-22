import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import ChatbotButton from "../components/controls/ChatBotButton";
import ChatbotPopover from "../components/controls/ChatbotPopover";
import GenericButton from "../components/controls/GenericButton";

interface ContentLayoutProps {
  title: string;
  children: React.ReactNode;
  icon?: React.ReactNode;
}

const ContentLayout: React.FC<ContentLayoutProps> = ({
  title,
  children,
  icon,
}) => {
  const [chatbotOpen, setChatbotOpen] = useState(false);
  const navigate = useNavigate();

  return (
    <div className="relative flex-1 flex flex-col bg-secondary overflow-x-hidden">
      {/* Centered Title */}
      <div className="w-full flex items-center justify-center mb-14">
        <div className="flex items-center gap-3">
          {icon && (
            <span className="text-3xl text-primary flex items-center">
              {icon}
            </span>
          )}
          <h1 className="text-3xl font-bold text-primary text-center">
            {title}
          </h1>
        </div>
      </div>

      {/* Scrollable Content */}
      <section className="flex-1 flex items-center justify-center px-4 md:px-8 pb-24">
        <div className="w-full">{children}</div>
      </section>
      {/* Return Home button fixed bottom-left */}
      <div className="fixed left-6 bottom-6 z-50">
        <GenericButton
          text="Return Home"
          variant="outline"
          className="px-4 py-2 shadow-lg"
          onClick={() => navigate("/home")}
        />
      </div>
      <ChatbotButton onClick={() => setChatbotOpen(true)} />
      <ChatbotPopover
        open={chatbotOpen}
        onClose={() => setChatbotOpen(false)}
      />
    </div>
  );
};

export default ContentLayout;
