// import { HateoasResponse } from "./hateoas.model";

import { Link } from "./hateoas.model";

export interface Genre  {

    id: number;
    name:string;
    links?: Link[];

}

