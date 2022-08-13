using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class BankAccountDA
    {
        public BankAccountDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BankAccount oBankAccount, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BankAccount]"
                                    + "%n, %s, %s, %n, %n, %n, %n, %n, %n, %n, %n",
                                    oBankAccount.BankAccountID, oBankAccount.AccountName, oBankAccount.AccountNo, oBankAccount.BankID, oBankAccount.BankBranchID, (int)oBankAccount.AccountType, oBankAccount.LimitAmount, oBankAccount.CurrentLimit, oBankAccount.BusinessUnitID, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, BankAccount oBankAccount, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BankAccount]"
                                    + "%n, %s, %s, %n, %n, %n, %n, %n, %n, %n, %n",
                                    oBankAccount.BankAccountID, oBankAccount.AccountName, oBankAccount.AccountNo, oBankAccount.BankID, oBankAccount.BankBranchID, (int)oBankAccount.AccountType, oBankAccount.LimitAmount, oBankAccount.CurrentLimit, oBankAccount.BusinessUnitID, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankAccount WHERE BankAccountID=%n", nID);
        }
        public static IDataReader GetViaCostCenter(TransactionContext tc, int nCCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankAccount WHERE BankAccountID = (SELECT ACC.ReferenceObjectID FROM View_ACCostCenter AS ACC WHERE ACC.ReferenceType=4 AND ACC.ACCostCenterID=" + nCCID + ")");
        }
        public static IDataReader GetPartyWiseDefultAccount(TransactionContext tc, int nPartyID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankAccount AS TT WHERE TT.BankAccountID =(SELECT CC.BankAccountID FROM Contractor AS CC WHERE CC.ContractorID=%n)", nPartyID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankAccount");
        }

        public static IDataReader GetsByBank(TransactionContext tc, int nBankID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankAccount WHERE BankID=%n", nBankID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        

        public static IDataReader GetsByBankBranch(TransactionContext tc, int nBankBranchID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankAccount WHERE BankBranchID=%n", nBankBranchID);
        }
        public static IDataReader GetsByDeptAndBU(TransactionContext tc, string sDeptIDs, int BUID)
        {
            return tc.ExecuteReader("Select * from View_BankAccount  where BankBranchID in (SELECT BankBranch.BankBranchID FROM BankBranch WHERE BankBranchID IN (SELECT BankBranchID FROM BankBranchBU WHERE BUID  =" + BUID + ") AND BankBranchID IN (SELECT BankBranchID FROM BankBranchDept WHERE OperationalDept  IN (" + sDeptIDs + ")) ) order by AccountNo");
        }

        #endregion
    }
}
