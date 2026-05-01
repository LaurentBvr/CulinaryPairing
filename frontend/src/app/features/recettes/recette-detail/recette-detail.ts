import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { RecetteService, Recette, ModeAdaptation } from '../../../core/services/recette';
import { AccordsService, Accord } from '../../../core/services/accords';
import { FavorisService } from '../../../core/services/favoris';
import { AuthService } from '../../../core/services/auth';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-recette-detail',
  standalone: true,
  imports: [RouterLink, CommonModule, FormsModule],
  templateUrl: './recette-detail.html',
  styleUrl: './recette-detail.scss'
})
export class RecetteDetail implements OnInit {
  private route = inject(ActivatedRoute);
  private recetteService = inject(RecetteService);
  private accordsService = inject(AccordsService);
  protected favorisService = inject(FavorisService);
  private auth = inject(AuthService);
  private router = inject(Router);

  recette: Recette | null = null;
  accords: Accord[] = [];
  loading = true;
  error = '';
  mode: ModeAdaptation = 'original';
  personnesAffichees = 4;
  private idRecette = 0;

  ngOnInit() {
    this.idRecette = Number(this.route.snapshot.paramMap.get('id'));
    this.loadRecette();

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

  /** R2 : nombre de personnes ∈ [1..10] */
  changePersonnes(value: number) {
    if (value < 1) value = 1;
    if (value > 10) value = 10;
    this.personnesAffichees = value;
  }

  onToggleFavori(): void {
    if (!this.auth.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }
    this.favorisService.toggle(this.idRecette).subscribe();
  }

  private loadRecette() {
    this.loading = true;
    this.recetteService.getById(this.idRecette, this.mode).subscribe({
      next: data => {
        this.recette = data;
        this.personnesAffichees = data.nombrePersonnesBase ?? 4;
        this.loading = false;
      },
      error: () => { this.error = 'Recette introuvable.'; this.loading = false; }
    });
  }

  /**
   * R6 (g, ml, l, kg, cl) : recalcul proportionnel direct.
   * R7 (pièce, gousse) : recalcul + affichage en fractions courantes (1/4, 1/2, 3/4).
   */
  calculerQuantite(quantiteBase: number, unite: string | null | undefined): string {
    const base = this.recette?.nombrePersonnesBase ?? 4;
    const ratio = this.personnesAffichees / base;
    const valeur = quantiteBase * ratio;

    const uniteLower = (unite ?? '').toLowerCase();
    const estPiece = uniteLower === 'pièce' || uniteLower === 'piece' || uniteLower === 'gousse';

    if (estPiece) {
      return this.formatFraction(valeur);
    }
    return Number.isInteger(valeur)
      ? valeur.toString()
      : valeur.toFixed(1).replace(/\.0$/, '');
  }

  private formatFraction(v: number): string {
    return Math.max(1, Math.round(v)).toString();
  }

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