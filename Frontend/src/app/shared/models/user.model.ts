// Generic API Response wrapper
export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data?: T;
  errors: Record<string, string[]>;
  timestamp: string;
}

// User model for local storage
export interface User {
  email: string;
  fullName: string;
  phoneNumber?: string;
  role?: string;
  token?: string;
}

// Login request
export interface LoginRequest {
  email: string;
  password: string;
}

// Login response data (inside data field)
export interface LoginData {
  email: string;
  fullName: string;
  token: string;
  role: string;
}

// Register request
export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
  phoneNumber: string;
}

// Profile response data
export interface ProfileData {
  email: string;
  fullName: string;
  phoneNumber: string;
  role: string;
  lastLoginAt: string;
  isEmailVerified: boolean;
  createdAt: string;
}

// Update profile request
export interface UpdateProfileRequest {
  fullName: string;
  phoneNumber: string;
}

// Change password request
export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}

// Legacy AuthResponse (keeping for backward compatibility during transition)
export interface AuthResponse {
  userId: string;
  email: string;
  fullName: string;
  token: string;
  role: string;
}