
using System.Collections.Generic;
using Java.Lang;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using MyAttandance.ViewHolders;
using static MyAttandance.ViewHolders.MenuViewHodler;

namespace MyAttandance.Adapters
{
    class MenuAdapter : BaseAdapter
    {
        private static string LOG_TAG = typeof(GeneralLogFragmentAdapter).Name;
        private List<int> mList;
        private LayoutInflater inflater;
        private IMenuClicked delegator;
        private string[] fragments;

        public MenuAdapter(Context context, List<int> objects, string[] fragments, IMenuClicked delegator)
        {
            string methodName = "Constructor";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));

            this.mList = objects;
            this.inflater = LayoutInflater.From(context);
            this.delegator = delegator;
            this.fragments = fragments;

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }

        public override int Count
        {
            get
            {
                return mList.Count;
            }
        }

        public override Object GetItem(int position)
        {
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

            MenuViewHodler viewHolder = new MenuViewHodler(delegator);

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.fragment_menu, null);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (MenuViewHodler)convertView.Tag;
            }

            viewHolder.setView(convertView, mList[position], fragments[position]);
            viewHolder.setListeners(position);

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
            return convertView;
        }
    }
}