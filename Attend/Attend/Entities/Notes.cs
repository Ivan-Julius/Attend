
namespace Attend.Entities
{
    public class Notes
    {
        public int id { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string noteType { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
        public string createDate { get; set; }
        public string owner { get; set; }

        public Notes(int ids, string fDates, string tDates, string nTypes, string reasons, string statuses, string createDates, string owners)
        {
            this.id = ids;
            this.fromDate = fDates;
            this.toDate = tDates;
            this.noteType = nTypes;
            this.status = statuses;
            this.reason = reasons;
            this.createDate = createDates;
            this.owner = owners;
        }

    }
}