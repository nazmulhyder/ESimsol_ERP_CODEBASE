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

    public class VPTransactionSummaryService : MarshalByRefObject, IVPTransactionSummaryService
    {
        #region Private functions and declaration
        private VPTransactionSummary MapObject(NullHandler oReader)
        {
            VPTransactionSummary oVPTransactionSummary = new VPTransactionSummary();
            oVPTransactionSummary.ProductID = oReader.GetInt32("ProductID");
            oVPTransactionSummary.ProductCode = oReader.GetString("ProductCode");
            oVPTransactionSummary.ProductName = oReader.GetString("ProductName");
            oVPTransactionSummary.OpeiningValue = oReader.GetDouble("OpeiningValue");
            oVPTransactionSummary.IsDebit = oReader.GetBoolean("IsDebit");
            oVPTransactionSummary.DebitAmount = oReader.GetDouble("DebitAmount");
            oVPTransactionSummary.CreditAmount = oReader.GetDouble("CreditAmount");
            oVPTransactionSummary.ClosingValue = oReader.GetDouble("ClosingValue");
            //oVPTransactionSummary.IsDrClosing = oReader.GetBoolean("IsDrClosing");
            oVPTransactionSummary.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oVPTransactionSummary.VoucherID = oReader.GetInt32("VoucherID");
            oVPTransactionSummary.VoucherNo = oReader.GetString("VoucherNo");
            oVPTransactionSummary.VoucherDate = oReader.GetDateTime("VoucherDate");
            oVPTransactionSummary.AccountHeadName = oReader.GetString("AccountHeadName");
            oVPTransactionSummary.ParentHeadID = oReader.GetInt32("ParentHeadID");
            oVPTransactionSummary.ParentHeadName = oReader.GetString("ParentHeadName");
            oVPTransactionSummary.ParentHeadCode = oReader.GetString("ParentHeadCode");
            oVPTransactionSummary.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            oVPTransactionSummary.Narration = oReader.GetString("Narration");
            oVPTransactionSummary.VoucherNarration = oReader.GetString("VoucherNarration");
            oVPTransactionSummary.Description = oReader.GetString("Description");
            oVPTransactionSummary.OpeiningQty = oReader.GetDouble("OpeiningQty");
            oVPTransactionSummary.DebitQty = oReader.GetDouble("DebitQty");
            oVPTransactionSummary.CreditQty = oReader.GetDouble("CreditQty");
            oVPTransactionSummary.ClosingQty = oReader.GetDouble("ClosingQty");
            oVPTransactionSummary.UnitPrice = oReader.GetDouble("UnitPrice");
            return oVPTransactionSummary;
        }

        private VPTransactionSummary CreateObject(NullHandler oReader)
        {
            VPTransactionSummary oVPTransactionSummary = new VPTransactionSummary();
            oVPTransactionSummary = MapObject(oReader);
            return oVPTransactionSummary;
        }

        private List<VPTransactionSummary> CreateObjects(IDataReader oReader)
        {
            List<VPTransactionSummary> oVPTransactionSummary = new List<VPTransactionSummary>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VPTransactionSummary oItem = CreateObject(oHandler);
                oVPTransactionSummary.Add(oItem);
            }
            return oVPTransactionSummary;
        }

        #endregion

        #region Interface implementation
        public VPTransactionSummaryService() { }
        public List<VPTransactionSummary> Gets(string BUIDs, int nAccountHeadID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved, int nUserId)
        {
            List<VPTransactionSummary> oVPTransactionSummarys = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VPTransactionSummaryDA.Gets(tc, BUIDs, nAccountHeadID, nCurrencyID, StartDate, EndDate, bIsApproved);
                oVPTransactionSummarys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VPTransactionSummary", e);
                #endregion
            }

            return oVPTransactionSummarys;
        }


        public List<VPTransactionSummary> GetsForProduct(string BUIDs, int nAccountHeadID, int nProductID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool IsApproved, int nUserId)
        {
            List<VPTransactionSummary> oVPTransactionSummarys = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VPTransactionSummaryDA.GetsForProduct(tc, BUIDs, nAccountHeadID, nProductID, nCurrencyID, StartDate, EndDate, IsApproved);
                oVPTransactionSummarys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VPTransactionSummary", e);
                #endregion
            }

            return oVPTransactionSummarys;
        }
        #endregion
    } 
    
    
}
