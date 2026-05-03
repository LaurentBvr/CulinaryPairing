import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

// ============================================================
// Interfaces (alignées sur les DTOs C# côté API)
// ============================================================

/**
 * Élément de liste pour la page /soirees (DTO SoireeListItemDto).
 */
export interface SoireeListItem {
  idSoiree: number;
  nombrePersonnes: number;
  typeSoiree: string | null;
  dateCreation: string;
  nbContraintes: number;
  menuComplet: boolean;
}

/**
 * Contrainte alimentaire (DTO ContrainteDto, partagée avec /api/contraintes).
 */
export interface Contrainte {
  idContrainte: number;
  nom: string;
  type: string;
}

/**
 * Recette résumée pour les slots de menu et la liste recettes-éligibles
 * (DTO RecetteSlotDto). Distinct du listing /api/recettes pour découpler
 * les contrats Mode Soirée et Catalogue Recettes.
 */
export interface RecetteSlot {
  idRecette: number;
  titre: string;
  imageUrl: string | null;
  typePlat: string | null;
  tempsPreparation: number | null;
  tempsCuisson: number | null;
  coutEstime: number | null;
}

/**
 * Menu d'une soirée : 3 slots fixes (entree/plat/dessert) + totaux calculés.
 */
export interface Menu {
  idMenu: number;
  entree: RecetteSlot | null;
  plat: RecetteSlot | null;
  dessert: RecetteSlot | null;
  coutTotalEstime: number | null;
  tempsTotalEstime: number | null;
}

/**
 * Détail d'une soirée (DTO SoireeDetailDto) avec contraintes agrégées
 * (saisies + déduites des cohortes vegé/vegan) et menu courant.
 */
export interface SoireeDetail {
  idSoiree: number;
  nombrePersonnes: number;
  nombreVegetariens: number;
  nombreVegans: number;
  budget: number | null;
  tempsDisponible: number | null;
  typeSoiree: string | null;
  preferenceAlcool: string;
  dateCreation: string;
  contraintesAgregees: Contrainte[];
  menu: Menu | null;
}

/**
 * Payload de création d'une soirée (DTO SoireeCreateDto).
 */
export interface SoireeCreate {
  nombrePersonnes: number;
  nombreVegetariens: number;
  nombreVegans: number;
  budget: number | null;
  tempsDisponible: number | null;
  typeSoiree: string | null;
  preferenceAlcool: string;
  contraintesIds: number[];
}

/**
 * Payload de modification d'une soirée (DTO SoireeUpdateDto, identique à Create).
 */
export type SoireeUpdate = SoireeCreate;

/**
 * Slots autorisés pour assignation/désassignation de recette dans un menu.
 */
export type MenuSlot = 'entree' | 'plat' | 'dessert';

// ============================================================
// Service
// ============================================================

@Injectable({ providedIn: 'root' })
export class SoireesService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/soirees`;

  // ----- CRUD soirée -----

  getMine(): Observable<SoireeListItem[]> {
    return this.http.get<SoireeListItem[]>(this.apiUrl);
  }

  getById(idSoiree: number): Observable<SoireeDetail> {
    return this.http.get<SoireeDetail>(`${this.apiUrl}/${idSoiree}`);
  }

  /** Création. Retourne l'id de la soirée créée. */
  create(dto: SoireeCreate): Observable<number> {
    return this.http.post<number>(this.apiUrl, dto);
  }

  /** Modification : remplace les paramètres + l'ensemble des contraintes saisies. */
  update(idSoiree: number, dto: SoireeUpdate): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${idSoiree}`, dto);
  }

  delete(idSoiree: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${idSoiree}`);
  }

  // ----- Menu (1 menu par soirée pour le MVP V1.3) -----

  /** Get-or-create : crée un menu vide si la soirée n'en a pas encore. */
  getMenu(idSoiree: number): Observable<Menu> {
    return this.http.get<Menu>(`${this.apiUrl}/${idSoiree}/menu`);
  }

  /**
   * Assigne une recette à un slot. L'API valide :
   *  - type_plat de la recette doit matcher le slot
   *  - la recette ne doit violer aucune contrainte agrégée
   * Retourne le menu mis à jour (avec totaux recalculés).
   */
  assignSlot(idSoiree: number, slot: MenuSlot, idRecette: number): Observable<Menu> {
    return this.http.put<Menu>(`${this.apiUrl}/${idSoiree}/menu/slot/${slot}`, { idRecette });
  }

  unassignSlot(idSoiree: number, slot: MenuSlot): Observable<Menu> {
    return this.http.delete<Menu>(`${this.apiUrl}/${idSoiree}/menu/slot/${slot}`);
  }

  // ----- Recettes éligibles (cycle 7) -----

  /**
   * Recettes Statut=Publiee filtrées par type_plat=slot ET contraintes agrégées.
   * Tri alphabétique par titre.
   */
  getRecettesEligibles(idSoiree: number, slot: MenuSlot): Observable<RecetteSlot[]> {
    return this.http.get<RecetteSlot[]>(
      `${this.apiUrl}/${idSoiree}/recettes-eligibles?slot=${slot}`
    );
  }
}