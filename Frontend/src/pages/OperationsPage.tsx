import { useEffect, useState } from 'react';
import { Box, Button, Stack, TextField, Typography, MenuItem, Paper, Card, CardContent, Container } from '@mui/material';
import { createOperation, getAllPositions } from '../services/api';
import { OperationType, NewOperationRequest, PositionDto } from '../types/api';

type Props = { noContainer?: boolean };

export default function OperationsPage({ noContainer = false }: Props) {
  const [positions, setPositions] = useState<PositionDto[]>([]);
  const [form, setForm] = useState<NewOperationRequest>({
    tickerSymbol: '',
    quantity: 0,
    type: OperationType.Buy,
  });
  const [loading, setLoading] = useState(false);

  const fetchPositions = () => {
    getAllPositions().then(res => {
      if (res.data.result) setPositions(res.data.result);
    });
  };

  useEffect(() => {
    fetchPositions();
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({
      ...prev,
      [name]: name === 'quantity' ? Number(value) : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      await createOperation(form);
      fetchPositions();
      setForm({ tickerSymbol: '', quantity: 0, type: OperationType.Buy });
    } catch {
      alert('Erro ao registrar operação.');
    } finally {
      setLoading(false);
    }
  };

  const content = (
    <>
      <Paper
        elevation={6}
        sx={{
          p: 4,
          borderRadius: 4,
          background: 'rgba(30,30,30,0.97)',
          boxShadow: '0 8px 32px 0 rgba(0,0,0,0.25)',
          mb: 4,
        }}
      >
        <Typography variant="h5" fontWeight={700} mb={2}>
          Nova Operação
        </Typography>
        <Box component="form" onSubmit={handleSubmit}>
          <Stack
            direction={{ xs: 'column', sm: 'row' }}
            spacing={2}
            alignItems="flex-start"
          >
            <TextField
              label="Ticker"
              name="tickerSymbol"
              value={form.tickerSymbol}
              onChange={handleChange}
              fullWidth
              required
              sx={{ minWidth: 150, flex: 1 }}
              inputProps={{ style: { textTransform: 'uppercase' } }}
            />
            <TextField
              label="Quantidade"
              name="quantity"
              type="number"
              value={form.quantity}
              onChange={handleChange}
              fullWidth
              required
              sx={{ minWidth: 150, flex: 1 }}
              inputProps={{ min: 1 }}
            />
            <TextField
              select
              label="Tipo"
              name="type"
              value={form.type}
              onChange={handleChange}
              fullWidth
              required
              sx={{ minWidth: 150, flex: 1 }}
            >
              <MenuItem value={OperationType.Buy}>Compra</MenuItem>
              <MenuItem value={OperationType.Sell}>Venda</MenuItem>
            </TextField>
            <Button
              type="submit"
              variant="contained"
              color="primary"
              sx={{ minWidth: 140, fontWeight: 700, borderRadius: 2, height: 56 }}
              disabled={loading}
            >
              {loading ? 'Registrando...' : 'Registrar'}
            </Button>
          </Stack>
        </Box>
      </Paper>

      <Typography variant="h5" fontWeight={700} mb={2}>
        Minhas Posições
      </Typography>
      <Stack
        direction="row"
        flexWrap="wrap"
        spacing={2}
        justifyContent="flex-start"
      >
        {positions.length === 0 && (
          <Typography color="text.secondary" sx={{ ml: 1 }}>
            Nenhuma posição encontrada.
          </Typography>
        )}
        {positions.map(pos => (
          <Card
            key={pos.tickerSymbol}
            sx={{
              flex: '1 1 300px',
              minWidth: 300,
              background: 'rgba(30,30,30,0.97)',
              borderRadius: 3,
              mb: 2,
              boxShadow: '0 4px 16px 0 rgba(0,0,0,0.10)',
            }}
          >
            <CardContent>
              <Typography variant="h6" fontWeight={700} color="primary" gutterBottom>
                {pos.assetName} ({pos.tickerSymbol})
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Quantidade: <b>{pos.quantity}</b>
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Preço Médio: <b>R$ {pos.averagePrice.toFixed(2)}</b>
              </Typography>
              <Typography variant="body2" color={pos.currentProfitAndLoss >= 0 ? 'success.main' : 'error.main'}>
                P/L Atual: <b>R$ {pos.currentProfitAndLoss.toFixed(2)}</b>
              </Typography>
            </CardContent>
          </Card>
        ))}
      </Stack>
    </>
  );

  if (noContainer) return content;
  return <Container sx={{ mt: 4, mb: 4 }}>{content}</Container>;
}