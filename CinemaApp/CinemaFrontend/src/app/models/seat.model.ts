import {HateoasResponse} from './hateoas.model';


export interface Seat extends HateoasResponse {

    row:string;

    number:number;

}