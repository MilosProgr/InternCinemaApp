export interface Link {
    rel: string;
    href: string;
    method: string;
}


export interface HateoasResponse {
    id: number;
    links: Link[];
}