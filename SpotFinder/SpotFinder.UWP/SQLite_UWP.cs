﻿using System.IO;
using Windows.Storage;
using SQLite;
using Xamarin.Forms;
using XamarinForms.SQLite;
using XamarinForms.SQLite.UWP;
using XamarinForms.SQLite.SQLite;

[assembly: Dependency(typeof(SQLite_Uwp))]

namespace XamarinForms.SQLite.UWP
{
    public class SQLite_Uwp : ISQLite
    {

        public SQLite_Uwp()
        {
        }

        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "MySQLiteDB.db3";
            string documentsPath = ApplicationData.Current.LocalFolder.Path;
            var path = Path.Combine(documentsPath, sqliteFilename);
            // Create the connection
            var conn = new SQLiteConnection(path);
            // Return the database connection
            return conn;
        }
    }
}