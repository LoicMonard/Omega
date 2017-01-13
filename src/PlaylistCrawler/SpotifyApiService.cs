﻿using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Omega.DAL;

namespace PlaylistCrawler
{
    public class SpotifyApiService
    {
        readonly TrackGateway _trackGateway;
        readonly PlaylistGateway _playlistGateway;
        readonly UserGateway _userGateway;
        public SpotifyApiService(TrackGateway trackGateway, PlaylistGateway playlistGateway, UserGateway userGateway)
        {
            _trackGateway = trackGateway;
            _playlistGateway = playlistGateway;
            _userGateway = userGateway;
        }
        public async Task<SpotifyToken> TokenRefresh(string guid)
        {
            string grantType = "grant_type=refresh_token";
            string refreshToken = "refresh_token=" + await _userGateway.FindSpotifyRefreshToken(guid);
            string postString = grantType + "&" + refreshToken;

            string url = "https://accounts.spotify.com/api/token";

            using (HttpClient client = new HttpClient())
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add("Authorization", string.Format("Basic {0}", "NTJiZDZhOGQ2MzM5NDY0MDg4ZGYwNjY3OWZjNGM5NmE6MjBjMDU0MTBkOWFlNDQ5YzhkNTdkZWMwNmI2YmExMGU="));
                HttpResponseMessage message = await client.PostAsync(url, new StringContent(postString, Encoding.UTF8, "application/x-www-form-urlencoded"));

                using (Stream responseStream = await message.Content.ReadAsStreamAsync())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string responseFromServer = reader.ReadToEnd();
                    var token = JsonConvert.DeserializeObject<SpotifyToken>(responseFromServer);
                    return token;
                }
            }
        }

        public async Task GetSpotifyPlaylist(string guid)
        {
            string accessToken = TokenRefresh(guid).Result.access_token;

            using (HttpClient client = new HttpClient())
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add("Authorization", string.Format("Bearer {0}", accessToken));
                HttpResponseMessage message = await client.GetAsync("https://api.spotify.com/v1/me/playlists");

                using (Stream responseStreamAllPlaylists = await message.Content.ReadAsStreamAsync())
                using (StreamReader readerAllPlaylists = new StreamReader(responseStreamAllPlaylists))
                {
                    string allplaylist;

                    string allPlaylistsString = readerAllPlaylists.ReadToEnd();

                    JObject allPlaylistsJson = JObject.Parse(allPlaylistsString);
                    JArray allPlaylistsArray = (JArray)allPlaylistsJson["items"];
                    List<Playlist> listOfPlaylists = new List<Playlist>();

                    for (int i = 0; i < allPlaylistsArray.Count; i++)
                    {
                        var playlist = allPlaylistsArray[i];

                        string requestTracksInPlaylist = (string)playlist["tracks"]["href"];

                        string idOwner = (string)playlist["owner"]["id"];
                        string name = (string)playlist["name"];
                        string idPlaylist = (string)playlist["id"];
                        string coverPlaylist = (string)playlist["images"][0]["url"];

                        Playlist p = new Playlist(idOwner, idPlaylist, await GetAllTracksInPlaylists(requestTracksInPlaylist, accessToken, idOwner, idPlaylist, coverPlaylist), name, coverPlaylist);
                        await _playlistGateway.InsertPlaylist(p);
                        listOfPlaylists.Add(p);
                    }
                    allplaylist = JsonConvert.SerializeObject(listOfPlaylists);
                    JToken playlistsJson = JToken.Parse(allplaylist);
                    //return playlistsJson;
                }
            }
        }

        private async Task<List<Track>> GetAllTracksInPlaylists(string urlRequest, string accessToken, string userdId, string playlistId, string cover)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add("Authorization", string.Format("Bearer {0}", accessToken));
                HttpResponseMessage message = await client.GetAsync(urlRequest);

                using (Stream responseStream = await message.Content.ReadAsStreamAsync())
                using (StreamReader readerTracksInPlaylists = new StreamReader(responseStream))
                {
                    List<Track> tracksInPlaylist = new List<Track>();

                    string allTracksInPlaylistString = readerTracksInPlaylists.ReadToEnd();
                    JObject allTracksInPlaylistJson = JObject.Parse(allTracksInPlaylistString);
                    JArray allTracksInPlaylistArray = (JArray)allTracksInPlaylistJson["items"];

                    for (int i = 0; i < allTracksInPlaylistArray.Count; i++)
                    {
                        string trackTitle = (string)allTracksInPlaylistJson["items"][i]["track"]["name"];
                        string trackId = (string)allTracksInPlaylistJson["items"][i]["track"]["id"];
                        string albumName = (string)allTracksInPlaylistJson["items"][i]["track"]["album"]["name"];
                        string trackPopularity = (string)allTracksInPlaylistJson["items"][i]["track"]["popularity"];
                        string duration = (string)allTracksInPlaylistJson["items"][i]["track"]["duration_ms"];
                        string coverAlbum = (string)allTracksInPlaylistJson["items"][i]["track"]["album"]["images"][0]["url"];

                        if (await _trackGateway.RetrieveTrack("s", playlistId, trackId) == null)
                            await _trackGateway.InsertTrack("s", playlistId, trackId, trackTitle, albumName, trackPopularity, duration, coverAlbum);
                        tracksInPlaylist.Add(new Track("s", playlistId, trackId, trackTitle, albumName, trackPopularity, duration, coverAlbum));
                    }
                    return tracksInPlaylist;
                }
            }
        }
    }
}
