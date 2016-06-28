// **************************************************************
// *
// * Written By: Ashish Gupta & Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using SocialManager.TwitterManager;
using System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Example
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PostTweet : Page
    {
        public PostTweet()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                Frame.Navigate(typeof(TwitterUserInfo));
            };
        }

        private async void TweetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string struserInput = txtTweet.Text.Trim();
                if (!string.IsNullOrEmpty(struserInput))
                {
                    var TweetResult = await TSGTwitterManager.PostTweet(struserInput);
                    if (TweetResult.Item1 == true)
                    {
                        await new MessageDialog("You Tweeted: " + TweetResult.Item2.Text, "Success!").ShowAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("-------Posttweet---TweetButton_Click------" + ex.Message);
            }

        }
    }
}
