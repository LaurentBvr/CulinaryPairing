import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface Recette {
  idRecette: number;
  nom: string;
  description: string;
  tempsPreparation: number;
  tempsCuisson: number;
  niveauDifficulte: string;
  typeRepas: string;
  categorie: string;
  imageUrl?: string;
}

@Injectable({ providedIn: 'root' })
export class RecetteService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/recettes`;

  getAll(): Observable<Recette[]> {
    return this.http.get<Recette[]>(this.apiUrl);
  }

  getById(id: number): Observable<Recette> {
    return this.http.get<Recette>(`${this.apiUrl}/${id}`);
  }
}