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
    public class SalarySummaryDetail_F2Service : MarshalByRefObject, ISalarySummaryDetail_F2Service
    {
        #region Private functions and declaration
        private SalarySummaryDetail_F2 MapObject(NullHandler oReader)
        {
            SalarySummaryDetail_F2 oSalarySummaryDetail_F2 = new SalarySummaryDetail_F2();

            oSalarySummaryDetail_F2.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oSalarySummaryDetail_F2.LocationID = oReader.GetInt32("LocationID");
            oSalarySummaryDetail_F2.DepartmentID = oReader.GetInt32("DepartmentID");
            oSalarySummaryDetail_F2.Amount = oReader.GetDouble("Amount");
            oSalarySummaryDetail_F2.CompAmount = oReader.GetDouble("CompAmount");
            oSalarySummaryDetail_F2.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oSalarySummaryDetail_F2.EmployeeTypeID = oReader.GetInt32("EmployeeTypeID");
            oSalarySummaryDetail_F2.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oSalarySummaryDetail_F2.SalaryHeadIDType = (EnumSalaryHeadType)oReader.GetInt16("SalaryHeadIDType");
            oSalarySummaryDetail_F2.SalaryHeadSequence = oReader.GetInt32("SalaryHeadSequence");
            
            return oSalarySummaryDetail_F2;
        }

        private SalarySummaryDetail_F2 CreateObject(NullHandler oReader)
        {
            SalarySummaryDetail_F2 oSalarySummaryDetail_F2 = MapObject(oReader);
            return oSalarySummaryDetail_F2;
        }

        private List<SalarySummaryDetail_F2> CreateObjects(IDataReader oReader)
        {
            List<SalarySummaryDetail_F2> oSalarySummaryDetail_F2 = new List<SalarySummaryDetail_F2>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalarySummaryDetail_F2 oItem = CreateObject(oHandler);
                oSalarySummaryDetail_F2.Add(oItem);
            }
            return oSalarySummaryDetail_F2;
        }

        #endregion

        #region Interface implementation
        public SalarySummaryDetail_F2Service() { }
        public List<SalarySummaryDetail_F2> Gets(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, Int64 nUserID)
        {
            List<SalarySummaryDetail_F2> oSalarySummaryDetail_F2s = new List<SalarySummaryDetail_F2>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                reader = SalarySummaryDetail_F2DA.Gets(BusinessUnitIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, PayType, MonthID, Year, NewJoin, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, nUserID, tc);
                oSalarySummaryDetail_F2s = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_SalarySummaryDetail_F2", e);
                #endregion
            }
            return oSalarySummaryDetail_F2s;
        }
        public List<SalarySummaryDetail_F2> GetsGroupDetail(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, int EmpGroup, double nStartSalaryRange, double nEndSalaryRange, Int64 nUserID)
        {
            List<SalarySummaryDetail_F2> oSalarySummery_F2s = new List<SalarySummaryDetail_F2>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                reader = SalarySummaryDetail_F2DA.GetsGroupDetail(BusinessUnitIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, PayType, MonthID, Year, NewJoin, EmpGroup, nStartSalaryRange, nEndSalaryRange, nUserID, tc);
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
                throw new ServiceException("Failed to Get GroupDetail", e);
                #endregion
            }
            return oSalarySummery_F2s;
        }
        public List<SalarySummaryDetail_F2> GetsSalarySummaryDetailComp(string BusinessUnitIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string EmployeeIDs, int PayType, int MonthID, int Year, bool NewJoin, bool bIsOutSheet, string sGroupIDs, string sBlockIDs, Int64 nUserID)
        {
            List<SalarySummaryDetail_F2> oSalarySummaryDetail_F2s = new List<SalarySummaryDetail_F2>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                reader = SalarySummaryDetail_F2DA.GetsSalarySummaryDetailComp(BusinessUnitIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, EmployeeIDs, PayType, MonthID, Year, NewJoin, bIsOutSheet, sGroupIDs, sBlockIDs, nUserID, tc);
                oSalarySummaryDetail_F2s = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_SalarySummaryDetail_F2", e);
                #endregion
            }
            return oSalarySummaryDetail_F2s;
        }

        #endregion

    }
}
