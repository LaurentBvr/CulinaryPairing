import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'recettes', pathMatch: 'full' },
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
  {
    path: 'boissons',
    loadComponent: () => import('./features/boissons/boisson-list/boisson-list').then(m => m.BoissonList),
  },
  {
    path: 'boissons/:id',
    loadComponent: () => import('./features/boissons/boisson-detail/boisson-detail').then(m => m.BoissonDetail),
  },
  {
    path: 'soirees',
    loadComponent: () => import('./features/soirees/soiree-list/soiree-list').then(m => m.SoireeList),
    canActivate: [authGuard]
  },
  {
    path: 'soirees/nouveau',
    loadComponent: () => import('./features/soirees/soiree-create/soiree-create').then(m => m.SoireeCreate),
    canActivate: [authGuard]
  },
  {
    path: 'soirees/:id',
    loadComponent: () => import('./features/soirees/soiree-detail/soiree-detail').then(m => m.SoireeDetail),
    canActivate: [authGuard]
  },
  {
    path: 'mes-favoris',
    loadComponent: () => import('./features/favoris/favoris-list/favoris-list').then(m => m.FavorisList),
    canActivate: [authGuard]
  },
  {
    path: 'profile',
    loadComponent: () => import('./features/profile/profile').then(m => m.Profile),
    canActivate: [authGuard]
  },
  {
  path: 'vide-frigo',
  loadComponent: () => import('./vide-frigo/vide-frigo').then(m => m.VideFrigo)
  },
  {
    path: 'recherche',
    loadComponent: () => import('./features/recherche/recherche-page/recherche-page').then(m => m.RecherchePage)
  },
  { path: '**', redirectTo: 'recettes' }
];