const { animation } = require("@angular/animations");

module.exports = {
  purge: {
    enabled: process.env.NODE_ENV === 'production',
    content: [
      './src/**/*.{html,ts}',
    ]
  },
  darkMode: false, // or 'media' or 'class'
  theme: {
    extend: {
    },
  },
  variants: {
    extend: {
      display: ['group-hover']
    },
  },
  plugins: [],
};
