import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterLink, Router } from '@angular/router';
import { RecettePicker } from '../../../shared/components/recette-picker/recette-picker';
import { CommonModule } from '@angular/common';
import {
  SoireesService, SoireeDetail as SoireeDetailDto,
  Menu, MenuSlot, RecetteSlot
} from '../../../core/services/soirees';

interface SlotConfig {
  key: MenuSlot;
  label: string;
  icone: string;
}

@Component({
  selector: 'app-soiree-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, RecettePicker],
  templateUrl: './soiree-detail.html',
  styleUrl: './soiree-detail.scss'
})
export class SoireeDetail implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private soirees = inject(SoireesService);

  soiree: SoireeDetailDto | null = null;
  menu: Menu | null = null;
  loading = true;
  error = '';

  // Action en cours sur un slot (pour désactiver les boutons pendant l'appel)
  slotEnCours: MenuSlot | null = null;

  /** Slot dont la modale RecettePicker est ouverte (null = fermée). */
  slotPickerOuvert: MenuSlot | null = null;

  readonly slots: SlotConfig[] = [
    { key: 'entree',  label: 'Entrée',  icone: '🥗' },
    { key: 'plat',    label: 'Plat',    icone: '🍽️' },
    { key: 'dessert', label: 'Dessert', icone: '🍰' }
  ];

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id) {
      this.error = 'Identifiant de soirée invalide.';
      this.loading = false;
      return;
    }

    this.soirees.getById(id).subscribe({
      next: data => {
        this.soiree = data;
        this.menu = data.menu;
        this.loading = false;
        // Si pas encore de menu (get-or-create), on l'initialise
        if (!this.menu) {
          this.soirees.getMenu(id).subscribe({
            next: m => this.menu = m,
            error: () => {}
          });
        }
      },
      error: err => {
        this.error = err.status === 404
          ? 'Soirée introuvable.'
          : 'Impossible de charger la soirée.';
        this.loading = false;
      }
    });
  }

  /** Recette assignée à un slot, ou null. */
  recetteDuSlot(slot: MenuSlot): RecetteSlot | null {
    if (!this.menu) return null;
    return this.menu[slot];
  }

  desassigner(slot: MenuSlot) {
    if (!this.soiree || !this.menu) return;
    if (!confirm(`Retirer la recette du slot "${slot}" ?`)) return;

    this.slotEnCours = slot;
    this.soirees.unassignSlot(this.soiree.idSoiree, slot).subscribe({
      next: m => {
        this.menu = m;
        this.slotEnCours = null;
      },
      error: () => {
        alert('Erreur lors de la désassignation.');
        this.slotEnCours = null;
      }
    });
  }

  supprimer() {
    if (!this.soiree) return;
    if (!confirm('Supprimer définitivement cette soirée ?')) return;
    this.soirees.delete(this.soiree.idSoiree).subscribe({
      next: () => this.router.navigate(['/soirees']),
      error: () => alert('Suppression échouée.')
    });
  }

  ouvrirPicker(slot: MenuSlot) {
    this.slotPickerOuvert = slot;
  }

  fermerPicker() {
    this.slotPickerOuvert = null;
  }

  onRecetteChoisie(r: RecetteSlot) {
    if (!this.soiree || !this.slotPickerOuvert) return;
    const slot = this.slotPickerOuvert;
    this.slotEnCours = slot;
    this.soirees.assignSlot(this.soiree.idSoiree, slot, r.idRecette).subscribe({
      next: m => {
        this.menu = m;
        this.slotEnCours = null;
        this.slotPickerOuvert = null;
      },
      error: err => {
        const msg = err.error?.detail || err.error?.title || 'Assignation refusée par le serveur.';
        alert(msg);
        this.slotEnCours = null;
      }
    });
  }

  formaterTemps(min: number | null): string {
    if (min === null) return '—';
    if (min < 60) return `${min} min`;
    const h = Math.floor(min / 60);
    const m = min % 60;
    return m === 0 ? `${h}h` : `${h}h${m.toString().padStart(2, '0')}`;
  }
}