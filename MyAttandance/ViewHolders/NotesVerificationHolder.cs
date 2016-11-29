
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MyAttandance.ViewHolders
{
    class NotesVerificationHolder : Object
    {
        public interface ifBoxIsChecked
        {
            void ifChecked(int position, bool isChecked);
            void ifConfirm(int position, string from);
        }

        public TextView no;
        public TextView cDate;
        public TextView type;
        public TextView name;
        public TextView requestDateFrom;
        public TextView requestDateTo;
        public TextView reason;
        public CheckBox select;
        public TextView status;
        public ImageButton approve;
        public ImageButton reject;

        public void setView(View v, bool isAll)
        {
            no = (TextView)v.FindViewById(Resource.Id.notes_no);
            name = (TextView)v.FindViewById(Resource.Id.notes_name);
            cDate = (TextView)v.FindViewById(Resource.Id.notes_create_date);
            type = (TextView)v.FindViewById(Resource.Id.notes_type);
            requestDateFrom = (TextView)v.FindViewById(Resource.Id.notes_from_date);
            requestDateTo = (TextView)v.FindViewById(Resource.Id.notes_to_date);
            reason = (TextView)v.FindViewById(Resource.Id.notes_reason);
            select = (CheckBox)v.FindViewById(Resource.Id.notes_select);
            approve = (ImageButton)v.FindViewById(Resource.Id.approve_btn);
            reject = (ImageButton)v.FindViewById(Resource.Id.reject_btn);
            status = (TextView)v.FindViewById(Resource.Id.notes_status);

            select.Visibility = (isAll) ? ViewStates.Visible : ViewStates.Gone;
        }

        public void setNo(string nos)
        {
            no.Text = nos;
        }

        public void setName(string names)
        {
            name.Text = names;
        }

        public void setcDate(string cDates)
        {
            cDate.Text = cDates;
        }

        public void setType(string types)
        {
            type.Text = types;
        }

        public void setRequestDateFrom(string requestDateFroms)
        {
            requestDateFrom.Text = requestDateFroms;
        }

        public void setRequestDateTo(string requestDateTos)
        {
            requestDateTo.Text = requestDateTos;
        }

        public void setReason(string reasons)
        {
            reason.Text = reasons;
        }

        public void setStatus(string statuses)
        {
            status.Text = statuses;
        }

        public void setSelect(bool check) { select.Checked = check; }

        public void setListeners(int position, ifBoxIsChecked callee)
        {
            ifBoxIsChecked callee_1 = callee;
            int positions = position;

            select.CheckedChange += delegate
            {
                callee_1.ifChecked(positions, select.Checked);
            };

            approve.Click += delegate
            {
                //callee_1.ifConfirm(positions, "Approved");
            };

            reject.Click += delegate
            {
               // callee_1.ifConfirm(positions, "Rejected");
            };
        }

    }
}