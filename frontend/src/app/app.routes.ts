import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register').then(m => m.RegisterComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./features/dashboard/dashboard').then(m => m.Dashboard),
    canActivate: [authGuard]
  },
  {
    path: 'recettes',
    loadComponent: () => import('./features/recettes/recette-list/recette-list').then(m => m.RecetteList), 
  },
  {
    path: 'recettes/:id',
    loadComponent: () => import('./features/recettes/recette-detail/recette-detail').then(m => m.RecetteDetail),
  },
  { path: '**', redirectTo: 'login' }
];