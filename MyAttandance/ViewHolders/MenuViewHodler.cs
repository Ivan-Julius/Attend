using Java.Lang;
using Android.Views;
using Android.Widget;
using Android.Util;


namespace MyAttandance.ViewHolders
{
    class MenuViewHodler : Object
    { 
        private static string LOG_TAG = typeof(MenuViewHodler).Name;

        //Field Declaration & id pairing MUST have get In-front of name
        private ImageView imageMenu;
        private IMenuClicked delegator;
        private string ButtonName = "";

        public interface IMenuClicked
        {
            // interface members
            void clicked(string button);
        }

        public MenuViewHodler(IMenuClicked delegator)
        {
            this.delegator = delegator;
        }

        public void setView(View v, int img, string buttonName)
        {
            imageMenu = (ImageView)v.FindViewById(Resource.Id.menu_image);
          
            imageMenu.SetImageResource(img);
            ButtonName = buttonName;
        }

        public void setListeners(int viewPosition)
        {
            string methodName = "setListeners()";
            Log.Debug(LOG_TAG, string.Format("{0}: --- start data : {1}---", methodName, viewPosition.ToString()));

            imageMenu.Click += delegate
            {
                delegator.clicked(ButtonName);
            };

            Log.Debug(LOG_TAG, string.Format("{0}: --- end ---", methodName));
        }

    }
}