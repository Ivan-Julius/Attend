
using System.Collections.Generic;

using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

using MyAttandance.Entities;

using Java.Lang;
using MyAttandance.ViewHolders;

namespace MyAttandance.Adapters
{
    class DailyFragmentAdapter : BaseAdapter
    {
        private static string LOG_TAG = typeof(DailyFragmentAdapter).Name;
        private List<DailyReport> mList;
        private LayoutInflater inflater;

        public DailyFragmentAdapter(Context context, List<DailyReport> sList)
        {

            string methodName = "DailyFragmentAdapter";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));

            this.mList = sList;
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
            return position;
        }

        
        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            string methodName = "GetView";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));
           
            DailyFragmentHolder viewHolder = new DailyFragmentHolder();

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.fragment_dailyRows, null);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (DailyFragmentHolder)convertView.Tag;
            }

            //binding Views to ViewHolder
            viewHolder.SetView(convertView);

            //Populating Views with data
            mappingData(viewHolder, mList[position]);

            //binding listeners to view
            viewHolder.SetListeners(position);

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));

            return convertView;
        }


        protected void mappingData(DailyFragmentHolder viewHolder, DailyReport entity)
        {

            string methodName = "mappingData";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));

            //Content Base on Entity and viewHolder
            viewHolder.setFirst_login(entity.first_login);
            viewHolder.setDate(entity.first_login);
            viewHolder.setLast_login(entity.last_login);
            viewHolder.setStart_overtime(entity.start_overtime);
            viewHolder.setLogin_type(entity.login_type);
            viewHolder.setLogout_time(entity.logout_time);
            viewHolder.setStrId(entity.id);
            viewHolder.setStrOvertime(entity.start_overtime, entity.last_login);
            viewHolder.setStrIsFull(entity.is_full.ToString());
            viewHolder.setStrIsLate(entity.is_late.ToString());
            viewHolder.setFirstLoginBG(entity.is_late.ToString());
            viewHolder.setLastLoginBG(entity.is_full.ToString());
            viewHolder.setUser_name(entity.user_name);
            viewHolder.setStrLoginElapse(entity.first_login, entity.last_login);

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }

        public void updateData(List<DailyReport> objects)
        {
            mList = objects;
        }

    }
}