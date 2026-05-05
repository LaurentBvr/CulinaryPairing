import { Component, Input, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-score-gauge',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './score-gauge.html',
  styleUrl: './score-gauge.scss',
})
export class ScoreGauge {
  @Input({ required: true }) set score(v: number) {
    this._score.set(Math.max(0, Math.min(100, v)));
  }

  @Input() size: number = 88;
  @Input() showLabel: boolean = true;

  protected _score = signal<number>(0);

  // Constantes SVG
  protected readonly radius = 42;
  protected readonly circumference = 2 * Math.PI * this.radius;

  // Couleur selon le score
  protected color = computed(() => {
    const s = this._score();
    if (s >= 75) return 'var(--score-high)';
    if (s >= 50) return 'var(--score-mid)';
    return 'var(--score-low)';
  });

  // Libellé selon le score
  protected label = computed(() => {
    const s = this._score();
    if (s >= 75) return 'Excellent';
    if (s >= 50) return 'Bon';
    return 'Faible';
  });

  // Offset du dasharray pour dessiner l'arc
  protected dashOffset = computed(() => {
    return this.circumference * (1 - this._score() / 100);
  });

  // Taille de la police du chiffre central (proportionnelle)
  protected fontSize = computed(() => this.size * 0.32);
  protected labelFontSize = computed(() => Math.max(9, this.size * 0.11));
}