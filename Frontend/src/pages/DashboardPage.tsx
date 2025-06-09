import { useEffect, useState } from 'react';
import { Box, Card, CardContent, Stack, Typography, Button, Divider, Container } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import {
  getAllPositions,
  getTop10ByPositionValue,
  getLatestQuote,
  getTotalBrokerageRevenue,
} from '../services/api';
import { PositionDto, QuoteDto, TopClientsDto } from '../types/api';

type Props = { noContainer?: boolean };

export default function DashboardPage({ noContainer = false }: Props) {
  const [positions, setPositions] = useState<PositionDto[]>([]);
  const [topClients, setTopClients] = useState<TopClientsDto[]>([]);
  const [quote, setQuote] = useState<QuoteDto | null>(null);
  const [revenue, setRevenue] = useState<number>(0);
  const navigate = useNavigate();

  useEffect(() => {
    getAllPositions().then(res => res.data.result && setPositions(res.data.result));
    getTop10ByPositionValue().then(res => res.data.result && setTopClients(res.data.result));
    getTotalBrokerageRevenue().then(res => res.data.result && setRevenue(res.data.result.totalRevenue));
    getLatestQuote('PETR4').then(res => res.data.result && setQuote(res.data.result));
  }, []);

  const totalInvested = positions.reduce((sum, p) => sum + p.quantity * p.averagePrice, 0);
  const totalPnL = positions.reduce((sum, p) => sum + p.currentProfitAndLoss, 0);

  const content = (
    <>
      <Box display="flex" alignItems="center" justifyContent="space-between" mb={3}>
        <Typography variant="h4" fontWeight={700}>
          Visão Geral
        </Typography>
      </Box>

      <Stack
        direction={{ xs: 'column', sm: 'row' }}
        spacing={2}
        flexWrap="wrap"
        justifyContent="space-between"
        mb={4}
      >
        <Card sx={{ flex: '1 1 250px', minWidth: 250, background: 'rgba(30,30,30,0.97)', borderRadius: 3 }}>
          <CardContent>
            <Typography variant="subtitle2" color="text.secondary" gutterBottom>
              Total Investido
            </Typography>
            <Typography variant="h5" color="primary" fontWeight={700}>
              R$ {totalInvested.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
            </Typography>
          </CardContent>
        </Card>

        <Card sx={{ flex: '1 1 250px', minWidth: 250, background: 'rgba(30,30,30,0.97)', borderRadius: 3 }}>
          <CardContent>
            <Typography variant="subtitle2" color="text.secondary" gutterBottom>
              Lucro/Prejuízo
            </Typography>
            <Typography
              variant="h5"
              fontWeight={700}
              color={totalPnL >= 0 ? 'success.main' : 'error.main'}
            >
              R$ {totalPnL.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
            </Typography>
          </CardContent>
        </Card>

        <Card sx={{ flex: '1 1 250px', minWidth: 250, background: 'rgba(30,30,30,0.97)', borderRadius: 3 }}>
          <CardContent>
            <Typography variant="subtitle2" color="text.secondary" gutterBottom>
              Receita da Corretora
            </Typography>
            <Typography variant="h5" color="primary" fontWeight={700}>
              R$ {revenue.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
            </Typography>
          </CardContent>
        </Card>
      </Stack>

      <Divider sx={{ my: 4 }} />

      <Box>
        <Typography variant="h5" mb={2} fontWeight={700}>
          Top 10 Investidores
        </Typography>
        <Stack direction="row" spacing={2} flexWrap="wrap" justifyContent="flex-start">
          {topClients.length === 0 && (
            <Typography color="text.secondary" sx={{ ml: 1 }}>
              Nenhum investidor encontrado.
            </Typography>
          )}
          {topClients.map(client => (
            <Card
              key={client.userId}
              sx={{
                flex: '1 1 300px',
                minWidth: 300,
                background: 'rgba(30,30,30,0.97)',
                borderRadius: 3,
                mb: 2,
              }}
            >
              <CardContent>
                <Typography fontWeight={600}>{client.userName}</Typography>
                <Typography variant="body2" color="primary">
                  R$ {client.totalPositionValue.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                </Typography>
              </CardContent>
            </Card>
          ))}
        </Stack>
      </Box>
    </>
  );

  if (noContainer) return content;
  return <Container sx={{ mt: 4, mb: 4 }}>{content}</Container>;
}