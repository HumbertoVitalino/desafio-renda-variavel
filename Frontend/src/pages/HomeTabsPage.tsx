import { useState, useEffect } from 'react';
import { Box, Tabs, Tab, Paper } from '@mui/material';
import DashboardPage from './DashboardPage';
import OperationsPage from './OperationsPage';
import { useLocation, useNavigate } from 'react-router-dom';

export default function HomeTabsPage() {
  const location = useLocation();
  const navigate = useNavigate();
  const [tab, setTab] = useState(0);

  // Permite abrir direto na aba de operações via ?tab=1
  useEffect(() => {
    const params = new URLSearchParams(location.search);
    const tabParam = params.get('tab');
    if (tabParam === '1') setTab(1);
    else setTab(0);
    if (tabParam) navigate('/dashboard', { replace: true });
    // eslint-disable-next-line
  }, [location.search]);

  return (
    <Box sx={{ mt: 4, mb: 4 }}>
      <Paper
        elevation={6}
        sx={{
          borderRadius: 4,
          background: 'rgba(30,30,30,0.97)',
          boxShadow: '0 8px 32px 0 rgba(0,0,0,0.25)',
        }}
      >
        <Tabs
          value={tab}
          onChange={(_, newValue) => setTab(newValue)}
          indicatorColor="primary"
          textColor="primary"
          variant="fullWidth"
          sx={{ borderBottom: 1, borderColor: 'divider' }}
        >
          <Tab label="Dashboard" />
          <Tab label="Operações" />
        </Tabs>
        <Box sx={{ p: 3 }}>
          {tab === 0 && <DashboardPage noContainer />}
          {tab === 1 && <OperationsPage noContainer />}
        </Box>
      </Paper>
    </Box>
  );
}