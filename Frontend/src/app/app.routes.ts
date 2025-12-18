import { Routes } from '@angular/router';
import { authGuard } from './Core/auth/auth.guard';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./features/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: 'studio',
    loadComponent: () => import('./features/studio/studio.component').then(m => m.StudioComponent),
    canActivate: [authGuard]
  },
  {
    path: '',
    redirectTo: 'auth',
    pathMatch: 'full'
  }
];

