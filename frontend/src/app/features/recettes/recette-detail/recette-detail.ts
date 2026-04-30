import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { RecetteService, Recette, ModeAdaptation } from '../../../core/services/recette';
import { AccordsService, Accord } from '../../../core/services/accords';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-recette-detail',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './recette-detail.html',
  styleUrl: './recette-detail.scss'
})
export class RecetteDetail implements OnInit {
  private route = inject(ActivatedRoute);
  private recetteService = inject(RecetteService);
  private accordsService = inject(AccordsService);

  recette: Recette | null = null;
  accords: Accord[] = [];
  loading = true;
  error = '';
  mode: ModeAdaptation = 'original';
  private idRecette = 0;

  ngOnInit() {
    this.idRecette = Number(this.route.snapshot.paramMap.get('id'));
    this.loadRecette();

    // Accords chargés une seule fois (Option 1 : pas de recalcul en mode végé/végan)
    this.accordsService.getByRecette(this.idRecette).subscribe({
      next: accords => this.accords = accords,
      error: () => {}
    });
  }

  changeMode(newMode: ModeAdaptation) {
    if (this.mode === newMode) return;
    this.mode = newMode;
    this.loadRecette();
  }

  private loadRecette() {
    this.loading = true;
    this.recetteService.getById(this.idRecette, this.mode).subscribe({
      next: data => { this.recette = data; this.loading = false; },
      error: () => { this.error = 'Recette introuvable.'; this.loading = false; }
    });
  }

  /** Convertit "R10bis,R10,R14bis" en tableau pour affichage en badges. */
  splitRegles(regles: string | null): string[] {
    if (!regles) return [];
    return regles.split(',').map(r => r.trim()).filter(r => r.length > 0);
  }

  /** Mappe la confiance numérique en label lisible pour l'utilisateur. */
  labelConfiance(c: number | null): string {
    if (c === null) return '';
    if (c >= 60) return 'Élevée';
    if (c >= 35) return 'Bonne';
    if (c >= 20) return 'Modérée';
    return 'Faible';
  }
}