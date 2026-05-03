import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SoireesService, RecetteSlot, MenuSlot } from '../../../core/services/soirees';

@Component({
  selector: 'app-recette-picker',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './recette-picker.html',
  styleUrl: './recette-picker.scss'
})
export class RecettePicker implements OnInit {
  private soirees = inject(SoireesService);

  /** Identifiant de la soirée (pour scoper les recettes éligibles). */
  @Input({ required: true }) idSoiree!: number;
  /** Slot ciblé : entree | plat | dessert. */
  @Input({ required: true }) slot!: MenuSlot;
  /** Recette actuellement assignée (pour la marquer "courante" dans la liste). */
  @Input() idRecetteCourante: number | null = null;

  /** Emis quand l'utilisateur sélectionne une recette. */
  @Output() recetteChoisie = new EventEmitter<RecetteSlot>();
  /** Emis quand l'utilisateur ferme la modale sans choisir. */
  @Output() ferme = new EventEmitter<void>();

  recettes: RecetteSlot[] = [];
  loading = true;
  error = '';

  /** Libellé humain du slot pour le titre de la modale. */
  get slotLabel(): string {
    return { entree: 'Entrée', plat: 'Plat', dessert: 'Dessert' }[this.slot];
  }

  ngOnInit() {
    this.soirees.getRecettesEligibles(this.idSoiree, this.slot).subscribe({
      next: data => {
        this.recettes = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Impossible de charger les recettes éligibles.';
        this.loading = false;
      }
    });
  }

  choisir(r: RecetteSlot) {
    this.recetteChoisie.emit(r);
  }

  fermer() {
    this.ferme.emit();
  }

  /** Empêche la fermeture si on clique sur le contenu de la modale. */
  stopPropagation(e: Event) {
    e.stopPropagation();
  }
}