import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Accord {
  idAccord: number;
  typeAccord: string;
  justification: string;
  scoreCompatibilite: number;
  niveauConfiance: number | null;
  malusApplique: number | null;
  nomBoisson: string;
  typeBoisson: string;
}

@Injectable({ providedIn: 'root' })
export class AccordsService {
  private http = inject(HttpClient);
  private api = 'http://localhost:5011/api';

  getByRecette(idRecette: number): Observable<Accord[]> {
    return this.http.get<Accord[]>(`${this.api}/recettes/${idRecette}/accords`);
  }
}