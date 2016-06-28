// **************************************************************
// *
// * Written By: Ashish Gupta & Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using SocialManager.TwitterManager;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Example
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TwitterMainPage : Page
    {
        public TwitterMainPage()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                Frame.Navigate(typeof(MainPage));
            };
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isAuthenticate = await TSGTwitterManager.UserAuthenticate();
                if (isAuthenticate == true)
                {
                    Frame.Navigate(typeof(TwitterUserInfo));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("-------MainPage---btnTwitterLogin_Click------" + ex.Message);
            }
        }
    }
}
