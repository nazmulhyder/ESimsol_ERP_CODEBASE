using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class BudgetVarianceService : MarshalByRefObject, IBudgetVarianceService
    {
        #region Private functions and declaration
        private BudgetVariance MapObject(NullHandler oReader)
        {
            BudgetVariance oBudgetVariance = new BudgetVariance();
            oBudgetVariance.BudgetID = oReader.GetInt32("BudgetID");
            oBudgetVariance.BudgetDetailID = oReader.GetInt32("BudgetDetailID");
            oBudgetVariance.ComponentID = oReader.GetInt32("ComponentID");
            oBudgetVariance.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oBudgetVariance.AccountCode = oReader.GetString("AccountCode");
            oBudgetVariance.AccountHeadName = oReader.GetString("AccountHeadName");
            oBudgetVariance.SubGroupID = oReader.GetInt32("SubGroupID");
            oBudgetVariance.SubGroupName = oReader.GetString("SubGroupName");
            oBudgetVariance.SubGroupCode = oReader.GetString("SubGroupCode");
            oBudgetVariance.MonthNo = oReader.GetInt32("MonthNo");
            oBudgetVariance.NameofMonth = oReader.GetString("NameofMonth");
            oBudgetVariance.StartDate = oReader.GetDateTime("StartDate");
            oBudgetVariance.EndDate = oReader.GetDateTime("EndDate");
            oBudgetVariance.BudgetAmount = oReader.GetDouble("BudgetAmount");
            oBudgetVariance.DebitAmount = oReader.GetDouble("DebitAmount");
            oBudgetVariance.CreditAmount = oReader.GetDouble("CreditAmount");
            oBudgetVariance.ActualAmount = oReader.GetDouble("ActualAmount");
            oBudgetVariance.VarianceAmount = oReader.GetDouble("VarianceAmount");
            oBudgetVariance.AchievePercent = oReader.GetDouble("AchievePercent");
            oBudgetVariance.TotalBudgetAmount = oReader.GetDouble("TotalBudgetAmount");
            oBudgetVariance.TotalActualAmount = oReader.GetDouble("TotalActualAmount");
            oBudgetVariance.TotalVarianceAmount = oReader.GetDouble("TotalVarianceAmount");
            oBudgetVariance.TotalAchievePercent = oReader.GetDouble("TotalAchievePercent");
            return oBudgetVariance;
        }

        private BudgetVariance CreateObject(NullHandler oReader)
        {
            BudgetVariance oBudgetVariance = new BudgetVariance();
            oBudgetVariance = MapObject(oReader);
            return oBudgetVariance;
        }
        private List<BudgetVariance> CreateObjects(IDataReader oReader)
        {
            List<BudgetVariance> oBudgetVariance = new List<BudgetVariance>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BudgetVariance oItem = CreateObject(oHandler);
                oBudgetVariance.Add(oItem);
            }
            return oBudgetVariance;
        }

        #endregion

        #region Interface implementation
        public BudgetVarianceService() { }
        public List<BudgetVariance> Gets(Int64 nUserID)
        {
            List<BudgetVariance> oBudgetVariances = new List<BudgetVariance>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BudgetVarianceDA.Gets(tc);
                oBudgetVariances = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BudgetVariance", e);
                #endregion
            }
            return oBudgetVariances;
        }
        public List<BudgetVariance> Gets(string sSQL,Int64 nUserID)
        {
            List<BudgetVariance> oBudgetVariances = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BudgetVarianceDA.Gets(tc,sSQL);
                oBudgetVariances = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BudgetVariance", e);
                #endregion
            }
            return oBudgetVariances;
        }
        public List<BudgetVariance> GetsReport(int nBudgetID, int nReportType, bool IsApproved, string sStartDateSt, string sEndDateSt, Int64 nUserID)
        {
            List<BudgetVariance> oBudgetVariances = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BudgetVarianceDA.GetsReport(tc, nBudgetID, nReportType, IsApproved, sStartDateSt, sEndDateSt);
                oBudgetVariances = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BudgetVariance", e);
                #endregion
            }
            return oBudgetVariances;
        }
        #endregion
    }   
}
