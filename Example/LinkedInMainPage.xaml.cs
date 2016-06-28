// **************************************************************
// *
// * Written By: Ashish Gupta & Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using SocialManager.LinkedInManager;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Example
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LinkedInMainPage : Page
    {
        public LinkedInMainPage()
        {
            this.InitializeComponent();
        }

        private async Task checkAndGetAccessToken()
        {
            try
            {
                // If we don't have an access token, we will try to get one
                if (string.IsNullOrEmpty(LinkedInCredential.LinkedInAccessToken))
                {
                    await TSGLinkedInManager.getAuthorizeCode();
                    bool isGetAccessToken = await TSGLinkedInManager.getAccessToken();
                    if (isGetAccessToken)
                    {
                        System.Diagnostics.Debug.WriteLine("Access Token is found, ready to send LinkedIn request...");
                        Frame.Navigate(typeof(LinkedInUserInfo));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur checkAndGetAccessToken() MainPage : " + ex.ToString());
            }

        }

        private async void btnLinkedInLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await checkAndGetAccessToken();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur btnLinkedInLogin_Click() MainPage : " + ex.ToString());
            }
        }
    }
}
