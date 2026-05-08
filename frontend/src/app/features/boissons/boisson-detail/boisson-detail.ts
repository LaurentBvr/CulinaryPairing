import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BoissonsService, BoissonDetail as BoissonDetailDto, AccordInverse } from '../../../core/services/boissons';
import { ScoreGauge } from '../../../shared/components/score-gauge/score-gauge';

@Component({
  selector: 'app-boisson-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, ScoreGauge],
  templateUrl: './boisson-detail.html',
  styleUrl: './boisson-detail.scss'
})
export class BoissonDetail implements OnInit {
  private route = inject(ActivatedRoute);
  private boissonsService = inject(BoissonsService);

  boisson = signal<BoissonDetailDto | null>(null);
  accords = signal<AccordInverse[]>([]);
  loading = signal<boolean>(true);
  loadingAccords = signal<boolean>(true);
  error = signal<string>('');
  accordsError = signal<string>('');

  // Origine formatée (appellation · région · pays)
  origineFormatee = computed(() => {
    const b = this.boisson();
    if (!b) return '';
    const parts = [b.appellation, b.region, b.pays].filter(p => !!p);
    return parts.join(' · ');
  });

  // Température service formatée
  temperatureServiceFormatee = computed(() => {
    const b = this.boisson();
    if (!b || b.temperatureOptimale === null) return null;
    return `${b.temperatureOptimale}°C (±${b.toleranceTemperature}°C)`;
  });

  // Label type formaté ("VinRouge" → "Vin rouge")
  typeLabel = computed(() => {
    const t = this.boisson()?.typeBoisson;
    if (!t) return '';
    return t.replace(/([A-Z])/g, ' $1').trim().toLowerCase().replace(/^./, c => c.toUpperCase());
  });

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id) {
      this.error.set('Identifiant de boisson invalide.');
      this.loading.set(false);
      return;
    }

    this.boissonsService.getById(id).subscribe({
      next: data => {
        this.boisson.set(data);
        this.loading.set(false);
      },
      error: err => {
        this.error.set(err.status === 404 ? 'Boisson introuvable.' : 'Impossible de charger la boisson.');
        this.loading.set(false);
        this.loadingAccords.set(false);
      }
    });

    this.boissonsService.getAccordsInverse(id, 20).subscribe({
      next: data => {
        this.accords.set(data);
        this.loadingAccords.set(false);
      },
      error: () => {
        this.accordsError.set('Impossible de charger les recettes accordées.');
        this.loadingAccords.set(false);
      }
    });
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

  // Temps total recette formaté
  formatTempsRecette(prep: number | null, cuisson: number | null): string {
    const total = (prep ?? 0) + (cuisson ?? 0);
    if (total === 0) return '';
    if (total >= 60) {
      const h = Math.floor(total / 60);
      const m = total % 60;
      return m === 0 ? `${h}h` : `${h}h${m.toString().padStart(2, '0')}`;
    }
    return `${total} min`;
  }

  scrollToAccords(): void {
  const el = document.getElementById('accords-section');
  if (el) {
    el.scrollIntoView({ behavior: 'smooth', block: 'start' });
  }
}
}