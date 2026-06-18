import { NgFor, NgIf } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input'
@Component({
  selector: 'app-generic-reusable-table',
  imports: [NgIf,NgFor],
  templateUrl: './generic-reusable-table.html',
  styleUrl: './generic-reusable-table.css',
})
export class GenericReusableTable {
  @Input() podaci: unknown[] = [];
  @Input() kljucevi: { imeKolone: string; kljuc: string }[] = [];
  @Input() kljuceviStringovi: string[] = [];

  @Input() editRoute = '/';
  @Input() naslov = '';

  @Output() obrisiEvent = new EventEmitter<number>();
  @Output() izmeniEvent = new EventEmitter<number>();

  currentPage = 1;
  itemsPerPage = 15;

  ukloni(id: number): void {
    this.obrisiEvent.emit(id);
  }

  izmena(id: number): void {
    this.izmeniEvent.emit(id);
  }

  isArray(value: unknown): value is unknown[] {
    return Array.isArray(value);
  }

  isObject(value: unknown): value is Record<string, unknown> {
    return value !== null && typeof value === 'object' && !Array.isArray(value);
  }

  getObjectKeys(obj: Record<string, unknown>): string[] {
    return Object.keys(obj);
  }

  getJoinedValues(value: unknown): string {
    if (Array.isArray(value)) {
      return value
        .map((obj: unknown) => {
          if (typeof obj !== 'object' || obj === null) {
            return String(obj);
          }

          const o = obj as Record<string, unknown>;

          const korisnik = o['korisnik'] as Record<string, unknown> | undefined;

          return (
            (korisnik?.['korisnickoIme'] as string) ||
            (o['ime'] as string) ||
            (o['id'] as string | number) ||
            ''
          );
        })
        .join(', ');
    }

    return String(value);
  }

  getKljucevi(): { imeKolone: string; kljuc: string }[] {
    if (this.kljucevi.length > 0) {
      return this.kljucevi;
    }

    return this.kljuceviStringovi.map(k => ({
      imeKolone: k,
      kljuc: k
    }));
  }
}
