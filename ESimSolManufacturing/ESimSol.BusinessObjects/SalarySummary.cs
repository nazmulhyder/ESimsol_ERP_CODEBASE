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
    #region SalarySummary

    public class SalarySummary : BusinessObject
    {
        public SalarySummary()
        {
            DepartmentID = 0;
            DepartmentName = "";
            SalaryHeadName = "";
            SalaryHeadType = EnumSalaryHeadType.None;
            Amount = 0;
            ErrorMessage = "";
        }

        #region Properties

        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string SalaryHeadName { get; set; }
        public EnumSalaryHeadType SalaryHeadType { get; set; }
        public double Amount { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        public int SalaryHeadTypeInt
        {
            get
            {
                return (int)this.SalaryHeadType;
            }
        }

        #endregion

        #region Functions
        public static List<SalarySummary> Gets(DateTime StartTime, DateTime EndTime, bool IsDateSearch, int nLocationID, long nUserID)
        {
            return SalarySummary.Service.Gets(StartTime,EndTime,IsDateSearch,nLocationID,nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ISalarySummaryService Service
        {
            get { return (ISalarySummaryService)Services.Factory.CreateService(typeof(ISalarySummaryService)); }
        }

        #endregion
    }
    #endregion

    #region ISalarySummary interface

    public interface ISalarySummaryService
    {
        List<SalarySummary> Gets(DateTime StartTime, DateTime EndTime, bool IsDateSearch, int nLocationID,Int64 nUserID);
    }
    #endregion
}
