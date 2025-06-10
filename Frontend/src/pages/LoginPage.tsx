import React, { useState } from 'react';
import { Box, TextField, Button, Typography, Container, CircularProgress, Paper, IconButton, Grow, Alert } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import { LoginUserRequest } from '../types/api';
import LockPersonRoundedIcon from '@mui/icons-material/LockPersonRounded';
import ArrowBackIosNewRoundedIcon from '@mui/icons-material/ArrowBackIosNewRounded';

const LoginPage = () => {
  const { signIn } = useAuth();
  const navigate = useNavigate();

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [formData, setFormData] = useState<LoginUserRequest>({
    email: '',
    password: '',
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prevState => ({ ...prevState, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    try {
      await signIn(formData);
      navigate('/dashboard');
    } catch (err: any) {
      setError(err.message || 'Falha no login. Verifique suas credenciais.');
    } finally {
      setLoading(false);
    }
  };

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
      <Grow in={true} timeout={700}>
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
              position: 'relative'
            }}
          >
            <IconButton
              onClick={() => navigate('/')}
              sx={{
                position: 'absolute',
                top: 16,
                left: 16,
                color: 'text.secondary',
              }}
              aria-label="Voltar para início"
            >
              <ArrowBackIosNewRoundedIcon />
            </IconButton>
            <LockPersonRoundedIcon sx={{ fontSize: 48, color: 'primary.main', mb: 2, mt: 4 }} />
            <Typography component="h1" variant="h5" sx={{ fontWeight: 700, mb: 0.5 }}>
              Login
            </Typography>
            <Typography variant="body2" sx={{ color: 'text.secondary', mb: 2, textAlign: 'center' }}>
              Acesse sua conta para acompanhar seus investimentos.
            </Typography>
            <Box component="form" onSubmit={handleSubmit} sx={{ width: '100%', mt: 1 }}>
              <TextField
                margin="normal"
                required
                fullWidth
                id="email"
                label="Endereço de E-mail"
                name="email"
                autoComplete="email"
                autoFocus
                value={formData.email}
                onChange={handleChange}
                disabled={loading}
                InputProps={{ sx: { borderRadius: 2 } }}
              />
              <TextField
                margin="normal"
                required
                fullWidth
                name="password"
                label="Senha"
                type="password"
                id="password"
                autoComplete="current-password"
                value={formData.password}
                onChange={handleChange}
                disabled={loading}
                InputProps={{ sx: { borderRadius: 2 } }}
              />
              {error && (
                <Alert severity="error" sx={{ mt: 2, borderRadius: 2, width: '100%' }}>
                  {error}
                </Alert>
              )}
              <Button
                type="submit"
                fullWidth
                variant="contained"
                size="large"
                sx={{ mt: 3, mb: 2, fontWeight: 700, borderRadius: 2, letterSpacing: 1, py: 1.5 }}
                disabled={loading}
              >
                {loading ? <CircularProgress size={24} color="inherit" /> : 'Entrar'}
              </Button>
              <Button
                fullWidth
                variant="text"
                color="primary"
                onClick={() => navigate('/register')}
                disabled={loading}
                sx={{ fontWeight: 600, borderRadius: 2 }}
              >
                Não tem conta? Crie uma agora
              </Button>
            </Box>
          </Paper>
        </Container>
      </Grow>
    </Box>
  );
};

export default LoginPage;