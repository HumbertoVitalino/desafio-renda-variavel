import { useState } from 'react';
import { Box, Button, Container, MenuItem, TextField, Typography, Paper, IconButton } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { InvestorProfile, NewUserRequest } from '../types/api';
import { registerUser } from '../services/api';
import LockPersonRoundedIcon from '@mui/icons-material/LockPersonRounded';
import ArrowBackIosNewRoundedIcon from '@mui/icons-material/ArrowBackIosNewRounded';

export default function RegisterPage() {
  const navigate = useNavigate();
  const [form, setForm] = useState<NewUserRequest>({
    name: '',
    email: '',
    password: '',
    confirmation: '',
    profile: InvestorProfile.Conservative,
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    if (form.password !== form.confirmation) {
      setError('Senhas não conferem.');
      return;
    }
    setLoading(true);
    try {
      await registerUser(form);
      navigate('/login');
    } catch {
      setError('Erro ao registrar usuário.');
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
          <IconButton
            onClick={() => navigate('/')}
            sx={{
              alignSelf: 'flex-start',
              color: 'primary.main',
              mb: 1,
              ml: -1,
            }}
            aria-label="Voltar para início"
          >
            <ArrowBackIosNewRoundedIcon />
          </IconButton>
          <LockPersonRoundedIcon sx={{ fontSize: 48, color: 'primary.main', mb: 1 }} />
          <Typography variant="h5" sx={{ fontWeight: 700, mb: 0.5 }}>
            Criar Conta
          </Typography>
          <Typography variant="body2" sx={{ color: 'text.secondary', mb: 2, textAlign: 'center' }}>
            Cadastre-se para acessar a plataforma InvestTrack e gerenciar seus investimentos.
          </Typography>
          <Box component="form" onSubmit={handleSubmit} sx={{ width: '100%', mt: 1 }}>
            <TextField
              fullWidth
              label="Nome"
              name="name"
              margin="normal"
              value={form.name}
              onChange={handleChange}
              required
              autoFocus
              InputProps={{ sx: { borderRadius: 2 } }}
            />
            <TextField
              fullWidth
              label="Email"
              name="email"
              margin="normal"
              value={form.email}
              onChange={handleChange}
              required
              type="email"
              InputProps={{ sx: { borderRadius: 2 } }}
            />
            <TextField
              fullWidth
              label="Senha"
              name="password"
              type="password"
              margin="normal"
              value={form.password}
              onChange={handleChange}
              required
              placeholder="Use letras, números e símbolos"
              InputProps={{ sx: { borderRadius: 2 } }}
            />
            <TextField
              fullWidth
              label="Confirme a Senha"
              name="confirmation"
              type="password"
              margin="normal"
              value={form.confirmation}
              onChange={handleChange}
              required
              InputProps={{ sx: { borderRadius: 2 } }}
            />
            <TextField
              select
              fullWidth
              label="Perfil de Investidor"
              name="profile"
              margin="normal"
              value={form.profile}
              onChange={e => setForm({ ...form, profile: Number(e.target.value) })}
              required
              InputProps={{ sx: { borderRadius: 2 } }}
            >
              <MenuItem value={InvestorProfile.Conservative}>Conservador</MenuItem>
              <MenuItem value={InvestorProfile.Moderate}>Moderado</MenuItem>
              <MenuItem value={InvestorProfile.Bold}>Arrojado</MenuItem>
            </TextField>
            {error && (
              <Typography color="error" variant="body2" sx={{ mt: 2, textAlign: 'center' }}>
                {error}
              </Typography>
            )}
            <Button
              type="submit"
              variant="contained"
              fullWidth
              sx={{
                mt: 3,
                fontWeight: 700,
                borderRadius: 2,
                boxShadow: '0 2px 8px 0 rgba(255,102,0,0.12)',
                letterSpacing: 1,
              }}
              disabled={loading}
              size="large"
            >
              {loading ? 'Registrando...' : 'Registrar'}
            </Button>
            <Button
              fullWidth
              variant="text"
              color="primary"
              onClick={() => navigate('/login')}
              disabled={loading}
              sx={{ fontWeight: 600, mt: 1, borderRadius: 2 }}
            >
              Já tem conta? Entrar
            </Button>
          </Box>
        </Paper>
      </Container>
    </Box>
  );
}