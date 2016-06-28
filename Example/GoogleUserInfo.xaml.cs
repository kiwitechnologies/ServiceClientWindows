using SocialManager.GoogleManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Example
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GoogleUserInfo : Page
    {
        public GoogleUserInfo()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            UserInfo user = await TSGGoogleManager.GetUserInfo();
            if (user != null)
            {
                imgUser.Source = new BitmapImage(new Uri(user.Picture, UriKind.RelativeOrAbsolute));
                tbUserName.Text = user.Name;
                tbUserID.Text = user.Id;
                tbUserEmail.Text = user.Email;
            }
            else
            {
                tbUserID.Text = "failed to get user information";
            }
        }

        private async void GetCircle_Click(object sender, RoutedEventArgs e)
        {
           // await TSGGoogleManager.GetCircleInfo();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            TSGGoogleManager.LogOut();
            Frame.Navigate(typeof(GoogleMainPage));
        }
    }
}
