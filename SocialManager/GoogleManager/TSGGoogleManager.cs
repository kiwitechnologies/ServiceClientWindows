// **************************************************************
// *
// * Written By: Rohit Gupta & Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TSGSocialManager.Helpers;
using Windows.Security.Authentication.Web;

namespace SocialManager.GoogleManager
{
    public static class TSGGoogleManager
    {
        private const string AuthorizationUrl = "https://accounts.google.com/o/oauth2/auth";
        private const string ApprovalUrl = "https://accounts.google.com/o/oauth2/approval?";
        private const string UserInfoUrl = "https://www.googleapis.com/oauth2/v1/userinfo";
        private const string TokenUrl = "https://accounts.google.com/o/oauth2/token";
        // for google plus circles https://www.googleapis.com/auth/plus.me https://www.googleapis.com/auth/plus.circles.read
        private const string DefaultScope = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/plus.circles.read";
        private static string _clientId;
        private static string _redirectId;
        private static string _clientSecret;
        private static string _scope;

        public static string AccessToken { get; private set; }
        public static string RefreshToken { get; private set; }

        public static void Configure(GoogleConfiguration configuration)
        {
            _clientId = configuration.GoogleClientId;
            _redirectId = configuration.GoogleRedirectUrl;
            _clientSecret = configuration.GoogleClientSecret;
            try
            {
                _scope = DefaultScope + configuration.GoogleScope;
            }
            catch (RuntimeBinderException)
            {
                _scope = DefaultScope;
            }
        }

        public static async Task Authenticate()
        {
            var googleUrl = AuthorizationUrl + "?client_id=" + Uri.EscapeDataString(_clientId) + "&redirect_uri=" + _redirectId + "&response_type=code&scope=" + Uri.EscapeDataString(_scope);
            var startUri = new Uri(googleUrl);
            var endUri = new Uri(ApprovalUrl);
            try
            {
                var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.UseTitle, startUri, endUri);
                if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    var code = webAuthenticationResult.ResponseData.Remove(0, 13);
                    Token token = await RequestToken(_clientId, code);
                    if (token != null)
                    {
                        GlobalVariable.GoogleAccessToken = token.access_token;
                        GlobalVariable.GoogleRefreshToken = token.refresh_token;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception at Authenticate(): " + ex.Message);
            }
        }

        public static async Task<Token> RequestToken(string clientId, string key)
        {
            var client = new HttpClient();
            try
            {
                var auth = await client.PostAsync("https://accounts.google.com/o/oauth2/token", new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("code", key),
                    new KeyValuePair<string, string>("client_id",clientId),
                    new KeyValuePair<string, string>("client_secret",_clientSecret),
                    new KeyValuePair<string, string>("grant_type","authorization_code"),
                    new KeyValuePair<string, string>("redirect_uri", _redirectId),
                }));

                var data = await auth.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Token>(data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception at RequestToken(): " + ex.Message);
            }
            return null;
        }

        public static async Task<string> RefreshTokenAsync(string refreshToken)
        {
            HttpMessageHandler handler = new HttpClientHandler();

            var httpClient = new HttpClient(handler);
            try
            {
                string postData = string.Format("client_id={0}&client_secret={1}&refresh_token={2}&grant_type=refresh_token", _clientId, _clientSecret, refreshToken);
                var c = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");

                httpClient.MaxResponseContentBufferSize = 100000;

                var result = await httpClient.PostAsync(TokenUrl, c);
                result.EnsureSuccessStatusCode();
                var json = await result.Content.ReadAsStringAsync();
                AccessToken = JsonConvert.DeserializeObject<Token>(json).access_token;
                return AccessToken;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception at RefreshTokenAsync(): " + ex.Message);
            }
            return string.Empty;
        }

        public static async Task<UserInfo> GetUserInfo()
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GlobalVariable.GoogleAccessToken);
                var result = await client.GetAsync(UserInfoUrl);
                result.EnsureSuccessStatusCode();
                var json = await result.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<GoogleUser>(json);
                string handle = null;
                if (user != null && !string.IsNullOrEmpty(user.email))
                {
                    var emailParts = user.email.Split('@');
                    if (emailParts.Length == 2)
                    {
                        handle = emailParts[0];
                    }
                    GlobalVariable.GoogleUserId = user.id;
                }
                if (user != null)
                {
                    return new UserInfo { Id = user.id, Name = user.name, UserName = handle ?? user.name, Picture = user.picture, Email = user.email };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception at GetUserInfo(): " + ex.Message);
            }
            return null;
        }

        //public static async Task GetCircleInfo()
        //{
        //    try
        //    {
        //        //"for people who use Google Apps at college, at work, or at home.(https://developers.google.com/+/domains/)"
        //        //won't work for regular google + accounts
        //        var client = new HttpClient();
        //        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GlobalVariable.GoogleAccessToken);
        //        var circles = await client.GetAsync("https://www.googleapis.com/plusDomains/v1/people/" + GlobalVariable.GoogleUserId + "/circles");
        //        var circlesjson = await circles.Content.ReadAsStringAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Exception at GetCircleInfo(): " + ex.Message);

        //    }
        //}

        public static void LogOut()
        {
            try
            {
                GlobalVariable.GoogleAppSecret = string.Empty;
                GlobalVariable.GoogleAppId = string.Empty;
                GlobalVariable.GoogleAccessToken = string.Empty;
                GlobalVariable.GoogleRefreshToken = string.Empty;
                GlobalVariable.GoogleUserId = string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception at GetCircleInfo(): " + ex.Message);

            }
        }

    }
}
