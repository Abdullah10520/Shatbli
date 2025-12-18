export interface User {
  id: string;
  email: string;
  fullName: string;
  phoneNumber?: string;
  token: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
  phoneNumber: string;
}

export interface AuthResponse {
  user: User;
  token: string;
}