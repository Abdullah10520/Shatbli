import { Routes } from '@angular/router';
import { AuthLayout } from './auth/auth-layout/auth-layout';
import { RegisterComponent } from './auth/register.component/register.component';
import { LoginComponent } from './auth/login/login.component/login.component';

export const AUTH_ROUTES: Routes = [
  {
    path: '',
    component: AuthLayout,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: '', redirectTo: 'login', pathMatch: 'full' }
    ]
  }
];