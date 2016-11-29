using System;
using System.Collections.Generic;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.Preferences;
using Android.Util;
using Java.Text;
using MyAttandance.Entities;
using MyAttandance.Helper;

namespace MyAttandance.SqlSource
{
    class LogDataSource<T>
    {
        private const string LOG_LEVEL_DEBUG = "DEBUG";
        private const string LOG_LEVEL_ERROR = "ERROR";
        private const string LOG_LEVEL_INFO = "INFO";

        private static string[] locationColumns = {

            LocationSqlHelper.COLUMN_ID
            , LocationSqlHelper.COLUMN_DTSTR
            , LocationSqlHelper.COLUMN_LAT
            , LocationSqlHelper.COLUMN_LON
            , LocationSqlHelper.COLUMN_LOCSTAT };

        private static string[] wifiColumns = {

            WifiSqlHelper.COLUMN_ID
            , WifiSqlHelper.COLUMN_DTSTR
            , WifiSqlHelper.COLUMN_SSID
            , WifiSqlHelper.COLUMN_BSSID
            , WifiSqlHelper.COLUMN_STATUS   };

        private static string[] generalColumns =
        {
            GeneralLogSqlHelper.COLUMN_ID,
            GeneralLogSqlHelper.COLUMN_DTSTR,
            GeneralLogSqlHelper.COLUMN_DT,
            GeneralLogSqlHelper.COLUMN_LEVEL,
            GeneralLogSqlHelper.COLUMN_CLASS,
            GeneralLogSqlHelper.COLUMN_METHOD,
            GeneralLogSqlHelper.COLUMN_MESSAGE
        };



        public static void Open() 
        {
            //string methodName = "open()";
         
            //database = dbHelper.getWritableDatabase();
        }

        public void Close(Context mContext)
        {
       
            LocationSqlHelper.Instance(mContext).Close();
        }

        public void Debug(Context mContext, List<string> data)
        {

            Log.Debug("Debug", "Info : T(" + typeof(T).Name + ")");

            createLogItem(mContext, LOG_LEVEL_DEBUG, data);
        }


        public void Info(Context mContext, List<string> data)
        {
            Log.Debug("Info", "Info : T(" + typeof(T).Name + ")");

            createLogItem(mContext, LOG_LEVEL_INFO, data);
        }


        public void error(Context mContext, List<string> data)
        {
            Log.Debug("Error", "Info : T(" + typeof(T).Name + ")");

            createLogItem(mContext, LOG_LEVEL_ERROR, data);
        }

        private void createLogItem(Context mContext, string logLevel, List<string> data)
        {
            SimpleDateFormat simpleFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
            data.Add((DateTime.Now).ToString(DateFormatingHelper.YearInFrontFormat));

            string tableLogs = "";

            ContentValues values = contentSorting(mContext, new ContentValues(), data, logLevel);
            SQLiteDatabase database = getTypeDB(mContext, out tableLogs);

            if (database != null)
            {
                Log.Debug("createLogItem", "Info : DB Not null");

                if (database.IsOpen)
                {
                    Log.Debug("createLogItem", "Info : DB is open");

                    database.Insert(tableLogs, null, values);
                }
            }
        }


        private SQLiteDatabase getTypeDB(Context mContext, out string tableLogs)
        {

            SQLiteDatabase database = null;
            tableLogs = "";

            Log.Debug("getTypeDB", "Info : T(" + typeof(T).Name + ")");

            switch (typeof(T).Name)
            {

                case "LocationLog":

                    database = LocationSqlHelper.Instance(mContext).WritableDatabase;
                    tableLogs = LocationSqlHelper.TABLE_LOGS;

                    break;

                case "WifiLog":

                    database = WifiSqlHelper.Instance(mContext).WritableDatabase;
                    tableLogs = WifiSqlHelper.TABLE_LOGS;

                    break;

                case "GeneralLog":

                    database = GeneralLogSqlHelper.Instance(mContext).WritableDatabase;
                    tableLogs = GeneralLogSqlHelper.TABLE_LOGS;

                    break;
            }

            return database;
        }

