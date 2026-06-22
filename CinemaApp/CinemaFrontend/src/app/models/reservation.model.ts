import { ScreeningSeat } from "./screeningSeat";


export interface Reservation  {


    id: number;
    userId?: number;
    guestEmail?: string;
    movieScreeningId: number;
    reservationCode: string;
    totalPrice: number;
    createdAt: string;
    isCancelled: boolean;
    seats: ScreeningSeat[];   



}

export interface ReservationSeat {
    id:number;
    row:string;
    number:number;
}

export interface CreateReservationRequest {
    movieScreeningId: number;
    seatIds: number[];
    guestEmail?: string | null;
    totalPrice: number;
}

