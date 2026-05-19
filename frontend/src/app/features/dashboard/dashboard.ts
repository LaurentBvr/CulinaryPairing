import { Component, computed, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth';
import { RecetteService, Recette } from '../../core/services/recette';
import { HistoriqueWidget } from './historique-widget/historique-widget';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, HistoriqueWidget],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit {
  private auth = inject(AuthService);
  private recetteService = inject(RecetteService);

  user = signal<{ prenom: string; nom: string; email: string } | null>(null);
  recettesRecentes = signal<Recette[]>([]);
  totalRecettes = signal<number>(0);
  loading = signal<boolean>(true);

  // Date du jour formatée en français
  dateAujourdHui = computed(() => {
    const d = new Date();
    const jours = ['DIMANCHE', 'LUNDI', 'MARDI', 'MERCREDI', 'JEUDI', 'VENDREDI', 'SAMEDI'];
    const mois = ['JAN', 'FÉV', 'MAR', 'AVR', 'MAI', 'JUIN', 'JUIL', 'AOÛT', 'SEPT', 'OCT', 'NOV', 'DÉC'];
    const heure = d.getHours().toString().padStart(2, '0');
    const minutes = d.getMinutes().toString().padStart(2, '0');
    return `${jours[d.getDay()]} ${d.getDate().toString().padStart(2, '0')} ${mois[d.getMonth()]} · ${heure}H${minutes}`;
  });

  // Salutation selon l'heure
  salutation = computed(() => {
    const h = new Date().getHours();
    if (h < 12) return 'Bonjour';
    if (h < 18) return 'Bon après-midi';
    return 'Bonsoir';
  });

  ngOnInit(): void {
    this.auth.currentUser$.subscribe(u => {
      if (u) this.user.set({ prenom: u.prenom, nom: u.nom, email: u.email });
    });

    this.recetteService.getAll().subscribe({
      next: recettes => {
        this.totalRecettes.set(recettes.length);
        this.recettesRecentes.set(recettes.slice(0, 3));
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }
}