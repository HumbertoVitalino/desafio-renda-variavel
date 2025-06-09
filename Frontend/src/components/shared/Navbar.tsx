import { AppBar, Toolbar, Button, Typography, Box } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';

export default function Navbar() {
  const { user, signOut } = useAuth();
  const navigate = useNavigate();

  return (
    <AppBar position="static" elevation={0}>
      <Toolbar>
        <Typography
          variant="h6"
          component={Link}
          to={user ? "/dashboard" : "/"}
          sx={{
            flexGrow: 1,
            textDecoration: 'none',
            color: 'text.primary',
            fontWeight: 700,
            letterSpacing: 1,
          }}
        >
          Camarão Investimentos
        </Typography>
        {user ? (
          <Box>
            <Button component={Link} to="/dashboard" sx={{ mx: 1 }}>
              Dashboard
            </Button>
            <Button onClick={signOut} sx={{ mx: 1 }}>
              Sair
            </Button>
          </Box>
        ) : (
          <Box>
            <Button component={Link} to="/login" sx={{ mx: 1 }}>
              Login
            </Button>
            <Button component={Link} to="/register" sx={{ mx: 1 }}>
              Criar Conta
            </Button>
          </Box>
        )}
      </Toolbar>
    </AppBar>
  );
}