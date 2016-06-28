// **************************************************************
// *
// * Written By: Ashish Gupta & Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using Newtonsoft.Json;
using SocialManager.LinkedInManager;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Example
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LinkedInUserInfo : Page
    {
        public LinkedInUserInfo()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await GetUserInfo();
        }

        // Get UserInfo 
        public async Task GetUserInfo()
        {
            try
            {
                StatusAndResponseClass response = await TSGLinkedInManager.GetConfirmation(LinkedInCredential.GetUserProfileURl(LinkedInCredential.LinkedInAccessToken));
                if (response.statusCode == 200 || response.statusCode == 201)
                {
                    csUserInfo responseUserInfo;
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(csUserInfo));
                    using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(response.responseString)))
                    {
                        responseUserInfo = serializer.ReadObject(stream) as csUserInfo;
                    }
                    if (responseUserInfo != null)
                    {
                        grdUserProfile.DataContext = responseUserInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur GetUserInfo() UserInfo : " + ex.ToString());
            }
        }

        // Get Linkedin Formattted Object 
        public static string PostLinkedInNetworkUpdate(string accessToken, string title, string submittedUrl = "", string submittedImageUrl = "")
        {
            string strrequestJson = string.Empty;
            try
            {
                var shareMsg =
                new
                {
                    comment =
                        "BIG Code is hello",
                    content =
                        new
                        {
                            title = "Api code in C#",
                            submitted_url = "http://www.bigcode.net",
                            submitted_image_url =
                                "http://2.bp.blogspot.com/-8r_lWT_32lQ/TxrQW12ngPI/AAAAAAAAI70/ifMF4Z16M-Y/s1600/SQL+Server+session+state.png",
                            description = string.Empty
                        },
                    visibility = new { code = "anyone" }
                };
                strrequestJson = JsonConvert.SerializeObject(shareMsg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur PostLinkedInNetworkUpdate() LinkedInHelper : " + ex.ToString());
            }
            return strrequestJson;
        }

        private void PublishComment_Click(object sender, RoutedEventArgs e)
        {
            TSGLinkedInManager.PublishComment(LinkedInCredential.LinkedInAccessToken, "Test");
        }

        private void PublishCommentandContent_Click(object sender, RoutedEventArgs e)
        {
            TSGLinkedInManager.PublishCommentandContent(LinkedInCredential.LinkedInAccessToken, "comment", "title", "http://www.bigcode.net", "http://2.bp.blogspot.com/-8r_lWT_32lQ/TxrQW12ngPI/AAAAAAAAI70/ifMF4Z16M-Y/s1600/SQL+Server+session+state.png", "description");
        }
    }
}
