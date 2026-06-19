using CinemaApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.Common.HATEOAS
{
    public static class UserLinkBuilder
    {
        public static List<Link> Build(
            User user,
            string baseUrl,
            ClaimsPrincipal currentUser)
        {
            var links = new List<Link>
            {
                new Link(
                    $"{baseUrl}/{user.Id}",
                    "self",
                    "GET")
            };


            if (currentUser.IsInRole("ADMIN"))
            {
                links.Add(
                    new Link(
                        $"{baseUrl}/{user.Id}",
                        "update",
                        "PUT")
                );

                links.Add(
                    new Link(
                        $"{baseUrl}/{user.Id}",
                        "delete",
                        "DELETE")
                );
            }


            return links;
        }
    }
}