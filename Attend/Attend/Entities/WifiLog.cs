
namespace Attend.Entities
{
    class WifiLog
    {
        public WifiLog() { }
        public string Id { get; set; }
        public string dtStr { get; set; }
        public string ssid { get; set; }
        public string bssid { get; set; }
        public string status { get; set; }
    }
}