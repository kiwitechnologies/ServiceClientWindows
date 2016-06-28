// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using Facebook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TSGSocialManager.Helpers;
using Windows.ApplicationModel.Core;
using Windows.Security.Authentication.Web;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace SocialManager.FacebookManager
{
    public class TSGFacebookManager
    {
       /// <summary>
       /// This method will be for logout from facebook
       /// </summary>
        public void FacebookLogout()
        {
            var _logoutUrl = GlobalVariable.fbclient.GetLogoutUrl(new
            {
                next = "https://www.facebook.com/connect/login_success.html",
                access_token = GlobalVariable.FbAccessToken
            });
            WebAuthenticationBroker.AuthenticateAndContinue(_logoutUrl);
        }
        
        /// <summary>
        /// This is to get user profile
        /// </summary>
        /// <returns></returns>
        public async Task<UserProfile> GetUserProfile()
        {
            UserProfile oUserProfile = new UserProfile();
            //Fetch facebook UserProfile:  
            dynamic result = await GlobalVariable.fbclient.GetTaskAsync("me");
            string id = result.id;
            string email = result.email;
            string FBName = result.name;

            //Format UserProfile:  
            string profilePictureUrl = string.Format("https://graph.facebook.com/{0}/picture?type={1}&access_token={2}", id, "large", GlobalVariable.FbAccessToken);
            oUserProfile.Id = id;
            oUserProfile.Name = FBName;
            oUserProfile.Email = email;
            oUserProfile.ImageURL = profilePictureUrl;
            return oUserProfile;
        }

        /// <summary>
        /// This is to post message on wall
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="strPrivacy"></param>
        /// <returns></returns>
        public async Task<string> PostMessageOnWall(string strMessage, string strPrivacy)
        {
            string strResponse = string.Empty;
            try
            {
                var parameters = new Dictionary<string, object>
            {
                 { "message", strMessage },
                 { "privacy", new Dictionary<string, object>
                     {
                         { "value", strPrivacy }
                     }
                 }
             };
                dynamic fbPostTaskResult = await GlobalVariable.fbclient.PostTaskAsync("/me/feed", parameters);
                var responseresult = (IDictionary<string, object>)fbPostTaskResult;
                return "Message posted sucessfully on facebook wall";
            }
            catch (Exception ex)
            {
                strResponse = ex.Message;
                return strResponse;
                //MessageDialog ErrMsg = new MessageDialog("Error Ocuured!");  
            }
        }

        /// <summary>
        /// This is to post link on wall
        /// </summary>
        /// <param name="oPostLink"></param>
        /// <returns></returns>
        public async Task<string> PostLinkOnWall(PostLink oPostLink)
        {
            string strResponse = string.Empty;
            try
            {
                var parameters = new Dictionary<string, object>
            {
                 { "message", oPostLink.message },
                 { "link", oPostLink.link },
                 { "picture", oPostLink.picture },
                 { "name", oPostLink.name },
                 { "caption", oPostLink.caption },
                 { "description", oPostLink.description },
                 { "privacy", new Dictionary<string, object>
                     {
                         { "value",  oPostLink.privacy }
                     }
                 }
             };
                dynamic fbPostTaskResult = await GlobalVariable.fbclient.PostTaskAsync("/me/feed", parameters);
                var responseresult = (IDictionary<string, object>)fbPostTaskResult;
                return "Message posted sucessfully on facebook wall";
            }
            catch (Exception ex)
            {
                strResponse = ex.Message;
                return strResponse;
                //MessageDialog ErrMsg = new MessageDialog("Error Ocuured!");  
            }
        }

        /// <summary>
        /// This is to get friend list
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> GetFriendList()
        {
            dynamic friendListData = null;
            try
            {
                friendListData = await GlobalVariable.fbclient.GetTaskAsync("/me/friends");
                return friendListData;
            }
            catch (Exception ex)
            {
                //MessageDialog ErrMsg = new MessageDialog("Error Ocuured!");  
                return friendListData;
            }
        }

        /// <summary>
        /// This is to post local image on wall
        /// </summary>
        /// <param name="m_StorageFile"></param>
        /// <param name="strMessage"></param>
        /// <param name="strPrivacy"></param>
        /// <returns></returns>
        public async Task<string> PostLocalImageOnWall(StorageFile m_StorageFile, string strMessage, string strPrivacy)
        {
            string strResponse = string.Empty;
            await Task.Run(async () =>
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    try
                    {
                        byte[] imageByte = new byte[0];
                        if (m_StorageFile != null)
                        {
                            var stream = await m_StorageFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
                            var reader = new DataReader(stream.GetInputStreamAt(0));
                            imageByte = new byte[stream.Size];
                            await reader.LoadAsync((uint)stream.Size);
                            reader.ReadBytes(imageByte);
                        }
                        var mediaObject = new FacebookMediaObject();
                        await Task.Run(() =>
                        {
                            var source = File.OpenRead(m_StorageFile.Path);
                            mediaObject = new FacebookMediaObject
                            {
                                ContentType = m_StorageFile.ContentType.ToString(),
                                FileName = Path.GetFileName(m_StorageFile.Path)
                            };
                            mediaObject.SetValue(imageByte);
                        });

                        var parameters = new Dictionary<string, object>
                           {
                                { "message", strMessage },
                                { "source", mediaObject },
                                { "privacy", new Dictionary<string, object>
                                    {
                                        { "value",  strPrivacy }
                                    }
                                }
                            };
                        dynamic fbPostTaskResult = await GlobalVariable.fbclient.PostTaskAsync("/me/photos", parameters);
                        var responseresult = (IDictionary<string, object>)fbPostTaskResult;
                        strResponse = "Image posted sucessfully on facebook wall";
                    }
                    catch (Exception ex)
                    {
                        //MessageDialog ErrMsg = new MessageDialog("Error Ocuured!");  
                        System.Diagnostics.Debug.WriteLine("Exception Occur BtnFaceBookPost_Click : " + ex.ToString());
                        strResponse = ex.Message;
                    }
                });

            });
            return strResponse;
        }
    }

    public class UserProfile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageURL { get; set; }
    }

    public class PostLink
    {
        public string message { get; set; }
        public string link { get; set; }
        public string picture { get; set; }
        public string name { get; set; }
        public string caption { get; set; }
        public string description { get; set; }
        public string privacy { get; set; }
    }

    public enum PrivacyType { EVERYONE, ALL_FRIENDS, FRIENDS_OF_FRIENDS, SELF, CUSTOM }
}
