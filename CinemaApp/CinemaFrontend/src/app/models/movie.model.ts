import { Genre } from "./genre.model";
// import { HateoasResponse } from "./hateoas.model";

export interface Movie {

    name:string;

    originalName:string;

    duration:number;

    posterUrl:string;

    genreId:number;

    genre?:Genre;

    averageRating?:number;
}