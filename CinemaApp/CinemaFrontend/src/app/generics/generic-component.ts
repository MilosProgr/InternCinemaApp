import { Component, Input, OnInit, ChangeDetectorRef  } from '@angular/core';
import { CrudService } from './generic-service';
import { Link } from '../models/hateoas.model';


@Component({
    template: ''
})
export abstract class GenericCrudComponent<T> implements OnInit {
    entities: T[] = [];
    selectedEntity: T | null = null;

    // paginacija
    currentPage: number = 1;
    pageSize: number = 10;
    totalPages: number = 1;
    links: Link[] = [];

    constructor(
        protected crudService: CrudService<T>,
        protected cdr: ChangeDetectorRef 
    ) { }

    ngOnInit(): void {
        this.getAllEntities();
    }

    getAllEntities() {
        this.crudService.getPaged(this.currentPage, this.pageSize).subscribe(result => {
            this.entities = [...result.items];
            this.totalPages = result.totalPages;
            this.links = result.links;
            this.cdr.detectChanges();
        });
    }

    goToPage(page: number) {
        if (page < 1 || page > this.totalPages) return;
        this.currentPage = page;
        this.getAllEntities();
    }

    create(formValues: any) {
        if (!formValues) return;

        const data = formValues;
        if (data.id) {
            // Update existing record
            this.crudService.update(data.id, data).subscribe(() => this.getAllEntities());
        } else {
            // Create new record
            this.crudService.create(data).subscribe(() => this.getAllEntities());
        }
    }

    delete(id: number) {
        this.crudService.delete(id).subscribe(() => this.getAllEntities());
    }

    edit(entity: T) {
        this.selectedEntity = { ...entity };
    }
}
