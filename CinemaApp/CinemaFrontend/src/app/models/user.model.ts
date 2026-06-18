import {HateoasResponse} from './hateoas.model';


export interface User extends HateoasResponse {


    username:string;

    email:string;

    firstName:string;

    lastName:string;

    role: 'ADMIN' | 'CONSUMER';

    isBlocked:boolean;

}