// **************************************************************
// *
// * Written By: Ashish Gupta & Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using System;

namespace SocialManager.LinkedInManager
{
    public class LinkedInCredential
    {
        public static string HostUrl = "https://api.linkedin.com/v1/people/~";
        public static string LinkedInConsumerKey = "753i1gaqp91jyx"; // "75qbjbmcc2yh3m"; // 
        public static string LinkedInConsumerSecret = "ZCR91R4g8ps0EUWJ"; // "ZOlpEZvC7b5LaSKx"; // 
        public static string LinkedInCallBackUri = "http://www.buildwindows.com";
        public static string LinkedInAccessToken { get; set; }
        public static string _authorizationCode;
        public static string linkedinSharesEndPoint = "https://api.linkedin.com/v1/people/~/shares?oauth2_access_token={0}";
        public static string UserHostUrl = "https://api.linkedin.com/v1/people/~:(id,first-name,last-name,headline,picture-url,industry,summary,specialties,positions:(id,title,summary,start-date,end-date,is-current,company:(id,name,type,size,industry,ticker)),educations:(id,school-name,field-of-study,start-date,end-date,degree,activities,notes),associations,interests,num-recommenders,date-of-birth,publications:(id,title,publisher:(name),authors:(id,name),date,url,summary),patents:(id,title,summary,number,status:(id,name),office:(name),inventors:(id,name),date,url),languages:(id,language:(name),proficiency:(level,name)),skills:(id,skill:(name)),certifications:(id,name,authority:(name),number,start-date,end-date),courses:(id,name,number),recommendations-received:(id,recommendation-type,recommendation-text,recommender),honors-awards,three-current-positions,three-past-positions,volunteer)";

        //Base URL for  Get UserProfile
        public static string GetUserProfileURl(string _accessToken)
        {
            string getUserProfile = UserHostUrl + "?oauth2_access_token=" + _accessToken;
            return getUserProfile;
        }

        //Base URL for Post Message
        public static string PostLinkedInMessageURl(string straccessToken)
        {
            string requestUrl = String.Format(linkedinSharesEndPoint, straccessToken);
            return requestUrl;
        }
    }
}
