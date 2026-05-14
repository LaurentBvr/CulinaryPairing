import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ContrainteDto } from './contraintes';

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
  contraintesViolees?: ContrainteDto[];
  statutCompatibilite?: 'Compatible' | 'Adaptable' | 'Incompatible';
}

@Injectable({ providedIn: 'root' })
export class RecetteService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/recettes`;

  /**
   * Liste des recettes publiées. Filtres optionnels (combinables) :
   *   - ingredient : nom d'ingrédient (match exact insensible casse côté backend)
   *   - type       : type de plat (Entree/Plat/Dessert)
   * Sans filtres : comportement legacy inchangé.
   */
  getAll(filters?: { ingredient?: string; type?: string }): Observable<Recette[]> {
    let params = new HttpParams();
    if (filters?.ingredient) {
      params = params.set('ingredient', filters.ingredient);
    }
    if (filters?.type) {
      params = params.set('type', filters.type);
    }
    return this.http.get<Recette[]>(this.apiUrl, { params });
  }

  getById(id: number, mode?: ModeAdaptation): Observable<Recette> {
    let params = new HttpParams();
    if (mode && mode !== 'original') {
      params = params.set('mode', mode);
    }
    return this.http.get<Recette>(`${this.apiUrl}/${id}`, { params });
  }
}