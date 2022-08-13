using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region MonthlyAttendanceReport
    [DataContract]
    public class MonthlyAttendanceReport_Extend : BusinessObject
    {
        public MonthlyAttendanceReport_Extend()
        {
            ConfirmationDate = DateTime.Now;
            LastWorkingDate = DateTime.Now;
            EmployeeType = ""; 
            EmployeeCategory = EnumEmployeeCategory.None;
            ReportingPerson="";
            NOTMin =0;
            HOTMin = 0;
            NightAllDay = 0;

        }

        #region Properties
        public DateTime ConfirmationDate { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public string EmployeeType { get; set; }
        public EnumEmployeeCategory EmployeeCategory { get; set; }
        public string ReportingPerson { get; set; }
        public int NOTMin { get; set; }
        public int HOTMin { get; set; }
        public int NightAllDay { get; set; }

        #endregion

        #region Derived Property

        public string NOTMinSt
        {
            get
            {
                if (NOTMin > 0)
                    return NOTMin.ToString();
                else
                    return "-";
            }
        }
        public string HOTMinSt
        {
            get
            {
                if (HOTMin > 0)
                    return HOTMin.ToString();
                else
                    return "-";
            }
        }

        public string NightAllDaySt
        {
            get
            {
                if (NightAllDay > 0)
                    return NightAllDay.ToString();
                else
                    return "-";
            }
        }

        public string ConfirmationDateInString
        {
            get
            {
                return ConfirmationDate.ToString("dd MMM yyyy");
            }
        }
        public string LastWorkingDateInString
        {
            get
            {
                return LastWorkingDate.ToString("dd MMM yyyy");
            }
        }
        public string EmployeeCategoryInString
        {
            get
            {
                return EmployeeCategory.ToString();
            }
        }
        #endregion
    }
    #endregion
}
