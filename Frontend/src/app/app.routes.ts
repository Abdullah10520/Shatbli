import { Routes } from '@angular/router';
import { authGuard } from './Core/auth/auth.guard';

export const routes: Routes = [
  // Auth routes (login, register)
  {
    path: 'auth',
    loadChildren: () => import('./features/auth.routes').then(m => m.AUTH_ROUTES)
  },

  // Public layout wrapper (home, not-found)
  {
    path: '',
    loadComponent: () => import('./shared/components/public-layout/public-layout.component').then(m => m.PublicLayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./features/home/home.component').then(m => m.HomeComponent)
      },
      {
        path: 'not-found',
        loadComponent: () => import('./features/not-found/not-found.component').then(m => m.NotFoundComponent)
      }
    ]
  },

  // Protected layout wrapper for authenticated pages
  {
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
      }
    ]
  },

  // Wildcard - redirect to not-found
  {
    path: '**',
    redirectTo: 'not-found'
  }
];


