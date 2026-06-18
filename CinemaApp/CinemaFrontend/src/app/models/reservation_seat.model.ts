import {HateoasResponse} from './hateoas.model';


export interface ReservationSeat extends HateoasResponse {


    reservationId:number;


    seatNumber:string;

}