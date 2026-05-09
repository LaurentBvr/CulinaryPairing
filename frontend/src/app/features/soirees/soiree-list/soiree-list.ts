import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SoireesService, SoireeListItem } from '../../../core/services/soirees';

@Component({
  selector: 'app-soiree-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './soiree-list.html',
  styleUrl: './soiree-list.scss'
})
export class SoireeList implements OnInit {
  private soireesService = inject(SoireesService);

  soirees: SoireeListItem[] = [];
  loading = true;
  error = '';

  ngOnInit() {
    this.charger();
  }

  private charger() {
    this.loading = true;
    this.error = '';
    this.soireesService.getMine().subscribe({
      next: data => {
        this.soirees = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Impossible de charger vos soirées.';
        this.loading = false;
      }
    });
  }

  countMenusComplets(): number {
    return this.soirees.filter(s => s.menuComplet).length;
  }

  supprimer(s: SoireeListItem) {
    const label = s.typeSoiree ?? `${s.nombrePersonnes} personnes`;
    if (!confirm(`Supprimer la soirée "${label}" ? Cette action est irréversible.`)) return;
    this.soireesService.delete(s.idSoiree).subscribe({
      next: () => {
        this.soirees = this.soirees.filter(x => x.idSoiree !== s.idSoiree);
      },
      error: () => {
        alert('La suppression a échoué.');
      }
    });
  }
}