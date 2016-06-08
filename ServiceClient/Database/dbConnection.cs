// **************************************************************
// *
// * Written By: Nishant Sukhwal
// * Copyright © 2016 kiwitech. All rights reserved.
// **************************************************************

using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ServiceClient.Database
{
    public class dbConnection
    {
        public static SQLiteAsyncConnection conn;

        /// <summary>
        /// Create database tables at the time of App launch.
        /// </summary>
        /// <param name="sDBName"></param>
        public async static Task CheckDatabaseExist(string sDBName)
        {
            try
            {
                string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, sDBName);
                if (!string.IsNullOrEmpty(dbPath))
                {
                    conn = new SQLiteAsyncConnection(dbPath);
                    await conn.CreateTableAsync<API>();
                    await conn.CreateTableAsync<Key>();
                    await conn.CreateTableAsync<Project>();
                    await conn.CreateTableAsync<Headers>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CheckDatabaseExist Exception Occur : " + ex.ToString());
            }
        }

        /// <summary>
        /// This will drop all tables and then recreate in case if prject is changed.
        /// </summary>
        /// <returns></returns>
        public static async Task DropTable()
        {
            try
            {
                await conn.DropTableAsync<API>();
                await conn.DropTableAsync<Key>();
                await conn.DropTableAsync<Project>();
                await conn.DropTableAsync<Headers>();

                await conn.CreateTableAsync<API>();
                await conn.CreateTableAsync<Key>();
                await conn.CreateTableAsync<Project>();
                await conn.CreateTableAsync<Headers>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Occur DropTable : " + ex.ToString());
            }
        }

    }
}
