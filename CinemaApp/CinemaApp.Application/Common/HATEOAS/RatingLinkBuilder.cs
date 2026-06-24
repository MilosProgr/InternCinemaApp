using CinemaApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace CinemaApp.Application.Common.HATEOAS
{
    public static class RatingLinkBuilder
    {
        public static List<Link> Build(
            Rating rating,
            string baseUrl,
            System.Security.Claims.ClaimsPrincipal user)
        {
            var links = new List<Link>
            {
                new Link(
                    $"{baseUrl}/{rating.Id}",
                    "self",
                    "GET")
            };


            if (user.IsInRole("CONSUMER"))
            {
                links.Add(
                    new Link(
                        $"{baseUrl}/{rating.Id}",
                        "update",
                        "PUT"));
            }


            if (user.IsInRole("ADMIN"))
            {
                links.Add(
                    new Link(
                        $"{baseUrl}/{rating.Id}",
                        "update",
                        "PUT"));

                links.Add(
                    new Link(
                        $"{baseUrl}/{rating.Id}",
                        "delete",
                        "DELETE"));
            }


            return links;
        }
    }
}