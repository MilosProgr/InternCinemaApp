import { HateoasResponse } from './hateoas.model';
import { ReservationSeat } from './reservation_seat.model';


export interface Reservation extends HateoasResponse {


    userId?:number;


    guestEmail?:string;


    movieScreeningId:number;


    reservationCode:string;


    totalPrice:number;


    createdAt:string;


    isCancelled:boolean;


    seats?:ReservationSeat[];

}