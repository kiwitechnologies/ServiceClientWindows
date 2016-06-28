// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using InstaSharp;
using ServiceClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TSGSocialManager.Helpers;
using Windows.Security.Authentication.Web;

namespace SocialManager.InstagramManager
{
    public static class TSGInstagramManager
    {
        public static string AuthorizationUrl = "https://api.instagram.com/oauth/authorize/?";
        //Users Url
        public static string GetSelfProfileUrl = "https://api.instagram.com/v1/users/self";
        public static string GetUserProfileUrl = "https://api.instagram.com/v1/users/";
        public static string GetSelfRecentMediaUrl = "https://api.instagram.com/v1/users/self/media/recent";
        public static string GetUserRecentMediaUrl = "https://api.instagram.com/v1/users/";
        public static string GetRecentMediaLikedUrl = "https://api.instagram.com/v1/users/self/media/liked";
        public static string SearchUrl = "https://api.instagram.com/v1/users/search";
        //Relationship Url
        public static string GetFollowsUrl = "https://api.instagram.com/v1/users/self/follows";
        public static string GetFollowedByUrl = "https://api.instagram.com/v1/users/self/followed-by";

        /// <summary>
        /// User needs to authenticate first.
        /// </summary>
        /// <param name="strClientId"></param>
        /// <param name="strSecretId"></param>
        /// <param name="strRedirectUrl"></param>
        public static async void Authorization(string strClientId, string strSecretId, string strRedirectUrl)
        {
            InstagramConfig config = new InstagramConfig(strClientId, strSecretId, strRedirectUrl);

            var instasharp = new OAuth(config);
            var scopes = new List<OAuth.Scope>();
            scopes.Add(OAuth.Scope.Likes);
            scopes.Add(OAuth.Scope.Comments);
            scopes.Add(OAuth.Scope.Basic);
            scopes.Add(OAuth.Scope.Follower_List);
            scopes.Add(OAuth.Scope.Public_Content);
            scopes.Add(OAuth.Scope.Relationships);
            var link = OAuth.AuthLink(config, scopes, OAuth.ResponseType.Token);
            var startUri = new Uri(link);
            var endUri = new Uri(GlobalVariable.InstagramRedirectURI);
            string code = string.Empty;
            try
            {
                var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri, endUri);
                if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    code = webAuthenticationResult.ResponseData.Remove(0, 37);
                    GlobalVariable.InstagramAccessToken = code;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception at Authenticate(): " + ex.Message);
            }
            //if (!string.IsNullOrEmpty(GlobalVariable.InstagramAccessToken))
            //{
            //    await GetSelf();
            //}
        }

        /// <summary>
        /// Users : Get information about the owner of the access_token.
        /// </summary>
        /// <param name="strAccessToken"></param>
        /// <returns></returns>
        public static async Task GetSelf(string strAccessToken)
        {
            string strURL = GetSelfProfileUrl + "/?access_token=" + strAccessToken;
            StatusAndResponseClass result = await TSGServiceManager.Request(ServiceClient.RequestType.GET, strURL, string.Empty, null);
        }

        /// <summary>
        /// Users : Get basic information about a user.
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strAccessToken"></param>
        /// <returns></returns>
        public static async Task GetUsersProfile(string strUserId, string strAccessToken)
        {
            string strURL = GetUserProfileUrl + strUserId + "/?access_token=" + strAccessToken;
            StatusAndResponseClass result = await TSGServiceManager.Request(ServiceClient.RequestType.GET, strURL, string.Empty, null);
        }

        /// <summary>
        /// Users : Get the most recent media published by the owner of the access_token.
        /// </summary>
        /// <param name="strAccessToken"></param>
        /// <returns></returns>
        public static async Task GetRecentMedia(string strAccessToken)
        {
            string strURL = GetSelfRecentMediaUrl + "/?access_token=" + strAccessToken;
            StatusAndResponseClass result = await TSGServiceManager.Request(ServiceClient.RequestType.GET, strURL, string.Empty, null);
        }

        /// <summary>
        /// Users : Get the most recent media published by the logged in user. 
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strAccessToken"></param>
        /// <returns></returns>
        public static async Task GetUserRecentMedia(string strUserId, string strAccessToken)
        {
            string strURL = GetUserRecentMediaUrl + strUserId + "/media/recent/?access_token=" + strAccessToken;
            StatusAndResponseClass result = await TSGServiceManager.Request(ServiceClient.RequestType.GET, strURL, string.Empty, null);
        }

        /// <summary>
        /// Users : Get the list of recent media liked by the owner of the access_token.
        /// </summary>
        /// <param name="strAccessToken"></param>
        /// <returns></returns>
        public static async Task GetRecentMediaLiked(string strAccessToken)
        {
            string strURL = GetRecentMediaLikedUrl + "/?access_token=" + strAccessToken;
            StatusAndResponseClass result = await TSGServiceManager.Request(ServiceClient.RequestType.GET, strURL, string.Empty, null);
        }

        /// <summary>
        /// Users : Get a list of users matching the query.
        /// </summary>
        /// <param name="strQuery"></param>
        /// <param name="strAccessToken"></param>
        /// <returns></returns>
        public static async Task GetSearchQueryList(string strQuery, string strAccessToken)
        {
            string strURL = SearchUrl + "?q=" + strQuery + "&access_token=" + strAccessToken;
            StatusAndResponseClass result = await TSGServiceManager.Request(ServiceClient.RequestType.GET, strURL, string.Empty, null);
        }

        /// <summary>
        /// Relationship : Get the list of users this user follows.
        /// </summary>
        /// <param name="strAccessToken"></param>
        /// <returns></returns>
        public static async Task GetFollowsList(string strAccessToken)
        {
            string strURL = GetFollowsUrl + "&access_token=" + strAccessToken;
            StatusAndResponseClass result = await TSGServiceManager.Request(ServiceClient.RequestType.GET, strURL, string.Empty, null);
        }

        /// <summary>
        /// Relationship : Get the list of users this user is followed by.
        /// </summary>
        /// <param name="strAccessToken"></param>
        /// <returns></returns>
        public static async Task GetFollowedByList(string strAccessToken)
        {
            string strURL = GetFollowedByUrl + "&access_token=" + strAccessToken;
            StatusAndResponseClass result = await TSGServiceManager.Request(ServiceClient.RequestType.GET, strURL, string.Empty, null);
        }

        /// <summary>
        /// Current User Logout 
        /// </summary>
        public static void Logout()
        {
            GlobalVariable.InstagramAccessToken = string.Empty;
        }
    }
}
