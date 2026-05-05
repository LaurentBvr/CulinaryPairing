import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth';

export interface FavoriDto {
  idRecette: number;
  titre: string;
  imageUrl?: string | null;
  typePlat?: string | null;
  difficulte?: string | null;
  tempsPreparation?: number | null;
  tempsCuisson?: number | null;
  dateAjout: string;
}

@Injectable({ providedIn: 'root' })
export class FavorisService {
  private http = inject(HttpClient);
  private auth = inject(AuthService);
  private apiUrl = `${environment.apiUrl}/api/favoris`;

  // Set d'IDs en signal → permet @if (favoris.has(id)) en O(1) sur RecetteList
  private favorisIds = signal<Set<number>>(new Set());
  readonly ids = this.favorisIds.asReadonly();

  constructor() {
    // Charge les IDs au login (et reset au logout)
    this.auth.currentUser$.subscribe(user => {
      if (user) this.refreshIds();
      else this.favorisIds.set(new Set());
    });
  }

  /** Appelé après login pour précharger les favoris du user */
  private refreshIds(): void {
    this.http.get<number[]>(`${this.apiUrl}/ids`).subscribe({
      next: ids => this.favorisIds.set(new Set(ids)),
      error: () => this.favorisIds.set(new Set())
    });
  }

  /** True si la recette est en favori (lecture O(1) depuis le signal) */
  isFavori(idRecette: number): boolean {
    return this.favorisIds().has(idRecette);
  }

  /** Toggle : ajoute ou retire selon l'état actuel. Met à jour le signal. */
  toggle(idRecette: number): Observable<void> {
    if (this.isFavori(idRecette)) {
      return this.http.delete<void>(`${this.apiUrl}/${idRecette}`).pipe(
        tap(() => {
          const next = new Set(this.favorisIds());
          next.delete(idRecette);
          this.favorisIds.set(next);
        })
      );
    } else {
      return this.http.post<void>(`${this.apiUrl}/${idRecette}`, {}).pipe(
        tap(() => {
          const next = new Set(this.favorisIds());
          next.add(idRecette);
          this.favorisIds.set(next);
        })
      );
    }
  }

  /** Liste détaillée pour la page "Mes favoris" */
  getAll(): Observable<FavoriDto[]> {
    return this.http.get<FavoriDto[]>(this.apiUrl);
  }
}