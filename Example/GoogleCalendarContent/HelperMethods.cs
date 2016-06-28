using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TSGSocialManager.Helpers;
using Windows.Storage;

namespace Example.GoogleCalendarContent
{
    public class HelperMethods
    {
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly, CalendarService.Scope.Calendar };
        static string ApplicationName = "Google Calendar API .NET Quickstart";

        public static async Task Authenticatoin()
        {
            try
            {
                UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(new Uri(@"ms-appx:///GoogleCalendarContent/client_secret.json"), Scopes, "user", CancellationToken.None);
                GlobalVariable.GoogleCalendarUserCredential = credential;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur : HelperMehods - ReadOptionsJsonFile : " + ex.ToString());
            }
        }
    }
}
