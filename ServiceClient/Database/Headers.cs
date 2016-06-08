// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient.Database
{
    public class Headers
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

        public string KeyName
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// This method will insert and update the details of Headers.
        /// </summary>
        /// <returns></returns>
        public async Task<int> InsertUpdateHeadersDetails()
        {
            int iInsertedUpdated = 0;
            try
            {
                var data = dbConnection.conn.Table<Headers>().Where(x => x.ActionID == this.ActionID);
                Headers dataResult = await data.FirstOrDefaultAsync();
                if (dataResult != null)
                {
                    dataResult.KeyName = this.KeyName;
                    dataResult.Value = this.Value;
                    iInsertedUpdated = await dbConnection.conn.UpdateAsync(dataResult);
                }
                else
                {
                    iInsertedUpdated = await dbConnection.conn.InsertAsync(this);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur InsertUpdateHeadersDetails -- Headers: " + ex.ToString());
                return iInsertedUpdated;
            }
            return iInsertedUpdated;
        }

        /// <summary>
        /// This method will return the all details of Headers.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Headers>> GetHeadersDetail()
        {
            List<Headers> lstHeaders = null;
            try
            {
                var data = dbConnection.conn.Table<Headers>().Where(x => x.ActionID == this.ActionID);
                lstHeaders = await data.ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur GetHeadersDetail -- Headers: " + ex.ToString());
                return lstHeaders;
            }
            return lstHeaders;
        }
    }
}
