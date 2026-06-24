using CinemaApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace CinemaApp.Application.Common.HATEOAS
{
    public static class MovieScreeningLinkBuilder
    {
        public static List<Link> Build(
            MovieScreening screening,
            string baseUrl,
            ClaimsPrincipal user)
        {
            var links = new List<Link>
            {
                new Link(
                    $"{baseUrl}/{screening.Id}",
                    "self",
                    "GET")
            };


            if (user.IsInRole("ADMIN"))
            {
                links.Add(
                    new Link(
                        $"{baseUrl}/{screening.Id}",
                        "update",
                        "PUT")
                );

                links.Add(
                    new Link(
                        $"{baseUrl}/{screening.Id}",
                        "delete",
                        "DELETE")
                );
            }


            return links;
        }
    }
}
