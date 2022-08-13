using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class OTSheetXL
    {
        public OTSheetXL()
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
            Gross= "";
            OT_NHR = "";
            OT_HHR = "";
            FHOT = "";
            OTAmount = "";
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
        public string Gross { get; set; }
        public string OT_NHR { get; set; }
        public string OT_HHR { get; set; }
        public string FHOT { get; set; }
        public string OTAmount { get; set; }
        public string Status { get; set; }
    }

    public class OTSheetWithOutSalaryXL
    {
        public OTSheetWithOutSalaryXL()
        {
            SL = "";
            Code = "";
            Name = "";
            Department = "";
            Designation = "";
            DateOfJoin = "";
            OT_NHR = "";
            OT_HHR = "";
            FHOT = "";
            OTAmount = "";
            Status = "";
        }
        public string SL { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string DateOfJoin { get; set; }
        public string OT_NHR { get; set; }
        public string OT_HHR { get; set; }
        public string FHOT { get; set; }
        public string OTAmount { get; set; }
        public string Status { get; set; }
    }
}
