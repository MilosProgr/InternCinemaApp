import { Genre } from "./genre.model";
// import { HateoasResponse } from "./hateoas.model";

export interface Movie {

    id: number;
    
    name:string;

    originalName:string;

    duration:number;

    posterUrl:string;

    genres?: Genre[];  

    averageRating?:number;
}