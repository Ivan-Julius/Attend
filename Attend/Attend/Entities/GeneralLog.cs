
namespace Attend.Entities
{
    class GeneralLog
    { 
        public string id { get; set; }
        public long dt { get; set; }
        public string dt_str { get; set; }
        public string level { get; set; }
        public string _class { get; set; }
        public string methods { get; set; }
        public string message { get; set; }
    }
}