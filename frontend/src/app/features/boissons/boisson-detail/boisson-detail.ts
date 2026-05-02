import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BoissonsService, BoissonDetail as BoissonDetailDto, AccordInverse } from '../../../core/services/boissons';
import { AccordCard, AccordCardData } from '../../../shared/components/accord-card/accord-card';

@Component({
  selector: 'app-boisson-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, AccordCard],
  templateUrl: './boisson-detail.html',
  styleUrl: './boisson-detail.scss'
})
export class BoissonDetail implements OnInit {
  private route = inject(ActivatedRoute);
  private boissonsService = inject(BoissonsService);

  boisson: BoissonDetailDto | null = null;
  accords: AccordInverse[] = [];
  loading = true;
  loadingAccords = true;
  error = '';
  accordsError = '';

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id) {
      this.error = 'Identifiant de boisson invalide.';
      this.loading = false;
      return;
    }

    this.boissonsService.getById(id).subscribe({
      next: data => {
        this.boisson = data;
        this.loading = false;
      },
      error: err => {
        this.error = err.status === 404
          ? 'Boisson introuvable.'
          : 'Impossible de charger la boisson.';
        this.loading = false;
        this.loadingAccords = false;
      }
    });

    // Accord inversé V1.3 : recettes accordées avec cette boisson
    this.boissonsService.getAccordsInverse(id, 20).subscribe({
      next: data => {
        this.accords = data;
        this.loadingAccords = false;
      },
      error: () => {
        this.accordsError = 'Impossible de charger les recettes accordées.';
        this.loadingAccords = false;
      }
    });
  }

  /**
   * Mappe un AccordInverse vers le DTO du composant partagé AccordCard.
   * En mode 'boisson', on met en avant la RECETTE (titre + type de plat).
   */
  toAccordCardData(a: AccordInverse): AccordCardData {
    return {
      idAccord: a.idAccord,
      scoreCompatibilite: a.scoreCompatibilite,
      niveauConfiance: a.niveauConfiance,
      malusApplique: a.malusApplique,
      reglesSatisfaites: a.reglesSatisfaites,
      justification: a.justification,
      titre: a.titre,
      sousTitre: a.typePlat
    };
  }

  formaterOrigine(): string {
    if (!this.boisson) return '';
    const parts = [
      this.boisson.appellation,
      this.boisson.region,
      this.boisson.pays
    ].filter(p => !!p);
    return parts.join(' · ');
  }

  formaterTemperatureService(): string | null {
    if (!this.boisson || this.boisson.temperatureOptimale === null) return null;
    const t = this.boisson.temperatureOptimale;
    const tol = this.boisson.toleranceTemperature;
    return `${t}°C (±${tol}°C)`;
  }
}