// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Security.Authentication.Web;

namespace Example.Helpers
{
    public class FaceBookHelper
    {
        FacebookClient _fb = new FacebookClient();
        readonly Uri _callbackUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri();
        readonly Uri _loginUrl;
        private const string FacebookAppId = "541366669384344";//"586284774885042"; ////Enter your FaceBook App ID here  
        private const string FacebookPermissions = "email, public_profile, user_location, user_birthday, user_location, publish_actions, user_friends, user_posts";

        public string AccessToken
        {
            get { return _fb.AccessToken; }
        }

        /// <summary>
        /// This is the constructor. This will create an object with all mandatory values.
        /// </summary>
        public FaceBookHelper()
        {
            _loginUrl = _fb.GetLoginUrl(new
            {
                client_id = FacebookAppId,
                redirect_uri = _callbackUri.AbsoluteUri,
                scope = FacebookPermissions,
                display = "popup",
                response_type = "token"
            });
            //Debug.WriteLine(_callbackUri);//This is useful for fill Windows Store ID in Facebook WebSite  
        }

        /// <summary>
        /// This will validate and process the result and then return the access token.
        /// </summary>
        /// <param name="result"></param>
        private void ValidateAndProccessResult(WebAuthenticationResult result)
        {
            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                var responseUri = new Uri(result.ResponseData.ToString());
                var facebookOAuthResult = _fb.ParseOAuthCallbackUrl(responseUri);

                if (string.IsNullOrWhiteSpace(facebookOAuthResult.Error))
                    _fb.AccessToken = facebookOAuthResult.AccessToken;
                else
                {//error de acceso denegado por cancelación en página  
                }
            }
            else if (result.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
            {// error de http  
            }
            else
            {
                _fb.AccessToken = null;//Keep null when user signout from facebook  
            }
        }

        /// <summary>
        /// This will authenticate with the given object and then continue.
        /// </summary>
        public void LoginAndContinue()
        {
            //WebAuthenticationBroker.AuthenticateAndContinue(_loginUrl);
            WebAuthenticationBroker.AuthenticateAndContinue(_loginUrl, _callbackUri, null, WebAuthenticationOptions.None);
        }

        /// <summary>
        /// This will validate all the request.
        /// </summary>
        /// <param name="args"></param>
        public void ContinueAuthentication(WebAuthenticationBrokerContinuationEventArgs args)
        {
            ValidateAndProccessResult(args.WebAuthenticationResult);
        }
    }
}
