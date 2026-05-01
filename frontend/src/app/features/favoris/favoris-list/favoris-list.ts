import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FavorisService, FavoriDto } from '../../../core/services/favoris';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-favoris-list',
  standalone: true,
  imports: [RouterLink, DatePipe],
  templateUrl: './favoris-list.html',
  styleUrl: './favoris-list.scss'
})
export class FavorisList implements OnInit {
  private favorisService = inject(FavorisService);

  favoris: FavoriDto[] = [];
  loading = true;
  error = '';

  ngOnInit(): void {
    this.favorisService.getAll().subscribe({
      next: data => { this.favoris = data; this.loading = false; },
      error: () => { this.error = 'Erreur lors du chargement des favoris.'; this.loading = false; }
    });
  }

  onRetirer(event: Event, idRecette: number): void {
    event.preventDefault();
    event.stopPropagation();
    this.favorisService.toggle(idRecette).subscribe({
      next: () => {
        this.favoris = this.favoris.filter(f => f.idRecette !== idRecette);
      }
    });
  }
}