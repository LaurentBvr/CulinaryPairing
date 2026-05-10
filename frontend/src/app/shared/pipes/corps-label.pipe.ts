import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'corpsLabel',
  standalone: true,
})
export class CorpsLabelPipe implements PipeTransform {
  private static readonly LABELS: Record<string, string> = {
    Leger: 'Léger',
    Moyen: 'Moyen',
    Corse: 'Corsé',
  };

  transform(value: string | null | undefined): string {
    if (!value) return '';
    return CorpsLabelPipe.LABELS[value] ?? value;
  }
}