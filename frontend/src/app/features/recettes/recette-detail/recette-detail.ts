import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { RecetteService, Recette } from '../../../core/services/recette';
import { AccordsService, Accord } from '../../../core/services/accords';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-recette-detail',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './recette-detail.html',
  styleUrl: './recette-detail.scss'
})
export class RecetteDetail implements OnInit {
  private route = inject(ActivatedRoute);
  private recetteService = inject(RecetteService);
  private accordsService = inject(AccordsService);

  recette: Recette | null = null;
  accords: Accord[] = [];
  loading = true;
  error = '';

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.recetteService.getById(id).subscribe({
      next: data => {
        this.recette = data;
        this.loading = false;
        this.accordsService.getByRecette(id).subscribe({
          next: accords => this.accords = accords,
          error: () => {}
        });
      },
      error: () => { this.error = 'Recette introuvable.'; this.loading = false; }
    });
  }
}