﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Omega.DAL;
using OmegaWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OmegaWebApp.Authentication
{
    public class SpotifyAuthenticationEvents
    {
        readonly UserService _userService;

        public SpotifyAuthenticationEvents( UserService userService )
        {
            _userService = userService;
        }

        public async Task OnCreatingTicket( OAuthCreatingTicketContext context )
        {
            string email = GetEmail( context );
            _userService.CreateOrUpdateSpotifyUser( email );
            User user = await _userService.FindUser( email );
            ClaimsPrincipal principal = CreatePrincipal( user );
            context.Ticket = new AuthenticationTicket( principal, context.Ticket.Properties, CookieAuthentication.AuthenticationScheme );
            return;
        }

        ClaimsPrincipal CreatePrincipal( User user )
        {
            List<Claim> claims = new List<Claim>
            {
                //new Claim( ClaimTypes.NameIdentifier, user.UserId.ToString(), ClaimValueTypes.String ),
                //new Claim( ClaimTypes.Email, user.Email )
            };
            ClaimsPrincipal principal = new ClaimsPrincipal( new ClaimsIdentity( claims, "Cookies", ClaimTypes.Email, string.Empty ) );
            return principal;
        }

        string GetEmail( OAuthCreatingTicketContext context )
        {
            return context.Identity.FindFirst( c => c.Type == ClaimTypes.Email ).Value;
        }
    }
}
