// import {HateoasResponse} from './hateoas.model';
import {Movie} from './movie.model';


export interface MovieScreening  {

    id: number,
    
    movieId:number;

    movie?:Movie | null;

    startTime:string;

    ticketPrice:number;

}