
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using MyAttandance.ViewHolders;

namespace MyAttandance.Adapters
{
    public class CheckboxDialogAdapter : BaseAdapter
    {
        private LayoutInflater inflater;
        private List<string> mList;
        private checkboxSelected sendBack;
        private List<string> selected;
        private string type = "";

        public override int Count
        {
            get
            {
                return mList.Count;
            }
        }

        public interface checkboxSelected
        { 
            void onCheckBoxSelected(string content, bool check, string type);
        }

        public CheckboxDialogAdapter(Context context, string type, List<string> objects, checkboxSelected sendBack)
        {
            mList = objects;
            this.inflater = LayoutInflater.From(context);
            this.sendBack = sendBack;
            this.type = type;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return mList[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        { 
            CheckBoxDialogViewHolder viewHolder = new CheckBoxDialogViewHolder();

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.dialog_option_list, null);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (CheckBoxDialogViewHolder)convertView.Tag;
            }

            viewHolder.SetView(convertView);

            string value = mList[position];

            System.Console.WriteLine("value"+value);

            bool check = (selected != null && selected.Contains(value));
            viewHolder.setChecker(value, check, type);
            viewHolder.SetListeners(sendBack);

            return convertView;
        }

        public override void NotifyDataSetChanged()
        {       
            base.NotifyDataSetChanged();      
        }

        public void updateSelected(List<string> selected)
        {
            this.selected = selected;

        }


    }
}