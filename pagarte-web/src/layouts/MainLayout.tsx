/**
 * MainLayout is the primary container for the authenticated user experience.
 * It provides the persistent UI elements like the top navigation bar and the
 * sliding sidebar menu.
 *
 * The <Outlet /> component is the placeholder where React Router will render the
 * child route's component (e.g., HomePage, SettingsPage).
 */

import React, { useState } from "react";
import { Outlet } from "react-router-dom";
import TopBar from "./TopBar";
import SidebarDrawer from "./SidebarDrawer";

interface MainLayoutProps {
  isLoggedIn: boolean;
}

const MainLayout: React.FC<MainLayoutProps> = ({ isLoggedIn }) => {
  // State to manage whether the mobile-style sidebar is open or closed
  const [drawerOpen, setDrawerOpen] = useState(false);

  return (
    <div className="flex flex-col min-h-screen bg-gray-100">
      {/* The persistent top navigation bar */}
      <TopBar onMenuClick={() => setDrawerOpen(true)} />

      {/* The sliding sidebar drawer, which is controlled by this layout */}
      <SidebarDrawer
        isOpen={drawerOpen}
        onClose={() => setDrawerOpen(false)}
        isLoggedIn={isLoggedIn}
      />

      {/* 
        This is the main content area. The <Outlet> from react-router-dom
        will render the specific page component for the current URL.
        For example, if you are at '/home', it renders <HomePage /> here.
      */}
      <main className="flex-1">
        <Outlet />
      </main>
    </div>
  );
};

export default MainLayout;
