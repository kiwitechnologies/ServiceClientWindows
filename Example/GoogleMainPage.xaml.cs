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
using Windows.UI.Xaml.Navigation;
using SocialManager.GoogleManager;
using TSGSocialManager.Helpers;
using System.Diagnostics;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Example
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GoogleMainPage : Page
    {
        public GoogleMainPage()
        {
            this.InitializeComponent();
        }

        private async void btnGoogleLogin_Click(object sender, RoutedEventArgs e)
        {
            TSGGoogleManager.Configure(new GoogleConfiguration { GoogleClientId = GlobalVariable.GoogleAppId, GoogleRedirectUrl = GlobalVariable.RedirectURI, GoogleClientSecret = GlobalVariable.GoogleAppSecret });
            try
            {
                await TSGGoogleManager.Authenticate();
                Frame.Navigate(typeof(GoogleUserInfo));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception at btnGoogleLogin_Click(): " + ex.Message);
            }
            
        }
    }
}
