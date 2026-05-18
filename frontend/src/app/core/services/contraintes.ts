import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface ContrainteDto {
  idContrainte: number;
  nom: string;
  type: string; // 'Regime' | 'Allergie' | 'Conviction'
}

@Injectable({ providedIn: 'root' })
export class ContraintesService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/contraintes`;

  /** Catalogue public des 8 contraintes (CdC §3.5.1) */
  getCatalogue(): Observable<ContrainteDto[]> {
    return this.http.get<ContrainteDto[]>(this.apiUrl);
  }

  /** Contraintes activées par l'utilisateur courant */
  getMesContraintes(): Observable<ContrainteDto[]> {
    return this.http.get<ContrainteDto[]>(`${this.apiUrl}/me`);
  }

  /** Remplace toutes les contraintes du user (PUT idempotent) */
  updateMesContraintes(idsContraintes: number[]): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/me`, { idsContraintes });
  }
}