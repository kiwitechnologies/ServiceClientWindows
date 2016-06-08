// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using ServiceClient;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Example
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ServiceClientExample : Page
    {
        public ServiceClientExample()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void SetProjectID_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        private void GetRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<string, object> dictData = new Dictionary<string, object>();
                dictData.Add("userID", "asdfhjasd8foasdf");
                dictData.Add("pwd", "asdfhjasd8foasdf");
                TSGServiceManager.PerformAction("5745591afec9101a0a63f23d", dictData);
            }
            catch (Exception ex)
            {

            }
        }

        private void PostRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        private void PutRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        private void UploadRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame.Navigate(typeof(Upload));
            }
            catch (Exception ex)
            {

            }
        }

        private void DownloadRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame.Navigate(typeof(Download));
            }
            catch (Exception ex)
            {

            }
        }

        private void DeleteRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }
    }
}
