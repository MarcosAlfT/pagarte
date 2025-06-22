import { useState } from "react";
import AppRouter from "./router/AppRouter";

function App() {
  // --- STATE MANAGEMENT ---
  const [isAuthenticated, setIsAuthenticated] = useState(
    !!localStorage.getItem("token")
  );

  // --- HANDLER FUNCTIONS ---
  const handleLogin = () => {
    setIsAuthenticated(true);
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    setIsAuthenticated(false);
  };

  return (
    <AppRouter
      isLoggedIn={isAuthenticated}
      onLogin={handleLogin}
      onLogout={handleLogout}
    />
  );
}

export default App;
