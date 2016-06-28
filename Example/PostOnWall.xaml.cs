// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using Facebook;
using Facebook.Client;
using SocialManager.FacebookManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TSGSocialManager.Helpers;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
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
    public sealed partial class PostOnWall : Page
    {
        StorageFile m_StorageFile;
        byte[] imageByte;

        public PostOnWall()
        {
            this.InitializeComponent();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                Frame.Navigate(typeof(FacebookMainPage));
            };
        }

        private async void BtnFaceBookPost_Click(object sender, RoutedEventArgs e)
        {
            TSGFacebookManager oTSGFacebookManager = new TSGFacebookManager();
            string strResult = await oTSGFacebookManager.PostLocalImageOnWall(m_StorageFile, txtMessage.Text, PrivacyType.ALL_FRIENDS.ToString());
            if (string.IsNullOrEmpty(strResult))
            {
                strResult = "Error Ocuured!";
            }
            MessageDialog SuccessMsg = new MessageDialog(strResult);
            await SuccessMsg.ShowAsync();
        }

        private async void imgToPost_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string strEx = string.Empty;
            try
            {
                CoreApplicationView view = CoreApplication.GetCurrentView();
                FileOpenPicker picker = new FileOpenPicker();
                // Set SuggestedStartLocation    
                picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Clear();
                picker.FileTypeFilter.Add("*");
                m_StorageFile = await picker.PickSingleFileAsync();
                if (m_StorageFile != null)
                {
                    var stream = await m_StorageFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    var image = new BitmapImage();
                    image.SetSource(stream);
                    imgToPost.Source = image;
                    var reader = new DataReader(stream.GetInputStreamAt(0));
                    imageByte = new byte[stream.Size];
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(imageByte);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                strEx = ex.ToString();
            }
            if (!string.IsNullOrEmpty(strEx))
            {
                MessageDialog msg = new MessageDialog(strEx);
                await msg.ShowAsync();
            }
        }
    }
}
