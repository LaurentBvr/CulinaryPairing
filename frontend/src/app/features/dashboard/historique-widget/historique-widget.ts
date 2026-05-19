import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { HistoriqueService, HistoriqueRecetteDto } from '../../../core/services/historique';
import { AuthService } from '../../../core/services/auth';

@Component({
  selector: 'app-historique-widget',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './historique-widget.html',
  styleUrl: './historique-widget.scss'
})
export class HistoriqueWidget implements OnInit {
  private historiqueService = inject(HistoriqueService);
  private auth = inject(AuthService);

  recettes = signal<HistoriqueRecetteDto[]>([]);
  loading = signal<boolean>(true);

  ngOnInit(): void {
    if (!this.auth.isLoggedIn()) {
      this.loading.set(false);
      return;
    }

    this.historiqueService.getMesDernieres(5).subscribe({
      next: data => {
        this.recettes.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.recettes.set([]);
        this.loading.set(false);
      }
    });
  }
}