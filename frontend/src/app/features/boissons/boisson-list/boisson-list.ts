import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BoissonsService, BoissonListItem } from '../../../core/services/boissons';

// Types backend (valeurs exactes de l'enum) avec labels lisibles
interface FiltreOption {
  value: string;       // valeur backend (PascalCase)
  label: string;       // libellé affiché
  cssClass: string;    // classe CSS pour le liseré coloré
}

@Component({
  selector: 'app-boisson-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './boisson-list.html',
  styleUrl: './boisson-list.scss'
})
export class BoissonList implements OnInit {
  private boissonsService = inject(BoissonsService);

  boissons = signal<BoissonListItem[]>([]);
  loading = signal<boolean>(true);
  error = signal<string>('');
  filtreType = signal<string>('tous');

  // Mapping backend → label affiché → classe CSS
  // On l'inférera dynamiquement à partir des données pour rester en phase avec le seed
  optionsFiltres = computed<FiltreOption[]>(() => {
    const set = new Set<string>();
    this.boissons().forEach(b => { if (b.typeBoisson) set.add(b.typeBoisson); });
    const types = Array.from(set).sort();
    return types.map(t => ({
      value: t,
      label: this.formaterLabelType(t),
      cssClass: this.getCssClassForType(t)
    }));
  });

  // Compteurs par type
  countByType = computed(() => {
    const result: Record<string, number> = {};
    this.boissons().forEach(b => {
      if (b.typeBoisson) {
        result[b.typeBoisson] = (result[b.typeBoisson] ?? 0) + 1;
      }
    });
    return result;
  });

  // Compteurs agrégés (pour eyebrow en haut à droite)
  totalBoissons = computed(() => this.boissons().length);
  totalVins = computed(() => this.boissons().filter(b => (b.typeBoisson ?? '').toLowerCase().startsWith('vin')).length);
  totalSpiritueux = computed(() => this.boissons().filter(b => {
    const t = (b.typeBoisson ?? '').toLowerCase();
    return t === 'whisky' || t === 'sake';
  }).length);
  totalAutres = computed(() => this.totalBoissons() - this.totalVins() - this.totalSpiritueux());

  boissonsFiltrees = computed(() => {
    if (this.filtreType() === 'tous') return this.boissons();
    return this.boissons().filter(b => b.typeBoisson === this.filtreType());
  });

  ngOnInit() {
    this.boissonsService.getAll().subscribe({
      next: data => {
        this.boissons.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Impossible de charger les boissons.');
        this.loading.set(false);
      }
    });
  }

  setFiltre(value: string): void {
    this.filtreType.set(value);
  }

  /** Formate "VinRouge" → "Vin rouge", "VinDouxNaturel" → "Vin doux naturel" */
  formaterLabelType(type: string): string {
    return type.replace(/([A-Z])/g, ' $1').trim().toLowerCase().replace(/^./, c => c.toUpperCase());
  }

  /** Classe CSS pour le liseré coloré gauche de la card */
  getCssClassForType(type: string): string {
    return type.toLowerCase();
  }

  formaterOrigine(b: BoissonListItem): string {
    const parts = [b.region, b.pays].filter(p => !!p);
    return parts.join(', ');
  }
}