using Java.Lang;
using System.Collections.Generic;

using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

using MyAttandance.Entities;
using MyAttandance.ViewHolders;

namespace MyAttandance.Adapters
{
    class MonthlyFragmentAdapter : BaseAdapter
    {

        private static string LOG_TAG = typeof(MonthlyFragmentAdapter).Name;
        private List<MonthlyReport> mList;
        private LayoutInflater inflater;

        public MonthlyFragmentAdapter(Context context, List<MonthlyReport> objects)
        {

            string methodName = "MonthlyFragmentAdapter";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));

            this.mList = objects;
            this.inflater = LayoutInflater.From(context);

            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));
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

            MonthlyFragmentHolder viewHolder = new MonthlyFragmentHolder();

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.fragment_monthlyRows, null);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (MonthlyFragmentHolder)convertView.Tag;
            }

            //binding Views to ViewHolder
            viewHolder.setView(convertView);

            //Populating Views with data
            mappingData(viewHolder, mList[position]);

            //binding listeners to view
            viewHolder.setListeners(position);

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));

            return convertView;
        }


        protected void mappingData(MonthlyFragmentHolder viewHolder, MonthlyReport entity)
        {
            string methodName = "mappingData";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));

            viewHolder.setReport_owner(entity.name);
            viewHolder.setCount_AllowedLate(entity.count_AllowedLate);
            viewHolder.setCount_earlyLeave(entity.count_earlyLeave);
            viewHolder.setCount_full(entity.count_full);
            viewHolder.setCount_late(entity.count_late);
            viewHolder.setCount_leave(entity.count_leave);
            viewHolder.setCount_login(entity.count_login);
            viewHolder.setCount_not_full(entity.count_not_full);
            viewHolder.setOvertime_elapse_time(entity.overtime_elapse_time);
            viewHolder.setReport_month(entity.report_month);
            viewHolder.setReport_year(entity.report_year);
            viewHolder.setTotal_attendance(entity.total_attendance);
            viewHolder.setCount_sick(entity.count_sick);
            viewHolder.setCount_not_login(entity.count_not_login);

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }

        public void updateData(List<MonthlyReport> objects)
        {
            mList = objects;
        }
    }

}