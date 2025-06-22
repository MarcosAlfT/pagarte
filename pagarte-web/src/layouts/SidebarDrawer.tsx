import React from 'react';
import { Link } from 'react-router-dom';
import {
  FaHome,
  FaEnvelope,
  FaMoneyBillWave,
  FaCog,
  FaIcons,
  FaSignInAlt,
  FaWallet,
  FaSignOutAlt,
} from 'react-icons/fa';

type SidebarDrawerProps = {
  isOpen: boolean;
  onClose: () => void;
  isLoggedIn: boolean;
};

const sidebarItems = [
  { label: 'Home', path: '/home', icon: <FaHome /> },
  { label: 'Make a Payment', path: '/makepayment', icon: <FaMoneyBillWave />, whenLoggedIn: true },
  { label: 'Add Payment Method', path: '/addpaymentmethod', icon: <FaWallet />, whenLoggedIn: true },
  { label: 'Settings', path: '/settings', icon: <FaCog />, whenLoggedIn: true },
  { label: 'Messages', path: '/messages', icon: <FaEnvelope />, whenLoggedIn: true },
  { label: 'Login', path: '/login', icon: <FaSignInAlt />, whenLoggedOut: true },
  { label: 'Logout', path: '/logout', icon: <FaSignOutAlt />, whenLoggedIn: true },
  { label: 'Icons Refence', path: '/icon-reference', icon: <FaIcons /> },
];

const SidebarDrawer: React.FC<SidebarDrawerProps> = ({ isOpen, onClose, isLoggedIn }) => {
  const visibleItems = sidebarItems.filter(
    (item) => (isLoggedIn && !item.whenLoggedOut) || (!isLoggedIn && !item.whenLoggedIn),
  );

  return (
    <>
      {/* Overlay */}
      <div
        className={`fixed inset-0 bg-secondary bg-opacity-40 z-40 transition-opacity ${
          isOpen ? 'opacity-100 pointer-events-auto' : 'opacity-0 pointer-events-none'
        }`}
        onClick={onClose}
        aria-hidden={!isOpen}
      />
      {/* Drawer */}
      <aside
        className={`
          fixed top-14 left-0 h-full w-64 bg-secondary z-50 shadow-xl transition-transform
          ${isOpen ? 'translate-x-0' : '-translate-x-full'}
          flex flex-col
        `}
        tabIndex={-1}
        aria-label="Sidebar Menu"
      >
        {/* <div className="flex justify-end p-4">
          <button onClick={onClose} className="text-2xl text-primary hover:text-error">
            &times;
          </button>
        </div> */}
        <nav className="flex-1 text-primary">
          {visibleItems.map((item) => (
            <Link
              key={item.path}
              to={item.path}
              onClick={onClose}
              className="flex items-center h-12 px-4 py-2 mb-1 text-sm rounded hover:bg-comp_hover hover:text-secondary transition-colors"
            >
              <span className="text-lg">{item.icon}</span>
              <span className="ml-4">{item.label}</span>
            </Link>
          ))}
        </nav>
      </aside>
    </>
  );
};

export default SidebarDrawer;
