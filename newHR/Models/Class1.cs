using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newHR.Models
{
    public class salaryAttModel
    {

        public string day { get; set; }
        public string date { get; set; }
        public string name { get; set; }
        public string timeFrom { get; set; }
        public string timeTo { get; set; }
        public int fileNo { get; set; }
        public int shiftId { get; set; }
        public decimal dailyHour { get; set; }
        public int late { get; set; }
        public int leave { get; set; }
        public decimal leaveValue { get; set; }
        public decimal lateValue { get; set; }
        public decimal addition { get; set; }
        public decimal attend { get; set; }

        public decimal absenceValue { get; set; }
        public int mission { get; set; }

        public int inbetween { get; set; }
        public decimal hourSalary { get; set; }
        public decimal dailySalary { get; set; }
        public decimal monthlySalary { get; set; }
        public decimal skill { get; set; }
        public decimal regular { get; set; }
        public decimal expensive { get; set; }
        public decimal management { get; set; }
        public decimal insurance { get; set; }
        public decimal loans { get; set; }
        public decimal deductions { get; set; }
        public decimal sanctions { get; set; }
        public decimal clothes { get; set; }
        public decimal rewards { get; set; }
        public decimal overTime { get; set; }
        public decimal balance { get; set; }
        public string department { get; set; }
        public string joiningDate { get; set; }
        public int statusId { get; set; }
        public int absenceType { get; set; }
        public decimal dayPart { get; set; }
        public decimal officialAtt { get; set; }

    }


}