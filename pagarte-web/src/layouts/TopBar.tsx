import React, { useState, useEffect, useRef } from "react";
import { Link } from "react-router-dom";
import {
  FaBars,
  FaBell,
  FaUserCircle,
  FaCog,
  FaSignOutAlt,
} from "react-icons/fa";
import GenericButton from "../components/controls/GenericButton";

interface TopBarProps {
  onMenuClick?: () => void;
  // In a real app, you might pass the user's name or a user object
  // For now, a simple greeting is great.
  greeting?: string;
}

const TopBar: React.FC<TopBarProps> = ({
  onMenuClick,
  greeting = "Welcome",
}) => {
  // State to manage the visibility of the user profile dropdown
  const [isUserMenuOpen, setIsUserMenuOpen] = useState(false);
  const userMenuRef = useRef<HTMLDivElement>(null);

  // --- Click-outside handler for the dropdown ---
  // This effect closes the dropdown if the user clicks anywhere else on the page.
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        userMenuRef.current &&
        !userMenuRef.current.contains(event.target as Node)
      ) {
        setIsUserMenuOpen(false);
      }
    };
    // Add event listener when the menu is open
    if (isUserMenuOpen) {
      document.addEventListener("mousedown", handleClickOutside);
    }
    // Cleanup: remove event listener when the component unmounts or the menu closes
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [isUserMenuOpen]);

  return (
    <header className="w-full flex items-center justify-between h-14 px-4 sm:px-6 bg-secondary text-primary shadow-md sticky top-0 z-30">
      {/* Left Side: Hamburger Menu & Greeting */}
      <div className="flex items-center space-x-4">
        <GenericButton
          onClick={onMenuClick}
          className="p-2"
          aria-label="Open menu"
          variant="secondary"
        >
          <FaBars className="text-xl" />
        </GenericButton>
        <span className="hidden sm:block text-base font-medium">
          {greeting}
        </span>
      </div>

      {/* Right Side: Notifications & User Menu */}
      <div className="flex items-center space-x-4">
        <GenericButton
          className="relative p-2"
          variant="secondary"
          aria-label="Notifications"
        >
          <FaBell className="text-xl" />
          {/* Example of an alert dot */}
          <span className="absolute top-1 right-1 block h-2 w-2 rounded-full bg-error ring-2 ring-secondary"></span>
        </GenericButton>

        {/* --- User Profile Dropdown --- */}
        <div className="relative" ref={userMenuRef}>
          <GenericButton
            onClick={() => setIsUserMenuOpen(!isUserMenuOpen)}
            className="p-1 rounded-full"
            variant="secondary"
          >
            {/* Replace this with a user's avatar image if you have one */}
            <FaUserCircle className="text-2xl" />
          </GenericButton>

          {/* The Dropdown Menu Panel */}
          {isUserMenuOpen && (
            <div className="absolute right-0 mt-2 w-48 bg-secondary rounded-md shadow-lg py-1 z-40 border border-border_default">
              <Link
                to="/settings"
                className="flex items-center w-full px-4 py-2 text-sm text-primary hover:bg-comp_hover hover:text-secondary"
                onClick={() => setIsUserMenuOpen(false)}
              >
                <FaCog className="mr-3" /> Settings
              </Link>
              <div className="border-t border-border_default my-1"></div>
              <Link
                to="/logout"
                className="flex items-center w-full px-4 py-2 text-sm text-primary hover:bg-comp_hover hover:text-secondary"
                onClick={() => setIsUserMenuOpen(false)}
              >
                <FaSignOutAlt className="mr-3" /> Logout
              </Link>
            </div>
          )}
        </div>
      </div>
    </header>
  );
};

export default TopBar;
