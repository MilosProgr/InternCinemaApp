import {HateoasResponse} from './hateoas.model';


export interface Rating extends HateoasResponse {


    userId:number;


    movieId:number;


    stars:number;


    createdAt:string;


}