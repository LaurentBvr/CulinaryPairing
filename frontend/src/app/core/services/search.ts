import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment';

/**
 * Résultat d'une recherche transversale (DTO SearchResultDto côté API).
 * Groupé par catégorie pour faciliter l'affichage en dropdown live et en page complète.
 */
export interface SearchResult {
  recettes: SearchRecetteItem[];
  boissons: SearchBoissonItem[];
  ingredients: SearchIngredientItem[];
  typesPlat: SearchTypePlatItem[];
  totalResultats: number;
}

export interface SearchRecetteItem {
  id: number;
  titre: string;
  typePlat: string | null;
  difficulte: string | null;
}

export interface SearchBoissonItem {
  id: number;
  nom: string;
  type: string | null;
}

export interface SearchIngredientItem {
  id: number;
  nom: string;
  nombreRecettes: number;
}

export interface SearchTypePlatItem {
  valeur: string;
  nombreRecettes: number;
}

/**
 * Service de recherche transversale multi-entités.
 * Endpoint unique consommé par :
 *   - le dropdown live de la navbar (limit=5)
 *   - la page /recherche complète (limit=50)
 */
@Injectable({ providedIn: 'root' })
export class SearchService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/search`;

  /**
   * Garde-fou côté front : requête < 2 caractères → résultat vide synchrone,
   * évite un round-trip API inutile (le backend retournerait vide de toute façon).
   */
  search(query: string, limit: number = 5): Observable<SearchResult> {
    const q = query?.trim() ?? '';
    if (q.length < 2) {
      return of(this.emptyResult());
    }

    const params = new HttpParams()
      .set('q', q)
      .set('limit', limit.toString());

    return this.http.get<SearchResult>(this.apiUrl, { params });
  }

  private emptyResult(): SearchResult {
    return {
      recettes: [],
      boissons: [],
      ingredients: [],
      typesPlat: [],
      totalResultats: 0
    };
  }
}