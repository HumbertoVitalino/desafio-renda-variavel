import { useState } from 'react';
import { Box, Button, Container, MenuItem, TextField, Typography, Paper, IconButton, Grow, Alert } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { InvestorProfile, NewUserRequest } from '../types/api';
import { registerUser } from '../services/api';
import PersonAddAlt1RoundedIcon from '@mui/icons-material/PersonAddAlt1Rounded';
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
      setError('As senhas não conferem.');
      return;
    }
    setLoading(true);
    try {
      await registerUser(form);
      navigate('/login');
    } catch {
      setError('Erro ao registrar usuário. Tente outro e-mail.');
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
        py: 4
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
              position: 'relative',
            }}
          >
            <IconButton
              onClick={() => navigate('/')}
              sx={{ position: 'absolute', top: 16, left: 16, color: 'text.secondary' }}
              aria-label="Voltar para início"
            >
              <ArrowBackIosNewRoundedIcon />
            </IconButton>
            <PersonAddAlt1RoundedIcon sx={{ fontSize: 48, color: 'primary.main', mb: 2, mt: 4 }} />
            <Typography variant="h5" sx={{ fontWeight: 700, mb: 0.5 }}>
              Criar Conta
            </Typography>
            <Typography variant="body2" sx={{ color: 'text.secondary', mb: 2, textAlign: 'center' }}>
              Cadastre-se para começar a gerenciar seus investimentos.
            </Typography>
            <Box component="form" onSubmit={handleSubmit} sx={{ width: '100%', mt: 1 }}>
              <TextField fullWidth label="Nome Completo" name="name" margin="normal" value={form.name} onChange={handleChange} required autoFocus InputProps={{ sx: { borderRadius: 2 } }} />
              <TextField fullWidth label="Email" name="email" type="email" margin="normal" value={form.email} onChange={handleChange} required InputProps={{ sx: { borderRadius: 2 } }} />
              <TextField fullWidth label="Senha" name="password" type="password" margin="normal" value={form.password} onChange={handleChange} required InputProps={{ sx: { borderRadius: 2 } }} />
              <TextField fullWidth label="Confirme a Senha" name="confirmation" type="password" margin="normal" value={form.confirmation} onChange={handleChange} required InputProps={{ sx: { borderRadius: 2 } }} />
              <TextField select fullWidth label="Perfil de Investidor" name="profile" margin="normal" value={form.profile} onChange={e => setForm({ ...form, profile: Number(e.target.value) })} required InputProps={{ sx: { borderRadius: 2 } }}>
                <MenuItem value={InvestorProfile.Conservative}>Conservador</MenuItem>
                <MenuItem value={InvestorProfile.Moderate}>Moderado</MenuItem>
                <MenuItem value={InvestorProfile.Bold}>Arrojado</MenuItem>
              </TextField>
              {error && (
                <Alert severity="error" sx={{ mt: 2, borderRadius: 2, width: '100%' }}>
                  {error}
                </Alert>
              )}
              <Button type="submit" variant="contained" fullWidth size="large" sx={{ mt: 3, mb: 2, fontWeight: 700, borderRadius: 2, letterSpacing: 1, py: 1.5 }} disabled={loading}>
                {loading ? 'Registrando...' : 'Registrar'}
              </Button>
            </Box>
          </Paper>
        </Container>
      </Grow>
    </Box>
  );
}