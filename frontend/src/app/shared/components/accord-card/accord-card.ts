import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

/**
 * Mode d'affichage du composant :
 * - 'recette' : on est sur une page recette → on met en avant la BOISSON
 * - 'boisson' : on est sur une page boisson → on met en avant la RECETTE
 *
 * Permet une seule source de vérité pour l'affichage d'un accord (CdC v1.3).
 */
export type AccordCardMode = 'recette' | 'boisson';

/**
 * Données minimales nécessaires à l'affichage d'un accord, indépendantes du sens.
 * Le composant projette le bon nom (boisson ou recette) selon le mode.
 */
export interface AccordCardData {
  idAccord: number;
  scoreCompatibilite: number;
  niveauConfiance: number | null;
  malusApplique: number | null;
  reglesSatisfaites: string | null;
  justification: string;

  // Nom + sous-titre injectés par le composant parent selon le mode
  // (ex : "Chardonnay" / "VinBlanc" en mode 'recette',
  //       "Risotto aux champignons" / "Plat" en mode 'boisson')
  titre: string;
  sousTitre: string | null;
}

@Component({
  selector: 'app-accord-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './accord-card.html',
  styleUrl: './accord-card.scss'
})
export class AccordCard {
  @Input({ required: true }) accord!: AccordCardData;
  @Input() mode: AccordCardMode = 'boisson';

  splitRegles(regles: string | null): string[] {
    if (!regles) return [];
    return regles.split(',').map(r => r.trim()).filter(r => r.length > 0);
  }

  labelConfiance(c: number | null): string {
    if (c === null) return '';
    if (c >= 60) return 'Élevée';
    if (c >= 35) return 'Bonne';
    if (c >= 20) return 'Modérée';
    return 'Faible';
  }
}