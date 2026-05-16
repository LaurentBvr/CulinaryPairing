import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface IngredientDto {
  idIngredient: number;
  nom: string;
}

export interface IngredientInfoDto {
  recettesCount: number;
  minIngredients: number;
  maxIngredients: number;
}

export interface VideFrigoResultDto {
  recetteId: number;
  titre: string;
  score: number;
  ingredientsPresents: string[];
  ingredientsManquants: string[];
  badgeVeg: boolean;
}

@Injectable({ providedIn: 'root' })
export class VideFrigoService {
  private api = `${environment.apiUrl}/api`;

  constructor(private http: HttpClient) {}

  searchIngredients(q: string): Observable<IngredientDto[]> {
    return this.http.get<IngredientDto[]>(`${this.api}/ingredients?q=${q}`);
  }

  getIngredientInfo(id: number): Observable<IngredientInfoDto> {
    return this.http.get<IngredientInfoDto>(`${this.api}/ingredients/${id}/info`);
  }

  rechercher(ingredientIds: number[], nombreResultats: number, inclureVeg: boolean): Observable<VideFrigoResultDto[]> {
    return this.http.post<VideFrigoResultDto[]>(`${this.api}/vide-frigo`, {
      ingredientIds,
      nombreResultats,
      inclureVeg
    });
  }
}