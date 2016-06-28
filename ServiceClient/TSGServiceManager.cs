// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceClient.Classes;
using ServiceClient.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace ServiceClient
{
    public static class TSGServiceManager
    {
        static BackgroundDownloader backgroundDownloader;
        static DownloadOperation downloadOperation;
        static CancellationTokenSource cancellationToken;
        static StorageFile file = null;

        /// <summary>
        /// Common method to request in POST, GET, PUT and DELETE form to get data in json format.
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns name="StatusAndResponseClass"></returns>
        public static async Task<StatusAndResponseClass> Request(RequestType requestType, string url, string data, Dictionary<string, string> header)
        {
            StatusAndResponseClass getResponse = new StatusAndResponseClass();
            HttpClient client = new HttpClient();
            if (header != null && header.Count() > 0)
            {
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            HttpResponseMessage response = new HttpResponseMessage();
            StringContent queryString;
            System.Diagnostics.Debug.WriteLine("PostStringJsonData (URL): " + url);
            System.Diagnostics.Debug.WriteLine("PostStringJsonData (POSTDATA): " + data);
            try
            {
                switch (requestType)
                {
                    case RequestType.POST:
                        queryString = new StringContent(data, UTF8Encoding.UTF8, "application/json");
                        response = await client.PostAsync(new Uri(url), queryString);
                        break;
                    case RequestType.GET:
                        response = await client.GetAsync(new Uri(url));
                        break;
                    case RequestType.PUT:
                        queryString = new StringContent(data, UTF8Encoding.UTF8, "application/json");
                        response = await client.PutAsync(new Uri(url), queryString);
                        break;
                    case RequestType.DELETE:
                        response = await client.DeleteAsync(new Uri(url + data));
                        break;
                    default:
                        break;
                }
                client.Dispose();
                HttpStatusCode statuscode = response.StatusCode;
                string responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    getResponse.responseString = responseBody;
                }
                else
                {
                    getResponse.responseString = responseBody;
                }
                getResponse.statusCode = Convert.ToInt32(statuscode);
                if (getResponse.statusCode == 404)
                {
                    getResponse.responseString = string.Empty;
                }
                return getResponse;
            }
            catch (Exception ex)
            {
                return getResponse;
            }
        }

        /// <summary>
        /// Common method to request in POST, GET, PUT and DELETE form to get data in key value format.
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns name="StatusAndResponseClass"></returns>
        public static async Task<StatusAndResponseClass> Request(RequestType requestType, string url, Dictionary<string, string> data, Dictionary<string, string> header)
        {
            StatusAndResponseClass getResponse = new StatusAndResponseClass();
            HttpClient client = new HttpClient();
            if (header != null && header.Count() > 0)
            {
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            IProgress<Windows.Web.Http.HttpProgress> progress = new Progress<Windows.Web.Http.HttpProgress>(ProgressHandler);
            HttpResponseMessage response = new HttpResponseMessage();
            HttpContent content;
            System.Diagnostics.Debug.WriteLine("PostStringJsonData (URL): " + url);
            System.Diagnostics.Debug.WriteLine("PostStringJsonData (POSTDATA): " + data);
            try
            {
                switch (requestType)
                {
                    case RequestType.POST:
                        content = new FormUrlEncodedContent(data);
                        response = await client.PostAsync(new Uri(url), content, cancellationToken.Token);
                        break;
                    case RequestType.GET:
                        var message = new HttpRequestMessage(HttpMethod.Get, url);
                        response = await client.SendAsync(message, cancellationToken.Token);
                        //response = await client.GetAsync(new Uri(url));
                        break;
                    case RequestType.PUT:
                        content = new FormUrlEncodedContent(data);
                        response = await client.PutAsync(new Uri(url), content, cancellationToken.Token);
                        break;
                    case RequestType.DELETE:
                        content = new FormUrlEncodedContent(data);
                        response = await client.DeleteAsync(new Uri(url + content), cancellationToken.Token);
                        break;
                    default:
                        break;
                }
                client.Dispose();
                HttpStatusCode statuscode = response.StatusCode;
                string responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    getResponse.responseString = responseBody;
                }
                else
                {
                    getResponse.responseString = responseBody;
                }
                getResponse.statusCode = Convert.ToInt32(statuscode);
                if (getResponse.statusCode == 404)
                {
                    getResponse.responseString = string.Empty;
                }
                return getResponse;
            }
            catch (Exception ex)
            {
                return getResponse;
            }
        }


        private static void ProgressHandler(Windows.Web.Http.HttpProgress progress)
        {
            Debug.WriteLine("StageField : " + progress.Stage.ToString());
            Debug.WriteLine("RetriesField : " + progress.Retries.ToString(CultureInfo.InvariantCulture));
            Debug.WriteLine("BytesSentField : " + progress.BytesSent.ToString(CultureInfo.InvariantCulture));
            Debug.WriteLine("BytesReceivedField : " + progress.BytesReceived.ToString(CultureInfo.InvariantCulture));

            ulong totalBytesToSend = 0;
            if (progress.TotalBytesToSend.HasValue)
            {
                totalBytesToSend = progress.TotalBytesToSend.Value;
                Debug.WriteLine("TotalBytesToSendField : " + progress.TotalBytesToSend.Value);
            }
            else
            {
                Debug.WriteLine("TotalBytesToSendField : unknown");
            }

            ulong totalBytesToReceive = 0;
            if (progress.TotalBytesToReceive.HasValue)
            {
                totalBytesToReceive = progress.TotalBytesToReceive.Value;
                Debug.WriteLine("totalBytesToReceive : " + totalBytesToReceive.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                Debug.WriteLine("TotalBytesToReceiveField : unknown");
            }

            double requestProgress = 0;
            if (progress.Stage == Windows.Web.Http.HttpProgressStage.SendingContent && totalBytesToSend > 0)
            {
                requestProgress = progress.BytesSent * 50 / totalBytesToSend;
            }
            else if (progress.Stage == Windows.Web.Http.HttpProgressStage.ReceivingContent)
            {
                // Start with 50 percent, request content was already sent.
                requestProgress += 50;

                if (totalBytesToReceive > 0)
                {
                    requestProgress += progress.BytesReceived * 50 / totalBytesToReceive;
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// This is to cancel all Http request.
        /// </summary>
        public static void CancelHttpRequest()
        {
            if (cancellationToken != null)
            {
                cancellationToken.Cancel();
                cancellationToken.Dispose();

                // Re-create the CancellationTokenSource.
                cancellationToken = new CancellationTokenSource();
            }
        }

        /// <summary>
        /// This will perform action with http client. User have to pass action name and body params.
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="bodyParams"></param>
        public static async void PerformAction(string actionName, Dictionary<string, object> bodyParams)
        {
            try
            {
                PerformAction(actionName, bodyParams, null);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// This will perform action with http client. User have to pass action name, body params and header params.
        /// </summary>
        /// <param name="actionID"></param>
        /// <param name="bodyParams"></param>
        /// <param name="headerParams"></param>
        public static async void PerformAction(string actionID, Dictionary<string, object> bodyParams, Dictionary<string, string> headerParams)
        {
            try
            {
                Err_Logger.Reset();
                if (string.IsNullOrEmpty(actionID))
                {
                    if (Err_Logger.err_MissingAction == null)
                    {
                        Err_Logger.err_MissingAction = new Err_MissingAction();
                    }
                    Err_Logger.err_MissingAction.MissingAction = string.Format(ValidatorHandler.ERR_EMPTY_ACTION_NAME, "Action Name");
                    //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    //{
                    //    MessageDialog message = new MessageDialog(string.Format(ValidatorHandler.ERR_EMPTY_ACTION_NAME, "Action Name"));
                    //    await message.ShowAsync();
                    //    return;
                    //});
                }
                ValidatorHandler oValidatorHandler = new ValidatorHandler();
                oValidatorHandler.checkHeaders(actionID, headerParams);
                string strResult = await oValidatorHandler.checkBodyParameters(actionID, bodyParams);

                JObject jsonObject = new JObject();
                jsonObject["Err_BodyParameter"] = JObject.Parse(JsonConvert.SerializeObject(Err_Logger.err_BodyParameter));
                jsonObject["Err_HeaderParameter"] = JObject.Parse(JsonConvert.SerializeObject(Err_Logger.err_HeaderParameter));
                jsonObject["MissingAction"] = Err_Logger.err_MissingAction.MissingAction;
                strResult = jsonObject.ToString();
                if (!string.IsNullOrEmpty(strResult))
                {
                    MessageDialog message = new MessageDialog(strResult);
                    await message.ShowAsync();
                }
                else
                {
                    API oAPI = new API();
                    oAPI.ActionID = actionID;
                    var result = await oAPI.GetAPIDetail();
                    if (result != null)
                    {
                        RequestType m_RequestType = RequestType.POST;
                        switch (result.request_type)
                        {
                            case "POST":
                                m_RequestType = RequestType.POST;
                                break;
                            case "GET":
                                m_RequestType = RequestType.GET;
                                break;
                            case "PUT":
                                m_RequestType = RequestType.PUT;
                                break;
                            case "DELETE":
                                m_RequestType = RequestType.DELETE;
                                break;
                            default:
                                break;
                        }
                        Dictionary<string, string> tempBodyParams = bodyParams.ToDictionary(k => k.Key, k => k.Value.ToString());
                        StatusAndResponseClass m_StatusAndResponseClass = await Request(m_RequestType, actionID, tempBodyParams, headerParams);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// This will download the file and give the progress in % and bytes.
        /// </summary>
        /// <param name="strURL"></param>
        public static async void downloadFile(string strURL)
        {
            try
            {
                backgroundDownloader = new BackgroundDownloader();
                if (!string.IsNullOrEmpty(strURL))
                {
                    string strVideoExtensions = "webm,mkv,flv,mp4,vob,gif,avi,mpeg,3gp";
                    string strImageExtensions = "jpeg,bmp,png,jpg";
                    string strTextExtensions = "txt";
                    string strExtension = string.Empty;
                    MediaTypes mediaType = 0;
                    strExtension = strURL.Substring(strURL.LastIndexOf('.'));
                    strExtension = strExtension.Split('.')[1];
                    if (strVideoExtensions.Contains(strExtension))
                    {
                        mediaType = MediaTypes.Video;
                    }
                    else if (strImageExtensions.Contains(strExtension))
                    {
                        mediaType = MediaTypes.Image;
                    }
                    else if (strTextExtensions.Contains(strExtension))
                    {
                        mediaType = MediaTypes.Text;
                    }
                    await CreateFolderAndFile(mediaType, strExtension);
                    MediaDownloader(strURL, mediaType);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// This is the download handler.
        /// </summary>
        /// <param name="strURL"></param>
        /// <param name="mediaType"></param>
        private static async void MediaDownloader(string strURL, MediaTypes mediaType)
        {
            try
            {
                downloadOperation = backgroundDownloader.CreateDownload(new Uri(strURL, UriKind.RelativeOrAbsolute), file);
                Progress<DownloadOperation> progress = new Progress<DownloadOperation>(ProgressChanged);
                cancellationToken = new CancellationTokenSource();
                await downloadOperation.StartAsync().AsTask(cancellationToken.Token, progress);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Event and delegate to receive file progress.
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="downloadedBytes"></param>
        /// <param name="totalBytes"></param>
        public delegate void DelegateDownloadProgressChanged(int progress, double downloadedBytes, double totalBytes);
        public static event DelegateDownloadProgressChanged OnDownloadProgressChanged = null;

        /// <summary>
        /// This is the progress changed event which will return progress.
        /// </summary>
        /// <param name="downloadOperation"></param>
        private static void ProgressChanged(DownloadOperation downloadOperation)
        {
            int progress = (int)(100 * ((double)downloadOperation.Progress.BytesReceived / (double)downloadOperation.Progress.TotalBytesToReceive));
            if (OnDownloadProgressChanged != null)
            {
                OnDownloadProgressChanged(progress, (double)downloadOperation.Progress.BytesReceived, (double)downloadOperation.Progress.TotalBytesToReceive);
            }
            //System.Diagnostics.Debug.WriteLine(progress);
            //tblockPercentageDownloaded.Text = progress.ToString();
            //pbProgress.Value = progress;
            switch (downloadOperation.Progress.Status)
            {
                case BackgroundTransferStatus.Running:
                    break;
                case BackgroundTransferStatus.PausedByApplication:
                    break;
                case BackgroundTransferStatus.PausedCostedNetwork:
                    break;
                case BackgroundTransferStatus.PausedNoNetwork:
                    break;
                case BackgroundTransferStatus.Error:
                    break;
            }
            if (progress >= 100)
            {
                downloadOperation = null;
            }
        }

        /// <summary>
        /// This will save the file in folder.
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="strExtension"></param>
        /// <returns></returns>
        private static async Task CreateFolderAndFile(MediaTypes mediaType, string strExtension)
        {
            var savedPicturesFolder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Downloads", CreationCollisionOption.OpenIfExists);
            if (mediaType == MediaTypes.Image)
            {
                file = await savedPicturesFolder.CreateFileAsync("IMG_" + DateTime.Now.ToString("yyyyMMddHHss") + "." + strExtension, CreationCollisionOption.ReplaceExisting);
            }
            else if (mediaType == MediaTypes.Text)
            {
                file = await savedPicturesFolder.CreateFileAsync("DOC_" + DateTime.Now.ToString("yyyyMMddHHss") + "." + strExtension, CreationCollisionOption.ReplaceExisting);
            }
            else if (mediaType == MediaTypes.Video)
            {
                file = await savedPicturesFolder.CreateFileAsync("VID_" + DateTime.Now.ToString("yyyyMMddHHss") + "." + strExtension, CreationCollisionOption.ReplaceExisting);
            }
            else
            {
                file = await savedPicturesFolder.CreateFileAsync("FILE_" + DateTime.Now.ToString("yyyyMMddHHss") + "." + strExtension, CreationCollisionOption.ReplaceExisting);
            }
        }

        /// <summary>
        /// This will upload the file and give the progress in % and bytes.
        /// </summary>
        /// <param name="strURL"></param>
        /// <param name="headers"></param>
        public static async void uploadFile(string strURL, StorageFile file)
        {
            try
            {
                Uri uri = new Uri(strURL);
                cancellationToken = new CancellationTokenSource();
                BackgroundUploader uploader = new BackgroundUploader();
                if (HelperMethods.dictHeader != null && HelperMethods.dictHeader.Count() > 0)
                {
                    foreach (var items in HelperMethods.dictHeader)
                    {
                        uploader.SetRequestHeader(items.Key, items.Value);
                    }
                }
                UploadOperation upload = uploader.CreateUpload(uri, file);
                HandleUploadAsync(upload, true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur uploadFile -- TSGServiceManager: " + ex.ToString());
            }
        }

        /// <summary>
        /// This is the upload handler.
        /// </summary>
        /// <param name="upload"></param>
        /// <param name="start"></param>
        private static async void HandleUploadAsync(UploadOperation upload, bool start)
        {
            try
            {
                Progress<UploadOperation> progressCallback = new Progress<UploadOperation>(UploadProgress);
                if (start)
                {
                    await upload.StartAsync().AsTask(cancellationToken.Token, progressCallback);
                }
                ResponseInformation response = upload.GetResponseInformation();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Event and delegate to receive file progress.
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="uploadedBytes"></param>
        /// <param name="totalBytes"></param>
        public delegate void DelegateUploadProgressChanged(double progress, double uploadedBytes, double totalBytes);
        public static event DelegateUploadProgressChanged OnUploadProgressChanged = null;

        /// <summary>
        /// This is the progress changed event which will return progress.
        /// </summary>
        /// <param name="upload"></param>
        private static void UploadProgress(UploadOperation upload)
        {
            BackgroundUploadProgress progress = upload.Progress;

            double percentSent = 100;
            if (progress.TotalBytesToSend > 0)
            {
                percentSent = progress.BytesSent * 100 / progress.TotalBytesToSend;
                if (OnUploadProgressChanged != null)
                {
                    OnUploadProgressChanged(percentSent, (double)progress.BytesSent, (double)progress.TotalBytesToSend);
                }
            }
            //pbProgress.Value = percentSent;
            //tblockPercentageDownloaded.Text = percentSent + "%";
        }

        public static Dictionary<string, string> GetHeaders()
        {
            return HelperMethods.dictHeader;
        }

        public static void SetHeaders(Dictionary<string, string> dictHeaders)
        {
            foreach (var item in dictHeaders)
            {
                if (HelperMethods.dictHeader.ContainsKey(item.Key))
                {
                    HelperMethods.dictHeader[item.Key] = item.Value;
                }
                else
                {
                    HelperMethods.dictHeader.Add(item.Key, item.Value);
                }

            }
        }

        public static void ClearHeaders()
        {
            HelperMethods.dictHeader.Clear();
        }
    }

    /// <summary>
    /// Enum of request type.
    /// </summary>
    public enum RequestType
    {
        POST,
        GET,
        PUT,
        DELETE
    }

    /// <summary>
    /// Enum of media type
    /// </summary>
    public enum MediaTypes
    {
        Text, Image, Video
    }
}
