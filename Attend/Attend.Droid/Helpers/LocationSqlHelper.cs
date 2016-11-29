using System;
using Android.Content;
using Android.Database.Sqlite;
using Android.Util;

namespace Attend.HelperAndroid
{
    class LocationSqlHelper : SQLiteOpenHelper
    {
        public const string TABLE_LOGS = "table_logs";
        private const int DATABASE_VERSION = 7;

        public const string SEQUENCE_TBL = "sqlite_sequence";
        public const string SEQUENCE_POINTER = "name";

        private const string DATABASE_NAME = "locationlogs.db";

        public const string COLUMN_ID = "_id";
        public const string COLUMN_DTSTR = "dt_str";
        public const string COLUMN_LAT = "lat";
        public const string COLUMN_LON = "lon";
        public const string COLUMN_LOCSTAT = "loc_status"; // effected by Accuracy
      
        private const string DATABASE_CREATE = "create table " + TABLE_LOGS + "("  
                                                                + COLUMN_ID + " integer primary key autoincrement, "
                                                                + COLUMN_DTSTR + " text not null, "
                                                                + COLUMN_LAT + " text not null, "
                                                                + COLUMN_LON + " text not null, "
                                                                + COLUMN_LOCSTAT + " text not null); ";

        private static LocationSqlHelper instance;

        private LocationSqlHelper(Context context) : base(context, DATABASE_NAME, null, DATABASE_VERSION) { }

        public static LocationSqlHelper Instance(Context context)
        {

            if (instance == null)
            {
                instance = new LocationSqlHelper(context);
            }

            return instance;
        }

        public override void OnCreate(SQLiteDatabase database)
        {
            string methodName = "OnCreate";
          
            try
            {
                database.ExecSQL(DATABASE_CREATE);
            }
            catch (Exception ex)
            {
               Log.Debug(methodName, "Error "+ex.Message);
            }
        }

       
        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {

           // Log.w(LOG_TAG, String.format("Upgrading database from version (%d) to (%d), which will destroy all old data", oldVersion, newVersion));
            db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_LOGS);
            OnCreate(db);

        }
    



    }
}