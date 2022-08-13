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
    #region ELEncashReport

    public class ELEncashReport : BusinessObject
    {
        public ELEncashReport()
        {
            EmployeeID =0;
            EmployeeCode ="";
            EmployeeName ="";
            BusinessUnitName ="";
            BusinessUnitAddress = "";
            LocationName = "";
            DepartmentName ="";
            DesignationName = "";
            Joining =DateTime.Now;
            Gross =0;
            Amount = 0;
            DeclarationDate = DateTime.Now;
            TotalEL =0;
            Enjoyed =0;
            EncashedEL =0;
            EncashedELAmount =0;
            Stamp =0;
            NetPayable = 0;
            ErrorMessage = "";
            TotalDays = 0;
            BusinessUnitID = 0;
            LocationID = 0;
            DepartmentID = 0;
        }

        #region Properties
        public int EmployeeID { get; set; }
        public int DepartmentID { get; set; }
        public int BusinessUnitID { get; set; }
        public int LocationID { get; set; }
        public double TotalDays { get; set; }
        public string EmployeeCode {get; set;}
        public string EmployeeName {get; set;}
        public string BusinessUnitName {get; set;}
        public string BusinessUnitAddress { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public DateTime Joining { get; set; }
        public double Gross { get; set; }
        public double Amount { get; set; }
        public DateTime DeclarationDate { get; set; }
        public int TotalEL { get; set; }
        public int Enjoyed { get; set; }
        public double EncashedEL { get; set; }
        public double EncashedELAmount { get; set; }
        public double Stamp { get; set; }
        public double NetPayable { get; set; }
        public string ErrorMessage { get; set; }

        public int ManPower { get; set; }
        public string JoiningInString
        {
            get
            {
                return Joining.ToString("dd MMM yyyy");
            }
        }
        public string DeclarationDateInString
        {
            get
            {
                return DeclarationDate.ToString("dd MMM yyyy");
            }
        }
        public List<ELEncashReport> ELEncahsreports { get; set; }
        #endregion

        #region Functions
        public static List<ELEncashReport> Gets(string sELEncashIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIDs, string sEmployeeIDs, int nACSID, bool IsDeclarationDate, DateTime DeclarationDate, double nStartSalary, double nEndSalary, long nUserID)
        {
            return ELEncashReport.Service.Gets(sELEncashIDs,sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIDs, sEmployeeIDs, nACSID, IsDeclarationDate, DeclarationDate,nStartSalary,nEndSalary, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static IELEncashReportService Service
        {
            get { return (IELEncashReportService)Services.Factory.CreateService(typeof(IELEncashReportService)); }
        }

        #endregion
    }
    #endregion

    #region IELEncashReport interface
    public interface IELEncashReportService
    {
        List<ELEncashReport> Gets(string sELEncashIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIDs, string sEmployeeIDs, int nACSID, bool IsDeclarationDate, DateTime DeclarationDate, double nStartSalary, double nEndSalary, Int64 nUserID);

    }
    #endregion
}
