using CinemaApp.Domain.Entities;
using System.Security.Claims;

namespace CinemaApp.Application.Common.HATEOAS
{
    public static class ScreeningSeatLinkBuilder
    {
        public static List<Link> Build(
            ScreeningSeat seat,
            string baseUrl,
            ClaimsPrincipal user)
        {
            var links = new List<Link>
            {
                new Link(
                    $"{baseUrl}/{seat.Id}",
                    "self",
                    "GET")
            };

            if (user.IsInRole("ADMIN"))
            {
                links.Add(
                    new Link(
                        $"{baseUrl}/{seat.Id}",
                        "update",
                        "PUT")
                );

                links.Add(
                    new Link(
                        $"{baseUrl}/{seat.Id}",
                        "delete",
                        "DELETE")
                );
            }

            return links;
        }
    }
}