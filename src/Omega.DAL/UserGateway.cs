﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omega.DAL
{
    public class UserGateway
    {
        readonly CloudStorageAccount storageAccount;
        readonly CloudTableClient tableClient;
        readonly CloudTable tableUser;
        readonly CloudTable tableFacebookUser;

        public UserGateway( string connectionString )
        {
            // Retrieve the storage account from the connection string.
            storageAccount = CloudStorageAccount.Parse( connectionString );

            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTables objects that represent the different tables.
            tableUser = tableClient.GetTableReference( "User" );
            // Create the table if it doesn't exist.
            tableUser.CreateIfNotExistsAsync();

            tableFacebookUser = tableClient.GetTableReference( "FacebookUser" );
            // Create the table if it doesn't exist.
            tableFacebookUser.CreateIfNotExistsAsync();
        }

        public async Task<User> FindUserByEmail( string email )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, email );
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            return (User)retrievedResult.Result;
        }

        public async Task CreateUser( User user )
        {
            TableOperation insertUserOperation = TableOperation.Insert( user );
            await tableUser.ExecuteAsync( insertUserOperation );
        }

        public async void InsertOrUpdateUserByDeezer( User deezerUser )
        {
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, deezerUser.RowKey );

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User)retrievedResult.Result;

            if (retrievedResult.Result != null && (retrievedUser.DeezerId != deezerUser.DeezerId || retrievedUser.DeezerAccessToken != deezerUser.DeezerAccessToken))
            {
                retrievedUser.DeezerAccessToken = deezerUser.DeezerAccessToken;
                retrievedUser.DeezerId = deezerUser.DeezerId;

                TableOperation updateOperation = TableOperation.Replace( retrievedUser );
                await tableUser.ExecuteAsync( updateOperation );
            }
            else if (retrievedUser == null)
            {
                TableOperation insertOperation = TableOperation.Insert( deezerUser );
                await tableUser.ExecuteAsync( insertOperation );
            }
        }
        
        public async Task UpdateDeezerUser( User deezerUser )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, deezerUser.RowKey );
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            retrievedUser.DeezerId = deezerUser.SpotifyId;
            retrievedUser.DeezerAccessToken = deezerUser.SpotifyAccessToken;
            retrievedUser.DeezerRefreshToken = deezerUser.SpotifyRefreshToken;

            TableOperation updateOperation = TableOperation.Replace( retrievedUser );
            await tableUser.ExecuteAsync( updateOperation );
        }

        public async Task UpdateSpotifyUser( User spotifyUser )
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, spotifyUser.RowKey );
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User retrievedUser = (User) retrievedResult.Result;
            retrievedUser.SpotifyId = spotifyUser.SpotifyId;
            retrievedUser.SpotifyAccessToken = spotifyUser.SpotifyAccessToken;
            retrievedUser.SpotifyRefreshToken = spotifyUser.SpotifyRefreshToken;

            TableOperation updateOperation = TableOperation.Replace( retrievedUser );
            await tableUser.ExecuteAsync( updateOperation );
        }
        
        public async Task UpdateFacebookUser( User facebookUser)
        {
            /*
             * updating FacebookAccessToken and FacebookId If they have changed in table User
             */
            TableOperation retrieveOperation = TableOperation.Retrieve<User>(string.Empty, facebookUser.RowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableUser.ExecuteAsync(retrieveOperation);
            User retrievedUser = (User)retrievedResult.Result;
            
            retrievedUser.FacebookAccessToken = facebookUser.FacebookAccessToken;

            TableOperation updateOperation = TableOperation.Replace(retrievedUser);
            await tableUser.ExecuteAsync(updateOperation);
        }

        public async Task<IEnumerable<string>> GetAuthenticationProviders( string email )
        {
            IList<string> providers = new List<string>();

            TableOperation retrieveOperation = TableOperation.Retrieve<User>( string.Empty, email );

            // Execute the retrieve operation.
            TableResult retrievedResult = await tableUser.ExecuteAsync( retrieveOperation );
            User user = (User) retrievedResult.Result;
            if( user.DeezerId != null )
                providers.Add( "Deezer" );
            if( user.SpotifyId != null )
                providers.Add( "Spotify" );
            if( user.FacebookId != null )
                providers.Add( "Facebook" );

            return providers;
        }
    }
}
