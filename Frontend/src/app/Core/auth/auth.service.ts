import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { AuthResponse, User, LoginRequest, RegisterRequest } from '../../shared/models/user.model';
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
    const userJson = localStorage.getItem('user');
    const token = localStorage.getItem('token');
    if (userJson && token) {
      const user = JSON.parse(userJson);
      user.token = token;
      this.currentUserSig.set(user);
    }
  }

  login(credentials: LoginRequest) {
    return this.http.post<AuthResponse>(`${this.apiUrl}/Users/login`, credentials)
      .pipe(
        tap(res => {
          this.saveAuthData(res);
        })
      );
  }

  register(data: RegisterRequest) {
    return this.http.post<AuthResponse>(`${this.apiUrl}/Users/register`, data)
      .pipe(
        tap(res => {
          this.saveAuthData(res);
        })
      );
  }

  private saveAuthData(res: AuthResponse): void {
    localStorage.setItem('user', JSON.stringify(res.user));
    localStorage.setItem('token', res.token);
    const user = { ...res.user, token: res.token };
    this.currentUserSig.set(user);
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