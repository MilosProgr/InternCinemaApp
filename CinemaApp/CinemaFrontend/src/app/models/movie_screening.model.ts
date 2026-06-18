import {HateoasResponse} from './hateoas.model';
import {Movie} from './movie.model';


export interface MovieScreening extends HateoasResponse {

    movieId:number;

    movie:Movie;


    startTime:string;


    ticketPrice:number;


    availableSeats:number;

}