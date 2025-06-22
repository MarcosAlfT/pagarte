import React from 'react';

interface ChatbotButtonProps {
  onClick: () => void;
}

const ChatbotButton: React.FC<ChatbotButtonProps> = ({ onClick }) => (
  <button
    className="fixed bottom-6 right-6 bg-white rounded-full shadow-md p-3 z-50 hover:bg-comp_hover transition"
    title="Need help?"
    onClick={onClick}
    type="button"
  >
    <svg
      className="w-6 h-6 text-primary"
      fill="none"
      stroke="currentColor"
      strokeWidth={2}
      viewBox="0 0 24 24"
    >
      <path d="M12 19c4.418 0 8-2.686 8-6V7c0-3.314-3.582-6-8-6S4 3.686 4 7v6c0 3.314 3.582 6 8 6z" />
      <circle cx="8" cy="11" r="1" />
      <circle cx="12" cy="11" r="1" />
      <circle cx="16" cy="11" r="1" />
    </svg>
  </button>
);

export default ChatbotButton;
