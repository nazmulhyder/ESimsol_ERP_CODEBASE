using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class AccountingSessionDA
    {
        public AccountingSessionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, AccountingSession oAccountingSession, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AccountingSession]"
                                    + "%n, %n, %s, %s, %n, %d, %d, %D, %D, %n, %s, %b, %s, %n, %n",
                                    oAccountingSession.AccountingSessionID, (int)oAccountingSession.SessionType, oAccountingSession.SessionCode, oAccountingSession.SessionName, (int)oAccountingSession.YearStatus, oAccountingSession.StartDate, oAccountingSession.EndDate, oAccountingSession.LockDateTime, oAccountingSession.ActivationDateTime, oAccountingSession.ParentSessionID, oAccountingSession.SessionHierarchy, oAccountingSession.IsDateActivation, oAccountingSession.WeekLyHolidays, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, AccountingSession oAccountingSession, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AccountingSession]"
                                    + "%n, %n, %s, %s, %n, %d, %d, %D, %D, %n, %s, %b, %s, %n, %n",
                                    oAccountingSession.AccountingSessionID, (int)oAccountingSession.SessionType, oAccountingSession.SessionCode, oAccountingSession.SessionName, (int)oAccountingSession.YearStatus, oAccountingSession.StartDate, oAccountingSession.EndDate, oAccountingSession.LockDateTime, oAccountingSession.ActivationDateTime, oAccountingSession.ParentSessionID, oAccountingSession.SessionHierarchy, oAccountingSession.IsDateActivation, oAccountingSession.WeekLyHolidays, nUserId, (int)eEnumDBOperation);
        }
        public static IDataReader LockUnLock(TransactionContext tc, AccountingSession oAccountingSession)
        {
            return tc.ExecuteReader("EXEC [SP_AccountingSessionLockUnLock]" + "%n, %d, %D, %b",
                                    oAccountingSession.AccountingSessionID, oAccountingSession.EndDate, oAccountingSession.LockDateTime, oAccountingSession.IsLock);
        }
        public static void RestAccountOpeningNeedTransfer(TransactionContext tc)
        {
            tc.ExecuteNonQuery("TRUNCATE TABLE AccountOpeningNeedTransfer");
        }
        public static void RestAccountOpeningBalanceTransfer(TransactionContext tc)
        {
            tc.ExecuteNonQuery("TRUNCATE TABLE AccountOpeningBalanceTransfered");
        }
        public static IDataReader DeclareNewAccountingYear(TransactionContext tc, AccountingSession oAccountingSession, Int64 nUserId)
        {            
            return tc.ExecuteReader("EXEC [SP_CommitNewAccountingYear]" + "%n, %n, %n", oAccountingSession.BUID, oAccountingSession.AccountingSessionID, nUserId);//IN TestDB
        }
        public static void TransferOpenningBalance(TransactionContext tc, int nNewRunningSessionID, int nPreRunningSessionID, int nBusinessUnitID, int nAccountHeadID, int nSubledgerID, int nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_CommitTransferOpeningBalance]" + "%n, %n, %n, %n, %n, %n", nNewRunningSessionID, nPreRunningSessionID, nBusinessUnitID, nAccountHeadID, nSubledgerID, nUserID);//IN TestDB
        }
        public static IDataReader AccountingYearClose(TransactionContext tc, AccountingSession oAccountingSession, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_CloseAccountingYear]" + "%n, %n", oAccountingSession.AccountingSessionID, nUserId);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountingSession WHERE AccountingSessionID=%n", nID);
        }
        public static IDataReader GetSessionByDate(TransactionContext tc, DateTime dSessionDate)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountingSession WHERE AccountingSessionID IN (SELECT SessionID FROM AccountingSession  WHERE CONVERT(DATE,CONVERT(VARCHAR(12),StartDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),%d,106)) AND SessionType=6)", dSessionDate);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountingSession ORDER BY AccountingSessionID");
        }
        public static IDataReader GetsTitleSessions(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountingSession WHERE ParentSessionID IN (-1,1) ORDER BY AccountingSessionID");
        }
        public static IDataReader GetsAccountingYears(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountingSession WHERE ParentSessionID=1 ORDER BY AccountingSessionID");
        }
        public static IDataReader GetOpenningAccountingYear(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountingSession AS AO WHERE AO.AccountingSessionID=(SELECT MIN(TT.AccountingSessionID) FROM AccountingSession AS TT WHERE TT.ParentSessionID=1)");
        }
        public static IDataReader GetRunningAccountingYear(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountingSession WHERE ParentSessionID=1 AND YearStatus=1");
        }
        public static IDataReader GetRunningFreezeAccountingYear(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountingSession WHERE ParentSessionID=1 AND YearStatus IN (1,2)");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static int GetNumberOfAccountsHead(TransactionContext tc)
        {
            //ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}
            int nNumberOfAccountHead = 0;
            object oNumberOfAccountHead = tc.ExecuteScalar("SELECT	COUNT(*) FROM dbo.GetAccountHeadByComponents('2,3,4') AS COA WHERE COA.AccountType=5");
            if (oNumberOfAccountHead != null)
            {
                nNumberOfAccountHead = Convert.ToInt32(oNumberOfAccountHead);
            }
            return nNumberOfAccountHead;
        }
        public static int GetNumberOfAccountsHeadFromNeedTransfer(TransactionContext tc)
        {
            //ComponentID{Asset = 2,Laibility = 3,OwnerEquity=4,Income = 5,Expeness = 6}
            int nNumberOfAccountHead = 0;
            object oNumberOfAccountHead = tc.ExecuteScalar("SELECT COUNT(*) FROM AccountOpeningNeedTransfer");
            if (oNumberOfAccountHead != null)
            {
                nNumberOfAccountHead = Convert.ToInt32(oNumberOfAccountHead);
            }
            return nNumberOfAccountHead;
        }
        #endregion
    }
}
