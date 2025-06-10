import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './contexts/AuthContext';
import { ThemeProvider, CssBaseline, Container } from '@mui/material';
import ProtectedRoute from './components/ProtectedRoute';
import Navbar from './components/shared/Navbar';
import Footer from './components/shared/Footer';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import LandingPage from './pages/LandingPage';
import HomeTabsPage from './pages/HomeTabsPage';
import theme from './styles/theme';

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <AuthProvider>
        <Router>
          <Navbar />
          <Routes>
            <Route path="/" element={<LandingPage />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
            <Route
              path="/dashboard"
              element={
                <ProtectedRoute>
                  <Container
                    component="main"
                    maxWidth={false}
                    disableGutters
                    sx={{
                      mt: 4,
                      mb: 4,
                      minHeight: 'calc(100vh - 120px)',
                      display: 'flex',
                      flexDirection: 'column',
                      background: 'transparent !important',
                      flex: 1,
                    }}
                  >
                    <HomeTabsPage />
                  </Container>
                </ProtectedRoute>
              }
            />
            <Route path="*" element={<Navigate to="/" />} />
          </Routes>
          <Footer />
        </Router>
      </AuthProvider>
    </ThemeProvider>
  );
}

export default App;