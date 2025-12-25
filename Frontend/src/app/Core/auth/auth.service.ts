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
    return this.http.post<AuthResponse>(`${this.apiUrl}/Users/login`, credentials)
      .pipe(
        tap(res => {
          this.saveAuthData(res);
        })
      );
  }

  register(data: RegisterRequest) {
    // الـ Register API بيرجع userId فقط (plain text string)، مش JSON
    return this.http.post(`${this.apiUrl}/Users/register`, data, {
      responseType: 'text'
    });
  }

  private saveAuthData(res: AuthResponse): void {
    // التأكد من وجود البيانات قبل حفظها
    // الـ API بيرجع البيانات مباشرة: { userId, email, fullName, token, role }
    if (res && res.token && res.userId) {
      const user: User = {
        id: res.userId,
        email: res.email,
        fullName: res.fullName,
        role: res.role,
        token: res.token
      };
      localStorage.setItem('user', JSON.stringify(user));
      localStorage.setItem('token', res.token);
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