import { createTheme } from '@mui/material/styles';

const theme = createTheme({
  palette: {
    mode: 'dark',
    primary: {
      main: '#FF6600',
      light: '#FF881A',
      dark: '#CC5200',
      contrastText: '#fff',
    },
    secondary: {
      main: '#232323',
      light: '#444444',
      dark: '#101010',
      contrastText: '#fff',
    },
    background: {
      default: '#191919',
      paper: '#191919', // igual ao default para evitar faixas
    },
    text: {
      primary: '#fff',
      secondary: '#FF6600',
      disabled: 'rgba(255,255,255,0.5)',
    },
  },
  shape: {
    borderRadius: 12,
  },
  typography: {
    fontFamily: [
      '"Inter"',
      '-apple-system',
      'BlinkMacSystemFont',
      '"Segoe UI"',
      'Roboto',
      '"Helvetica Neue"',
      'Arial',
      'sans-serif',
    ].join(','),
    h1: { fontWeight: 700, fontSize: '2.5rem' },
    h2: { fontWeight: 600 },
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          textTransform: 'none',
          fontWeight: 600,
          borderRadius: 8,
        },
        containedPrimary: {
          '&:hover': {
            backgroundColor: '#FF881A',
          },
        },
      },
    },
    MuiCard: {
      styleOverrides: {
        root: {
          border: '1px solid rgba(255, 102, 0, 0.15)',
          background: 'linear-gradient(145deg, #232323 0%, #191919 100%)',
          boxShadow: '0 4px 24px 0 rgba(0,0,0,0.15)',
          borderRadius: 16,
        },
      },
    },
    MuiContainer: {
      styleOverrides: {
        root: {
          background: 'transparent',
        },
      },
    },
    MuiTextField: {
      styleOverrides: {
        root: {
          background: '#232323',
          borderRadius: 8,
        },
      },
    },
    MuiAppBar: {
      styleOverrides: {
        root: {
          background: 'linear-gradient(to right, #232323, #FF6600)',
          boxShadow: '0 2px 10px rgba(0, 0, 0, 0.3)',
        },
      },
    },
  },
});

export default theme;