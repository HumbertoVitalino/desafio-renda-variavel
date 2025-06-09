import { Box, Button, Typography, Container, Paper } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import logo from '../assets/logo.png';

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
        background: 'linear-gradient(135deg, #191919 60%, #FF6600 100%)',
        transition: 'background 0.5s',
      }}
    >
      <Container maxWidth="xs">
        <Paper
          elevation={6}
          sx={{
            p: 4,
            borderRadius: 4,
            background: 'rgba(30,30,30,0.97)',
            boxShadow: '0 8px 32px 0 rgba(0,0,0,0.25)',
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            gap: 2,
          }}
        >
          <img
            src={logo}
            alt="Logo camarão"
            style={{ width: 72, height: 72, marginBottom: 16, marginTop: 8 }}
          />
          <Typography component="h1" variant="h5" sx={{ fontWeight: 700, mb: 0.5 }}>
            Bem-vindo à InvestTrack
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
              sx={{
                fontWeight: 700,
                borderRadius: 2,
                boxShadow: '0 2px 8px 0 rgba(255,102,0,0.12)',
                letterSpacing: 1,
              }}
              onClick={() => navigate('/login')}
            >
              Login
            </Button>
            <Button
              variant="outlined"
              color="primary"
              size="large"
              fullWidth
              sx={{
                fontWeight: 700,
                borderRadius: 2,
                borderWidth: 2,
                letterSpacing: 1,
              }}
              onClick={() => navigate('/register')}
            >
              Criar Conta
            </Button>
          </Box>
        </Paper>
      </Container>
    </Box>
  );
}