
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using MyAttandance.Adapters;
using MyAttandance.Entities;
using MyAttandance.SqlSource;

namespace MyAttandance
{
    public class GeneralLogViewFragment : Fragment
    {
        private static string LOG_TAG = typeof(GeneralLogViewFragment).Name;
        private GeneralLogFragmentAdapter mAdapter;
        private LogDataSource<GeneralLog> lds;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            lds = new LogDataSource<GeneralLog>();
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return inflater.Inflate(Resource.Layout.fragment_log, container, false);
        }


        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            string methodName = "OnActivityCreated";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));

            Context mContext = Application.Context;

            mAdapter = new GeneralLogFragmentAdapter(mContext, new List<GeneralLog>());
            ListView mTableView = Activity.FindViewById<ListView>(Resource.Id.logtables);
            mTableView.Adapter = mAdapter;
            retrieveData(null, null);

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }


        private void retrieveData(string from, string to)
        {
            List<GeneralLog> gls = lds.GetAllLogItems(Application.Context);

            mAdapter.replaceData(gls);
            mAdapter.NotifyDataSetChanged();
        }

   

    }
}