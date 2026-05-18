import { Component, inject, OnInit } from '@angular/core';
import { ContraintesService, ContrainteDto } from '../../core/services/contraintes';
import { AuthService } from '../../core/services/auth';
import { AsyncPipe } from '@angular/common';

interface ContrainteGroup {
  type: string;
  label: string;
  items: ContrainteDto[];
}

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [AsyncPipe],
  templateUrl: './profile.html',
  styleUrl: './profile.scss'
})
export class Profile implements OnInit {
  private contraintesService = inject(ContraintesService);
  auth = inject(AuthService);

  groups: ContrainteGroup[] = [];
  selectedIds = new Set<number>();
  loading = true;
  saving = false;
  message = '';
  error = '';

  private readonly typeLabels: Record<string, string> = {
    'Regime': 'Régimes alimentaires',
    'Allergie': 'Allergies',
    'Conviction': 'Convictions'
  };

  private readonly typeHints: Record<string, string> = {
    'Regime': 'Régimes que vous suivez : végétarien, végan, sans gluten, sans lactose. Influence le filtre R9 sur les recettes.',
    'Allergie': 'Allergies alimentaires strictes (arachides, fruits de mer, etc.). Les recettes incompatibles seront masquées (R9 strict).',
    'Conviction': 'Convictions religieuses ou éthiques : halal, casher, etc. Les recettes non-conformes seront masquées.'
  };

  ngOnInit(): void {
    this.contraintesService.getCatalogue().subscribe({
      next: catalogue => {
        this.groups = this.groupByType(catalogue);
        this.contraintesService.getMesContraintes().subscribe({
          next: mes => {
            this.selectedIds = new Set(mes.map(c => c.idContrainte));
            this.loading = false;
          },
          error: () => { this.error = 'Erreur de chargement de vos contraintes.'; this.loading = false; }
        });
      },
      error: () => { this.error = 'Erreur de chargement du catalogue.'; this.loading = false; }
    });
  }

  private groupByType(catalogue: ContrainteDto[]): ContrainteGroup[] {
    const map = new Map<string, ContrainteDto[]>();
    for (const c of catalogue) {
      if (!map.has(c.type)) map.set(c.type, []);
      map.get(c.type)!.push(c);
    }
    const order = ['Regime', 'Allergie', 'Conviction'];
    return order
      .filter(t => map.has(t))
      .map(t => ({
        type: t,
        label: this.typeLabels[t] ?? t,
        items: map.get(t)!.sort((a, b) => a.nom.localeCompare(b.nom))
      }));
  }

  toggle(id: number): void {
    if (this.selectedIds.has(id)) this.selectedIds.delete(id);
    else this.selectedIds.add(id);
  }

  isSelected(id: number): boolean {
    return this.selectedIds.has(id);
  }

  hasSelectedInGroup(group: ContrainteGroup): boolean {
    return group.items.some(c => this.selectedIds.has(c.idContrainte));
  }

  groupHint(type: string): string {
    return this.typeHints[type] ?? '';
  }

  totalContraintes(): number {
    return this.groups.reduce((acc, g) => acc + g.items.length, 0);
  }

  initiales(user: { prenom: string; nom: string }): string {
    const p = user.prenom?.charAt(0) ?? '';
    const n = user.nom?.charAt(0) ?? '';
    return (p + n).toUpperCase();
  }

  save(): void {
    this.saving = true;
    this.message = '';
    this.error = '';
    this.contraintesService.updateMesContraintes(Array.from(this.selectedIds)).subscribe({
      next: () => { this.message = 'Préférences enregistrées.'; this.saving = false; },
      error: () => { this.error = 'Erreur lors de l\'enregistrement.'; this.saving = false; }
    });
  }
}