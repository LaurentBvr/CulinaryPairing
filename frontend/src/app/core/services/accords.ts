import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface Accord {
  idAccord: number;
  typeAccord: string;
  justification: string;
  scoreCompatibilite: number;
  niveauConfiance: number | null;
  malusApplique: number | null;
  reglesSatisfaites: string | null;   // V1.3 : "R10bis,R10,R14bis,..."
  dateCalcul: string;                  // V1.3
  versionMoteur: string;               // V1.3
  nomBoisson: string;
  typeBoisson: string;
}

@Injectable({ providedIn: 'root' })
export class AccordsService {
  private http = inject(HttpClient);
  private api = `${environment.apiUrl}/api`;

  getByRecette(idRecette: number): Observable<Accord[]> {
    return this.http.get<Accord[]>(`${this.api}/recettes/${idRecette}/accords`);
  }
}