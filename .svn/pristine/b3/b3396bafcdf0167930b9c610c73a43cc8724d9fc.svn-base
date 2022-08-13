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
    public class SalarySummaryDetail_F2 : BusinessObject
    {
        public SalarySummaryDetail_F2()
        {
            BusinessUnitID = 0;
            LocationID = 0;
            DepartmentID = 0;
            Amount = 0;
            CompAmount = 0;
            SalaryHeadID = 0;
            SalaryHeadName = ""; 
            EmployeeTypeID = 0;
            SalaryHeadIDType = EnumSalaryHeadType.None;
            SalaryHeadSequence = 0;
            
        }

        #region Properties
        public int EmployeeTypeID { get; set; }
        public int BusinessUnitID { get; set; }
        public int LocationID { get; set; }
        public int DepartmentID { get; set; }
        public double Amount { get; set; }
        public double CompAmount { get; set; }
        public int SalaryHeadSequence { get; set; }
        public int SalaryHeadID { get; set; }
        public string SalaryHeadName { get; set; }
        public EnumSalaryHeadType SalaryHeadIDType { get; set; }

        #endregion

        #region Functions
        public static List<SalarySummaryDetail_F2> Gets(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, long nUserID)
        {
            return SalarySummaryDetail_F2.Service.Gets(BusinessUnitIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, PayType, MonthID, Year, NewJoin,sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, nUserID);
        }
        public static List<SalarySummaryDetail_F2> GetsSalarySummaryDetailComp(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, bool bIsOutSheet, string sGroupIDs, string sBlockIDs, long nUserID)
        {
            return SalarySummaryDetail_F2.Service.GetsSalarySummaryDetailComp(BusinessUnitIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, PayType, MonthID, Year, NewJoin, bIsOutSheet,sGroupIDs, sBlockIDs, nUserID);
        }
        public static List<SalarySummaryDetail_F2> GetsGroupDetail(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, int EmpGrouping, double nStartSalaryRange, double nEndSalaryRange, long nUserID)
        {
            return SalarySummaryDetail_F2.Service.GetsGroupDetail(BusinessUnitIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, PayType, MonthID, Year, NewJoin, EmpGrouping, nStartSalaryRange, nEndSalaryRange, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISalarySummaryDetail_F2Service Service
        {
            get { return (ISalarySummaryDetail_F2Service)Services.Factory.CreateService(typeof(ISalarySummaryDetail_F2Service)); }
        }
        #endregion
    }
    #endregion

    #region ISalarySummery_F2 interface

    public interface ISalarySummaryDetail_F2Service
    {
        List<SalarySummaryDetail_F2> Gets(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, Int64 nUserID);
        List<SalarySummaryDetail_F2> GetsSalarySummaryDetailComp(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, bool bIsOutSheet, string sGroupIDs, string sBlockIDs, Int64 nUserID);
        List<SalarySummaryDetail_F2> GetsGroupDetail(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, int EmpGrouping, double nStartSalaryRange, double nEndSalaryRange, Int64 nUserID);
    }
    #endregion
}
