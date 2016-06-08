// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ServiceClient.Classes
{
    public static class HelperMethods
    {
        /// <summary>
        /// This method will read the json file from storage and return data in string format.
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public async static Task<string> ReadAPIFormatJsonFile(string strFilePath)
        {
            string fileContent;
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(strFilePath));
                using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync()))
                {
                    fileContent = await sRead.ReadToEndAsync();
                }
                return fileContent;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur : HelperMehods - ReadAPIFormatJsonFile : " + ex.ToString());
            }
            return "";
        }

        public static Dictionary<string, string> dictHeader = new Dictionary<string, string>();

    }
}
