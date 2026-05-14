import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { RecetteService, Recette } from '../../../core/services/recette';
import { FavorisService } from '../../../core/services/favoris';
import { AuthService } from '../../../core/services/auth';

type FiltreType = 'Toutes' | 'Entrée' | 'Plat' | 'Dessert';
type FiltreDifficulte = 'Toutes' | 'Facile' | 'Moyen' | 'Difficile';

@Component({
  selector: 'app-recette-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './recette-list.html',
  styleUrl: './recette-list.scss'
})
export class RecetteList implements OnInit {
  private recetteService = inject(RecetteService);
  protected favorisService = inject(FavorisService);
  private auth = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  // Filtres URL-driven (depuis SearchBar) — affichés en pilule au-dessus de la grille
  filtreIngredient = signal<string | null>(null);
  filtreTypeUrl = signal<string | null>(null);

  // Données brutes
  recettes = signal<Recette[]>([]);
  loading = true;
  error = '';

  // Filtres
  filtreType = signal<FiltreType>('Toutes');
  filtreDifficulte = signal<FiltreDifficulte>('Toutes');
  masquerIncompatibles = signal(true); // R9 strict par défaut, override possible

  // Helper : compare 2 strings en ignorant accents et casse
private normalize(s: string): string {
  return s.normalize('NFD').replace(/[\u0300-\u036f]/g, '').toLowerCase();
}

// Vue filtrée : type + difficulté + masquer incompatibles
recettesAffichees = computed(() => {
  let result = this.recettes();

  // Filtre type (tolérant aux accents)
  if (this.filtreType() !== 'Toutes') {
    const target = this.normalize(this.filtreType());
    result = result.filter(r => this.normalize(r.typeRepas) === target);
  }

  // Filtre difficulté (tolérant aux accents)
  if (this.filtreDifficulte() !== 'Toutes') {
    const target = this.normalize(this.filtreDifficulte());
    result = result.filter(r => this.normalize(r.niveauDifficulte) === target);
  }

  // Masquer incompatibles (R9 + R17/R18/R19 : on ne masque que les strictement incompatibles, pas les adaptables)
  if (this.masquerIncompatibles()) {
    result = result.filter(r => r.statutCompatibilite !== 'Incompatible');
  }

  return result;
});

  // Compteurs pour les chips de filtre (calculés sur l'ensemble brut, pas sur le filtré)
  countToutes = computed(() => this.recettes().length);
  countEntrees = computed(() => this.recettes().filter(r => this.normalize(r.typeRepas) === 'entree').length);
  countPlats = computed(() => this.recettes().filter(r => this.normalize(r.typeRepas) === 'plat').length);
  countDesserts = computed(() => this.recettes().filter(r => this.normalize(r.typeRepas) === 'dessert').length);

  // Compteur masqué pour info utilisateur (uniquement les strictement incompatibles)
  nbMasquees = computed(() =>
    this.recettes().filter(r => r.statutCompatibilite === 'Incompatible').length
  );

  // Compteur des recettes adaptables (info utile pour le badge "Adaptable")
  nbAdaptables = computed(() =>
    this.recettes().filter(r => r.statutCompatibilite === 'Adaptable').length
  );

  // Helper : visible si l'user est connecté ET qu'au moins 1 recette est incompatible
  toggleVisible = computed(() => this.auth.isLoggedIn() && this.nbMasquees() > 0);

  ngOnInit() {
    // Réactif aux changements de queryParams : si l'utilisateur clique sur un autre
    // ingrédient dans la navbar, on recharge la liste avec le nouveau filtre sans
    // unmount/remount du composant.
    this.route.queryParamMap.subscribe(params => {
      const ingredient = params.get('ingredient');
      const type = params.get('type');

      this.filtreIngredient.set(ingredient);
      this.filtreTypeUrl.set(type);

      this.loading = true;
      this.recetteService.getAll({
        ingredient: ingredient ?? undefined,
        type: type ?? undefined
      }).subscribe({
        next: data => { this.recettes.set(data); this.loading = false; },
        error: () => { this.error = 'Erreur lors du chargement des recettes.'; this.loading = false; }
      });
    });
  }

  /**
   * Retire les filtres URL (clic sur la croix de la pilule filtrante).
   * On reset les queryParams, ce qui déclenche ngOnInit qui rechargera sans filtres.
   */
  clearFiltresUrl(): void {
    this.router.navigate(['/recettes'], { queryParams: {} });
  }

  onToggleFavori(event: Event, idRecette: number): void {
    event.preventDefault();
    event.stopPropagation();
    if (!this.auth.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }
    this.favorisService.toggle(idRecette).subscribe();
  }

  toggleMasquer(): void {
    this.masquerIncompatibles.update(v => !v);
  }

  setFiltreType(type: FiltreType): void {
    this.filtreType.set(type);
  }

  setFiltreDifficulte(diff: FiltreDifficulte): void {
    this.filtreDifficulte.set(diff);
  }

  // Concatène les noms de contraintes violées pour l'affichage badge
  contraintesLabel(r: Recette): string {
    return (r.contraintesViolees ?? []).map(c => c.nom).join(', ');
  }

  // Label du badge selon le statut de compatibilité (R9 + R17/R18/R19)
  statutLabel(r: Recette): string {
    if (r.statutCompatibilite === 'Adaptable') {
      // Détecter si c'est végétarien ou végan qui est résolvable
      const adaptableType = r.adaptableVegan ? 'Végan' : 'Végétarien';
      return `Adaptable en ${adaptableType}`;
    }
    if (r.statutCompatibilite === 'Incompatible') {
      return 'Incompatible · ' + (r.contraintesViolees ?? []).map(c => c.nom).join(', ');
    }
    return '';
  }
}