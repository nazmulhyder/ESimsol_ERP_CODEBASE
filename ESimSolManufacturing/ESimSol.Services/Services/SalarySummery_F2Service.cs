using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class SalarySummery_F2Service : MarshalByRefObject, ISalarySummery_F2Service
    {
        #region Private functions and declaration
        private SalarySummary_F2 MapObject(NullHandler oReader)
        {
            SalarySummary_F2 oSalarySummery_F2 = new SalarySummary_F2();

            oSalarySummery_F2.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oSalarySummery_F2.EmployeeTypeID = oReader.GetInt32("EmployeeTypeID");
            oSalarySummery_F2.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oSalarySummery_F2.BusinessUnitAddress = oReader.GetString("BusinessUnitAddress");
            oSalarySummery_F2.LocationID = oReader.GetInt32("LocationID");
            oSalarySummery_F2.LocationName = oReader.GetString("LocationName");
            oSalarySummery_F2.GroupName = oReader.GetString("GroupName");
            oSalarySummery_F2.DepartmentID = oReader.GetInt32("DepartmentID");
            oSalarySummery_F2.DepartmentName = oReader.GetString("DepartmentName");
            oSalarySummery_F2.NoOfEmp = oReader.GetInt32("NoOfEmp");
            oSalarySummery_F2.OTHr = oReader.GetDouble("OTHr");
            oSalarySummery_F2.DayOffOT = oReader.GetDouble("DayOffOT");
            oSalarySummery_F2.HolidayOT = oReader.GetDouble("HolidayOT");
            oSalarySummery_F2.FirstOT = oReader.GetDouble("FirstOT");
            oSalarySummery_F2.SecondOT = oReader.GetDouble("SecondOT");
            oSalarySummery_F2.ThirdOT = oReader.GetDouble("ThirdOT");
            oSalarySummery_F2.EOA = oReader.GetDouble("EOA");
            oSalarySummery_F2.CompOTHr = oReader.GetDouble("CompOTHr");
            oSalarySummery_F2.GrossSalary = oReader.GetDouble("GrossSalary");
            oSalarySummery_F2.CompGrossSalary = oReader.GetDouble("CompGrossSalary");
            oSalarySummery_F2.OTAmount = oReader.GetDouble("OTAmount");
            oSalarySummery_F2.CompOTAmount = oReader.GetDouble("CompOTAmount");
            oSalarySummery_F2.TotalPayable = oReader.GetDouble("TotalPayable");
            oSalarySummery_F2.CompTotalPayable = oReader.GetDouble("CompTotalPayable");
            oSalarySummery_F2.Stamp = oReader.GetDouble("Stamp");
            oSalarySummery_F2.NetPay = oReader.GetDouble("NetPay");
            oSalarySummery_F2.CompNetPay = oReader.GetDouble("CompNetPay");
            oSalarySummery_F2.BankPay = oReader.GetDouble("BankPay");
            oSalarySummery_F2.CashPay = oReader.GetDouble("CashPay");
            oSalarySummery_F2.StartDate = oReader.GetDateTime("StartDate");
            oSalarySummery_F2.EndDate = oReader.GetDateTime("EndDate");
            return oSalarySummery_F2;
        }

        private SalarySummary_F2 CreateObject(NullHandler oReader)
        {
            SalarySummary_F2 oSalarySummery_F2 = MapObject(oReader);
            return oSalarySummery_F2;
        }

        private List<SalarySummary_F2> CreateObjects(IDataReader oReader)
        {
            List<SalarySummary_F2> oSalarySummery_F2 = new List<SalarySummary_F2>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalarySummary_F2 oItem = CreateObject(oHandler);
                oSalarySummery_F2.Add(oItem);
            }
            return oSalarySummery_F2;
        }

        #endregion

        #region Interface implementation
        public SalarySummery_F2Service() { }
        public List<SalarySummary_F2> Gets(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, Int64 nUserID)
        {
            List<SalarySummary_F2> oSalarySummery_F2s = new List<SalarySummary_F2>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                reader = SalarySummery_F2DA.Gets(BusinessUnitIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, PayType, MonthID, Year, NewJoin, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, nUserID, tc);
                oSalarySummery_F2s = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_SalarySummery_F2", e);
                #endregion
            }
            return oSalarySummery_F2s;
        }

        public List<SalarySummary_F2> GetsGroup(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, int EmpGroup, double nStartSalaryRange, double nEndSalaryRange, Int64 nUserID)
        {
            List<SalarySummary_F2> oSalarySummery_F2s = new List<SalarySummary_F2>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                reader = SalarySummery_F2DA.GetsGroup(BusinessUnitIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, PayType, MonthID, Year, NewJoin, EmpGroup, nStartSalaryRange, nEndSalaryRange, nUserID, tc);
                oSalarySummery_F2s = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Group", e);
                #endregion
            }
            return oSalarySummery_F2s;
        }

        public List<SalarySummary_F2> GetsSalarySummaryComp(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, bool bIsOutSheet, string sGroupIDs, string sBlockIDs, Int64 nUserID)
        {
            List<SalarySummary_F2> oSalarySummery_F2s = new List<SalarySummary_F2>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                reader = SalarySummery_F2DA.GetsSalarySummaryComp(BusinessUnitIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, PayType, MonthID, Year, NewJoin, bIsOutSheet, sGroupIDs, sBlockIDs, nUserID, tc);
                oSalarySummery_F2s = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_SalarySummery_F2", e);
                #endregion
            }
            return oSalarySummery_F2s;
        }

        #endregion

    }
}
