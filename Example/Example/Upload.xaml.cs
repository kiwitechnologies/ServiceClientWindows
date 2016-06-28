// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using ServiceClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Media.Editing;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Example
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Upload : Page
    {
        static Dictionary<string, string> uploadHeaders;
        static string uploadUri;
        StorageFile m_StorageFile;
        StorageFile m_NewStorageFile;

        public Upload()
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

        private async void btnBrowseFile_Click(object sender, RoutedEventArgs e)
        {
            string strEx = string.Empty;
            try
            {
                CoreApplicationView view = CoreApplication.GetCurrentView();
                view.Activated -= view_Activated;
                view.Activated += view_Activated;
                FileOpenPicker picker = new FileOpenPicker();
                picker.FileTypeFilter.Clear();
                picker.FileTypeFilter.Add("*");
                picker.PickSingleFileAndContinue();
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

        async void view_Activated(CoreApplicationView sender, Windows.ApplicationModel.Activation.IActivatedEventArgs args)
        {
            string strEx = string.Empty;
            try
            {
                FileOpenPickerContinuationEventArgs result = args as FileOpenPickerContinuationEventArgs;
                if (result != null && result.Files.Any())
                {
                    m_StorageFile = result.Files.FirstOrDefault();
                    //var composition = new MediaComposition();
                    //composition.Clips.Add(await MediaClip.CreateFromFileAsync(result.Files.FirstOrDefault()));
                    //await composition.RenderToFileAsync(file);
                    var basicProperties = await result.Files.FirstOrDefault().GetBasicPropertiesAsync();
                    ImageProperties imgProp = await result.Files.FirstOrDefault().Properties.GetImagePropertiesAsync();
                    var savedPictureStream = await result.Files.FirstOrDefault().OpenAsync(FileAccessMode.Read);
                    Debug.WriteLine("Default size :  " + Calculatesize(savedPictureStream.Size));
                    var length = basicProperties.Size;
                    //string strLength = Calculatesize(length);
                    WriteableBitmap wb = new WriteableBitmap(1, 1);
                    await wb.SetSourceAsync(savedPictureStream);
                    using (var storageStream = await result.Files.FirstOrDefault().OpenAsync(FileAccessMode.Read))
                    {
                        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, storageStream);
                        var pixelStream = wb.PixelBuffer.AsStream();
                        var pixels = new byte[pixelStream.Length];
                        await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                        encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)wb.PixelWidth, (uint)wb.PixelHeight, 48, 48, pixels);
                        await encoder.FlushAsync();
                    }
                    m_NewStorageFile = result.Files.FirstOrDefault();
                    //TSGServiceManager.uploadFile(strURL, dictHeader, result.Files.FirstOrDefault());
                    var thumb = await LoadVideoImageThumb(result.Files.FirstOrDefault());
                    //var thumb = new BitmapImage();
                    //var tt = await result.Files.FirstOrDefault().GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.MusicView, 200, ThumbnailOptions.UseCurrentScale);
                    //thumb.SetSource(tt);
                    imgUpload.Source = thumb;
                    imgTemp.Source = wb;
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

        public static async Task<BitmapImage> LoadVideoImageThumb(StorageFile file)
        {
            BitmapImage bitmapImage = new BitmapImage();
            try
            {
                var thumb = await file.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.VideosView, 100, Windows.Storage.FileProperties.ThumbnailOptions.UseCurrentScale);
                bitmapImage.SetSource(thumb);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("-----chatMedaiStorage------LoadVideoImageThumb()---" + ex.Message);
            }
            return bitmapImage;
        }

        void TSGServiceManager_OnUploadProgressChanged(double progress, double uploadedBytes, double totalBytes)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Progress is {0}%.Uploaded Bytes/Total Bytes {1}/{2}", progress, uploadedBytes.ToString(), totalBytes.ToString()));
            tblockPercentageDownloaded.Text = progress.ToString() + "%";
            pbProgress.Value = progress;
            if (progress >= 100)
            {
                TSGServiceManager.OnUploadProgressChanged -= TSGServiceManager_OnUploadProgressChanged;
            }
        }

        private async void btnUploadFile_Click(object sender, RoutedEventArgs e)
        {
            if (m_StorageFile != null)
            {
                string strURL = "http://api.imagga.com/v1/content";
                Dictionary<string, string> dictHeader = new Dictionary<string, string>();
                dictHeader.Add("Authorization", "Basic YWNjXzlhMzljN2M4ZTUyMjBjNzpjNTFkMTE2MmE5ZWQ2ZDRlMDVjMGMxNTFlMGRhMGU5Yw==");
                TSGServiceManager.SetHeaders(dictHeader);
                TSGServiceManager.OnUploadProgressChanged -= TSGServiceManager_OnUploadProgressChanged;
                TSGServiceManager.OnUploadProgressChanged += TSGServiceManager_OnUploadProgressChanged;
                TSGServiceManager.uploadFile(strURL, m_NewStorageFile);
                var thumb = await LoadVideoImageThumb(m_NewStorageFile);
                imgUpload.Source = thumb;
            }
            else
            {
                MessageDialog msg = new MessageDialog("Please browse a file.");
                await msg.ShowAsync();
            }
        }

        public string Calculatesize(double sizeInBytes)
        {
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            var size = new Size(bounds.Width * scaleFactor, bounds.Height * scaleFactor);

            const double terabyte = 1099511627776;
            const double gigabyte = 1073741824;
            const double megabyte = 1048576;
            const double kilobyte = 1024;

            string result;
            double theSize = 0;
            string units;

            if (sizeInBytes <= 0.1)
            {
                result = "0" + " " + "bytes";
                return result;
            }

            if (sizeInBytes >= terabyte)
            {
                theSize = sizeInBytes / terabyte;
                units = " TB";
            }
            else
            {
                if (sizeInBytes >= gigabyte)
                {
                    theSize = sizeInBytes / gigabyte;
                    units = " GB";
                }
                else
                {
                    if (sizeInBytes >= megabyte)
                    {
                        theSize = sizeInBytes / megabyte;
                        units = " MB";
                    }
                    else
                    {
                        if (sizeInBytes >= kilobyte)
                        {
                            theSize = sizeInBytes / kilobyte;
                            units = " KB";
                        }
                        else
                        {
                            theSize = sizeInBytes;
                            units = " bytes";
                        }
                    }
                }
            }

            if (units != "bytes")
            {
                result = theSize.ToString("0.00") + " " + units;
            }
            else
            {
                result = theSize.ToString("0.0") + " " + units;
            }
            return result;
        }

        // Summary:
        //     Specifies the alpha mode of pixel data.
        //public enum BitmapAlphaMode
        //{
        //    // Summary:
        //    //     The alpha value has been premultiplied. Each color is first scaled by the
        //    //     alpha value. The alpha value itself is the same in both straight and premultiplied
        //    //     alpha. Typically, no color channel value is greater than the alpha channel
        //    //     value. If a color channel value in a premultiplied format is greater than
        //    //     the alpha channel, the standard source-over blending math results in an additive
        //    //     blend.
        //    Premultiplied = 0,
        //    //
        //    // Summary:
        //    //     The alpha value has not been premultiplied. The alpha channel indicates the
        //    //     transparency of the color.
        //    Straight = 1,
        //    //
        //    // Summary:
        //    //     The alpha value is ignored.
        //    Ignore = 2,
        //}
        // Summary:
        //     Specifies the pixel format of pixel data. Each enumeration value defines
        //     a channel ordering, bit depth, and type.
        //public enum BitmapPixelFormat
        //{
        //    // Summary:
        //    //     The pixel format is unknown.
        //    Unknown = 0,
        //    //
        //    // Summary:
        //    //     The pixel format is R16B16G16A16 unsigned integer.
        //    Rgba16 = 12,
        //    //
        //    // Summary:
        //    //     The pixel format is R8G8B8A8 unsigned integer.
        //    Rgba8 = 30,
        //    //
        //    // Summary:
        //    //     The pixel format is B8G8R8A8 unsigned integer.
        //    Bgra8 = 87,
        //}

        private async Task<StorageFile> WriteableBitmapToStorageFile(WriteableBitmap WB, FileFormat fileFormat)
        {
            string FileName = "MyFile.";
            Guid BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
            switch (fileFormat)
            {
                case FileFormat.Jpeg:
                    FileName += "jpeg";
                    BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
                    break;
                case FileFormat.Png:
                    FileName += "png";
                    BitmapEncoderGuid = BitmapEncoder.PngEncoderId;
                    break;
                case FileFormat.Bmp:
                    FileName += "bmp";
                    BitmapEncoderGuid = BitmapEncoder.BmpEncoderId;
                    break;
                case FileFormat.Tiff:
                    FileName += "tiff";
                    BitmapEncoderGuid = BitmapEncoder.TiffEncoderId;
                    break;
                case FileFormat.Gif:
                    FileName += "gif";
                    BitmapEncoderGuid = BitmapEncoder.GifEncoderId;
                    break;
            }
            var file = await Windows.Storage.ApplicationData.Current.TemporaryFolder.CreateFileAsync(FileName, CreationCollisionOption.GenerateUniqueName);
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoderGuid, stream);
                Stream pixelStream = WB.PixelBuffer.AsStream();
                byte[] pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                          (uint)WB.PixelWidth,
                          (uint)WB.PixelHeight,
                          96.0,
                          96.0,
                          pixels);
                await encoder.FlushAsync();
            }
            return file;
        }
        private enum FileFormat
        {
            Jpeg,
            Png,
            Bmp,
            Tiff,
            Gif
        }

        private void RadioButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var result = sender as RadioButton;
        }

        private async void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                uint dpix = 0;
                uint dpiy = 0;
                var result = sender as RadioButton;

                if (m_StorageFile != null)
                {
                    using (var storageStream = await m_StorageFile.OpenAsync(FileAccessMode.Read))
                    {
                        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(storageStream);
                        var originalPixelWidth = decoder.PixelWidth;
                        var originalPixelHeight = decoder.PixelHeight;
                        switch (result.Content.ToString())
                        {
                            case "Low":
                                dpix = originalPixelWidth / 3;
                                dpiy = originalPixelHeight / 3;
                                break;
                            case "Medium":
                                dpix = originalPixelWidth / 2;
                                dpiy = originalPixelHeight / 2;
                                break;
                            case "High":
                                dpix = (uint)(originalPixelWidth / 1.25);
                                dpiy = (uint)(originalPixelHeight / 1.25);
                                break;
                            case "Default":
                                dpix = originalPixelWidth;
                                dpiy = originalPixelHeight;
                                break;
                            default:
                                dpix = originalPixelWidth;
                                dpiy = originalPixelHeight;
                                break;
                        }
                        Debug.WriteLine("originalPixelWidth :  " + originalPixelWidth);
                        Debug.WriteLine("originalPixelHeight :  " + originalPixelHeight);
                        Debug.WriteLine("dpix :  " + dpix);
                        Debug.WriteLine("dpiy :  " + dpiy);
                        // create a new stream and encoder for the new image
                        InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream();
                        BitmapEncoder enc = await BitmapEncoder.CreateForTranscodingAsync(ras, decoder);

                        // convert the entire bitmap to a 100px by 100px bitmap
                        enc.BitmapTransform.ScaledWidth = dpix;
                        enc.BitmapTransform.ScaledHeight = dpiy;
                        // write out to the stream
                        try
                        {
                            await enc.FlushAsync();
                        }
                        catch (Exception ex)
                        {
                            string s = ex.ToString();
                        }
                        // render the stream to the screen
                        BitmapImage bImg = new BitmapImage();
                        bImg.SetSource(ras);
                        imgTemp.Source = bImg; // image element in xaml
                        ulong size = ras.Size;
                        Debug.WriteLine("size :  " + Calculatesize(size));
                        var bytes = new byte[ras.Size];
                        var file = await Windows.Storage.ApplicationData.Current.TemporaryFolder.CreateFileAsync(m_StorageFile.Name, CreationCollisionOption.GenerateUniqueName);
                        using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            using (IOutputStream outputStream = fileStream.GetOutputStreamAt(0))
                            {
                                using (DataWriter dataWriter = new DataWriter(outputStream))
                                {
                                    dataWriter.WriteBytes(bytes);
                                    await dataWriter.StoreAsync(); // 
                                    dataWriter.DetachStream();
                                }
                                // write data on the empty file:
                                outputStream.FlushAsync();
                            }
                            fileStream.FlushAsync();
                        }
                        m_NewStorageFile = file;
                        var basicProperties = await m_NewStorageFile.GetBasicPropertiesAsync();
                        Debug.WriteLine("m_NewStorageFile size :  " + Calculatesize(basicProperties.Size));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

    }
}
