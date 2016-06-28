// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Example
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void btnFacebook_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FacebookMainPage));
        }

        private void btnTwitter_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TwitterMainPage));
        }

        private void btnLinkedIn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LinkedInMainPage));
        }

        private void btnGoogle_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GoogleMainPage));
        }

        private void btnInstagram_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(InstagramMainPage));
        }

        private void btnGoogleCalendar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GoogleCalendarMainPage));
        }
    }
}
