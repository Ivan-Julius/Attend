
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Text;
using Java.Util;
using MyAttandance.Helper;
using MyAttandance.Services;
using static MyAttandance.Helper.BindingHelper<MyAttandance.Services.AppLocationService.LocalBinder, MyAttandance.Services.AppLocationService>;
using static MyAttandance.Helper.HttpClientHelper;
using static MyAttandance.Services.AppLocationService;

namespace MyAttandance
{
    class NotesFragment : Fragment, IBindingStatus, IHttpClient, DatePickerDialog.IOnDateSetListener, TimePickerDialog.IOnTimeSetListener
    {
        private Context mContext;
        private string ErrorMessage = "";
        private Dictionary<int, string> inputedData = new Dictionary<int, string>();

        private bool isLocationBinded = false;
        private BindingHelper<LocalBinder, AppLocationService> helpers = null;

        private int contentCount = 4;

        private Spinner notesTypeSelector;
        private TextView fromDate;
        private TextView toDate;
        private TextView fromTime;
        private TextView toTime;
        private TextView reason;
        private TextView location;
        private Button submit;
        private Button reset;

        private View notesView;
        private View timeLabels;
        private View timeInputs;
        private Calendar newCalendar = Calendar.Instance;

        private View CurrentView;
        private int CUrrentViewID;

        private bool isFromDate = false;
        private bool isFromTime = false;
        private Location loc = null;
        private bool stat = false;

        public override void OnCreate(Bundle savedInstanceState)
        {
            locationServiceBinding(out ErrorMessage);
           
            mContext = Application.Context;
            base.OnCreate(savedInstanceState);
        }

        public override void OnDestroy()
        {
            locationServiceUnbinding();
            base.OnDestroy();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.fragment_notes, container, false);

            notesTypeSelector = rootView.FindViewById<Spinner>(Resource.Id.typeSelector);

            var adapter = ArrayAdapter.CreateFromResource(mContext, Resource.Array.notesTypes, Android.Resource.Layout.SimpleSpinnerDropDownItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            notesTypeSelector.Adapter = adapter;

            fromDate = rootView.FindViewById<TextView>(Resource.Id.fromDate);
            toDate = rootView.FindViewById<TextView>(Resource.Id.toDate);
            fromTime = rootView.FindViewById<TextView>(Resource.Id.fromTime);
            toTime = rootView.FindViewById<TextView>(Resource.Id.toTime);
            reason = rootView.FindViewById<TextView>(Resource.Id.notes_reason);
            location = rootView.FindViewById<TextView>(Resource.Id.locationContent);
            timeLabels = rootView.FindViewById<LinearLayout>(Resource.Id.timeLabels);
            timeInputs = rootView.FindViewById<LinearLayout>(Resource.Id.timeInputs);
            submit = rootView.FindViewById<Button>(Resource.Id.notes_submit);
            reset = rootView.FindViewById<Button>(Resource.Id.notes_reset);

            updateLocation();

            notesView = rootView;
            return rootView;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            mContext = Activity;

            if (fromDate != null)
                setListeners(fromDate);

            if (toDate != null)
                setListeners(toDate);

            if (fromTime != null)
                setListeners(fromTime);

            if (toTime != null)
                setListeners(toTime);

            if (reason != null)
                setListeners(reason);

            if (notesTypeSelector != null)
                setListeners(notesTypeSelector);

            if (location != null)
                setListeners(location);

            if (submit != null)
                setListeners(submit);

            if (reset != null)
                setListeners(reset);

            sendBoolean(true);
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            newCalendar.Set(year, monthOfYear, dayOfMonth);
            SimpleDateFormat dateFormatter = new SimpleDateFormat("dd-MM-yyyy", Locale.Us);
            sendBackString((TextView)CurrentView, CUrrentViewID, dateFormatter.Format(newCalendar.Time).ToString(), false);
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            string sHour = (hourOfDay > 9) ? hourOfDay.ToString() : "0" + hourOfDay;
            string sMinute = (minute < 10) ? "0" + minute : minute.ToString();
            sendBackString((TextView)CurrentView, CUrrentViewID, sHour + ":" + sMinute, true);
        }

