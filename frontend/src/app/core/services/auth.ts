import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, firstValueFrom, tap } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface User {
  idUtilisateur: number;
  email: string;
  prenom: string;
  nom: string;
  role: string;
}

export interface AuthResponse {
  token: string;
  expiration: string;
  idUtilisateur: number;
  prenom: string;
  nom: string;
  email: string;
  role: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly TOKEN_KEY = 'auth_token';
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();
  private readonly router = inject(Router);

  constructor(private http: HttpClient) {}

  register(data: { prenom: string; nom: string; email: string; password: string }): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/api/auth/register`, {
      prenom: data.prenom,
      nom: data.nom,
      email: data.email,
      motDePasse: data.password
    }).pipe(tap(res => this.handleAuth(res)));
  }

  login(email: string, password: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/api/auth/login`, {
      email,
      motDePasse: password
    }).pipe(tap(res => this.handleAuth(res)));
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    this.currentUserSubject.next(null);
    this.router.navigate(['/recettes']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  /**
   * Ré-hydrate l'utilisateur depuis le token JWT stocké en localStorage.
   * Appelée au bootstrap via provideAppInitializer dans app.config.ts,
   * ce qui garantit que currentUser$ est résolu AVANT que le router
   * n'évalue les routes (élimine la race condition guard / composant).
   * Retourne toujours une Promise résolue, succès ou échec.
   */
  async tryRestoreSession(): Promise<void> {
    const token = this.getToken();
    if (!token) return;

    try {
      const user = await firstValueFrom(
        this.http.get<User>(`${environment.apiUrl}/api/auth/me`)
      );
      this.currentUserSubject.next(user);
    } catch {
      // Token invalide ou expiré : on nettoie pour éviter une boucle d'erreurs
      this.logout();
    }
  }

  private handleAuth(res: AuthResponse): void {
    localStorage.setItem(this.TOKEN_KEY, res.token);
    this.currentUserSubject.next({
      idUtilisateur: res.idUtilisateur,
      prenom: res.prenom,
      nom: res.nom,
      email: res.email,
      role: res.role
    });
  }
}