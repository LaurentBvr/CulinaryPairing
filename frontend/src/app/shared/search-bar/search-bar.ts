import { Component, ElementRef, HostListener, inject, signal } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { SearchResult, SearchService } from '../../core/services/search';

/**
 * Barre de recherche transversale insérée dans la navbar.
 * Dropdown live avec debounce 300ms, top 5 par catégorie.
 * Sur Enter ou clic "Voir tous les résultats" → /recherche?q=...
 * Sur clic d'un item → destination logique :
 *   - recette/boisson → page détail
 *   - ingrédient → /recettes?ingredient=X
 *   - type de plat → /recettes?type=X
 */
@Component({
  selector: 'app-search-bar',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './search-bar.html',
  styleUrl: './search-bar.scss'
})
export class SearchBar {
  private searchService = inject(SearchService);
  private router = inject(Router);
  private elementRef = inject(ElementRef);

  query = new FormControl<string>('', { nonNullable: true });
  results = signal<SearchResult | null>(null);
  isOpen = signal<boolean>(false);
  isLoading = signal<boolean>(false);

  constructor() {
    this.query.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        switchMap(q => {
          const trimmed = q?.trim() ?? '';
          if (trimmed.length < 2) {
            this.results.set(null);
            this.isOpen.set(false);
            this.isLoading.set(false);
            return [];
          }
          this.isLoading.set(true);
          this.isOpen.set(true);
          return this.searchService.search(trimmed, 5);
        })
      )
      .subscribe(result => {
        this.results.set(result);
        this.isLoading.set(false);
      });
  }

  /** Fermer le dropdown en cas de clic à l'extérieur. */
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.isOpen.set(false);
    }
  }

  /** Submit (Enter) → page complète /recherche. */
  onSubmit() {
    const q = this.query.value?.trim() ?? '';
    if (q.length < 2) return;
    this.isOpen.set(false);
    this.router.navigate(['/recherche'], { queryParams: { q } });
  }

  /** Réouvre le dropdown au focus si on a déjà des résultats. */
  onFocus() {
    if (this.results() && (this.results()?.totalResultats ?? 0) > 0) {
      this.isOpen.set(true);
    }
  }

  /** Navigation au clic sur un résultat → ferme le dropdown et reset. */
  onResultClick() {
    this.isOpen.set(false);
    this.query.setValue('', { emitEvent: false });
    this.results.set(null);
  }

  /** Lien "Voir tous les résultats" en bas du dropdown. */
  onSeeAll() {
    this.onSubmit();
  }
}