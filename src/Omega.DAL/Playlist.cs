﻿using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace Omega.DAL
{
    public class Playlist : TableEntity
    {
        public string OwnerId
        {
            get
            {
                return PartitionKey;
            }
            set
            {
                PartitionKey = value;
            }
        }
        public string PlaylistId
        {
            get
            {
                return PlaylistId;
            }
            set
            {
                RowKey = value;
            }
        }
        public List<Track> Tracks { get; set; }
        public string Name { get; set; }
        public string Cover { get; set; }

        public Playlist() { }
        public Playlist( string ownerId, string playlistId, List<Track> tracks, string name, string cover )
        {
            PartitionKey = ownerId;
            RowKey = playlistId;
            Tracks = tracks;
            Name = name;
            Cover = cover;
        }
    }
}
