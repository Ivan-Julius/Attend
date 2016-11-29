using System;
using System.Net;
using System.IO;

namespace MyAttandance.Helper
{
    class HttpClientHelper
    {
        private static string LOG_TAG = typeof(HttpClientHelper).Name;
      
        //private static AsyncHttpClient client = null;

        public interface IHttpClient
        {
            void AsyncResponse(string response);
        }
      
        public static async void FetchDataAsync(string url, IHttpClient client)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
          
            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    client.AsyncResponse((new StreamReader(stream)).ReadToEnd());
                }
            }
        }


        public static void FetchDataSync(string url, IHttpClient client)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    client.AsyncResponse((new StreamReader(stream)).ReadToEnd());
                }
            }
        }
    }
}