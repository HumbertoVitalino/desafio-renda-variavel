import { useEffect, useState, useRef } from 'react';
import {
  Box,
  Button,
  Stack,
  TextField,
  Typography,
  Paper,
  Card,
  CardContent,
  Container,
  CircularProgress,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Autocomplete,
  ToggleButtonGroup,
  ToggleButton,
  Skeleton,
  Tooltip,
} from '@mui/material';
import { createOperation, getAllPositions, getLatestQuote, getAllAssets } from '../services/api';
import { OperationType, NewOperationRequest, PositionDto, QuoteDto, AssetDto } from '../types/api';
import AddShoppingCartIcon from '@mui/icons-material/AddShoppingCart';
import SellIcon from '@mui/icons-material/Sell';
import ShowChartIcon from '@mui/icons-material/ShowChart';

type Props = { noContainer?: boolean };

export default function OperationsPage({ noContainer = false }: Props) {
  const [positions, setPositions] = useState<PositionDto[]>([]);
  const [form, setForm] = useState<NewOperationRequest>({
    tickerSymbol: '',
    quantity: 1,
    type: OperationType.Buy,
  });
  const [submitLoading, setSubmitLoading] = useState(false);
  const [allAssets, setAllAssets] = useState<AssetDto[]>([]);
  const [marketQuotes, setMarketQuotes] = useState<Record<string, QuoteDto>>({});
  const [marketLoading, setMarketLoading] = useState(true);

  const operationFormRef = useRef<HTMLDivElement>(null);

  const fetchInitialData = async () => {
    setMarketLoading(true);
    try {
      const [positionsRes, assetsRes] = await Promise.all([getAllPositions(), getAllAssets()]);
      if (positionsRes.data.result) setPositions(positionsRes.data.result);
      if (assetsRes.data.result) {
        const assets = assetsRes.data.result;
        setAllAssets(assets);
        const quotePromises = assets.map(asset => getLatestQuote(asset.tickerSymbol));
        const quoteResults = await Promise.allSettled(quotePromises);
        const quotesMap: Record<string, QuoteDto> = {};
        quoteResults.forEach((result, index) => {
          if (result.status === 'fulfilled' && result.value.data.result) {
            quotesMap[assets[index].tickerSymbol] = result.value.data.result;
          }
        });
        setMarketQuotes(quotesMap);
      }
    } catch (error) {
      console.error("Erro ao buscar dados iniciais:", error);
    } finally {
      setMarketLoading(false);
    }
  };

  useEffect(() => {
    fetchInitialData();
  }, []);

  const handleFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: name === 'quantity' ? Number(value) : value.toUpperCase() }));
  };

  const handleOperationTypeChange = (_: React.MouseEvent<HTMLElement>, newType: OperationType | null) => {
    if (newType !== null) {
      setForm(prev => ({ ...prev, type: newType }));
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitLoading(true);
    try {
      await createOperation(form);
      setForm({ tickerSymbol: '', quantity: 1, type: OperationType.Buy });
      fetchInitialData();
    } catch (error: any) {
      alert(error?.response?.data?.errorMessages?.[0] || 'Erro ao registrar operação.');
    } finally {
      setSubmitLoading(false);
    }
  };
  
  const handleSelectAssetForTrade = (ticker: string, type: OperationType) => {
    setForm({
      tickerSymbol: ticker,
      quantity: 1,
      type: type,
    });
    operationFormRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  const content = (
    <>
      <Paper
        ref={operationFormRef}
        elevation={12}
        sx={{ p: 4, borderRadius: 4, background: 'rgba(30,30,30,0.9)', backdropFilter: 'blur(5px)', mb: 4 }}
      >
        <Typography variant="h5" fontWeight={700} mb={3} align="center">
          Registrar Operação
        </Typography>
        <Box component="form" onSubmit={handleSubmit}>
          <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2} alignItems="center" justifyContent="center">
            <Autocomplete
              freeSolo
              options={allAssets}
              getOptionLabel={(option) => typeof option === 'string' ? option : option.tickerSymbol}
              value={allAssets.find(asset => asset.tickerSymbol === form.tickerSymbol) || form.tickerSymbol}
              onChange={(_, newValue) => {
                const newTicker = typeof newValue === 'string' ? newValue : newValue?.tickerSymbol || '';
                setForm(prev => ({ ...prev, tickerSymbol: newTicker.toUpperCase() }));
              }}
              fullWidth
              sx={{ minWidth: 150, flex: 2 }}
              renderInput={(params) => <TextField {...params} label="Ticker do Ativo" required />}
            />
            <TextField label="Quantidade" name="quantity" type="number" value={form.quantity} onChange={handleFormChange} required sx={{ minWidth: 120, flex: 1 }} inputProps={{ min: 1 }}/>
            <ToggleButtonGroup value={form.type} exclusive onChange={handleOperationTypeChange} color="primary" sx={{ flex: 1.5, height: 56 }}>
              <ToggleButton value={OperationType.Buy} sx={{ flex: 1, fontWeight: 600 }}>Compra</ToggleButton>
              <ToggleButton value={OperationType.Sell} sx={{ flex: 1, fontWeight: 600 }}>Venda</ToggleButton>
            </ToggleButtonGroup>
            <Button type="submit" variant="contained" size="large" sx={{ height: 56, fontWeight: 700, flex: 1 }} disabled={submitLoading}>
              {submitLoading ? <CircularProgress size={24} color="inherit" /> : 'Registrar'}
            </Button>
          </Stack>
        </Box>
      </Paper>

      <Typography variant="h5" fontWeight={700} mb={2} align="center"><ShowChartIcon sx={{verticalAlign: 'middle', mr: 1}}/> Minhas Posições</Typography>
      <Stack direction="row" flexWrap="wrap" spacing={2} justifyContent="center" mb={4}>
        {marketLoading && Array.from(new Array(3)).map((_, index) => <Skeleton key={index} variant="rounded" width={240} height={158} sx={{ borderRadius: 3}} />)}
        {!marketLoading && positions.length === 0 && <Typography color="text.secondary">Nenhuma posição encontrada.</Typography>}
        {!marketLoading && positions.map(pos => (
          <Card key={pos.tickerSymbol} sx={{ flex: '1 1 240px', minWidth: 220, maxWidth: 280, background: 'rgba(40,40,40,0.8)', borderRadius: 3, transition: 'transform 0.2s', '&:hover': {transform: 'scale(1.03)'} }}>
            <CardContent>
              <Typography variant="h6" fontWeight={700} color="primary" gutterBottom>{pos.assetName} ({pos.tickerSymbol})</Typography>
              <Typography variant="body2" color="text.secondary">Quantidade: <b>{pos.quantity}</b></Typography>
              <Typography variant="body2" color="text.secondary">Preço Médio: <b>R$ {pos.averagePrice.toFixed(2)}</b></Typography>
              <Typography variant="body2" color={pos.currentProfitAndLoss >= 0 ? 'success.main' : 'error.main'}>P/L Atual: <b>R$ {pos.currentProfitAndLoss.toFixed(2)}</b></Typography>
              <Button size="small" variant="outlined" startIcon={<SellIcon />} onClick={() => handleSelectAssetForTrade(pos.tickerSymbol, OperationType.Sell)} sx={{ mt: 2, width: '100%' }}>Vender</Button>
            </CardContent>
          </Card>
        ))}
      </Stack>

      <Typography variant="h5" fontWeight={700} mb={2} align="center"><AddShoppingCartIcon sx={{verticalAlign: 'middle', mr: 1}}/> Mercado de Ativos</Typography>
      <Paper elevation={6} sx={{ background: 'rgba(30,30,30,0.9)', backdropFilter: 'blur(5px)', borderRadius: 4 }}>
        <TableContainer>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Ticker</TableCell>
                <TableCell>Nome do Ativo</TableCell>
                <TableCell align="right">Preço Atual (R$)</TableCell>
                <TableCell align="center">Ação</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {marketLoading && Array.from(new Array(5)).map((_, index) => (
                <TableRow key={index}>
                  <TableCell><Skeleton variant="text" /></TableCell>
                  <TableCell><Skeleton variant="text" /></TableCell>
                  <TableCell><Skeleton variant="text" /></TableCell>
                  <TableCell><Skeleton variant="text" /></TableCell>
                </TableRow>
              ))}
              {!marketLoading && allAssets.map((asset) => (
                <TableRow key={asset.id} hover>
                  <TableCell sx={{ fontWeight: 'bold' }}>{asset.tickerSymbol}</TableCell>
                  <TableCell>{asset.name}</TableCell>
                  <TableCell align="right" sx={{ fontWeight: 'bold' }}>{marketQuotes[asset.tickerSymbol] ? `R$ ${marketQuotes[asset.tickerSymbol].unitPrice.toFixed(2)}` : '---'}</TableCell>
                  <TableCell align="center">
                    <Tooltip title="Comprar este ativo">
                      <Button size="small" variant="contained" onClick={() => handleSelectAssetForTrade(asset.tickerSymbol, OperationType.Buy)}>Comprar</Button>
                    </Tooltip>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Paper>
    </>
  );

  if (noContainer) return content;
  return <Container sx={{ mt: 4, mb: 4 }}>{content}</Container>;
}