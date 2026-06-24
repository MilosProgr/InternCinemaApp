using CinemaApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.Common.HATEOAS
{
    public static class GenreLinkBuilder
    {
        public static List<Link> Build(
            Genre genre,
            string baseUrl,
            ClaimsPrincipal user)
        {
            var links = new List<Link>
            {
                new Link(
                    $"{baseUrl}/{genre.Id}",
                    "self",
                    "GET")
            };


            if (user.IsInRole("ADMIN"))
            {
                links.Add(
                    new Link(
                        $"{baseUrl}/{genre.Id}",
                        "update",
                        "PUT")
                );

                links.Add(
                    new Link(
                        $"{baseUrl}/{genre.Id}",
                        "delete",
                        "DELETE")
                );
            }

            return links;
        }
    }
}
