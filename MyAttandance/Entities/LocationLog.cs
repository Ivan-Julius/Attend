
using Java.Lang;

namespace MyAttandance.Entities
{
    class LocationLog : Object
    {
        public LocationLog() { }
        public string Id { get; set; }
        public string dtStr { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string locStat { get; set; }
    }
}