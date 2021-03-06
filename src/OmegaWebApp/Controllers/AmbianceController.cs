﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Omega.DAL;
using OmegaWebApp.Authentication;
using OmegaWebApp.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OmegaWebApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize(ActiveAuthenticationSchemes = JwtBearerAuthentication.AuthenticationScheme)]
    public class AmbianceController : Controller
    {
        readonly AmbianceService _ambianceService;
        public AmbianceController(AmbianceService ambianceService)
        {
            _ambianceService = ambianceService;
        }

        public class Mood
        {
            public MetaDonnees Metadonnees { get; set; }
            public string Name { get; set; }

            public string Cover { get; set; }

            public static string StringifyMood( Mood m )
            {
                return JsonConvert.SerializeObject(m);
            }
        }

        [HttpPost("InsertAmbiance")]
        public async Task InsertAmbiance([FromBody]Mood ambiance)
        {
            string guid = User.FindFirst("www.omega.com:guid").Value;
            await _ambianceService.InsertAmbiance(guid, Mood.StringifyMood(ambiance));
        }

        [HttpPost("DeleteAmbiance")]
        public async Task DeleteAmbiance([FromBody]string name)
        {
            string guid = User.FindFirst("www.omega.com:guid").Value;
            await _ambianceService.DeleteAmbiance(guid, name);
        }

        [HttpGet("RetrieveAllUserAmbiance")]
        public async Task<List<NewAmbiance>> RetrieveAllUserAmbiance()
        {
            string guid = User.FindFirst("www.omega.com:guid").Value;
            List<Ambiance> ambiances = await _ambianceService.RetrieveAllUSerAmbiance(guid);
            List<NewAmbiance> newAmbiances = new List<NewAmbiance>();
            
            foreach (Ambiance ambiance in ambiances)
            {
                NewMetadonnees metadonnees = new NewMetadonnees();
                NewAmbiance newAmbiance = new NewAmbiance(ambiance.PartitionKey, ambiance.RowKey);

                newAmbiance.Cover = ambiance.Cover;
                metadonnees.accousticness = ambiance.Accousticness;
                metadonnees.danceability = ambiance.Danceability;
                metadonnees.energy = ambiance.Energy;
                metadonnees.instrumentalness = ambiance.Instrumentalness;
                metadonnees.liveness = ambiance.Liveness;
                //metadonnees.loudness = ambiance.Loudness;
                metadonnees.speechiness = ambiance.Speechiness;
                metadonnees.popularity = ambiance.Popularity;
                newAmbiance.Metadonnees = metadonnees;
                newAmbiances.Add(newAmbiance);
            }
            return newAmbiances;
        }
    }
}
