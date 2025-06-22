import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import App from "./App";
import "./styles/index.css";
// API Client Configuration
import { OpenAPI as AuthOpenAPI } from "./api/generated/";
import { OpenAPI as ProductsOpenAPI } from "./api/generated/";

// Configure Base URLs from environment variables
AuthOpenAPI.BASE = import.meta.env.VITE_AUTH_API_URL;
ProductsOpenAPI.BASE = import.meta.env.VITE_PRODUCTS_API_URL;

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </React.StrictMode>
);
