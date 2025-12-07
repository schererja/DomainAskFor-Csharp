/***********************************
 * Tailwind configuration for Blazor
 ***********************************/
module.exports = {
  content: [
    "./wwwroot/index.html",
    "./**/*.razor",
    "./**/*.cshtml",
    "./**/*.html"
  ],
  theme: {
    extend: {
      fontFamily: {
        sans: ["Inter", "system-ui", "-apple-system", "BlinkMacSystemFont", "Segoe UI", "sans-serif"],
      },
      colors: {
        brand: {
          50: "#eef2ff",
          100: "#e0e7ff",
          200: "#c7d2fe",
          300: "#a5b4fc",
          400: "#818cf8",
          500: "#667eea",
          600: "#5a67d8",
          700: "#4c51bf",
          800: "#434190",
          900: "#3c366b"
        }
      }
    },
  },
  plugins: [],
};
