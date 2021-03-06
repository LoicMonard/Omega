﻿using Facebook;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Omega.DAL;
using OmegaWebApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OmegaWebApp.Controllers
{
    [Route( "api/[controller]" )]
    public class FacebookController : Controller
    {
        private static readonly string FacebookGraphApi = "https://graph.facebook.com";
        readonly UserService _userService;

        // GET: /<controller>/
        public FacebookController( UserService userService )
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all the user's facebook events
        /// </summary>
        /// <returns>List of FacebookEvents</returns>
        [HttpGet( "Groups" )]
        public async Task<JToken> GetAllFacebookGroups()
        {
            using( HttpClient client = new HttpClient() )
            {
                List<GroupOrEventFacebook> groups = new List<GroupOrEventFacebook>();

                string guid = User.FindFirst( "www.omega.com:guid" ).Value;
                string accessToken = await _userService.GetFacebookAccessToken( guid );
                string facebookId = await _userService.GetFacebookId( guid );
                string parameters = Uri.EscapeDataString( "id,cover,link,name,members{email,id,name}" );

                Uri groupDetailUri = new Uri(
                string.Format( "{0}/me/groups?" +
                    "access_token={1}" +
                    "&debug=all" +
                    "&fields={2}" +
                    "&format=json&method=get&pretty=0&suppress_http_code=1",
                FacebookGraphApi,
                accessToken,
                parameters ) );

                HttpResponseMessage message = await client.GetAsync( groupDetailUri );

                using( Stream responseStream = await message.Content.ReadAsStreamAsync() )
                using( StreamReader reader = new StreamReader( responseStream ) )
                {
                    string groupsStringResponse = reader.ReadToEnd();
                    JObject groupsJsonResponse = JObject.Parse( groupsStringResponse );

                    foreach( var group in groupsJsonResponse["data"] )
                    {
                        string groupId = (string) group["id"];
                        string groupName = (string) group["name"];
                        string groupCover = (string) group["cover"]["source"];

                        GroupOrEventFacebook fbGroup = new GroupOrEventFacebook( groupId, groupName, groupCover );
                        groups.Add( fbGroup );
                    }
                    string groupsString = JsonConvert.SerializeObject( groups );
                    JToken groupsJson = JToken.Parse( groupsString );

                    return groupsJson;

                }
            }
        }

        [HttpGet( "Events" )]
        public async Task<JToken> GetAllFacebookEvents()
        {
            string guid = User.FindFirst( "www.omega.com:guid" ).Value;
            string accessToken = await _userService.GetFacebookAccessToken( guid );
            //FacebookClient fbClient = new FacebookClient( accessToken );

            //dynamic result = await fbClient.GetTaskAsync( "/me/events?fields=cover,id,name,attending{id,email,name}" );
            ////result[]

            //return null;
            FacebookClient fbClient = new FacebookClient( accessToken );
            
            dynamic result = await fbClient.GetTaskAsync( "/me/events?fields=cover,id,name,attending{id,email,name}" );
            //string json = JsonConvert.SerializeObject( result );
            JObject eventsJson = JObject.FromObject( result );

            foreach( var _event in eventsJson["data"] )
            {
                string eventId = (string) _event["id"];
                string eventName = (string) _event["name"];
                string groupCover = (string) _event["cover"]["source"];
                List<User> eventAttendings = new List<User>();
                JArray attendingTokens = (JArray) _event["attending"]["data"];
                foreach( var attending in attendingTokens )
                {
                    string mail = (string) attending["email"];
                    string id = (string) attending["id"];
                    if( mail != null )
                    {
                        User u = new User();
                        u.PartitionKey = string.Empty;
                        u.RowKey = mail;
                        u.FacebookId = id;
                        eventAttendings.Add( u );
                    }
                }
            }
            return null;
        }
    }
}