/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        // --- Core Brand Colors ---
        primary: "#134e4a", // Your main color (Teal 900)
        secondary: "#fff", // Your secondary/background color (White)

        // --- Interactive State Colors ---
        comp_hover: "#14b8a6", // For hover states on primary elements (Teal 500)
        link_text: "#3b82f6", // Default link color
        link_hover: "#1e3a8a", // Link hover color

        // --- UI Feedback Colors ---
        error: "#f87171", // For error messages and destructive actions (Red 400)
        success: "#22c55e", // For success messages and icons (Green 500)

        // --- Neutral UI Colors ---
        border_default: "#d1d5db", // For standard borders on inputs, cards, etc. (Gray 300)
      },
      animation: {
        "fade-in": "fadeIn 0.5s ease-in-out",
      },
      keyframes: {
        fadeIn: {
          "0%": { opacity: "0" },
          "100%": { opacity: "1" },
        },
      },
    },
  },

  /**
   * The 'plugins' array is where you would add official or third-party
   * Tailwind plugins, such as `@tailwindcss/forms` or `@tailwindcss/typography`.
   */
  plugins: [],
};
