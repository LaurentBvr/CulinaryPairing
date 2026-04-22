import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { RecetteService, Recette } from '../../../core/services/recette';

@Component({
  selector: 'app-recette-detail',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './recette-detail.html',
  styleUrl: './recette-detail.scss'
})
export class RecetteDetail implements OnInit {
  private route = inject(ActivatedRoute);
  private recetteService = inject(RecetteService);

  recette: Recette | null = null;
  loading = true;
  error = '';

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.recetteService.getById(id).subscribe({
      next: data => { this.recette = data; this.loading = false; },
      error: () => { this.error = 'Recette introuvable.'; this.loading = false; }
    });
  }
}