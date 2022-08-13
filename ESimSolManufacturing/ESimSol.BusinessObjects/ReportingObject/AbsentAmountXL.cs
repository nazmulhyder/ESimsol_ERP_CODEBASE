using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class AbsentAmountXL
    {
        public AbsentAmountXL()
        {
            SL = "";
            Code = "";
            Name = "";
            Department = "";
            Designation = "";
            DateOfJoin = "";
            Basic = "";
            HRent = "";
            Medical = "";
            GrossAmount = "";
            AbsentHR_Sick = "";
            AbsentHR_LWP = "";
            TotalAbsentAmount = "";
            Status = "";
        }
        public string SL { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string DateOfJoin { get; set; }
        public string Basic { get; set; }
        public string HRent { get; set; }
        public string Medical { get; set; }
        public string GrossAmount { get; set; }
        public string AbsentHR_Sick { get; set; }
        public string AbsentHR_LWP { get; set; }
        public string TotalAbsentAmount { get; set; }
        public string Status { get; set; }

    }

    public class AbsentAmountWithouTsALARYXL
    {
        public AbsentAmountWithouTsALARYXL()
        {
            SL = "";
            Code = "";
            Name = "";
            Department = "";
            Designation = "";
            DateOfJoin = "";

            AbsentHR_Sick = "";
            AbsentHR_LWP = "";
            TotalAbsentAmount = "";
            Status = "";
        }
        public string SL { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string DateOfJoin { get; set; }
        public string AbsentHR_Sick { get; set; }
        public string AbsentHR_LWP { get; set; }
        public string TotalAbsentAmount { get; set; }
        public string Status { get; set; }

    }
}
