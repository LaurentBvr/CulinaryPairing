import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AsyncPipe } from '@angular/common';
import { RecetteService, Recette } from '../../../core/services/recette';

@Component({
  selector: 'app-recette-list',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './recette-list.html',
  styleUrl: './recette-list.scss'
})
export class RecetteList implements OnInit {
  private recetteService = inject(RecetteService);

  recettes: Recette[] = [];
  loading = true;
  error = '';

  ngOnInit() {
    this.recetteService.getAll().subscribe({
      next: data => { this.recettes = data; this.loading = false; },
      error: () => { this.error = 'Erreur lors du chargement des recettes.'; this.loading = false; }
    });
  }
}