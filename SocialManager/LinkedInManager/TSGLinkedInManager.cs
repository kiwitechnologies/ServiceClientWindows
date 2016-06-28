// **************************************************************
// *
// * Written By: Ashish Gupta & Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Security.Authentication.Web;
using Windows.UI.Popups;

namespace SocialManager.LinkedInManager
{
    public class TSGLinkedInManager
    {
        /// <summary>
        /// In this sample, we want to see the full profile and connections
        /// http://developer.linkedin.com/documents/authentication#granting
        /// </summary>
        private static string _scope = "r_basicprofile,w_share,rw_company_admin";
        public static string linkedinSharesEndPoint = "https://api.linkedin.com/v1/people/~/shares?oauth2_access_token={0}";

        //Method to get AccessToken
        public static async Task<bool> getAccessToken()
        {
            bool isAccessToken = false;
            try
            {
                var url = "https://www.linkedin.com/uas/oauth2/accessToken?grant_type=authorization_code"
                               + "&code=" + LinkedInCredential._authorizationCode
                               + "&redirect_uri=" + Uri.EscapeDataString(LinkedInCredential.LinkedInCallBackUri)
                               + "&client_id=" + LinkedInCredential.LinkedInConsumerKey
                               + "&client_secret=" + LinkedInCredential.LinkedInConsumerSecret;

                using (var httpClient = new HttpClient())
                {
                    httpClient.MaxResponseContentBufferSize = int.MaxValue;
                    httpClient.DefaultRequestHeaders.ExpectContinue = false;

                    var httpRequestMessage = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(url)
                    };
                    var response = await httpClient.SendAsync(httpRequestMessage);
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var json = JsonObject.Parse(jsonString);
                    LinkedInCredential.LinkedInAccessToken = json.GetNamedString("access_token");
                    if (!string.IsNullOrEmpty(LinkedInCredential.LinkedInAccessToken))
                    {
                        isAccessToken = true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur getAccessToken() LinkedInHelper : " + ex.ToString());
            }
            return isAccessToken;

        }

        //Method to get AuthorizeCode
        public static async Task getAuthorizeCode()
        {
            try
            {
                var url = "https://www.linkedin.com/uas/oauth2/authorization?response_type=code"
                                                           + "&client_id=" + LinkedInCredential.LinkedInConsumerKey
                                                           + "&scope=" + Uri.EscapeDataString(_scope)
                                                           + "&state=STATE"
                                                           + "&redirect_uri=" + Uri.EscapeDataString(LinkedInCredential.LinkedInCallBackUri);

                var startUri = new Uri(url);
                var endUri = new Uri(LinkedInCredential.LinkedInCallBackUri);

                WebAuthenticationResult war = await WebAuthenticationBroker.AuthenticateAsync(
                                                            WebAuthenticationOptions.None,
                                                            startUri,
                                                            endUri);
                switch (war.ResponseStatus)
                {
                    case WebAuthenticationStatus.Success:
                        {
                            // grab access_token and oauth_verifier
                            var response = war.ResponseData;
                            IDictionary<string, string> keyDictionary = new Dictionary<string, string>();
                            var qSplit = response.Split('?');
                            foreach (var kvp in qSplit[qSplit.Length - 1].Split('&'))
                            {
                                var kvpSplit = kvp.Split('=');
                                if (kvpSplit.Length == 2)
                                {
                                    keyDictionary.Add(kvpSplit[0], kvpSplit[1]);
                                }
                            }

                            LinkedInCredential._authorizationCode = keyDictionary["code"];
                            break;
                        }
                    case WebAuthenticationStatus.UserCancel:
                        {
                            System.Diagnostics.Debug.WriteLine("HTTP Error returned by AuthenticateAsync() : " + war.ResponseErrorDetail.ToString());
                            break;
                        }
                    default:
                    case WebAuthenticationStatus.ErrorHttp:
                        System.Diagnostics.Debug.WriteLine(("Error returned by AuthenticateAsync() : " + war.ResponseStatus.ToString()));
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur getAuthorizeCode() LinkedInHelper : " + ex.ToString());
            }

        }

        //Method to get UserInfo data
        public static async Task<StatusAndResponseClass> GetConfirmation(string url)
        {
            StatusAndResponseClass getResponse = new StatusAndResponseClass();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "WindowsApp");
            httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            httpClient.DefaultRequestHeaders.Add("Pragma", "no-cache");
            httpClient.MaxResponseContentBufferSize = int.MaxValue;
            httpClient.DefaultRequestHeaders.ExpectContinue = false;
            // By default, LinkedIn returns XML, we can get JSON instead by adding the header below
            httpClient.DefaultRequestHeaders.Add("x-li-format", "json");
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            var response = await httpClient.SendAsync(httpRequestMessage);
            // response.EnsureSuccessStatusCode();
            HttpStatusCode statuscode = response.StatusCode;
            string responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                getResponse.responseString = responseBody;
            }
            else
            {
                getResponse.responseString = responseBody;
            }
            getResponse.statusCode = Convert.ToInt32(statuscode);
            return getResponse;
        }

