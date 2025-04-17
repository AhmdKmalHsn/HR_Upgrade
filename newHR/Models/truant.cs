using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newHR.Models
{
    public class truant
    {
        public int fileNumber { get; set; }
        public string name { get; set; }
        public int abcenceDays { get; set; }
        public decimal vacation { get; set; }
    }
}