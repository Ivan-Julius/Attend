using System;
using Android.Content;
using Android.Database.Sqlite;
using Android.Util;

namespace MyAttandance.Helper
{
    class GeneralLogSqlHelper : SQLiteOpenHelper
    {
        public const string TABLE_LOGS = "table_logs";
        private const int DATABASE_VERSION = 7;

        public const string SEQUENCE_TBL = "sqlite_sequence";
        public const string SEQUENCE_POINTER = "name";

        private const string DATABASE_NAME = "generalLogs.db";

        public const string COLUMN_ID = "_id";
        public const string COLUMN_DT = "dt";
        public const string COLUMN_DTSTR = "dt_str";
        public const string COLUMN_LEVEL = "level";
        public const string COLUMN_CLASS = "class";
        public const string COLUMN_METHOD = "method";
        public const string COLUMN_MESSAGE = "message"; // effected by Accuracy

        private const string DATABASE_CREATE = "create table " + TABLE_LOGS + "("
                                                                + COLUMN_ID + " integer primary key autoincrement, "
                                                                + COLUMN_DT + " long not null, "
                                                                + COLUMN_DTSTR + " text not null, "
                                                                + COLUMN_LEVEL + " text not null, "
                                                                + COLUMN_CLASS + " text not null, "
                                                                + COLUMN_METHOD + " text not null, "
                                                                + COLUMN_MESSAGE + " text not null); ";

        private static GeneralLogSqlHelper instance;

        private GeneralLogSqlHelper(Context context) : base(context, DATABASE_NAME, null, DATABASE_VERSION) { }

        public static GeneralLogSqlHelper Instance(Context context)
        {

            if (instance == null)
            {
                instance = new GeneralLogSqlHelper(context);
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