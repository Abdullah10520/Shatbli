export interface User {
  id: string;
  email: string;
  fullName: string;
  phoneNumber?: string;
  role?: string;
  token?: string;
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

// استجابة تسجيل الدخول (الـ API بيرجع البيانات مباشرة مش nested)
export interface AuthResponse {
  userId: string;
  email: string;
  fullName: string;
  token: string;
  role: string;
}