using ServiceClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
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
    public sealed partial class Download : Page
    {
        public Download()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        private void btnDownloadClick(object sender, RoutedEventArgs e)
        {
            try
            {
                TSGServiceManager.OnDownloadProgressChanged -= TSGServiceManager_OnProgressChanged;
                Button button = ((Button)sender);
                string url = string.Empty;
                if (button.Content.ToString() == "download image")
                {
                    url = tbImageURL.Text;
                    button.Content = "pause";
                    System.Diagnostics.Debug.WriteLine(string.Format("The URL to download the file {0}", url));
                    TSGServiceManager.downloadFile(url);
                    TSGServiceManager.OnDownloadProgressChanged += TSGServiceManager_OnProgressChanged;
                }
                if (button.Content.ToString() == "download text")
                {
                    url = tbTextURL.Text;
                    button.Content = "pause";
                    System.Diagnostics.Debug.WriteLine(string.Format("The URL to download the file {0}", url));
                    TSGServiceManager.downloadFile(url);
                    TSGServiceManager.OnDownloadProgressChanged += TSGServiceManager_OnProgressChanged;
                }
                if (button.Content.ToString() == "download video")
                {
                    url = tbVideoURL.Text;
                    button.Content = "pause";
                    System.Diagnostics.Debug.WriteLine(string.Format("The URL to download the file {0}", url));
                    TSGServiceManager.downloadFile(url);
                    TSGServiceManager.OnDownloadProgressChanged += TSGServiceManager_OnProgressChanged;
                }
            }
            catch (Exception ex)
            {

            }
        }

        void TSGServiceManager_OnProgressChanged(int progress, double downloadedBytes, double totalBytes)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Progress is {0}%.Downloaded Bytes/Total Bytes {1}/{2}", progress, downloadedBytes.ToString(), totalBytes.ToString()));
            tblockPercentageDownloaded.Text = progress.ToString() + "%";
            pbProgress.Value = progress;
            if (progress >= 100)
            {
                TSGServiceManager.OnDownloadProgressChanged -= TSGServiceManager_OnProgressChanged;
            }
        }

    }
}
