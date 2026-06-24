

export interface User  {


    username:string;

    email:string;

    firstName:string;

    lastName:string;

    role: 'ADMIN' | 'CONSUMER';

    isBlocked:boolean;

}