using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Attend.HelperAndroid
{
    public class HttpClientHelper
    {
        private static string LOG_TAG = typeof(HttpClientHelper).Name;
      
        //private static AsyncHttpClient client = null;

        public interface IHttpClient
        {
            void AsyncResponse(string response, string from);
        }
      
        public static async void FetchDataAsync(string url, IHttpClient client, string from)
        {
            // Create an HTTP web request using the URL:
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";

                using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        client.AsyncResponse((new StreamReader(stream)).ReadToEnd(), from);
                    }
                }

            }catch(Exception ex)
            {
                Debug.WriteLine("Error : " + ex.Message);
            }
        }


    }
}