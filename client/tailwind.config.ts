import type { Config } from 'tailwindcss'

export default {
  theme: {
    extend: {
      colors: {
        // Deep mauve accent palette
        mauve: {
          50: '#faf8fc',
          100: '#f4f0f9',
          200: '#e9e0f3',
          300: '#d9cbe6',
          400: '#c4b3d4',
          500: '#a891c2',
          600: '#8b6f9e', // Primary accent - deep mauve
          700: '#6d5584',
          800: '#5a456e',
          900: '#4a3759',
          950: '#3a2b48',
        },
        // Dark base colors
        slate: {
          50: '#f8fafc',
          100: '#f1f5f9',
          200: '#e2e8f0',
          300: '#cbd5e1',
          400: '#94a3b8',
          500: '#64748b',
          600: '#475569',
          700: '#334155',
          800: '#1e293b',
          900: '#0f172a',
          950: '#020617',
        },
        // Muted warm tones for states
        'muted-amber': {
          50: '#fffbf0',
          100: '#fef3e2',
          200: '#fce8c9',
          300: '#fbd399',
          400: '#f8b856',
          500: '#e8a033', // Soft warning
          600: '#c88b28',
          700: '#a67a22',
          800: '#8a6621',
          900: '#6f5319',
          950: '#3f2d0a',
        },
        'muted-rust': {
          50: '#fef3f2',
          100: '#fee4e2',
          200: '#feccca',
          300: '#fdaca6',
          400: '#f97970',
          500: '#e85d52', // Soft error
          600: '#c94a3e',
          700: '#a8403a',
          800: '#8a3a36',
          900: '#733132',
          950: '#441a17',
        },
        'muted-emerald': {
          50: '#f0fdf6',
          100: '#dcfce7',
          200: '#bbf7d0',
          300: '#86efac',
          400: '#4ade80',
          500: '#22c55e',
          600: '#16a34a',
          700: '#15803d',
          800: '#166534',
          900: '#145231',
          950: '#052e16',
        },
        // Neutral dark for backgrounds and borders
        'dark-bg': '#0a0e27',
        'dark-surface': '#0f1630',
        'dark-border': '#1a2444',
      },
    },
  },
  plugins: [],
} satisfies Config
