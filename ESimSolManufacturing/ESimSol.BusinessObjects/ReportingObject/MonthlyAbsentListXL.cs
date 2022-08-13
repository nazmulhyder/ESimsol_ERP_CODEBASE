using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class MonthlyAbsentListXL
    {
        public MonthlyAbsentListXL()
        {
            SL = "";
            Code = "";
            Name = "";
            Department= "";
            Designation = "";
            AttendanceDate = "";
            Shift = "";
            InTime = "";
            OutTime = "";
            Status = "";
        }
        public string SL { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string AttendanceDate { get; set; }
        public string Shift { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string Status { get; set; }

    }
}
