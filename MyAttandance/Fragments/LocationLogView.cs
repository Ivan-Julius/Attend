using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using MyAttandance.Entities;
using MyAttandance.SqlSource;

namespace MyAttandance
{
    class LocationLogView : Fragment
    {

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View v = inflater.Inflate(Resource.Layout.fragment_log, container, false);

            List<LocationLog> lls = new LogDataSource<LocationLog>().GetAllLogItems(Application.Context);

            string text = "";

            foreach(LocationLog ll in lls)
            {
                string t = string.Format("date({0}), lat({1}), lon ({2}), status({3} \n", ll.dtStr, ll.lat, ll.lon, ll.locStat);
                Log.Debug("LocationLogView", t);
                text = text + t;
            }

            /*TextView tv = (TextView)v.FindViewById(Resource.Id.logView);
            tv.Text = text;*/

            return v; 
        }

    }
}