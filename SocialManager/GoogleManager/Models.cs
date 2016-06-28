// **************************************************************
// *
// * Written By: Rohit Gupta & Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

namespace SocialManager.GoogleManager
{
    /// <summary>
    /// Serializable Model for User information
    /// </summary>
    public class GoogleUser
    {
        public string id { get; set; }
        public string email { get; set; }
        public bool verified_email { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string link { get; set; }
        public string picture { get; set; }
        public string gender { get; set; }
        public string locale { get; set; }
    }

    /// <summary>
    /// Serializable Model for Token information
    /// </summary>
    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }
    }

    /// <summary>
    /// ViewModel for User
    /// </summary>
    public class UserInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
    }

    /// <summary>
    /// ViewModel for Configuration
    /// </summary>
    public class GoogleConfiguration
    {
        public string GoogleClientId { get; set; }
        public string GoogleRedirectUrl { get; set; }
        public string GoogleClientSecret { get; set; }
        public string GoogleScope { get; set; }
    }
}
