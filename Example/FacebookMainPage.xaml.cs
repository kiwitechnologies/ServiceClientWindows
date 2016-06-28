// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using Example.Helpers;
using Newtonsoft.Json.Linq;
using SocialManager.FacebookManager;
using System;
using TSGSocialManager.Helpers;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Example
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FacebookMainPage : Page, IWebAuthenticationBrokerContinuable
    {
        FaceBookHelper ObjFBHelper = new FaceBookHelper();

        /// <summary>
        /// This is the Facebook constructor
        /// </summary>
        public FacebookMainPage()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                Frame.Navigate(typeof(MainPage));
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            bool isHardwareButtonsAPIPresent =
        Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");
        }

        /// <summary>
        /// Click event of facebook login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFaceBookLogin_Click(object sender, RoutedEventArgs e)
        {
            ObjFBHelper.LoginAndContinue();
        }

        /// <summary>
        /// This will fire after login process ContinueWithWebAuthentication
        /// </summary>
        /// <param name="args"></param>
        public async void ContinueWithWebAuthenticationBroker(WebAuthenticationBrokerContinuationEventArgs args)
        {
            ObjFBHelper.ContinueAuthentication(args);
            if (ObjFBHelper.AccessToken != null)
            {
                GlobalVariable.fbclient = new Facebook.FacebookClient(ObjFBHelper.AccessToken);
                GlobalVariable.FbAccessToken = ObjFBHelper.AccessToken;
                StckPnlProfile_Layout.Visibility = Visibility.Visible;
                BtnLogin.Visibility = Visibility.Collapsed;
                BtnLogout.Visibility = Visibility.Visible;
                UserProfile oUserProfile = await GlobalVariable.oTSGFacebookManager.GetUserProfile();
                if (oUserProfile != null)
                {
                    TxtUserProfile.Text = oUserProfile.Name;
                    picProfile.Source = new BitmapImage(new Uri(oUserProfile.ImageURL));
                }
            }
            else
            {
            }
        }

        /// <summary>
        /// Click event to post status on wall
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnPostStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string strMessage = "Testing Facebook C# SDK";
                string strPrivacyType = PrivacyType.ALL_FRIENDS.ToString();
                string strResult = await GlobalVariable.oTSGFacebookManager.PostMessageOnWall(strMessage, strPrivacyType);
                if (string.IsNullOrEmpty(strResult))
                {
                    strResult = "Error Ocuured!";
                }
                MessageDialog SuccessMsg = new MessageDialog(strResult);
                await SuccessMsg.ShowAsync();
            }
            catch (Exception ex)
            {
                //MessageDialog ErrMsg = new MessageDialog("Error Ocuured!");  
            }
        }

        /// <summary>
        /// Click event to post link on wall
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnpostLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PostLink oPostLink = new PostLink();
                oPostLink.message = "Testing Facebook C# SDK";
                oPostLink.link = "http://facebooksdk.codeplex.com/";
                oPostLink.name = "Facebook C# SDK";
                oPostLink.description = "The Facebook C# SDK helps .Net developers build web, desktop, Silverlight, and Windows Phone 7 applications that integrate with Facebook.";
                oPostLink.privacy = PrivacyType.ALL_FRIENDS.ToString();
                oPostLink.picture = "http://download.codeplex.com/Project/Download/FileDownload.aspx?ProjectName=facebooksdk&DownloadId=170794&Build=17672";
                oPostLink.caption = "http://facebooksdk.codeplex.com/";
                string strResult = await GlobalVariable.oTSGFacebookManager.PostLinkOnWall(oPostLink);
                if (string.IsNullOrEmpty(strResult))
                {
                    strResult = "Error Ocuured!";
                }
                MessageDialog SuccessMsg = new MessageDialog(strResult);
                await SuccessMsg.ShowAsync();
            }
            catch (Exception ex)
            {
                //MessageDialog ErrMsg = new MessageDialog("Error Ocuured!");  
            }
        }

        /// <summary>
        /// Click event to post local image on wall  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPostLocalImage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PostOnWall));
        }

        /// <summary>
        /// Click event to logout from facebook
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFaceBookLogout_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariable.oTSGFacebookManager.FacebookLogout();
            BtnLogin.Visibility = Visibility.Visible;
            BtnLogout.Visibility = Visibility.Collapsed;
            StckPnlProfile_Layout.Visibility = Visibility.Collapsed;
            TxtUserProfile.Text = string.Empty;
            picProfile.Source = null;
        }

        /// <summary>
        /// Click event to get friends
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnGetFriends_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dynamic friendListData = await GlobalVariable.oTSGFacebookManager.GetFriendList();
                JObject friendListJson = JObject.Parse(friendListData.ToString());
            }
            catch (Exception ex)
            {
                //MessageDialog ErrMsg = new MessageDialog("Error Ocuured!");  
            }
        }
    }
}
