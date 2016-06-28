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
    public sealed partial class SerachTweet : Page
    {
        public SerachTweet()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                Frame.Navigate(typeof(TwitterUserInfo));
            };
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string struserInput = txtsearch.Text.Trim();
                if (!string.IsNullOrEmpty(struserInput))
                {
                    var SeacrhResult = await TSGTwitterManager.SearchTweet(struserInput);
                    if (SeacrhResult != null)
                    {
                        lstTweetList.ItemsSource = SeacrhResult;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("-------SerachTweet---AppBarButton_Click------" + ex.Message);
            }
        }

        private void txtsearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
