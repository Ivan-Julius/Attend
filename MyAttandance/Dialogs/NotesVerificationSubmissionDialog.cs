using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MyAttandance.Dialogs
{
    class NotesVerificationSubmissionDialog : AlertDialog
    {

        public Spinner typeSelector;


        public interface IOnNotesSubmitListener
        {
            void onNotesSubmit(string type);
        }

        static int resolveDialogTheme(Context context, int resId)
        {
            if (resId == 0)
            {
                TypedValue outValue = new TypedValue();
                context.Theme.ResolveAttribute(Resource.Attribute.popupTheme, outValue, true);
                return outValue.ResourceId;
            }
            else
            {
                return resId;
            }
        }

        public NotesVerificationSubmissionDialog(Context context, int theme) : base(context, resolveDialogTheme(context, theme))
        {
            this.inflater = LayoutInflater.From(context);

            mContext = Application.Context;
            LayoutInflater inflater = LayoutInflater.From(mContext);

            View view = inflater.Inflate(Resource.Layout.NoteVerificationDialog, null);
            typeSelector = view.FindViewById<Spinner>()



        }



    }
}