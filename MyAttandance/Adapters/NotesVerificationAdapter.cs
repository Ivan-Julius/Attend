
using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MyAttandance.Entities;
using MyAttandance.Helper;
using MyAttandance.ViewHolders;

namespace MyAttandance.Adapters
{
    class NotesVerificationAdapter : BaseAdapter
    {
        private static string LOG_TAG = typeof(NotesVerificationAdapter).Name;
        private List<Notes> mList;
        private LayoutInflater inflater;
        private bool isAll = false;
        private NotesVerificationHolder.ifBoxIsChecked callee;
       
        private List<int> checkedBox = new List<int>();

        public NotesVerificationAdapter(Context context, List<Notes> objects, NotesVerificationHolder.ifBoxIsChecked callees)
        {
            string methodName = "Constructor";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start ---", methodName));

            this.mList = objects;
            this.inflater = LayoutInflater.From(context);
            this.callee = callees;

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

            NotesVerificationHolder viewHolder = new NotesVerificationHolder();

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.fragment_notesVerificationRows, null);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (NotesVerificationHolder)convertView.Tag;
            }

            //binding Views to ViewHolder
            viewHolder.setView(convertView, true);

            Notes note = mList[position];

            //Populating Views with data
            //viewHolder.setcDate(DateFormatingHelper.StringToStringWithFormat(note.createDate, DateFormatingHelper.TwelveHoursFormat, DateFormatingHelper.TwentyFourHoursFormat));
            //viewHolder.setRequestDateFrom(DateFormatingHelper.StringToStringWithFormat(note.fromDate, DateFormatingHelper.TwelveHoursFormat, DateFormatingHelper.TwentyFourHoursFormat));
            //viewHolder.setRequestDateTo(DateFormatingHelper.StringToStringWithFormat(note.toDate, DateFormatingHelper.TwentyFourHoursFormat, DateFormatingHelper.TwentyFourHoursFormat));
            viewHolder.setcDate(note.createDate);
            viewHolder.setRequestDateFrom(note.fromDate);
            viewHolder.setRequestDateTo(note.toDate);
            viewHolder.setReason(note.reason);
            viewHolder.setType(note.noteType);
            viewHolder.setName(note.owner);
            viewHolder.setStatus(note.status);
            viewHolder.setNo((position + 1).ToString());

            //binding listeners to view
            viewHolder.setListeners(position, callee);

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
            return convertView;
        }

        public void replaceData(List<Notes> data, bool isAlls)
        {
            this.mList = data;
            this.isAll = isAlls;
        }

        public void updateCheckedBox(List<int> checkedBox)
        {
            this.checkedBox = checkedBox;
        }
    }
}