import axios, { AxiosResponse } from 'axios';
import {
  ApiResponse,
  LoginUserRequest,
  NewUserRequest,
  NewOperationRequest,
  UserLoginDto,
  UserDto,
  OperationDto,
  QuoteDto,
  PositionDto,
  TopClientsDto,
  AssetDto
} from '../types/api';

const apiClient = axios.create({
  baseURL: 'http://localhost:5000/api/v1',
});

apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('authToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const registerUser = (data: NewUserRequest): Promise<AxiosResponse<ApiResponse<UserDto>>> => {
  return apiClient.post('/user/register', data);
};

export const login = (data: LoginUserRequest): Promise<AxiosResponse<ApiResponse<UserLoginDto>>> => {
  return apiClient.post('/user/login', data);
};

export const createOperation = (data: NewOperationRequest): Promise<AxiosResponse<ApiResponse<OperationDto>>> => {
  return apiClient.post('/operations', data);
};

export const getLatestQuote = (tickerSymbol: string): Promise<AxiosResponse<ApiResponse<QuoteDto>>> => {
  return apiClient.get(`/quotes/${tickerSymbol}/latest`);
};

export const getPositionByTicker = (tickerSymbol: string): Promise<AxiosResponse<ApiResponse<PositionDto>>> => {
  return apiClient.get(`/positions/${tickerSymbol}`);
};

export const getAllPositions = (): Promise<AxiosResponse<ApiResponse<PositionDto[]>>> => {
  return apiClient.get('/positions');
};

export const getTotalBrokerageRevenue = (): Promise<AxiosResponse<ApiResponse<{ totalRevenue: number }>>> => {
  return apiClient.get('/reports/brokerage-revenue');
};

export const getTop10ByPositionValue = (): Promise<AxiosResponse<ApiResponse<TopClientsDto[]>>> => {
  return apiClient.get('/reports/top-10/by-position-value');
};

export const getTop10ByBrokerage = (): Promise<AxiosResponse<ApiResponse<any>>> => {
  return apiClient.get('/reports/top-10/by-brokerage');
};

export const getAllAssets = (): Promise<AxiosResponse<ApiResponse<AssetDto[]>>> => {
  return apiClient.get('/assets');
};

export default apiClient;