// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using ServiceClient.Database;
using ServiceClient.RequestManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient.Classes
{
    public class DummyAPIStructureDetails
    {
        /// <summary>
        /// This method will read the project json and save all the data in DB.
        /// </summary>
        public async void ReadJsonFile()
        {
            try
            {
                string strResult = await HelperMethods.ReadAPIFormatJsonFile("ms-appx:///ContentFile/APIFormat.json");
                if (!string.IsNullOrEmpty(strResult))
                {
                    APIResponse getFileResponse = JsonParseClass.DummyAPIResponseSerialization(strResult);
                    if (getFileResponse != null)
                    {
                        if (!string.IsNullOrEmpty(getFileResponse.project_id))
                        {
                            Project oProject = new Project();
                            oProject.ProjectID = getFileResponse.project_id;
                            oProject.LastModifiedDate = getFileResponse.updated_at;
                            await oProject.InsertUpdateProjectDetails();
                        }

                        if (getFileResponse.actions != null && getFileResponse.actions.Count() > 0)
                        {
                            foreach (var item in getFileResponse.actions)
                            {
                                API oAPI = new API();
                                oAPI.ActionID = item.action_id;
                                oAPI.ActionName = item.action;
                                oAPI.ActionType = item.request_type;
                                oAPI.BaseURL = item.base_url;
                                oAPI.DevURL = item.dev_url;
                                oAPI.QAURL= item.qa_url;
                                oAPI.StagingURL= item.staging_url;
                                oAPI.ProductionURL = item.production_url;
                                await oAPI.InsertUpdateAPIDetails();

                                if (item.body_parameters != null && item.body_parameters.Count() > 0)
                                {
                                    foreach (var innerItem in item.body_parameters)
                                    {
                                        Key oKey = new Key();
                                        oKey.ActionID = item.action_id;
                                        oKey.DataType = innerItem.validation_data_type;
                                        oKey.keyName = innerItem.key_name;
                                        if (innerItem.validations != null)
                                        {
                                            oKey.MaximumLength = innerItem.validations.max;
                                            oKey.MinimumLength = innerItem.validations.min;
                                            oKey.Format = innerItem.validations.format_string;
                                            oKey.Required = innerItem.validations.require;
                                        }
                                        await oKey.InsertUpdatekeyDetails();
                                    }
                                }

                                if (item.headers != null && item.headers.Count() > 0)
                                {
                                    foreach (var headersItem in item.headers)
                                    {
                                        Headers oHeaders = new Headers();
                                        oHeaders.ActionID = item.action_id;
                                        oHeaders.KeyName = headersItem.key_name;
                                        string strValue = string.Empty;
                                        if (headersItem.key_value != null && headersItem.key_value.Count() > 0)
                                        {
                                            int count = 0;
                                            foreach (var contentItem in headersItem.key_value)
                                            {
                                                if (count == 0)
                                                {
                                                    strValue = contentItem;
                                                }
                                                else
                                                {
                                                    strValue = strValue + "," + contentItem;
                                                }
                                                count++;
                                            }
                                            oHeaders.Value = strValue;
                                        }
                                        await oHeaders.InsertUpdateHeadersDetails();
                                    }
                                }
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
