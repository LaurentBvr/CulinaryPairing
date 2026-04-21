import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
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

  constructor(private http: HttpClient) {
    this.loadUserFromStorage();
  }

  register(data: { email: string; password: string; nom: string }): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/api/auth/register`, {
      email: data.email,
      motDePasse: data.password,
      nom: data.nom
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
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
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

  private loadUserFromStorage(): void {
    const token = this.getToken();
    if (!token) return;
    this.http.get<User>(`${environment.apiUrl}/api/auth/me`).subscribe({
      next: user => this.currentUserSubject.next(user),
      error: () => this.logout()
    });
  }
}