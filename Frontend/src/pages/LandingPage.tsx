import { Box, Button, Typography, Container, Paper, Grow } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import logo from '../assets/logo.png';
import LoginIcon from '@mui/icons-material/Login';
import PersonAddIcon from '@mui/icons-material/PersonAdd';

export default function LandingPage() {
  const navigate = useNavigate();

  return (
    <Box
      display="flex"
      alignItems="center"
      justifyContent="center"
      flex={1}
      minHeight="100%"
      sx={{
        background: 'linear-gradient(135deg, #191919 50%, #FF6600 100%)',
        backgroundSize: '200% 200%',
        animation: 'gradient-animation 15s ease infinite',
      }}
    >
      <Grow in={true} style={{ transformOrigin: '0 0 0' }} timeout={1000}>
        <Container maxWidth="xs">
          <Paper
            elevation={12}
            sx={{
              p: 4,
              borderRadius: 4,
              background: 'rgba(30, 30, 30, 0.9)',
              backdropFilter: 'blur(10px)',
              boxShadow: '0 8px 32px 0 rgba(0,0,0,0.37)',
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              gap: 2,
            }}
          >
            <img
              src={logo}
              alt="Logo CamarãoInvestimentos"
              style={{ width: 72, height: 72, marginBottom: 16, marginTop: 8 }}
            />
            <Typography component="h1" variant="h5" sx={{ fontWeight: 700, mb: 0.5 }}>
              Camarão Investimentos
            </Typography>
            <Typography variant="body2" sx={{ color: 'text.secondary', mb: 2, textAlign: 'center' }}>
              Gerencie seus investimentos, acompanhe suas posições e descubra insights de mercado.
            </Typography>
            <Box mt={2} width="100%" display="flex" flexDirection="column" gap={2}>
              <Button
                variant="contained"
                color="primary"
                size="large"
                fullWidth
                onClick={() => navigate('/login')}
                startIcon={<LoginIcon />}
                sx={{ fontWeight: 700, borderRadius: 2, letterSpacing: 1, py: 1.5 }}
              >
                Login
              </Button>
              <Button
                variant="outlined"
                color="primary"
                size="large"
                fullWidth
                onClick={() => navigate('/register')}
                startIcon={<PersonAddIcon />}
                sx={{ fontWeight: 700, borderRadius: 2, borderWidth: 2, letterSpacing: 1, py: 1.5 }}
              >
                Criar Conta
              </Button>
            </Box>
          </Paper>
        </Container>
      </Grow>
    </Box>
  );
}