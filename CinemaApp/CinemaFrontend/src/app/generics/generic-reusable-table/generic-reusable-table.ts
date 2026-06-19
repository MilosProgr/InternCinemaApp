import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface Identifiable {
  id: number;
}

export interface TableColumn<T> {
  label: string;
  key: keyof T;
}

@Component({
  selector: 'app-generic-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './generic-reusable-table.html',
  styleUrl: './generic-reusable-table.css'
})
export class GenericTableComponent<T extends Identifiable> {

  @Input() data: T[] = [];

  @Input() columns: TableColumn<T>[] = [];

  @Input() showActions: boolean = false;


  @Output() edit = new EventEmitter<T>();

  @Output() delete = new EventEmitter<number>();


  getValue(item: T, key: keyof T): any {
    return item[key];
  }


  onEdit(item: T): void {
    this.edit.emit(item);
  }


  onDelete(id: number): void {
    this.delete.emit(id);
  }
}