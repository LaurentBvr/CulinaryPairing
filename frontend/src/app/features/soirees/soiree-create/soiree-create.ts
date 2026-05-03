import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { SoireesService } from '../../../core/services/soirees';
import { ContraintesService, ContrainteDto } from '../../../core/services/contraintes';

@Component({
  selector: 'app-soiree-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './soiree-create.html',
  styleUrl: './soiree-create.scss'
})
export class SoireeCreate implements OnInit {
  private fb = inject(FormBuilder);
  private soirees = inject(SoireesService);
  private contraintes = inject(ContraintesService);
  private router = inject(Router);

  form: FormGroup;
  catalogue: ContrainteDto[] = [];
  selectedIds = new Set<number>();
  loading = false;
  error = '';

  constructor() {
    this.form = this.fb.group({
      nombrePersonnes: [4, [Validators.required, Validators.min(1), Validators.max(50)]],
      nombreVegetariens: [0, [Validators.required, Validators.min(0)]],
      nombreVegans: [0, [Validators.required, Validators.min(0)]],
      budget: [null],
      tempsDisponible: [null],
      typeSoiree: [''],
      preferenceAlcool: ['Avec', Validators.required]
    });
  }

  ngOnInit() {
    this.contraintes.getCatalogue().subscribe({
      next: data => this.catalogue = data,
      error: () => this.error = 'Impossible de charger les contraintes.'
    });
  }

  /** Regroupe les contraintes par type pour l'affichage. */
  contraintesParType(type: string): ContrainteDto[] {
    return this.catalogue.filter(c => c.type === type);
  }

  toggleContrainte(id: number) {
    if (this.selectedIds.has(id)) this.selectedIds.delete(id);
    else this.selectedIds.add(id);
  }

  estSelectionnee(id: number): boolean {
    return this.selectedIds.has(id);
  }

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const v = this.form.value;
    const total = (v.nombreVegetariens ?? 0) + (v.nombreVegans ?? 0);
    if (total > v.nombrePersonnes) {
      this.error = 'Le nombre de végétariens + végans ne peut pas dépasser le nombre de personnes.';
      return;
    }

    this.loading = true;
    this.error = '';

    this.soirees.create({
      nombrePersonnes: v.nombrePersonnes,
      nombreVegetariens: v.nombreVegetariens,
      nombreVegans: v.nombreVegans,
      budget: v.budget || null,
      tempsDisponible: v.tempsDisponible || null,
      typeSoiree: v.typeSoiree?.trim() || null,
      preferenceAlcool: v.preferenceAlcool,
      contraintesIds: Array.from(this.selectedIds)
    }).subscribe({
      next: idSoiree => {
        this.router.navigate(['/soirees', idSoiree]);
      },
      error: err => {
        this.error = err.error?.detail || err.error?.title || 'Erreur lors de la création.';
        this.loading = false;
      }
    });
  }
}