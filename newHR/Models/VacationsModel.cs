using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newHR.Models
{
    public class VacationsModel
    {
        public int fileNumber { get; set; }
        public string name { get; set; }
        public string departmentId { get; set; }
        public decimal abcenceDays { get; set; }
        public decimal deferredDays { get; set; }
        public string deferredDate { get; set; }
        public decimal availableDays { get; set; }

    }
}