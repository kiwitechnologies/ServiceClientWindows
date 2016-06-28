// **************************************************************
// *
// * Written By: Ashish Gupta & Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using SocialManager.TwitterManager;
using System;
using TSGSocialManager.Helpers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Example
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TwitterUserInfo : Page
    {
        public TwitterUserInfo()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                Frame.Navigate(typeof(TwitterMainPage));
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                if (GlobalVariable.TwitterUserInfo != null)
                    grdUserProfile.DataContext = GlobalVariable.TwitterUserInfo;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("-------OnNavigatedTo---UserProfileInformation------" + ex.Message);
            }

        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PostTweet));
        }

        private void AppBarButton1_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SerachTweet));
        }

        private void FriendList_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TwitterFriendList));
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            TSGTwitterManager.TwitterLogout();
            Frame.Navigate(typeof(TwitterMainPage));
        }
    }
}
