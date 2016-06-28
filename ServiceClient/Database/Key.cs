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
    public class Key
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

        public int DataType
        {
            get;
            set;
        }

        public int Format
        {
            get;
            set;
        }

        public string FileFormat
        {
            get;
            set;
        }

        public string keyName
        {
            get;
            set;
        }

        public string Size
        {
            get;
            set;
        }

        public string MaximumLength
        {
            get;
            set;
        }

        public string MinimumLength
        {
            get;
            set;
        }

        public int Required
        {
            get;
            set;
        }

        /// <summary>
        /// This method will insert and update all details of Key.
        /// </summary>
        /// <returns></returns>
        public async Task<int> InsertUpdatekeyDetails()
        {
            int iInsertedUpdated = 0;
            try
            {
                var data = dbConnection.conn.Table<Key>().Where(x => x.ActionID == this.ActionID && x.keyName == this.keyName);
                Key dataResult = await data.FirstOrDefaultAsync();
                if (dataResult != null)
                {
                    dataResult.DataType = this.DataType;
                    dataResult.Format = this.Format;
                    dataResult.keyName = this.keyName;
                    dataResult.MaximumLength = this.MaximumLength;
                    dataResult.MinimumLength = this.MinimumLength;
                    dataResult.Required = this.Required;
                    iInsertedUpdated = await dbConnection.conn.UpdateAsync(dataResult);
                }
                else
                {
                    iInsertedUpdated = await dbConnection.conn.InsertAsync(this);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur InsertUpdatekeyDetails -- Key: " + ex.ToString());
                return iInsertedUpdated;
            }
            return iInsertedUpdated;
        }

        /// <summary>
        /// This method will return the all details of Key.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Key>> GetkeyDetail()
        {
            List<Key> lstKey = null;
            try
            {
                var data = dbConnection.conn.Table<Key>().Where(x => x.ActionID == this.ActionID);
                lstKey = await data.ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur GetkeyDetail -- Key: " + ex.ToString());
                return lstKey;
            }
            return lstKey;
        }
    }
}