        //Method to post Json data.
        private static async Task<StatusAndResponseClass> JsonPostData(string url, string strJsondata)
        {
            System.Diagnostics.Debug.WriteLine("CreateAboutSuggestion sending on server   : " + strJsondata);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-li-format", "json");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json;");
            StringContent queryString = new StringContent(strJsondata, UTF8Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(new Uri(url), queryString);
            //response.EnsureSuccessStatusCode();
            HttpStatusCode statuscode = response.StatusCode;
            string responseBody = await response.Content.ReadAsStringAsync();
            StatusAndResponseClass getResponse = new StatusAndResponseClass();
            if (response.IsSuccessStatusCode)
            {
                getResponse.responseString = responseBody;
            }
            else
            {
                getResponse.responseString = responseBody;
            }
            getResponse.statusCode = Convert.ToInt32(statuscode);
            return getResponse;


        }

        /// <summary>
        /// Method to publish comment
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="comment"></param>
        public async static void PublishComment(string accessToken, string comment)
        {
            try
            {
                var requestUrl = String.Format(linkedinSharesEndPoint, accessToken);
                var shareMsg =
                new
                {
                    comment = comment,
                    visibility = new { code = "anyone" }
                };

                var requestJson = JsonConvert.SerializeObject(shareMsg);
                StatusAndResponseClass response = await TSGLinkedInManager.JsonPostData(LinkedInCredential.PostLinkedInMessageURl(LinkedInCredential.LinkedInAccessToken), requestJson);
                if (response.statusCode == 200 || response.statusCode == 201)
                {
                    await new MessageDialog("Post published successfully.").ShowAsync();
                }
                else if (response.statusCode == 400)
                {
                    // For Duplicate messaage
                    await new MessageDialog("Do not post duplicate content").ShowAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur PostLinkedInNetworkUpdate() LinkedInHelper : " + ex.ToString());
            }
        }

        /// <summary>
        /// Method to publish comment with content
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="comment"></param>
        /// <param name="title"></param>
        /// <param name="submittedUrl"></param>
        /// <param name="submittedImageUrl"></param>
        /// <param name="description"></param>
        public async static void PublishCommentandContent(string accessToken, string comment, string title, string submittedUrl, string submittedImageUrl, [Optional] string description)
        {
            try
            {
                var requestUrl = String.Format(linkedinSharesEndPoint, accessToken);
                var shareMsg =
                new
                {
                    comment = comment,
                    content =
                        new
                        {
                            title = title,
                            submitted_url = submittedUrl, // "http://www.bigcode.net",
                            submitted_image_url = submittedImageUrl, //"http://2.bp.blogspot.com/-8r_lWT_32lQ/TxrQW12ngPI/AAAAAAAAI70/ifMF4Z16M-Y/s1600/SQL+Server+session+state.png",
                            description = description
                        },
                    visibility = new { code = "anyone" }
                };

                var requestJson = JsonConvert.SerializeObject(shareMsg);
                StatusAndResponseClass response = await TSGLinkedInManager.JsonPostData(LinkedInCredential.PostLinkedInMessageURl(LinkedInCredential.LinkedInAccessToken), requestJson);
                if (response.statusCode == 200 || response.statusCode == 201)
                {
                    await new MessageDialog("Post published successfully.").ShowAsync();
                }
                else if (response.statusCode == 400)
                {
                    // For Duplicate messaage
                    await new MessageDialog("Do not post duplicate content").ShowAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur PostLinkedInNetworkUpdate() LinkedInHelper : " + ex.ToString());
            }
        }

    }
}
