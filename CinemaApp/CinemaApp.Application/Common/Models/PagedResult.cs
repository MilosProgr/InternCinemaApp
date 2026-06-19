using CinemaApp.Application.Common.HATEOAS;
using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaApp.Application.Common.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public List<Link> Links { get; set; } = new();
    }
}
