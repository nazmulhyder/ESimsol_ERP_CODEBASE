using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region SalaryCorrectionProcess
    public class SalaryCorrection
    {
        public SalaryCorrection()
        {

            EmployeeID = 0;
            Name = "";
            Code = "";
            DepartmentName = "";
            DesignationName = "";
            LocationName = "";
            BUName = "";
            ErrorMessage = "";
            BUID = 0;
            LocationID = 0;
            DepartmentID = 0;
            DesignationID = 0;
            Reason = "";
            MonthID = 0;
            Year = 0;
            PayrollProcessManagement = new PayrollProcessManagement();
            IndexNo = 0;
        }

        #region Properties
        public int IndexNo { get; set; }
        public int BUID { get; set; }
        public int MonthID { get; set; }
        public int Year { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Reason { get; set; }
        public string Code { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string LocationName { get; set; }
        public string BUName { get; set; }
        public string ErrorMessage { get; set; }

        public PayrollProcessManagement PayrollProcessManagement { get; set; }
        #endregion

        #region Functions


        public static List<SalaryCorrection> Gets(string sSQL, long nUserID)
        {
            return SalaryCorrection.Service.Gets(sSQL, nUserID);
        }
        public static SalaryCorrection Get(string sSQL, long nUserID)
        {
            return SalaryCorrection.Service.Get(sSQL, nUserID);
        }
        public static List<SalaryCorrection> GetsReason(string sBU, string sLocationID, int nMonthID, int nYear, int nRowLength, int nLoadRecords, string sEmployeeIDs, bool bIsCallFromExcel, long nUserID)
        {
            return SalaryCorrection.Service.GetsReason(sBU, sLocationID, nMonthID, nYear, nRowLength, nLoadRecords, sEmployeeIDs, bIsCallFromExcel, nUserID);
        }
 
        #endregion

        #region ServiceFactory

        internal static ISalaryCorrectionService Service
        {
            get { return (ISalaryCorrectionService)Services.Factory.CreateService(typeof(ISalaryCorrectionService)); }
        }
        #endregion
    }
    #endregion

    #region ISalaryCorrection interface

    public interface ISalaryCorrectionService
    {
        List<SalaryCorrection> Gets(string sSQL, Int64 nUserID);
        SalaryCorrection Get(string sSQL, Int64 nUserID);
        List<SalaryCorrection> GetsReason(string sBU, string sLocationID, int nMonthID, int nYear, int nRowLength, int nLoadRecords, string sEmployeeIDs, bool bIsCallFromExcel, Int64 nUserID);
      
    }
    #endregion
}

