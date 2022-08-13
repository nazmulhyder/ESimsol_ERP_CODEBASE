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
    #region ExtraOTDynamic
    public class ExtraOTDynamic
    {
        public ExtraOTDynamic()
        {

            EmployeeSalaryID = 0;
            EmployeeID = 0;
            BUID = 0;
            LocationID = 0;
            DepartmentID = 0;
            DesignationID = 0;
            TotalDays = 0;
            LWP = 0;
            P = 0;
            Gross = 0.0;
            Basics = 0.0;
            OT_HR = 0.0;
            OT_Rate = 0.0;
            OT_Amount = 0.0;
            Code = "";
            Name = "";
            DateOfJoin = DateTime.Now;
            BUName = "";
            LocName = "";
            DptName = "";
            DsgName = "";
            Grade = "";
            EmpNameInBangla = "";
            DsgNameInBangla = "";
            BUAddress = "";

        }

        #region Properties
        public int EmployeeSalaryID { get; set; }
        public int EmployeeID { get; set; }
        public int BUID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public int TotalDays { get; set; }
        public int LWP { get; set; }
        public int P { get; set; }
        public double Gross { get; set; }
        public double Basics { get; set; }
        public double OT_HR { get; set; }
        public double OT_Rate { get; set; }
        public double OT_Amount { get; set; }
        public string Code { get; set; }
        public string EmpNameInBangla { get; set; }
        public string BUAddress { get; set; }
        public string DsgNameInBangla { get; set; }
        public string Name { get; set; }
        public DateTime DateOfJoin { get; set; }
        public string BUName { get; set; }
        public string LocName { get; set; }
        public string DptName { get; set; }
        public string DsgName { get; set; }
        public string Grade { get; set; }
        #endregion

        public string DateOfJoinInString
        {
            get
            {
                return DateOfJoin.ToString("dd MMM yyyy");
            }
        }

        #region DerivedProperty

        #endregion


        #region Functions



        public static List<ExtraOTDynamic> Gets(string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string BlockIds, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, int nMOCID, long nUserID)
        {
            return ExtraOTDynamic.Service.Gets(BUIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, BlockIds, EmployeeIDs, nMonthID, nYear, IsNewJoin, IsOutSheet, nMinSalary, nMaxSalary, nMOCID, nUserID);
        }
 
        #endregion

        #region ServiceFactory

        internal static IExtraOTDynamicService Service
        {
            get { return (IExtraOTDynamicService)Services.Factory.CreateService(typeof(IExtraOTDynamicService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceDaily interface

    public interface IExtraOTDynamicService
    {
        List<ExtraOTDynamic> Gets(string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string BlockIds, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, int nMOCID, long nUserID);
       
      
    }
    #endregion
}