        private ContentValues contentSorting(Context mContext, ContentValues v, List<string> data, string loglevel)
        {
            Log.Debug("contentSorting", "Info : T(" + typeof(T).Name + ")");

            switch (typeof(T).Name)
            {
                case "LocationLog":

                    v.Put(LocationSqlHelper.COLUMN_DTSTR, data[3]);
                    v.Put(LocationSqlHelper.COLUMN_LAT, data[0]);
                    v.Put(LocationSqlHelper.COLUMN_LON, data[1]);
                    v.Put(LocationSqlHelper.COLUMN_LOCSTAT, data[2]);

                break;

                case "WifiLog":

                    v.Put(WifiSqlHelper.COLUMN_DTSTR, data[3]);
                    v.Put(WifiSqlHelper.COLUMN_SSID, data[0]);
                    v.Put(WifiSqlHelper.COLUMN_BSSID, data[1]);
                    v.Put(WifiSqlHelper.COLUMN_STATUS, data[2]);

                    break;

                case "GeneralLog":

                    long dt_inMs = (DateFormatingHelper.stringToDateWithFormat(data[3], DateFormatingHelper.YearInFrontFormat)).Millisecond;


                    Log.Debug("contentSorting", string.Format("-- {0} - {1} - {2} - {3} - {4} - {5} --", dt_inMs.ToString(), data[3], data[0], data[1], data[2], loglevel));

                    v.Put(GeneralLogSqlHelper.COLUMN_DT, dt_inMs);
                    v.Put(GeneralLogSqlHelper.COLUMN_DTSTR, data[3]);
                    v.Put(GeneralLogSqlHelper.COLUMN_LEVEL, loglevel);
                    v.Put(GeneralLogSqlHelper.COLUMN_CLASS, data[0]);
                    v.Put(GeneralLogSqlHelper.COLUMN_METHOD, data[1]);
                    v.Put(GeneralLogSqlHelper.COLUMN_MESSAGE, data[2]);

                    break;
            }

            return v;
        }


        public void deleteAllItem(Context mContext)
        {
            
            string tableLogs = "";
            SQLiteDatabase database = getTypeDB(mContext, out tableLogs);

            if (database != null) {

                database.ExecSQL("delete from " + LocationSqlHelper.TABLE_LOGS);
            }
        }


        public List<T> GetAllLogItems(Context mContext)
        {

            List<T> logEntities = new List<T>();
            SQLiteDatabase database = null;
            string tableLogs = "";
            string columnID = "";
            string[] columns = null;

            switch (typeof(T).Name)
            {

                case "LocationLog":

                    database = LocationSqlHelper.Instance(mContext).WritableDatabase;
                    tableLogs = LocationSqlHelper.TABLE_LOGS;
                    columnID = LocationSqlHelper.COLUMN_ID;
                    columns = locationColumns;

                    break;

                case "WifiLog":

                    database = WifiSqlHelper.Instance(mContext).WritableDatabase;
                    tableLogs = WifiSqlHelper.TABLE_LOGS;
                    columnID = WifiSqlHelper.COLUMN_ID;
                    columns = wifiColumns;

                    break;

                case "GeneralLog":

                    database = GeneralLogSqlHelper.Instance(mContext).WritableDatabase;
                    tableLogs = GeneralLogSqlHelper.TABLE_LOGS;
                    columnID = GeneralLogSqlHelper.COLUMN_ID;
                    columns = generalColumns;

                    break;
            }

            if (columns != null)
            {
                ICursor cursor = database.Query(tableLogs, columns, null, null, null, null, columnID + " DESC");

                cursor.MoveToFirst();
                while (!cursor.IsAfterLast)
                {
                    logEntities.Add(cursorToLogItem(cursor));
                    cursor.MoveToNext();
                }

                // make sure to close the cursor
                cursor.Close();
                database.Close();
            }

            return logEntities;
        }

