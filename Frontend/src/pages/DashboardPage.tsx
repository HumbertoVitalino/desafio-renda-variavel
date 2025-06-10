import { useEffect, useState, ReactNode } from 'react';
import { Box, Card, CardContent, Stack, Typography, Divider, Container, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material';
import AccountBalanceWalletIcon from '@mui/icons-material/AccountBalanceWallet';
import TrendingUpIcon from '@mui/icons-material/TrendingUp';
import TrendingDownIcon from '@mui/icons-material/TrendingDown';
import MonetizationOnIcon from '@mui/icons-material/MonetizationOn';
import { getAllPositions, getTop10ByPositionValue, getTotalBrokerageRevenue } from '../services/api';
import { PositionDto, TopClientsDto } from '../types/api';

type SummaryCardProps = {
  title: string;
  value: string;
  icon: ReactNode;
  color?: string;
}

const SummaryCard = ({ title, value, icon, color = 'primary.main' }: SummaryCardProps) => (
  <Card sx={{
    flex: '1 1 250px',
    minWidth: 250,
    background: 'rgba(40,40,40,0.8)',
    backdropFilter: 'blur(5px)',
    borderRadius: 3,
    transition: 'transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out',
    '&:hover': {
      transform: 'scale(1.03)',
      boxShadow: '0 8px 24px 0 rgba(0,0,0,0.20)',
    }
  }}>
    <CardContent>
      <Stack direction="row" spacing={2} alignItems="center">
        <Box sx={{ p: 1.5, background: color, borderRadius: '50%', display: 'flex', color: 'white' }}>
          {icon}
        </Box>
        <Box>
          <Typography variant="subtitle2" color="text.secondary" gutterBottom>
            {title}
          </Typography>
          <Typography variant="h5" color={color} fontWeight={700}>
            {value}
          </Typography>
        </Box>
      </Stack>
    </CardContent>
  </Card>
);

type Props = { noContainer?: boolean };

export default function DashboardPage({ noContainer = false }: Props) {
  const [positions, setPositions] = useState<PositionDto[]>([]);
  const [topClients, setTopClients] = useState<TopClientsDto[]>([]);
  const [revenue, setRevenue] = useState<number>(0);

  useEffect(() => {
    getAllPositions().then(res => res.data.result && setPositions(res.data.result));
    getTop10ByPositionValue().then(res => res.data.result && setTopClients(res.data.result));
    getTotalBrokerageRevenue().then(res => res.data.result && setRevenue(res.data.result.totalRevenue));
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

      <Stack direction={{ xs: 'column', sm: 'row' }} spacing={3} flexWrap="wrap" mb={4}>
        <SummaryCard
          title="Total Investido"
          value={`R$ ${totalInvested.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}`}
          icon={<AccountBalanceWalletIcon />}
        />
        <SummaryCard
          title="Lucro/Prejuízo Total"
          value={`R$ ${totalPnL.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}`}
          color={totalPnL >= 0 ? 'success.main' : 'error.main'}
          icon={totalPnL >= 0 ? <TrendingUpIcon /> : <TrendingDownIcon />}
        />
        <SummaryCard
          title="Receita da Corretora"
          value={`R$ ${revenue.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}`}
          icon={<MonetizationOnIcon />}
        />
      </Stack>

      <Divider sx={{ my: 4 }} />

      <Box>
        <Typography variant="h5" mb={2} fontWeight={700}>
          Top 10 Investidores
        </Typography>
        <Paper elevation={6} sx={{ background: 'rgba(30,30,30,0.9)', backdropFilter: 'blur(5px)', borderRadius: 4 }}>
          <TableContainer>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Posição</TableCell>
                  <TableCell>Nome do Investidor</TableCell>
                  <TableCell align="right">Valor Total em Posição (R$)</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {topClients.map((client, index) => (
                  <TableRow key={client.userId} hover sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                    <TableCell sx={{ fontWeight: 'bold' }}>{index + 1}º</TableCell>
                    <TableCell>{client.userName}</TableCell>
                    <TableCell align="right" sx={{ fontWeight: 'bold', color: 'primary.main' }}>
                      {client.totalPositionValue.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Paper>
      </Box>
    </>
  );

  if (noContainer) return content;
  return <Container sx={{ mt: 4, mb: 4 }}>{content}</Container>;
}