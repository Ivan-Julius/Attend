
using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MyAttandance.Entities;
using MyAttandance.ViewHolders;
using MyAttandance;

namespace MyAttandance.Adapters
{
    class LocationLogFragmentAdapter : BaseAdapter
    {
        private static string LOG_TAG = typeof(LocationLogFragmentAdapter).Name;
        private List<LocationLog> mList;
        private LayoutInflater inflater;
       // private bool isAll = false;

        public LocationLogFragmentAdapter(Context context, List<LocationLog> objects)
        {
            string methodName = "Constructor";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));

            this.mList = objects;
            this.inflater = LayoutInflater.From(context);

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }

        public override int Count
        {
            get
            {
                string methodName = "Count.get";
                Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));

                Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
                return mList.Count;
            }
        }

        public override Object GetItem(int position)
        {
            string methodName = "GetItem";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));

            Log.Debug(LOG_TAG, string.Format("{0}: --- position : {1} ---", methodName, position.ToString()));

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));

            return mList[position];
        }

        public override long GetItemId(int position)
        {
            string methodName = "GetItemId";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            string methodName = "GetView";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));

            LocationLogViewHolder viewHolder = new LocationLogViewHolder();

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.fragment_logRows, null);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (LocationLogViewHolder)convertView.Tag;
            }

            //binding Views to ViewHolder
            viewHolder.setView(convertView);

            LocationLog lLog = mList[position];

            viewHolder.setDtStr(lLog.dtStr);
            viewHolder.setIdStr(lLog.Id);
            viewHolder.setLat(lLog.lat);
            viewHolder.setLon(lLog.lon);
            viewHolder.setLocstat(lLog.locStat);

            //binding listeners to view
            //viewHolder.setListeners(position, callee);

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
            return convertView;
        }

        public void replaceData(List<LocationLog> data)
        {
            this.mList = data;
        }

    }
}