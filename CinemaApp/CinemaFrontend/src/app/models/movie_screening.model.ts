// import {HateoasResponse} from './hateoas.model';
import {Movie} from './movie.model';


export interface MovieScreening  {

    id: number,
    
    movieId:number;

    movie:Movie;

    startTime:string;

    ticketPrice:number;

}