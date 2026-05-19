import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { RecetteService, Recette, ModeAdaptation } from '../../../core/services/recette';
import { AccordsService, Accord } from '../../../core/services/accords';
import { FavorisService } from '../../../core/services/favoris';
import { AuthService } from '../../../core/services/auth';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ScoreGauge } from '../../../shared/components/score-gauge/score-gauge';
import { HistoriqueService } from '../../../core/services/historique';

@Component({
  selector: 'app-recette-detail',
  standalone: true,
  imports: [RouterLink, CommonModule, FormsModule, ScoreGauge],
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
  private historiqueService = inject(HistoriqueService);

  recette = signal<Recette | null>(null);
  accords = signal<Accord[]>([]);
  loading = signal<boolean>(true);
  error = signal<string>('');
  mode = signal<ModeAdaptation>('original');
  personnesAffichees = signal<number>(4);
  private idRecette = 0;

  // Temps total formaté ("3h45" ou "55 min")
  tempsTotalFormate = computed(() => {
    const r = this.recette();
    if (!r) return '';
    const total = (r.tempsPreparation ?? 0) + (r.tempsCuisson ?? 0);
    if (total >= 60) {
      const h = Math.floor(total / 60);
      const m = total % 60;
      return m === 0 ? `${h}h` : `${h}h${m.toString().padStart(2, '0')}`;
    }
    return `${total} min`;
  });

  ngOnInit() {
    this.idRecette = Number(this.route.snapshot.paramMap.get('id'));
    this.loadRecette();

    this.accordsService.getByRecette(this.idRecette).subscribe({
      next: accords => this.accords.set(accords),
      error: () => {}
    });
  }

  changeMode(newMode: ModeAdaptation) {
    if (this.mode() === newMode) return;
    this.mode.set(newMode);
    this.loadRecette();
  }

  /** R2 : nombre de personnes ∈ [1..10] */
  changePersonnes(value: number) {
    if (value < 1) value = 1;
    if (value > 10) value = 10;
    this.personnesAffichees.set(value);
  }

  incrementPersonnes() {
    if (this.personnesAffichees() < 10) this.changePersonnes(this.personnesAffichees() + 1);
  }

  decrementPersonnes() {
    if (this.personnesAffichees() > 1) this.changePersonnes(this.personnesAffichees() - 1);
  }

  onToggleFavori(): void {
    if (!this.auth.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }
    this.favorisService.toggle(this.idRecette).subscribe();
  }

  private loadRecette() {
    this.loading.set(true);
    this.recetteService.getById(this.idRecette, this.mode()).subscribe({
      next: data => {
        this.recette.set(data);
        this.personnesAffichees.set(data.nombrePersonnesBase ?? 4);
        this.loading.set(false);

        // Enregistre la consultation si l'utilisateur est authentifie.
        // Fire-and-forget : l'echec eventuel ne doit pas degrader l'UX.
        if (this.auth.isLoggedIn()) {
          this.historiqueService.enregistrer(this.idRecette).subscribe({
            error: () => {}
          });
        }
      },
      error: () => { this.error.set('Recette introuvable.'); this.loading.set(false); }
    });
  }

  /**
   * R6 (g, ml, l, kg, cl) : recalcul proportionnel direct.
   * R7 (pièce, gousse) : recalcul + arrondi à l'unité.
   */
  calculerQuantite(quantiteBase: number, unite: string | null | undefined): string {
    const r = this.recette();
    const base = r?.nombrePersonnesBase ?? 4;
    const ratio = this.personnesAffichees() / base;
    const valeur = quantiteBase * ratio;

    const uniteLower = (unite ?? '').toLowerCase();
    const estPiece = uniteLower === 'pièce' || uniteLower === 'piece' || uniteLower === 'gousse';

    if (estPiece) {
      return Math.max(1, Math.round(valeur)).toString();
    }
    return Number.isInteger(valeur)
      ? valeur.toString()
      : valeur.toFixed(1).replace(/\.0$/, '');
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

  // Convertit numéro étape en chiffres romains (I, II, III, IV...)
  toRoman(num: number): string {
    const romans: [number, string][] = [
      [10, 'X'], [9, 'IX'], [5, 'V'], [4, 'IV'], [1, 'I']
    ];
    let result = '';
    for (const [val, sym] of romans) {
      while (num >= val) {
        result += sym;
        num -= val;
      }
    }
    return result;
  }
}