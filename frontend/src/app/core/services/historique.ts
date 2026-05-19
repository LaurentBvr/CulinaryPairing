import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface HistoriqueRecetteDto {
  idRecette: number;
  titre: string;
  imageUrl?: string | null;
  typePlat: string;
  derniereConsultation: string;
}

@Injectable({ providedIn: 'root' })
export class HistoriqueService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/historique`;

  /**
   * Enregistre la consultation d'une recette pour l'utilisateur courant.
   * Le backend deduplique automatiquement les consultations rapprochees (fenetre 5 min).
   * Renvoie { enregistree: true } si une nouvelle ligne a ete inseree, false sinon.
   */
  enregistrer(idRecette: number): Observable<{ enregistree: boolean }> {
    return this.http.post<{ enregistree: boolean }>(
      `${this.apiUrl}/consultation`,
      { idRecette }
    );
  }

  /**
   * Recupere les N dernieres recettes UNIQUES consultees par l'utilisateur courant,
   * triees par date de derniere consultation decroissante.
   */
  getMesDernieres(limit: number = 5): Observable<HistoriqueRecetteDto[]> {
    return this.http.get<HistoriqueRecetteDto[]>(
      `${this.apiUrl}/mes-dernieres?limit=${limit}`
    );
  }
}