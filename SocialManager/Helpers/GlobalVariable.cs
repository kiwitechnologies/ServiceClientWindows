// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using Facebook;
using Google.Apis.Auth.OAuth2;
using LinqToTwitter;
using SocialManager.FacebookManager;
using SocialManager.TwitterManager;
using System;
using System.Collections.Generic;

namespace TSGSocialManager.Helpers
{
    public static class GlobalVariable
    {
        public static FacebookClient fbclient = new FacebookClient();
        public static TSGFacebookManager oTSGFacebookManager = new TSGFacebookManager();
        public static string FbAccessToken = string.Empty;

        //TwitterLoginCredential
        public static string TwitterConsumerKey = "ButUFF4x30F2B6lEBqhtK1rJ2";
        public static string TwitterConsumerSecret = "UCvljavmpfwypEaQIPI6dZbXN2pjweBnAOFd4TXwpLgzjMVK8Y";
        public static string TwitterCallBackUri = "http://www.kiwitech.com/";
        public static string TwitterAccessToken { get; set; }
        public static string TwitterAccessSecret { get; set; }
        public static string TwitterScreenName { get; set; }
        public static UniversalAuthorizer TwitterAuthorizer = new UniversalAuthorizer();
        public static TwitterContext twitterContext = null;
        public static User TwitterUserInfo = new User();
        public static List<FriendViewModel> lstTwitterFriendList = new List<FriendViewModel>();

        //Google Secret Key and AppId
        public static string GoogleAppSecret = "OdEsCLXSWjndhnMAtqH7wBdz";
        public static string GoogleAppId = "1070666826694-r1mq3b0g8l37igrrvli9masmj01jijf2.apps.googleusercontent.com";
        /// <summary>
        /// Do not change
        /// Changing the redirect URI causes failure during Auth
        /// </summary>
        public static string RedirectURI = Uri.EscapeDataString("oob");
        public static string GoogleAccessToken { get; set; }
        public static string GoogleRefreshToken { get; set; }
        public static string GoogleUserId = string.Empty;

        //Instagram Details
        public static string InstagramClientSecret = "c8877f14524b4481bb0aaba981c6c077";
        public static string InstagramClientId = "60666c7453a14bca8fccf3817df38e36";
        public static string InstagramRedirectURI = "https://www.google.com";
        public static string InstagramAccessToken = string.Empty;

        //Google Calendar Keys
        public static string GoogleCalendarAppSecret = "RqW3ofBjg-lZENknueDkIehY";
        public static string GoogleCalendarAppId = "1070666826694-lt5hc5r877a0prd03hf31mpvmode19mu.apps.googleusercontent.com";
        public static UserCredential GoogleCalendarUserCredential;
    }
}
