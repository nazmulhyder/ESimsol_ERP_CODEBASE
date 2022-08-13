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
   
    public class SP_GeneralLedgerService : MarshalByRefObject, ISP_GeneralLedgerService
    {
        #region Private functions and declaration
        private SP_GeneralLedger MapObject(NullHandler oReader)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            oSP_GeneralLedger.SP_GeneralLedgerID = oReader.GetInt32("ID");
            oSP_GeneralLedger.VoucherID = oReader.GetInt32("VoucherID");
            oSP_GeneralLedger.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oSP_GeneralLedger.DebitAmount = oReader.GetDouble("DebitAmount");
            oSP_GeneralLedger.CreditAmount = oReader.GetDouble("CreditAmount");
            oSP_GeneralLedger.IsDebit = oReader.GetBoolean("IsDebit");
            oSP_GeneralLedger.CurrentBalance = oReader.GetDouble("CurrentBalance");
            oSP_GeneralLedger.Narration = oReader.GetString("Narration");
            oSP_GeneralLedger.VoucherNo = oReader.GetString("VoucherNo");
            oSP_GeneralLedger.VoucherDate = oReader.GetDateTime("VoucherDate");
            oSP_GeneralLedger.OpenningBalance = oReader.GetDouble("OpenningBalance");
            oSP_GeneralLedger.VoucherNarration = oReader.GetString("VoucherNarration");
            oSP_GeneralLedger.Particulars = oReader.GetString("RevarseHead");
            oSP_GeneralLedger.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            return oSP_GeneralLedger;
        }

        private SP_GeneralLedger CreateObject(NullHandler oReader)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            oSP_GeneralLedger = MapObject(oReader);
            return oSP_GeneralLedger;
        }

        private List<SP_GeneralLedger> CreateObjects(IDataReader oReader)
        {
            List<SP_GeneralLedger> oSP_GeneralLedger = new List<SP_GeneralLedger>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SP_GeneralLedger oItem = CreateObject(oHandler);
                oSP_GeneralLedger.Add(oItem);
            }
            return oSP_GeneralLedger;
        }

        #endregion

        #region Interface implementation
        public SP_GeneralLedgerService() { }


        public List<SP_GeneralLedger> GetsGeneralLedger(int nAccountHeadId, DateTime dstartDate, DateTime dendDate, int nCurrencyID, bool bIsApproved, string BusinessUnitIDs, int nUserId)
        {
           List<SP_GeneralLedger> oSP_GeneralLedgers = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                    //reader = SP_GeneralLedgerDA.GetsGeneralLedger(tc, nAccountHeadId, dstartDate, dendDate, nCurrencyID, bIsApproved, nBusinessUnitID);
                reader = SP_GeneralLedgerDA.GetsGeneralLedger(tc, nAccountHeadId, dstartDate, dendDate, nCurrencyID, bIsApproved, BusinessUnitIDs);
                
                oSP_GeneralLedgers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oSP_GeneralLedgers;
        }

        public List<SP_GeneralLedger> ProcessGeneralLedger(int nAccountHeadId, DateTime dstartDate, DateTime dendDate, int nCurrencyID, bool bIsApproved, string sSQL, int nUserId)
        {
            List<SP_GeneralLedger> oSP_GeneralLedgers = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SP_GeneralLedgerDA.ProcessGeneralLedger(tc, nAccountHeadId, dstartDate, dendDate, nCurrencyID, bIsApproved, sSQL);
                oSP_GeneralLedgers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSP_GeneralLedgers = new List<SP_GeneralLedger>();
                SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
                oSP_GeneralLedger.ErrorMessage = e.Message;
                oSP_GeneralLedgers.Add(oSP_GeneralLedger);
                #endregion
            }

            return oSP_GeneralLedgers;
        }
        public List<SP_GeneralLedger> Gets(string sSQL, int nUserId)
        {
            List<SP_GeneralLedger> oSP_GeneralLedger = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SP_GeneralLedgerDA.Gets(tc, sSQL);
                oSP_GeneralLedger = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oSP_GeneralLedger;
        }
        public List<SP_GeneralLedger> GetsForReport(SP_GeneralLedger oCCT, int nUserID)
        {
            List<SP_GeneralLedger> oCostCenterTransactions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                // IDataReader reader = null;
                oCostCenterTransactions = SP_GeneralLedgerDA.GetsForReport(tc, oCCT);
                //oCostCenterTransaction = CreateObjects_report(reader);
                //reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostCenterTransaction", e);
                #endregion
            }

            return oCostCenterTransactions;
        }
        #endregion
    }
}
