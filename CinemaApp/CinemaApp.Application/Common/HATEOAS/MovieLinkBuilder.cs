using CinemaApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.Common.HATEOAS
{
    public class MovieLinkBuilder
    {
        public static List<Link> Build(
            Movie movie,
            string baseUrl,
            ClaimsPrincipal user)
        {
            var links = new List<Link>
            {
                new Link(
                    $"{baseUrl}/{movie.Id}",
                    "self",
                    "GET")
            };


            if (user.IsInRole("ADMIN"))
            {
                links.Add(
                    new Link(
                        $"{baseUrl}/{movie.Id}",
                        "update",
                        "PUT")
                );

                links.Add(
                    new Link(
                        $"{baseUrl}/{movie.Id}",
                        "delete",
                        "DELETE")
                );
            }


            return links;
        }
    }
}
