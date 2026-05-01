import { Component, inject, OnInit } from '@angular/core';
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

  recettes: Recette[] = [];
  loading = true;
  error = '';

  ngOnInit() {
    this.recetteService.getAll().subscribe({
      next: data => { this.recettes = data; this.loading = false; },
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
}