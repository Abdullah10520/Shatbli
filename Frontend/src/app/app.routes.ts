import { Routes } from '@angular/router';
import { authGuard } from './Core/auth/auth.guard';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./features/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    // Main layout wrapper for authenticated pages
    path: '',
    loadComponent: () => import('./shared/components/main-layout/main-layout.component').then(m => m.MainLayoutComponent),
    canActivate: [authGuard],
    children: [
      {
        path: 'studio',
        loadComponent: () => import('./features/studio/studio.component').then(m => m.StudioComponent)
      },
      {
        path: 'profile',
        loadComponent: () => import('./features/profile/profile.component').then(m => m.ProfileComponent)
      },
      {
        path: 'subscription',
        loadComponent: () => import('./features/subscription/subscription.component').then(m => m.SubscriptionComponent)
      },
      {
        path: '',
        redirectTo: 'studio',
        pathMatch: 'full'
      }
    ]
  }
];


