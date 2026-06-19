export interface Link {
    rel: string;
    href: string;
    method: string;
}


export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  links: Link[];
}

export interface WithLinks {
    links: Link[];
}