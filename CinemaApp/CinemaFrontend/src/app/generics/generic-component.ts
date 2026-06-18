import { Component, Input, OnInit } from '@angular/core';
import { CrudService } from './generic-service';


@Component({
    template: ''
})
export abstract class GenericCrudComponent<T> implements OnInit {
    @Input() entities: T[] = [];
    selectedEntity: T | null = null;

    constructor(
        private crudService: CrudService<T>,
    ) { }

    ngOnInit(): void {
        this.getAllEntities();
    }

    getAllEntities() {
        this.crudService.getAll().subscribe(entities => {
            this.entities = entities;
        });
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
