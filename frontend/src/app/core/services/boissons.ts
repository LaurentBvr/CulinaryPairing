import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

/**
 * Élément de liste pour la page /boissons (DTO BoissonListItemDto côté API).
 */
export interface BoissonListItem {
  idBoisson: number;
  nom: string;
  typeBoisson: string | null;
  alcoolise: boolean;
  pays: string | null;
  region: string | null;
  appellation: string | null;
  cepage: string | null;
  degreAlcool: number | null;
  corps: string | null;
  imageUrl: string | null;
}

/**
 * Détail d'une boisson (DTO BoissonDetailDto côté API).
 */
export interface BoissonDetail {
  idBoisson: number;
  nom: string;
  typeBoisson: string | null;
  alcoolise: boolean;

  // Profil gustatif
  niveauAcidite: number | null;
  niveauSucre: number | null;
  niveauTannins: number | null;
  niveauAmertume: number | null;
  intensiteAromatique: number | null;
  niveauFume: number | null;
  degreAlcool: number | null;
  corps: string | null;

  // Service
  temperatureOptimale: number | null;
  toleranceTemperature: number;

  // Origine V1.3
  pays: string | null;
  region: string | null;
  appellation: string | null;
  cepage: string | null;

  coutMoyen: number | null;
  imageUrl: string | null;
  famillesAromatiques: string[];
}

/**
 * Accord inversé : à partir d'une boisson, info sur la recette accordée + score.
 * Distinct d'`Accord` (sens direct) : champs recette au lieu de champs boisson.
 */
export interface AccordInverse {
  idAccord: number;
  typeAccord: string;
  justification: string;
  scoreCompatibilite: number;
  niveauConfiance: number | null;
  malusApplique: number | null;
  reglesSatisfaites: string | null;
  dateCalcul: string;
  versionMoteur: string;

  // Champs recette
  idRecette: number;
  titre: string;
  imageUrl: string | null;
  typePlat: string | null;
  difficulte: string | null;
  tempsPreparation: number | null;
  tempsCuisson: number | null;
}

@Injectable({ providedIn: 'root' })
export class BoissonsService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/boissons`;

  getAll(): Observable<BoissonListItem[]> {
    return this.http.get<BoissonListItem[]>(this.apiUrl);
  }

  getById(id: number): Observable<BoissonDetail> {
    return this.http.get<BoissonDetail>(`${this.apiUrl}/${id}`);
  }

  /**
   * Accord inversé V1.3 : recettes publiées qui s'accordent avec cette boisson.
   * @param limit nombre max de résultats (1-50, défaut 20).
   */
  getAccordsInverse(idBoisson: number, limit: number = 20): Observable<AccordInverse[]> {
    const params = new HttpParams().set('limit', limit.toString());
    return this.http.get<AccordInverse[]>(`${this.apiUrl}/${idBoisson}/accords`, { params });
  }
}