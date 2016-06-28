// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using ServiceClient.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient.Database
{
    public class API
    {
        [PrimaryKey, AutoIncrement]
        public int ID
        {
            get;
            set;
        }

        public string ActionID
        {
            get;
            set;
        }

        public string ActionName
        {
            get;
            set;
        }

        public string ActionType
        {
            get;
            set;
        }

        public string BaseURL
        {
            get;
            set;
        }

        public string DevURL
        {
            get;
            set;
        }

        public string QAURL
        {
            get;
            set;
        }

        public string StagingURL
        {
            get;
            set;
        }

        public string ProductionURL
        {
            get;
            set;
        }

        /// <summary>
        /// This method will insert and update the details of API.
        /// </summary>
        /// <returns></returns>
        public async Task<int> InsertUpdateAPIDetails()
        {
            int iInsertedUpdated = 0;
            try
            {
                var data = dbConnection.conn.Table<API>().Where(x => x.ActionID == this.ActionID);
                API dataResult = await data.FirstOrDefaultAsync();
                if (dataResult != null)
                {
                    dataResult.ActionName = this.ActionName;
                    dataResult.ActionType = this.ActionType;
                    dataResult.DevURL = this.DevURL;
                    dataResult.BaseURL = this.BaseURL;
                    dataResult.QAURL= this.QAURL;
                    dataResult.StagingURL = this.StagingURL;
                    dataResult.ProductionURL = this.ProductionURL;
                    iInsertedUpdated = await dbConnection.conn.UpdateAsync(dataResult);
                }
                else
                {
                    iInsertedUpdated = await dbConnection.conn.InsertAsync(this);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur InsertUpdateAPIDetails -- API: " + ex.ToString());
                return iInsertedUpdated;
            }
            return iInsertedUpdated;
        }

        /// <summary>
        /// This method will return the all details of API.
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceClient.Classes.Action> GetAPIDetail()
        {
            ServiceClient.Classes.Action oAction = new ServiceClient.Classes.Action();
            try
            {
                var data = dbConnection.conn.Table<API>().Where(x => x.ActionID == this.ActionID);
                API dataResult = await data.FirstOrDefaultAsync();
                if (dataResult != null)
                {
                    List<BodyParameter> lstBodyParameter = await getKeyDetails(this.ActionID);
                    oAction.action = this.ActionName;
                    oAction.dev_url = dataResult.DevURL;
                    oAction.qa_url = dataResult.QAURL;
                    oAction.staging_url = dataResult.StagingURL;
                    oAction.production_url = dataResult.ProductionURL;
                    oAction.request_type = dataResult.ActionType;
                    oAction.body_parameters = lstBodyParameter;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur GetAPIDetail -- API: " + ex.ToString());
                return oAction;
            }
            return oAction;
        }

        /// <summary>
        /// This will return details of key.
        /// </summary>
        /// <param name="strActionId"></param>
        /// <returns></returns>
        public async Task<List<BodyParameter>> getKeyDetails(string strActionId)
        {
            List<BodyParameter> lstBodyParameter = new List<BodyParameter>();
            try
            {
                Key oKey = new Key();
                oKey.ActionID = strActionId;
                List<Key> result = await oKey.GetkeyDetail();
                if (result != null && result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        Validations oValidations = new Validations();
                        oValidations.format_file = item.FileFormat;
                        oValidations.format_string = item.Format;
                        oValidations.max = item.MaximumLength;
                        oValidations.min = item.MinimumLength;
                        oValidations.require = item.Required;
                        oValidations.size = item.Size;

                        lstBodyParameter.Add(new BodyParameter
                        {
                            key_name = item.keyName,
                            validation_data_type = item.DataType,
                            validations = oValidations,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return lstBodyParameter;

            }
            return lstBodyParameter;
        }
    }
}
