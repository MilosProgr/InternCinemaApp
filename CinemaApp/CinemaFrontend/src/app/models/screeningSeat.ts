export interface ScreeningSeat {
    id: number;
    movieScreeningId: number;
    row: string;
    number: number;
    isOccupied: boolean;
    reservationId?: number;

}