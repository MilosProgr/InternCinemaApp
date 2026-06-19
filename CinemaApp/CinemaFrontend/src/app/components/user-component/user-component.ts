import { Component } from '@angular/core';
import { GenericTableComponent } from '../../generics/generic-reusable-table/generic-reusable-table';

@Component({
  selector: 'app-user-component',
  imports: [GenericTableComponent],
  templateUrl: './user-component.html',
  styleUrl: './user-component.css',
})
export class UserComponent {}
