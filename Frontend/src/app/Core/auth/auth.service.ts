import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { ApiResponse, LoginData, User, LoginRequest, RegisterRequest } from '../../shared/models/user.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiUrl = environment.apiUrl;

  currentUserSig = signal<User | undefined | null>(undefined);
  isLoggedIn = computed(() => !!this.currentUserSig());

  constructor(private http: HttpClient) {
    this.loadUserFromStorage();
  }

  private loadUserFromStorage(): void {
    try {
      const userJson = localStorage.getItem('user');
      const token = localStorage.getItem('token');

      // التحقق من أن القيم موجودة وليست undefined
      if (userJson && userJson !== 'undefined' && userJson !== 'null' &&
        token && token !== 'undefined' && token !== 'null') {
        const user = JSON.parse(userJson);
        user.token = token;
        this.currentUserSig.set(user);
      } else {
        // مسح البيانات غير الصالحة
        this.logout();
      }
    } catch {
      // في حالة حدوث خطأ في JSON.parse
      this.logout();
    }
  }

  login(credentials: LoginRequest) {
    return this.http.post<ApiResponse<LoginData>>(`${this.apiUrl}/Users/login`, credentials)
      .pipe(
        tap(res => {
          if (res.success && res.data) {
            this.saveAuthData(res.data);
          }
        })
      );
  }

  register(data: RegisterRequest) {
    // الـ Register API بيرجع JSON: { success, message, errors }
    return this.http.post<ApiResponse<void>>(`${this.apiUrl}/Users/register`, data);
  }

  private saveAuthData(data: LoginData): void {
    if (data && data.token) {
      const user: User = {
        email: data.email,
        fullName: data.fullName,
        role: data.role,
        token: data.token
      };
      localStorage.setItem('user', JSON.stringify(user));
      localStorage.setItem('token', data.token);
      this.currentUserSig.set(user);
    }
  }

  logout(): void {
    localStorage.removeItem('user');
    localStorage.removeItem('token');
    this.currentUserSig.set(null);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }
}