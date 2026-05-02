import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BoissonsService, BoissonListItem } from '../../../core/services/boissons';

@Component({
  selector: 'app-boisson-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './boisson-list.html',
  styleUrl: './boisson-list.scss'
})
export class BoissonList implements OnInit {
  private boissonsService = inject(BoissonsService);

  boissons: BoissonListItem[] = [];
  loading = true;
  error = '';
  filtreType: string = 'tous';

  ngOnInit() {
    this.boissonsService.getAll().subscribe({
      next: data => {
        this.boissons = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Impossible de charger les boissons.';
        this.loading = false;
      }
    });
  }

  /** Liste des types disponibles, dérivée des données pour rester en phase avec le seed. */
  get typesDisponibles(): string[] {
    const set = new Set<string>();
    this.boissons.forEach(b => { if (b.typeBoisson) set.add(b.typeBoisson); });
    return Array.from(set).sort();
  }

  get boissonsFiltrees(): BoissonListItem[] {
    if (this.filtreType === 'tous') return this.boissons;
    return this.boissons.filter(b => b.typeBoisson === this.filtreType);
  }

  formaterOrigine(b: BoissonListItem): string {
    const parts = [b.region, b.pays].filter(p => !!p);
    return parts.join(', ');
  }
}