        private T cursorToLogItem(ICursor cursor)
        {
            T logItem = default(T);

            switch (typeof(T).Name)
            {
                case "LocationLog":

                    LocationLog locLog = new LocationLog();
                    locLog.Id = cursor.GetLong(0).ToString();
                    locLog.dtStr = cursor.GetString(1);
                    locLog.lat = cursor.GetString(2);
                    locLog.lon = cursor.GetString(3);
                    locLog.locStat = cursor.GetString(4);

                    logItem = (T)Convert.ChangeType(locLog, typeof(T));

                break;

                case "WifiLog":

                    WifiLog wifiLog = new WifiLog();
                    wifiLog.Id = cursor.GetLong(0).ToString();
                    wifiLog.dtStr = cursor.GetString(1);
                    wifiLog.ssid = cursor.GetString(2);
                    wifiLog.bssid = cursor.GetString(3);
                    wifiLog.status = cursor.GetString(4);

                    logItem = (T)Convert.ChangeType(wifiLog, typeof(T));

                    break;

                case "GeneralLog":

                    GeneralLog generalLog = new GeneralLog();
                    generalLog.id = cursor.GetLong(0).ToString();
                    generalLog.dt_str = cursor.GetString(1);
                    generalLog.dt = cursor.GetLong(2);
                    generalLog.level = cursor.GetString(3);
                    generalLog._class = cursor.GetString(4);
                    generalLog.methods = cursor.GetString(5);
                    generalLog.message = cursor.GetString(6);

                    logItem = (T)Convert.ChangeType(generalLog, typeof(T));

                    break;
            }

            return logItem;
        }


       

        /*public void deleteLogItem(LogEntity logEntity) {

          String methodName = String.format("deleteLogItem(%s)", logEntity.toString());
          Log.d(LOG_TAG, String.format("%s: --- start ---", methodName));

          long id = logEntity.getId();
          database.delete(LogSQLHelper.TABLE_LOGS, LogSQLHelper.COLUMN_ID + " = " + id, null);

          Log.d(LOG_TAG, String.format("%s: --- end ---", methodName));
      }*/

        /*
       public void getSequence(){ // for checking table sequence

           String methodName = "getSequence()";
           Log.d(LOG_TAG, String.format("%s: --- start ---", methodName));
           String[] seq_cols = { "seq", "name" };
           Cursor cursor = database.query(LogSQLHelper.SEQUENCE_TBL, seq_cols, null, null, null, null, null);
           cursor.moveToFirst();
           Log.d(LOG_TAG, String.format("%s: logItem(%s)", methodName, cursor.getColumnCount()));
           Log.d(LOG_TAG, String.format("%s: logItem(%s)", methodName, cursor.getColumnName(0) + ", " + cursor.getColumnName(1)));
           while (!cursor.isAfterLast()) {
               Log.d(LOG_TAG, String.format("%s: logItem(%s)", methodName, cursor.getLong(0) + ", " + cursor.getString(1)));
               cursor.moveToNext();
           }

           cursor.close();
           Log.d(LOG_TAG, String.format("%s: --- end ---", methodName));

       }*/

        //private String logLevel = "";

        /* public LogDataSource(Context context) {
             String methodName = String.format("LogDataSource(%s)", context.toString());
             Log.d(LOG_TAG, String.format("%s: --- start ---", methodName));

             mContext = context;

             if(dbHelper == null) {
                 dbHelper = LogSQLHelper.getInstance(context);
             }

             if(database == null) {
                 SQLiteDatabase database = dbHelper.getWritableDatabase();
             }

             SharedPreferences app_preferences = PreferenceManager.getDefaultSharedPreferences(context);
             logLevel = app_preferences.getString(mContext.getResources().getString(R.string.LogLevels), mContext.getResources().getString(R.string.A));

             Log.d(LOG_TAG, String.format("%s: --- (%s) ---", methodName, logLevel));
             Log.d(LOG_TAG, String.format("%s: --- end ---", methodName));
         }*/
    }
}