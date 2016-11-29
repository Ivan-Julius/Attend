
namespace Attend.Entities
{
    public class MonthlyReport
    {
        public string report_year { get; set; }
        public string report_month { get; set; }
        public string name { get; set; }
        public string count_late { get; set; }
        public string count_not_full { get; set; }
        public string count_full { get; set; }
        public string count_sick { get; set; }
        public string count_leave { get; set; }
        public string count_AllowedLate { get; set; }
        public string count_earlyLeave { get; set; }
        public string overtime_elapse_time { get; set; }
        public string count_not_login { get; set; }
        public string count_login { get; set; }
        public string total_attendance { get; set; }

        public MonthlyReport(string report_years, string report_months, string names, string count_AllowedLates, string count_earlyLeaves, string count_fulls,
                                   string count_lates, string count_logins, string count_not_fulls, string count_not_logins,
                                   string count_sicks, string count_leaves, string overtime_elapse_times, string total_attendances)
        {
            name = names;
            report_year = report_years;
            report_month = report_months;
            count_AllowedLate = count_AllowedLates;
            count_earlyLeave = count_earlyLeaves;
            count_full = count_fulls;
            count_late = count_lates;
            count_leave = count_leaves;
            count_login = count_logins;
            count_not_full = count_not_fulls;
            count_not_login = count_not_logins;
            count_sick = count_sicks;
            total_attendance = total_attendances;
            overtime_elapse_time = overtime_elapse_times;
        }




    }
}