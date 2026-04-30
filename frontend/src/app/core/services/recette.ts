import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export type ModeAdaptation = 'original' | 'vegetarien' | 'vegan';

export interface SubstitutIngredient {
  idIngredient: number;
  nom: string;
  quantiteAdaptee: number;
  unite: string;
  noteCuisson: string | null;
}

export interface IngredientRecette {
  idIngredient: number;
  nom: string;
  quantite: number;
  unite: string;
  estVege: boolean;
  estVegan: boolean;
  substitut?: SubstitutIngredient | null;
}

export interface Etape {
  numeroEtape: number;
  description: string;
}

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
  nombrePersonnesBase?: number;
  adaptableVege?: boolean;
  adaptableVegan?: boolean;
  mode?: ModeAdaptation;
  ingredients?: IngredientRecette[];
  ingredientsSansSubstitution?: string[];
  estCompletementAdaptable?: boolean;
  etapes?: Etape[];
}

@Injectable({ providedIn: 'root' })
export class RecetteService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/recettes`;

  getAll(): Observable<Recette[]> {
    return this.http.get<Recette[]>(this.apiUrl);
  }

  getById(id: number, mode?: ModeAdaptation): Observable<Recette> {
    let params = new HttpParams();
    if (mode && mode !== 'original') {
      params = params.set('mode', mode);
    }
    return this.http.get<Recette>(`${this.apiUrl}/${id}`, { params });
  }
}