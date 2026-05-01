import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { RecetteService, Recette } from '../../../core/services/recette';
import { FavorisService } from '../../../core/services/favoris';
import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-recette-list',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './recette-list.html',
  styleUrl: './recette-list.scss'
})
export class RecetteList implements OnInit {
  private recetteService = inject(RecetteService);
  protected favorisService = inject(FavorisService);
  private auth = inject(AuthService);
  private router = inject(Router);

  // Signals : permettent un filtrage réactif via computed()
  recettes = signal<Recette[]>([]);
  masquerIncompatibles = signal(true); // R9 strict par défaut, override possible
  loading = true;
  error = '';

  // Vue filtrée : si toggle activé, exclut les recettes ayant ≥1 contrainte violée
  recettesAffichees = computed(() => {
    const all = this.recettes();
    if (!this.masquerIncompatibles()) return all;
    return all.filter(r => !r.contraintesViolees || r.contraintesViolees.length === 0);
  });

  // Compteur masqué pour info utilisateur
  nbMasquees = computed(() =>
    this.recettes().filter(r => r.contraintesViolees && r.contraintesViolees.length > 0).length
  );

  // Helper : visible si l'user est connecté ET qu'au moins 1 recette est incompatible
  toggleVisible = computed(() => this.auth.isLoggedIn() && this.nbMasquees() > 0);

  ngOnInit() {
    this.recetteService.getAll().subscribe({
      next: data => { this.recettes.set(data); this.loading = false; },
      error: () => { this.error = 'Erreur lors du chargement des recettes.'; this.loading = false; }
    });
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

  // Concatène les noms de contraintes violées pour l'affichage badge
  contraintesLabel(r: Recette): string {
    return (r.contraintesViolees ?? []).map(c => c.nom).join(', ');
  }
}