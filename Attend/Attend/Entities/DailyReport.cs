
using Xamarin.Forms;

namespace Attend.Entities
{
    public class DailyReport
    {
        public string id { get; set; }
        public string user_name { get; set; }
        public string date { get; set; }
        public string login_type { get; set; }
        public string first_login { get; set; }
        public string last_login { get; set; }
        public string logout_time { get; set; }
        public string login_elapse { get; set; }
        public string start_overtime { get; set; }
        public string overtime_elapse { get; set; }
        public bool is_late { get; set; }
        public bool is_full { get; set; }

        public DailyReport(string id, string login_elapse, string overtime_elapse, bool is_late, bool is_full, string date, string user_name, string first_login, string last_login, string logout_time, string start_overtime, string login_type)
        {
            this.id = id;
            this.login_elapse = login_elapse;
            this.date = date;
            this.overtime_elapse = overtime_elapse;
            this.is_late = is_late;
            this.is_full = is_full;
            this.user_name = user_name;
            this.first_login = first_login;
            this.last_login = last_login;
            this.logout_time = logout_time;
            this.start_overtime = start_overtime;
            this.login_type = login_type;
        }

    }
}