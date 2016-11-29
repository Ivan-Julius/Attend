
using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MyAttandance.Entities;
using MyAttandance.ViewHolders;

namespace MyAttandance.Adapters
{
    class GeneralLogFragmentAdapter : BaseAdapter
    {

        private static string LOG_TAG = typeof(GeneralLogFragmentAdapter).Name;
        private List<GeneralLog> mList;
        private LayoutInflater inflater;
     
        public GeneralLogFragmentAdapter(Context context, List<GeneralLog> objects)
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
            Log.Debug(LOG_TAG, string.Format("{0}: --- data is null: {1} ---", methodName, (mList[position] == null)));

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

            GeneralLogsViewHolder viewHolder = new GeneralLogsViewHolder();

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.fragment_logRows, null);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (GeneralLogsViewHolder)convertView.Tag;
            }

            //binding Views to ViewHolder
            viewHolder.setView(convertView);

            mappingData(viewHolder, mList[position]);

            //binding listeners to view
            //viewHolder.setListeners(position, callee);

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
            return convertView;
        }

        public void replaceData(List<GeneralLog> data)
        {
            this.mList = data;
        }

        protected void mappingData(GeneralLogsViewHolder viewHolder, GeneralLog entity)
        {
            string methodName = "mappingData";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));
            if (entity != null)
            {
                viewHolder.setDtStr(entity.dt_str);
                viewHolder.setIdStr(entity.id);
                viewHolder.setLevel(entity.level);
                viewHolder.setMessage(entity.message);
                viewHolder.setMethods(entity.methods);
                viewHolder.setClass(entity._class);
            }

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }

    }
}