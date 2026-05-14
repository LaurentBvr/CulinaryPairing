import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { SearchResult, SearchService } from '../../../core/services/search';

/**
 * Page de recherche transversale complète accessible via /recherche?q=...
 * Atterrissage depuis :
 *   - Enter dans la SearchBar de la navbar
 *   - Clic "Voir tous les résultats →" en bas du dropdown
 * Limite remontée à 50 par catégorie (vs 5 dans le dropdown live).
 * Réactive aux changements de queryParams : taper une nouvelle recherche depuis
 * la navbar pendant qu'on est sur /recherche met à jour la page sans navigation.
 */
@Component({
  selector: 'app-recherche-page',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './recherche-page.html',
  styleUrl: './recherche-page.scss'
})
export class RecherchePage implements OnInit {
  private route = inject(ActivatedRoute);
  private searchService = inject(SearchService);

  query = signal<string>('');
  results = signal<SearchResult | null>(null);
  loading = signal<boolean>(false);
  error = signal<string>('');

  hasResults = computed(() => (this.results()?.totalResultats ?? 0) > 0);

  ngOnInit() {
    this.route.queryParamMap.subscribe(params => {
      const q = (params.get('q') ?? '').trim();
      this.query.set(q);

      if (q.length < 2) {
        this.results.set(null);
        this.loading.set(false);
        return;
      }

      this.loading.set(true);
      this.error.set('');
      this.searchService.search(q, 50).subscribe({
        next: r => {
          this.results.set(r);
          this.loading.set(false);
        },
        error: () => {
          this.error.set('Erreur lors de la recherche.');
          this.loading.set(false);
        }
      });
    });
  }
}