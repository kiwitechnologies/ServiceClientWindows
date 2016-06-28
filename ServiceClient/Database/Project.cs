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
    public class Project
    {
        [PrimaryKey, AutoIncrement]
        public int ID
        {
            get;
            set;
        }

        public string ProjectID
        {
            get;
            set;
        }

        public string ProjectName
        {
            get;
            set;
        }

        public string VersionNumber
        {
            get;
            set;
        }

        public long LastModifiedDate
        {
            get;
            set;
        }

        /// <summary>
        /// This method will insert and update all details of Project.
        /// </summary>
        /// <returns></returns>
        public async Task<int> InsertUpdateProjectDetails()
        {
            int iInsertedUpdated = 0;
            try
            {
                var data = dbConnection.conn.Table<Project>().Where(x => x.ProjectID == this.ProjectID);
                Project dataResult = await data.FirstOrDefaultAsync();
                if (dataResult != null)
                {
                    dataResult.ProjectName = this.ProjectName;
                    dataResult.VersionNumber = this.VersionNumber;
                    dataResult.LastModifiedDate = this.LastModifiedDate;
                    iInsertedUpdated = await dbConnection.conn.UpdateAsync(dataResult);
                }
                else
                {
                    iInsertedUpdated = await dbConnection.conn.InsertAsync(this);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur InsertUpdateProjectDetails -- Project: " + ex.ToString());
                return iInsertedUpdated;
            }
            return iInsertedUpdated;
        }

        /// <summary>
        /// This method will return the all details of Project.
        /// </summary>
        /// <returns></returns>
        public async Task<Project> GetprojectDetail()
        {
            Project oProject = null;
            try
            {
                var data = dbConnection.conn.Table<Project>().Where(x => x.ProjectID == this.ProjectID);
                Project dataResult = await data.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur GetprojectDetail -- Project: " + ex.ToString());
                return oProject;
            }
            return oProject;
        }
    }
}
