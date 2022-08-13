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
    public class StatementNoteService : MarshalByRefObject, IStatementNoteService
    {
        #region Private functions and declaration
        private StatementNote MapObject(NullHandler oReader)
        {
            StatementNote oStatementNote = new StatementNote();
            oStatementNote.VoucherID = oReader.GetInt32("VoucherID");
            oStatementNote.VoucherNo = oReader.GetString("VoucherNo");
            oStatementNote.VoucherDate = oReader.GetDateTime("VoucherDate");
            oStatementNote.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oStatementNote.ApprovedByName = oReader.GetString("ApprovedByName");
            oStatementNote.PrepareBy = oReader.GetInt32("PrepareBy");
            oStatementNote.PrepareByName = oReader.GetString("PrepareByName");
            oStatementNote.VoucherNarration = oReader.GetString("VoucherNarration");
            oStatementNote.CashVoucherDetailID = oReader.GetInt32("CashVoucherDetailID");
            oStatementNote.VoucherDetailID = oReader.GetInt32("VoucherDetailID");
            oStatementNote.CashAccountHeadID = oReader.GetInt32("CashAccountHeadID");
            oStatementNote.CashAccountCode = oReader.GetString("CashAccountCode");
            oStatementNote.CashAccountName = oReader.GetString("CashAccountName");
            oStatementNote.CashSubLedgerID = oReader.GetInt32("CashSubLedgerID");
            oStatementNote.CashSubLedgerName = oReader.GetString("CashSubLedgerName");
            oStatementNote.CashIsDebit = oReader.GetBoolean("CashIsDebit");
            oStatementNote.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oStatementNote.ParticularAccountCode = oReader.GetString("ParticularAccountCode");
            oStatementNote.ParticularAccountName = oReader.GetString("ParticularAccountName");
            oStatementNote.ParticularSubLedgerID = oReader.GetInt32("ParticularSubLedgerID");
            oStatementNote.ParticularSubLedgerName = oReader.GetString("ParticularSubLedgerName");
            oStatementNote.IsDebit = oReader.GetBoolean("IsDebit");
            oStatementNote.Amount = oReader.GetDouble("Amount");
            oStatementNote.CurrencyID = oReader.GetInt32("CurrencyID");
            oStatementNote.CurrencySymbol = oReader.GetString("CurrencySymbol");
            return oStatementNote;
        }

        private StatementNote CreateObject(NullHandler oReader)
        {
            StatementNote oStatementNote = new StatementNote();
            oStatementNote = MapObject(oReader);
            return oStatementNote;
        }

        private List<StatementNote> CreateObjects(IDataReader oReader)
        {
            List<StatementNote> oStatementNote = new List<StatementNote>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                StatementNote oItem = CreateObject(oHandler);
                oStatementNote.Add(oItem);
            }
            return oStatementNote;
        }

        #endregion

        public List<StatementNote> Gets(int nStatementSetupID, DateTime dstartDate, DateTime dendDate, int nBUID, int nAccountHeadID, bool bIsDebit, int nUserId)
        {
            List<StatementNote> oStatementNotes = new List<StatementNote>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = StatementNoteDA.Gets(tc, nStatementSetupID, dstartDate, dendDate, nBUID, nAccountHeadID, bIsDebit);
                oStatementNotes = CreateObjects(reader);
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
            return oStatementNotes;
        }
    }
}
