import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { debounceTime, distinctUntilChanged, Subject, switchMap } from 'rxjs';
import { VideFrigoService, IngredientDto, IngredientInfoDto, VideFrigoResultDto } from './vide-frigo.service';

interface SelectedIngredient extends IngredientDto {
  info?: IngredientInfoDto;
}

@Component({
  selector: 'app-vide-frigo',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './vide-frigo.html',
  styleUrl: './vide-frigo.scss'
})
export class VideFrigo {
  searchQuery = '';
  suggestions: IngredientDto[] = [];
  selected: SelectedIngredient[] = [];
  nombreResultats = 10;
  inclureVeg = false;
  results: VideFrigoResultDto[] = [];
  searched = false;
  error = '';

  private search$ = new Subject<string>();

  constructor(private service: VideFrigoService) {
    this.search$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(q => this.service.searchIngredients(q))
    ).subscribe(res => this.suggestions = res);
  }

  onSearch(event: Event) {
    const q = (event.target as HTMLInputElement).value;
    this.searchQuery = q;
    if (q.length >= 2) this.search$.next(q);
    else this.suggestions = [];
  }

  addIngredient(i: IngredientDto) {
    if (!this.selected.find(s => s.idIngredient === i.idIngredient)) {
      const newItem: SelectedIngredient = { ...i };
      this.selected.push(newItem);
      this.service.getIngredientInfo(i.idIngredient).subscribe(info => {
        newItem.info = info;
      });
    }
    this.suggestions = [];
    this.searchQuery = '';
  }

  removeIngredient(i: IngredientDto) {
    this.selected = this.selected.filter(s => s.idIngredient !== i.idIngredient);
  }

  formatInfo(info?: IngredientInfoDto): string {
    if (!info || info.recettesCount === 0) return 'aucune recette';
    const plage = info.minIngredients === info.maxIngredients
      ? `${info.minIngredients} ingrédients`
      : `${info.minIngredients}–${info.maxIngredients} ingrédients`;
    return `${info.recettesCount} recette${info.recettesCount > 1 ? 's' : ''} (${plage})`;
  }

  rechercher() {
    this.error = '';
    this.searched = false;
    this.service.rechercher(
      this.selected.map(s => s.idIngredient),
      this.nombreResultats,
      this.inclureVeg
    ).subscribe({
      next: res => { this.results = res; this.searched = true; },
      error: () => { this.error = 'Erreur lors de la recherche.'; }
    });
  }

  reset() {
    this.selected = [];
    this.results = [];
    this.searched = false;
    this.error = '';
    this.searchQuery = '';
    this.suggestions = [];
  }
}