        private void setListeners(View inputs)
        {
            int inputsId = inputs.Id;

            if (!(inputsId == Resource.Id.typeSelector))
            {
                TextView input = (TextView)inputs;

                switch (inputsId)
                {
                    case Resource.Id.fromDate:
                    case Resource.Id.toDate:

                        input.Click += delegate
                        {
                            CurrentView = input;
                            CUrrentViewID = input.Id;
                            isFromDate = (CUrrentViewID == Resource.Id.fromDate);
                            new DatePickerDialog(mContext, this, newCalendar.Get(CalendarField.Year), newCalendar.Get(CalendarField.Month), newCalendar.Get(CalendarField.DayOfMonth)).Show();
                        };

                        break;

                    case Resource.Id.fromTime:
                    case Resource.Id.toTime:

                        input.Click += delegate
                        {
                            CurrentView = input;
                            CUrrentViewID = input.Id;
                            isFromTime = (CUrrentViewID == Resource.Id.fromTime);
                            new TimePickerDialog(mContext, this, newCalendar.Get(CalendarField.HourOfDay), newCalendar.Get(CalendarField.Minute), true).Show();
                        };

                        break;

                    case Resource.Id.notes_submit:
                    case Resource.Id.notes_reset:

                        Button inButton = (Button)inputs;
                        CurrentView = inButton;
                        CUrrentViewID = inButton.Id;

                        inButton.Click += delegate
                        {
                            string posButton = (inputsId == Resource.Id.notes_reset) ? "Reset" : "Submit";
                            string message = "Are you sure you want to" + posButton + " ?";

                            AlertDialog.Builder alert = new AlertDialog.Builder(mContext);
                            alert.SetTitle("Confirm "+ posButton);
                            alert.SetMessage(message);

                            alert.SetPositiveButton(posButton, (senderAlert, args) => {
                                buttonsManager();
                            });

                            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                              
                            });

                            alert.Create().Show();
                        };
                        break;

                      case Resource.Id.locationContent:

                        input.Click += delegate
                        {
                            updateLocation();
                        };

                      break;

                }

             } else {

                Spinner input = (Spinner)inputs;

                input.ItemSelected += delegate
                {
                    CurrentView = input;
                    CUrrentViewID = input.Id;
                    updateInputedData(CUrrentViewID, input.SelectedItem.ToString());
                };
             }
        }

        private void locationServiceBinding(out string msg)
        {
            helpers = new BindingHelper<LocalBinder, AppLocationService>(this);
            msg = "Please Restart App";

            if (isMyServiceRunning())
            {
                System.Diagnostics.Debug.WriteLine("my service is running");
                if (!isLocationBinded) helpers.bind();
            }

            else msg = "Please Restart App";
        }
        private void updateLocation()
        {
            if (loc == null)
            {
                binderMonitor();
            }

            if(stat){ 

                Geocoder geo = new Geocoder(Application.Context, Locale.Us);
                var addresses = geo.GetFromLocation(loc.Latitude, loc.Latitude, 1);

                if (addresses.Count > 0)
                    location.Text = addresses[0].GetAddressLine(0);
            }
        }

        private void binderMonitor()
        {
            if (!isLocationBinded)
            {
                locationServiceBinding(out ErrorMessage);
            }
            else
            {
                if (helpers.binders.IsBinderAlive)
                {
                    loc = ((LocalBinder)helpers.binders).getLocation();
                    stat = ((LocalBinder)helpers.binders).getLocStatus();
                    updateLocation();
                }
            }
        }

        private void locationServiceUnbinding()
        {
            if (isLocationBinded) helpers.unBind();
        }

        public void isBinded(bool status)
        {
            isLocationBinded = status;
        }

        public void AsyncResponse(string response)
        {
            Toast.MakeText(mContext, response, ToastLength.Long).Show();
        }
         
        private void updateInputedData(int key, string data)
        {
            if (inputedData.ContainsKey(key))
            {
                inputedData[key] = data;

            }else
            {
                inputedData.Add(key, data);
            }

            if (key == Resource.Id.typeSelector)
            {
                if (inputedData.ContainsKey(Resource.Id.toTime)) inputedData.Remove(Resource.Id.toTime);
                if (inputedData.ContainsKey(Resource.Id.fromTime)) inputedData.Remove(Resource.Id.fromTime);

                sendBoolean(findHideShow(data));
            }
        }

        private void sendBoolean(bool data)
        {
            contentCount = (data) ? 6 : 4;

            timeInputs.Visibility = (data) ? ViewStates.Visible : ViewStates.Gone;
            timeLabels.Visibility = (data) ? ViewStates.Visible : ViewStates.Gone;
        }

        private bool findHideShow(string type)
        {
            bool result = false;

            if (type.ToLower().Contains("late") || type.ToLower().Contains("early"))
            {
                result = true;
            }

            return result;
        }

        private void sendBackString(TextView view, int key, string data, bool isTime)
        {
            if (key == Resource.Id.typeSelector)
            {
                if (inputedData.ContainsKey(Resource.Id.toTime)) inputedData.Remove(Resource.Id.toTime);
                if (inputedData.ContainsKey(Resource.Id.fromTime)) inputedData.Remove(Resource.Id.fromTime);
            }

            view.Text = reformatDate(data, isTime, true);
            updateInputedData(key, reformatDate(data, isTime, false));
        }

        private string reformatDate(string data, bool isTime, bool forDisplay)
        {
            return (!isTime) ? formater(forDisplay, data) : data + ":00";
        }

        private string formater(bool isForDisplay, string Data)
        {
            string result = "";

            System.DateTime date = DateFormatingHelper.stringToDateWithFormat(Data + " 00:00:00", DateFormatingHelper.ShortDateWithTimeFormat);
            result = (isForDisplay) ? date.ToString(DateFormatingHelper.LongDateOnlyFormat) : date.ToString(DateFormatingHelper.YearInFrontFormat);

            return result;
        }

        private void buttonsManager()
        {
            if (CUrrentViewID == Resource.Id.notes_reset)
            {
                inputedData.Clear();
                formClear();
            }
            else
            {
                updateInputedData(Resource.Id.notes_reason, reason.Text);
                runSubmit();
            }
        }

        private void formClear()
        {
            notesTypeSelector.SetSelection(1);
            fromDate.Text = "";
            toDate.Text = "";
            fromTime.Text = "";
            fromTime.Text = "";
            location.Text = "";
            reason.Text = "";
        }

        private void runSubmit()
        {
            if (contentCount == inputedData.Count)
            {
                Dictionary<string, string> setUpToSend = new Dictionary<string, string>();
                setUpToSend = submissionPreparations();

                updateLocation();
                FetchDataAsync(HttpRequestHelper.notesSubmissionString(WifiConnectivityHelper.getMacMarshmellow(), setUpToSend, loc.Longitude.ToString(), loc.Latitude.ToString()), this);
            }
        }

        private Dictionary<string, string> submissionPreparations()
        {
            int[] keysLabel = (contentCount == 4) ? new int[] { Resource.Id.FromDateLabel, Resource.Id.ToDateLabel, Resource.Id.reason_label, Resource.Id.typeLabel } :
               new int[] { Resource.Id.FromDateLabel, Resource.Id.ToDateLabel, Resource.Id.FromTimeLabel, Resource.Id.ToTimeLabel, Resource.Id.reason_label, Resource.Id.typeLabel };

            int[] keys = (contentCount == 4) ? new int[] { Resource.Id.fromDate, Resource.Id.toDate, Resource.Id.notes_reason, Resource.Id.typeSelector } :
            new int[] { Resource.Id.fromDate, Resource.Id.toDate, Resource.Id.fromTime, Resource.Id.toTime, Resource.Id.notes_reason, Resource.Id.typeSelector };

            Dictionary<string, string> setUpToSend = new Dictionary<string, string>();

            for (int i = 0; i < inputedData.Count; i++)
            {
                string name = notesView.FindViewById<TextView>(keysLabel[i]).Text.ToString();
                string value = inputedData[keys[i]];

                if (keys[i] == Resource.Id.typeSelector)
                {
                    name = name.Replace("Note ", "Reason_");
                }
                else if (keys[i] == Resource.Id.fromDate || keys[i] == Resource.Id.toDate)
                {
                    if (!(contentCount == 4))
                    {
                        string time = inputedData[(keys[i] == Resource.Id.fromDate) ? Resource.Id.fromTime : Resource.Id.toTime];
                        value = value.Substring(0, 11) + time;
                    }
                    name = name.Replace(" ", "_");
                }
                else
                {
                    name = name.Replace(" ", "_");
                }

                name = name.ToLower();
                setUpToSend.Add(name, value);
            }

            return setUpToSend;
        }
    }
}