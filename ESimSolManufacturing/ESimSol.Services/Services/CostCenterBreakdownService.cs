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

    public class CostCenterBreakdownService : MarshalByRefObject, ICostCenterBreakdownService
    {
        #region Private functions and declaration
        private CostCenterBreakdown MapObject(NullHandler oReader)
        {
            CostCenterBreakdown oCostCenterBreakdown = new CostCenterBreakdown();
            oCostCenterBreakdown.CCTID = oReader.GetInt32("CCTID");
            oCostCenterBreakdown.CCID = oReader.GetInt32("CCID");
            oCostCenterBreakdown.CCCode = oReader.GetString("CCCode");
            oCostCenterBreakdown.CCName = oReader.GetString("CCName");
            oCostCenterBreakdown.OpeiningValue = oReader.GetDouble("OpeiningValue");
            oCostCenterBreakdown.IsDebit = oReader.GetBoolean("IsDebit");
            oCostCenterBreakdown.DebitAmount = oReader.GetDouble("DebitAmount");
            oCostCenterBreakdown.CreditAmount = oReader.GetDouble("CreditAmount");
            oCostCenterBreakdown.ClosingValue = oReader.GetDouble("ClosingValue");
            oCostCenterBreakdown.IsDrClosing = oReader.GetBoolean("IsDrClosing");
            oCostCenterBreakdown.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oCostCenterBreakdown.VoucherID = oReader.GetInt32("VoucherID");
            oCostCenterBreakdown.VoucherNo = oReader.GetString("VoucherNo");
            oCostCenterBreakdown.VoucherDate = oReader.GetDateTime("VoucherDate");
            oCostCenterBreakdown.AccountHeadName = oReader.GetString("AccountHeadName");
            oCostCenterBreakdown.ParentHeadID = oReader.GetInt32("ParentHeadID");
            oCostCenterBreakdown.ParentHeadName = oReader.GetString("ParentHeadName");
            oCostCenterBreakdown.ParentHeadCode = oReader.GetString("ParentHeadCode");
            oCostCenterBreakdown.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            oCostCenterBreakdown.Narration = oReader.GetString("Narration");
            oCostCenterBreakdown.VoucherNarration = oReader.GetString("VoucherNarration");
            oCostCenterBreakdown.Description = oReader.GetString("Description");
            oCostCenterBreakdown.ComponentID = oReader.GetInt16("ComponentID");
            return oCostCenterBreakdown;
        }

        private CostCenterBreakdown CreateObject(NullHandler oReader)
        {
            CostCenterBreakdown oCostCenterBreakdown = new CostCenterBreakdown();
            oCostCenterBreakdown = MapObject(oReader);
            return oCostCenterBreakdown;
        }

        private List<CostCenterBreakdown> CreateObjects(IDataReader oReader)
        {
            List<CostCenterBreakdown> oCostCenterBreakdown = new List<CostCenterBreakdown>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CostCenterBreakdown oItem = CreateObject(oHandler);
                oCostCenterBreakdown.Add(oItem);
            }
            return oCostCenterBreakdown;
        }

        #endregion

        #region Interface implementation
        public CostCenterBreakdownService() { }
        public List<CostCenterBreakdown> Gets(string BUIDs, int nAccountHeadID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved, int nUserId)
        {
            List<CostCenterBreakdown> oCostCenterBreakdowns = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostCenterBreakdownDA.Gets(tc, BUIDs, nAccountHeadID, nCurrencyID, StartDate, EndDate, bIsApproved);
                oCostCenterBreakdowns = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostCenterBreakdown", e);
                #endregion
            }

            return oCostCenterBreakdowns;
        }


        public List<CostCenterBreakdown> GetsAccountWiseBreakdown(int nAccountHeadID, string BUIDs, int nCCID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved, EnumBalanceStatus eBalanceStatus, int nUserId)
        {
            List<CostCenterBreakdown> oCostCenterBreakdowns = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostCenterBreakdownDA.GetsAccountWiseBreakdown(tc, nAccountHeadID, BUIDs, nCCID, nCurrencyID, StartDate, EndDate, bIsApproved, eBalanceStatus);
                oCostCenterBreakdowns = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostCenterBreakdown", e);
                #endregion
            }

            return oCostCenterBreakdowns;
        }

        public List<CostCenterBreakdown> GetsForCostCenter(string BUIDs, int nAccountHeadID, int nCostCenterID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool IsApproved, int nUserId)
        {
            List<CostCenterBreakdown> oCostCenterBreakdowns = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostCenterBreakdownDA.GetsForCostCenter(tc, BUIDs, nAccountHeadID, nCostCenterID, nCurrencyID, StartDate, EndDate, IsApproved);
                oCostCenterBreakdowns = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostCenterBreakdown", e);
                #endregion
            }

            return oCostCenterBreakdowns;
        }
        #endregion
    } 
    
    
}
