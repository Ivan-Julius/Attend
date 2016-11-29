using System;
using Android.Content;
using Android.Database.Sqlite;
using Android.Util;

namespace Attend.HelperAndroid
{
    class WifiSqlHelper : SQLiteOpenHelper
    {
        private const int DATABASE_VERSION = 7;
        public const string TABLE_LOGS = "table_logs";

        private const string SEQUENCE_TBL = "sqlite_sequence";
        private const string SEQUENCE_POINTER = "name";

        private const string DATABASE_NAME = "wifilogs.db";

        public const string COLUMN_ID = "_id";
        public const string COLUMN_DTSTR = "dt_str";
        public const string COLUMN_SSID = "ssid";
        public const string COLUMN_BSSID = "bssid";
        public const string COLUMN_STATUS = "wifi_status";

        private const string DATABASE_CREATE = "create table " + TABLE_LOGS + "("
                                                                + COLUMN_ID + " integer primary key autoincrement, "
                                                                + COLUMN_DTSTR + " text not null, "
                                                                + COLUMN_SSID + " text not null, "
                                                                + COLUMN_BSSID + " text not null, "
                                                                + COLUMN_STATUS + " text not null); ";

        private static WifiSqlHelper instance;

        private WifiSqlHelper(Context context) : base(context, DATABASE_NAME, null, DATABASE_VERSION) { }

        public static WifiSqlHelper Instance(Context context)
        {

            if (instance == null)
            {
                instance = new WifiSqlHelper(context);
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
                Log.Debug(methodName, "Error " + ex.Message);
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