// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using ServiceClient.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServiceClient.Classes
{
    public class ValidatorHandler
    {
        public static string ERR_DB_NOT_INITIALISED = "DB not initialised. Please initialise it first by calling TSGAPIController.init()";
        public static string ERR_EMPTY_ACTION_NAME = "{0} can not be empty.";
        public static string ERR_ACTION_NAME_NOT_FOUND = "{0} not found.";
        public static string ERR_KEYNAME_NOT_FOUND = "not found.";
        public static string ERR_KEYNAME_WRONG_DATA_TYPE = "not a valid data type. {0} expects a {1} type.";
        public static string ERR_KEYNAME_WRONG_STRING_FORMAT_TYPE = "format is not valid. It expects a {0} format.";
        public static string ERR_KEYNAME_WRONG_LENGTH = "length is not correct. It should be between {0} - {1} Length.";
        public static string ERR_KEYNAME_WRONG_SIZE = "size is not correct. It should be less than {1}";

        /// <summary>
        /// This method will check the body parameter which will be given by user with already saved data in DB.
        /// </summary>
        /// <param name="strActionId"></param>
        /// <param name="bodyParams"></param>
        public async Task<string> checkBodyParameters(string strActionId, Dictionary<string, object> bodyParams)
        {
            string strError = string.Empty;
            API oAPI = new API();
            List<BodyParameter> lstBodyParameter = await oAPI.getKeyDetails(strActionId);
            if (lstBodyParameter != null && lstBodyParameter.Count() > 0)
            {
                foreach (var item in lstBodyParameter)
                {
                    //Check for all required key_name available
                    if (!bodyParams.ContainsKey(item.key_name))
                    {
                        if (item.validations.require == 1)
                        {
                            List<string> lstError = new List<string>();
                            List<string> tempErrorList;
                            lstError.Add(string.Format(ERR_KEYNAME_NOT_FOUND, item.key_name));

                            if (Err_Logger.err_BodyParameter.missing.TryGetValue(item.key_name, out tempErrorList))
                            {
                                Err_Logger.err_BodyParameter.missing[item.key_name] = lstError;
                            }
                            else
                            {
                                Err_Logger.err_BodyParameter.missing.Add(item.key_name, lstError);
                            }
                            //strError.Append(string.Format(ERR_KEYNAME_NOT_FOUND, item.key_name));
                            //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            //    {
                            //        MessageDialog message = new MessageDialog(string.Format(ERR_KEYNAME_NOT_FOUND, item.key_name));
                            //        await message.ShowAsync();
                            //    });
                            //break;
                        }
                    }

                    int dataTypeIndex = item.validation_data_type;
                    int stringFormat = item.validations.format_string;
                    object strKeysValue;
                    bodyParams.TryGetValue(item.key_name, out strKeysValue);
                    List<string> lstValidError = new List<string>();
                    List<string> tempList;

                    //Check for valid data type in body
                    string strDataTypeResult = isValidDataType(item.key_name, dataTypeIndex, strKeysValue);
                    if (!string.IsNullOrEmpty(strDataTypeResult))
                    {
                        lstValidError.Add(strDataTypeResult);
                        if (Err_Logger.err_BodyParameter.invalid.TryGetValue(item.key_name, out tempList))
                        {
                            Err_Logger.err_BodyParameter.invalid[item.key_name] = lstValidError;
                        }
                        else
                        {
                            Err_Logger.err_BodyParameter.invalid.Add(item.key_name, lstValidError);
                        }
                        //strError.Append(strDataTypeResult);
                    }

                    //Check for valid length of value
                    string strRangeResult = checkForRange(item.key_name, item.validations.max, item.validations.min, strKeysValue);
                    if (!string.IsNullOrEmpty(strRangeResult))
                    {
                        lstValidError.Add(strRangeResult);
                        if (Err_Logger.err_BodyParameter.invalid.TryGetValue(item.key_name, out tempList))
                        {
                            Err_Logger.err_BodyParameter.invalid[item.key_name] = lstValidError;
                        }
                        else
                        {
                            Err_Logger.err_BodyParameter.invalid.Add(item.key_name, lstValidError);
                        }
                        //strError.Append(strRangeResult);
                    }

                    //Check for valid string format
                    string strValidString = isValidString(item.key_name, stringFormat, strKeysValue);
                    if (!string.IsNullOrEmpty(strValidString))
                    {
                        lstValidError.Add(strValidString);
                        if (Err_Logger.err_BodyParameter.invalid.TryGetValue(item.key_name, out tempList))
                        {
                            Err_Logger.err_BodyParameter.invalid[item.key_name] = lstValidError;
                        }
                        else
                        {
                            Err_Logger.err_BodyParameter.invalid.Add(item.key_name, lstValidError);
                        }
                        //strError.Append(strValidString);
                    }
                }
            }
            return strError;
        }

        /// <summary>
        /// This will check the range of particular string.
        /// </summary>
        /// <param name="strKeyName"></param>
        /// <param name="strMax"></param>
        /// <param name="strMin"></param>
        /// <param name="oKeysValue"></param>
        /// <returns></returns>
        public string checkForRange(string strKeyName, string strMax, string strMin, object oKeysValue)
        {
            //bool isValid = true;
            string strError = string.Empty;
            int keyValueLength = 0;
            try
            {
                if (strMax.Equals("0") && strMin.Equals("0"))
                {
                    return strError;
                }
                if (oKeysValue.GetType() == typeof(string))
                {
                    string strMessage = oKeysValue as string;
                    keyValueLength = strMessage.Length;
                }
                else if (oKeysValue.GetType() == typeof(int?))
                {
                    int? iMessage = oKeysValue as int?;
                    keyValueLength = Convert.ToInt32(iMessage.ToString().Length);
                }
                else if (oKeysValue.GetType() == typeof(float))
                {
                    float? fMessage = oKeysValue as float?;
                    keyValueLength = Convert.ToInt32(fMessage.ToString().Length);
                }
                int maxLength = Convert.ToInt32(strMax);
                int minLength = Convert.ToInt32(strMin);
                if (keyValueLength < minLength || keyValueLength > maxLength)
                {
                    strError = string.Format(ERR_KEYNAME_WRONG_LENGTH, minLength, maxLength);
                    //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    //{
                    //    MessageDialog message = new MessageDialog(string.Format(ERR_KEYNAME_WRONG_LENGTH, strKeyName, minLength, maxLength));
                    //    await message.ShowAsync();
                    //});
                    //isValid = false;
                }
                return strError;
            }
            catch (Exception ex)
            {
                return strError;
            }
        }

        /// <summary>
        /// This will check the data type of given value.
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="dataTypeIndex"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string isValidDataType(string keyName, int dataTypeIndex, Object data)
        {
            //bool isValid = true;
            string strError = string.Empty;
            try
            {
                switch (dataTypeIndex)
                {
                    case 0:
                        if (data.GetType() != typeof(int?))
                        {
                            //isValid = false;
                            strError = string.Format(ERR_KEYNAME_WRONG_DATA_TYPE, keyName, "int");
                            //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            //{
                            //    MessageDialog message = new MessageDialog(string.Format(ERR_KEYNAME_WRONG_DATA_TYPE, keyName, "int"));
                            //    await message.ShowAsync();
                            //});
                        }
                        break;
                    case 1:
                        if (data.GetType() != typeof(float))
                        {
                            //isValid = false;
                            strError = string.Format(ERR_KEYNAME_WRONG_DATA_TYPE, keyName, "float");
                            //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            //{
                            //    MessageDialog message = new MessageDialog(string.Format(ERR_KEYNAME_WRONG_DATA_TYPE, keyName, "float"));
                            //    await message.ShowAsync();
                            //});
                        }
                        break;
                    case 2:
                        if (data.GetType() != typeof(string))
                        {
                            //isValid = false;
                            strError = string.Format(ERR_KEYNAME_WRONG_DATA_TYPE, keyName, "string");
                            //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            //{
                            //    MessageDialog message = new MessageDialog(string.Format(ERR_KEYNAME_WRONG_DATA_TYPE, keyName, "string"));
                            //    await message.ShowAsync();
                            //});
                        }
                        break;
                    case 3:
                        if (data.GetType() != typeof(string))
                        {
                            //isValid = false;
                            strError = string.Format(ERR_KEYNAME_WRONG_DATA_TYPE, keyName, "string");
                            //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            //{
                            //    MessageDialog message = new MessageDialog(string.Format(ERR_KEYNAME_WRONG_DATA_TYPE, keyName, "string"));
                            //    await message.ShowAsync();
                            //});
                        }
                        break;
                    case 4:
                        if (data.GetType() != typeof(byte[]))
                        {
                            //isValid = false;
                            strError = string.Format(ERR_KEYNAME_WRONG_DATA_TYPE, keyName, "byte[]");
                            //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            //{
                            //    MessageDialog message = new MessageDialog(string.Format(ERR_KEYNAME_WRONG_DATA_TYPE, keyName, "byte[]"));
                            //    await message.ShowAsync();
                            //});
                        }
                        break;
                    default:
                        //isValid = false;
                        break;
                }
                return strError;
            }
            catch (Exception ex)
            {
                return strError;
            }
        }

        /// <summary>
        /// This will check weather the given value string format is correct or not.
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="bodyParamterFormat"></param>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public string isValidString(string keyName, int bodyParamterFormat, Object objValue)
        {
            bool isValid = true;
            string strError = string.Empty;
            try
            {
                string value = (string)objValue;
                //csEnum.StringFormat strFormat = (csEnum.StringFormat)Enum.Parse(typeof(csEnum.StringFormat), value);
                switch (bodyParamterFormat)
                {
                    case 0: //csEnum.StringFormat.ALPHA:
                        string strAlphaRegex = @"^[a-zA-Z]*$";
                        isValid = Regex.IsMatch(value, strAlphaRegex);
                        if (!isValid)
                        {
                            //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            //{
                            //    MessageDialog message = new MessageDialog(string.Format(ERR_KEYNAME_WRONG_STRING_FORMAT_TYPE, keyName, "Alpha"));
                            //    await message.ShowAsync();
                            //});
                            strError = string.Format(ERR_KEYNAME_WRONG_STRING_FORMAT_TYPE, "Alpha");
                        }
                        break;
                    case 1: //csEnum.StringFormat.NUMERIC:
                        string strNumericRegex = @"^[0-9.]*$";
                        isValid = Regex.IsMatch(value, strNumericRegex);
                        if (!isValid)
                        {
                            //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            //{
                            //    MessageDialog message = new MessageDialog(string.Format(ERR_KEYNAME_WRONG_STRING_FORMAT_TYPE, keyName, "Numeric"));
                            //    await message.ShowAsync();
                            //});
                            strError = string.Format(ERR_KEYNAME_WRONG_STRING_FORMAT_TYPE, "Numeric");
                        }
                        break;
                    case 2: //csEnum.StringFormat.ALPHANUMERIC:
                        string strAlphaNumericRegex = @"^[a-zA-Z0-9_]*$";
                        isValid = Regex.IsMatch(value, strAlphaNumericRegex);
                        if (!isValid)
                        {
                            //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            //{
                            //    MessageDialog message = new MessageDialog(string.Format(ERR_KEYNAME_WRONG_STRING_FORMAT_TYPE, keyName, "AlphaNumeric"));
                            //    await message.ShowAsync();
                            //});
                            strError = string.Format(ERR_KEYNAME_WRONG_STRING_FORMAT_TYPE, "AlphaNumeric");
                        }
                        break;
                    case 3: //csEnum.StringFormat.EMAIL:
                        string strEmailRegex = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                        isValid = Regex.IsMatch(value, strEmailRegex);
                        if (!isValid)
                        {
                            //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                            //{
                            //    MessageDialog message = new MessageDialog(string.Format(ERR_KEYNAME_WRONG_STRING_FORMAT_TYPE, keyName, "Email"));
                            //    await message.ShowAsync();
                            //});
                            strError = string.Format(ERR_KEYNAME_WRONG_STRING_FORMAT_TYPE, "Email");
                        }
                        break;
                    default:
                        break;
                }
                return strError;
            }
            catch (Exception ex)
            {
                return strError;
            }
        }

        /// <summary>
        /// This will check the given header details with saved data from DB.
        /// </summary>
        /// <param name="strActionID"></param>
        /// <param name="headers"></param>
        public async void checkHeaders(string strActionID, Dictionary<string, string> headers)
        {
            try
            {
                Headers oHeaders = new Headers();
                oHeaders.ActionID = strActionID;
                List<Headers> headersResult = await oHeaders.GetHeadersDetail();
                if (headersResult != null && headersResult.Count() > 0)
                {
                    foreach (var item in headersResult)
                    {
                        string strKeyName = item.KeyName;
                        string strValue = item.Value;
                        if (!string.IsNullOrEmpty(strValue))
                        {
                            if (!headers.ContainsKey(strKeyName))
                            {
                                List<string> tempList;
                                List<string> lstValidError = new List<string>();
                                lstValidError.Add(ERR_KEYNAME_NOT_FOUND);
                                if (Err_Logger.err_HeaderParameter.missing.TryGetValue(item.KeyName, out tempList))
                                {
                                    Err_Logger.err_HeaderParameter.missing[item.KeyName] = lstValidError;
                                }
                                else
                                {
                                    Err_Logger.err_HeaderParameter.missing.Add(item.KeyName, lstValidError);
                                }
                                //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                                //{
                                //    MessageDialog message = new MessageDialog(string.Format(ERR_KEYNAME_NOT_FOUND, strKeyName));
                                //    await message.ShowAsync();
                                //});
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
