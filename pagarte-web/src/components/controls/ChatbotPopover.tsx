import React from 'react';

export interface ChatbotPopoverProps {
  open: boolean;
  onClose: () => void;
}

const ChatbotPopover: React.FC<ChatbotPopoverProps> = ({ open, onClose }) =>
  open ? (
    <div className="fixed bottom-24 right-6 w-72 bg-white rounded-xl shadow-xl border border-border_default z-50 p-4 animate-fade-in">
      <div className="flex items-center justify-between mb-2">
        <span className="font-semibold text-primary">Chatbot</span>
        <button onClick={onClose} className="text-gray-400 hover:text-error text-xl" type="button">
          &times;
        </button>
      </div>
      <div className="text-sm text-primary">
        Hi! Need help logging in? <br />
        (This is a demo chatbot box.)
      </div>
    </div>
  ) : null;

export default ChatbotPopover;
