export enum AssetRisk {
  Low = 1,
  Medium = 2,
  High = 3
}

export enum InvestorProfile {
  Conservative = 1,
  Bold = 2,
  Moderate = 3
}

export enum OperationType {
  Buy = 1,
  Sell = 2
}

export interface OperationDto {
  id: number;
  tickerSymbol: string;
  type: OperationType;
  quantity: number;
  unitPrice: number;
  brokerageFee: number;
  dateTime: string;
}

export interface PositionDto {
  tickerSymbol: string;
  assetName: string;
  quantity: number;
  averagePrice: number;
  currentProfitAndLoss: number;
  currentValue?: number;
}

export interface QuoteDto {
  tickerSymbol: string;
  assetName:string;
  unitPrice: number;
  dateTime: string;
}

export interface TopClientsDto {
  userId: number;
  userName: string;
  totalPositionValue: number;
}

export interface UserDto {
  name: string;
  email: string;
  brokerageRate: number;
}

export interface UserLoginDto {
  id: number;
  name: string;
  email: string;
  token: string;
}

export interface LoginUserRequest {
  email: string;
  password: string;
}

export interface NewOperationRequest {
  tickerSymbol: string;
  quantity: number;
  type: OperationType;
}

export interface NewUserRequest {
  name: string;
  email: string;
  password: string;
  confirmation: string;
  profile: InvestorProfile;
}

export interface ApiResponse<T> {
  isValid: boolean;
  result?: T;
  messages?: string[];
  errorMessages?: string[];
}

export interface AssetDto {
  id: number;
  tickerSymbol: string;
  name: string;
  risk: AssetRisk;
}