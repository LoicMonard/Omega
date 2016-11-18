﻿//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Omega.DAL;
//using System.Net;
//using System.IO;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using Newtonsoft.Json.Linq;

//// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

//namespace OmegaWebApp.Controllers
//{
//    public class SpotifyController : Controller
//    {
//        /// <summary>
//        /// Insert the tracks of the current playlist in the table Track if they aren't in already.
//        /// </summary>
//        /// <param name="urlRequest"></param>
//        /// <param name="accessToken"></param>
//        /// <param name="userdId"></param>
//        /// <param name="playlistId"></param>
//        /// <returns>Returns all the tracks of the playlist.</returns>


//        }

//        [Route( "Spotify/playlists" )]
//        public static async Task<JToken> GetAllSpotifyPlaylists( string email )
//        {
//            var allPlaylistsRequest = "https://api.spotify.com/v1/me/playlists";
//            WebRequest playlistsRequest = HttpWebRequest.Create( allPlaylistsRequest );
//            //playlistsRequest.Method = "GET";

//            //ClaimsIdentity claimsIdentity = await this.Request.GetOwinContext().Authentication.GetExternalIdentityAsync( DefaultAuthenticationTypes.ExternalCookie );
//            //Claim claim = claimsIdentity.Claims.Single( c => c.Type == "http://omega.fr:user_email" );
//            //string email = claim.Value;
//            string accessToken = DatabaseQueries.GetSpotifyAccessTokenByEmail( email );

//            playlistsRequest.Headers.Add( "Authorization", string.Format( "Bearer {0}", accessToken ) );

//            JObject allPlaylistsJson;
//            string allplaylist;

//            using (WebResponse responseAllPlaylists = await playlistsRequest.GetResponseAsync())
//            using (Stream responseStreamAllPlaylists = responseAllPlaylists.GetResponseStream())
//            using (StreamReader readerAllPlaylists = new StreamReader( responseStreamAllPlaylists ))
//            {
//                string allPlaylistsString = readerAllPlaylists.ReadToEnd();

//                allPlaylistsJson = JObject.Parse( allPlaylistsString );
//                JArray allPlaylistsArray = (JArray)allPlaylistsJson["items"];
//                List<PlaylistEntity> listOfPlaylists = new List<PlaylistEntity>();

//                for (int i = 0; i < allPlaylistsArray.Count; i++)
//                {
//                    var playlist = allPlaylistsArray[i];

//                    string requestTracksInPlaylist = (string)playlist["tracks"]["href"];

//                    string idOwner = (string)playlist["owner"]["id"];
//                    string name = (string)playlist["name"];
//                    string idPlaylist = (string)playlist["id"];
//                    string coverPlaylist = (string)playlist["images"][0]["url"];

//                    PlaylistEntity p = new PlaylistEntity( idOwner, idPlaylist, await GetAllTracksInPlaylists( requestTracksInPlaylist, accessToken, idOwner, idPlaylist, coverPlaylist ), name, coverPlaylist );
//                    DatabaseQueries.InsertPlaylist( p );
//                    listOfPlaylists.Add( p );
//                }
//                allplaylist = JsonConvert.SerializeObject( listOfPlaylists );
//            }
//            JToken playlistsJson = JToken.Parse( allplaylist );
//            return playlistsJson;
//        }
//    }
//}