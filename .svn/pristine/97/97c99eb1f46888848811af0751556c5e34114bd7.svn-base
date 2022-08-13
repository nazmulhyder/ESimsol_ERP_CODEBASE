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
    #region SalarySummery_F2
    [DataContract]
    public class SalarySummary_F2 : BusinessObject
    {
        public SalarySummary_F2()
        {
            BusinessUnitID = 0;
            BusinessUnitName = "";
            BusinessUnitAddress = "";
            LocationID = 0;
            LocationName = "";
            DepartmentID = 0;
            DepartmentName = "";
            NoOfEmp = 0;
            OTHr = 0;
            CompOTHr = 0;
            GrossSalary = 0;
            CompGrossSalary = 0;
            OTAmount = 0;
            CompOTAmount = 0;
            TotalPayable = 0;
            CompTotalPayable = 0;
            Stamp = 0;
            NetPay = 0;
            CompNetPay = 0;
            BankPay = 0;
            CashPay = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            EmployeeTypeID = 0;
            GroupName = "";
            EOA = 0;
            DayOffOT = 0;
            HolidayOT = 0;
            List = new List<SalarySummary_F2>();
        }

        #region Properties
        public int EmployeeTypeID { get; set; }
        public int BusinessUnitID { get; set; }
        public string BusinessUnitName { get; set; }
        public string GroupName { get; set; }
        public string BusinessUnitAddress { get; set; }
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int NoOfEmp { get; set; }
        public double OTHr { get; set; }
        public double EOA { get; set; }
        public double CompOTHr { get; set; }
        public double GrossSalary { get; set; }
        public double CompGrossSalary { get; set; }
        public double OTAmount { get; set; }
        public double CompOTAmount { get; set; }
        public double TotalPayable { get; set; }
        public double CompTotalPayable { get; set; }
        public double Stamp { get; set; }
        public double NetPay { get; set; }
        public double CompNetPay { get; set; }
        public double BankPay { get; set; }
        public double CashPay { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<SalarySummary_F2> List { get; set; }
        public double DayOffOT { get; set; }
        public double HolidayOT { get; set; }
        public double FirstOT { get; set; }
        public double SecondOT { get; set; }
        public double ThirdOT { get; set; }
        public double FirstOTLimit { get; set; }
        public double SecondOTLimit { get; set; }
        public string StartDateInString
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy");
            }
        }

        public string EndDateInString
        {
            get
            {
                return EndDate.ToString("dd MMM yyyy");
            }
        }
       
        #endregion

        #region Functions
        public static List<SalarySummary_F2> Gets(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, long nUserID)
        {
            return SalarySummary_F2.Service.Gets(BusinessUnitIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, PayType, MonthID, Year, NewJoin, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, nUserID);
        }
        public static List<SalarySummary_F2> GetsSalarySummaryComp(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, bool bIsOutSheet, string sGroupIDs, string sBlockIDs, long nUserID)
        {
            return SalarySummary_F2.Service.GetsSalarySummaryComp(BusinessUnitIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, PayType, MonthID, Year, NewJoin, bIsOutSheet, sGroupIDs, sBlockIDs, nUserID);
        }
        public static List<SalarySummary_F2> GetsGroup(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, int EmpGrouping, double nStartSalaryRange, double nEndSalaryRange, long nUserID)
        {
            return SalarySummary_F2.Service.GetsGroup(BusinessUnitIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, PayType, MonthID, Year, NewJoin, EmpGrouping, nStartSalaryRange, nEndSalaryRange, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISalarySummery_F2Service Service
        {
            get { return (ISalarySummery_F2Service)Services.Factory.CreateService(typeof(ISalarySummery_F2Service)); }
        }
        #endregion
    }
    #endregion

    #region ISalarySummery_F2 interface

    public interface ISalarySummery_F2Service
    {
        List<SalarySummary_F2> Gets(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, Int64 nUserID);
        List<SalarySummary_F2> GetsSalarySummaryComp(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, bool bIsOutSheet, string sGroupIDs, string sBlockIDs, Int64 nUserID);
        List<SalarySummary_F2> GetsGroup(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, int EmpGrouping, double nStartSalaryRange, double nEndSalaryRange, Int64 nUserID);
    }
    #endregion
}
