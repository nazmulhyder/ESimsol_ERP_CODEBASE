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
    public class SP_GeneralJournalService : MarshalByRefObject, ISP_GeneralJournalService
    {
        #region Private functions and declaration
        private SP_GeneralJournal MapObject(NullHandler oReader)
        {
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            oSP_GeneralJournal.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            oSP_GeneralJournal.VoucherID = oReader.GetInt32("VoucherID");
            oSP_GeneralJournal.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oSP_GeneralJournal.IsDebit = oReader.GetBoolean("IsDebit");
            oSP_GeneralJournal.Narration = oReader.GetString("Narration");
            oSP_GeneralJournal.DebitAmount = oReader.GetDouble("DebitAmount");
            oSP_GeneralJournal.CreditAmount = oReader.GetDouble("CreditAmount");
            oSP_GeneralJournal.VoucherNo = oReader.GetString("VoucherNo");
            oSP_GeneralJournal.VoucherTypeID = oReader.GetInt32("VoucherTypeID");
            oSP_GeneralJournal.VoucherDate = oReader.GetDateTime("VoucherDate");
            oSP_GeneralJournal.VoucherName = oReader.GetString("VoucherName");
            oSP_GeneralJournal.AccountCode = oReader.GetString("AccountCode");
            oSP_GeneralJournal.AccountHeadName = oReader.GetString("AccountHeadName");
            oSP_GeneralJournal.CurrencyID = oReader.GetInt32("CurrencyID");
            oSP_GeneralJournal.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oSP_GeneralJournal.VoucherNarration = oReader.GetString("VoucherNarration");
            return oSP_GeneralJournal;
        }
        private SP_GeneralJournal MapObject_SALog(NullHandler oReader)
        {
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
           
            oSP_GeneralJournal.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oSP_GeneralJournal.AccountHeadName = oReader.GetString("AccountHeadName");
            oSP_GeneralJournal.AccountCode = oReader.GetString("AccountCode");
            oSP_GeneralJournal.VoucherDate = oReader.GetDateTime("DBServerDateTime");
            oSP_GeneralJournal.IsDebit = oReader.GetBoolean("IsDebit");
            oSP_GeneralJournal.Narration = oReader.GetString("Remarks");
            oSP_GeneralJournal.CreditAmount = oReader.GetDouble("Amount");

            return oSP_GeneralJournal;
        }

        private SP_GeneralJournal CreateObject(NullHandler oReader)
        {
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            oSP_GeneralJournal = MapObject(oReader);
            return oSP_GeneralJournal;
        }
        private SP_GeneralJournal CreateObject_SALog(NullHandler oReader)
        {
            SP_GeneralJournal oSP_GeneralJournal = new SP_GeneralJournal();
            oSP_GeneralJournal = MapObject_SALog(oReader);
            return oSP_GeneralJournal;
        }

        private List<SP_GeneralJournal> CreateObjects(IDataReader oReader)
        {
            List<SP_GeneralJournal> oSP_GeneralJournal = new List<SP_GeneralJournal>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SP_GeneralJournal oItem = CreateObject(oHandler);
                oSP_GeneralJournal.Add(oItem);
            }
            return oSP_GeneralJournal;
        }

        private List<SP_GeneralJournal> CreateObjects_SALog(IDataReader oReader)
        {
            List<SP_GeneralJournal> oSP_GeneralJournal = new List<SP_GeneralJournal>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SP_GeneralJournal oItem = CreateObject_SALog(oHandler);
                oSP_GeneralJournal.Add(oItem);
            }
            return oSP_GeneralJournal;
        }

        #endregion

        #region Interface implementation
        public SP_GeneralJournalService() { }

        public List<SP_GeneralJournal> GetsGeneralJournal(string sSQL, int nUserId)
        {
            List<SP_GeneralJournal> oSP_GeneralJournals = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SP_GeneralJournalDA.GetsGeneralJournal(tc, sSQL);
                oSP_GeneralJournals = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Voucher Details", e);
                #endregion
            }

            return oSP_GeneralJournals;
        }

        public List<SP_GeneralJournal> Gets(string sSQL,int nCompanyID, int nUserId)
        {
            List<SP_GeneralJournal> oSP_GeneralJournals = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SP_GeneralJournalDA.Gets(tc, sSQL, nCompanyID);
                oSP_GeneralJournals = CreateObjects(reader);
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

            return oSP_GeneralJournals;
        }

        public List<SP_GeneralJournal> Gets_SuspendALog(string sSQL, int nCompanyID, int nUserId)
        {
            List<SP_GeneralJournal> oSP_GeneralJournals = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SP_GeneralJournalDA.Gets_SuspendALog(tc, sSQL, nCompanyID);
                oSP_GeneralJournals = CreateObjects_SALog(reader);
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
            return oSP_GeneralJournals;
        }
        #endregion
    }
}
