
using Android.Views;
using Android.Widget;
using Java.Lang;
using MyAttandance.Adapters;

namespace MyAttandance.ViewHolders
{
    public class CheckBoxDialogViewHolder : Object
    {
        private CheckBox checker;
        private string type;

        public void setChecker(string text, bool check, string type) {
            this.checker.Text = text;
            this.checker.Checked = check;
            this.type = type;
        }

        public void SetView(View v)
        {
            this.checker = (CheckBox)v.FindViewById(Resource.Id.options_box);
        }

        public void SetListeners(CheckboxDialogAdapter.checkboxSelected sendBack)
        {
            checker.CheckedChange += delegate
            {
                sendBack.onCheckBoxSelected(checker.Text, checker.Checked, type);
            };
        }

    }

}