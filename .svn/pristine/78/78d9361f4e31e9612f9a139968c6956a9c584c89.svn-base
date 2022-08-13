using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class BankBranchDA
    {
        public BankBranchDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BankBranch oBankBranch, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BankBranch]"
                                    + "%n, %n, %s, %s, %s, %s, %s, %s, %s, %b, %b, %n, %n",
                                     oBankBranch.BankBranchID,oBankBranch.BankID,oBankBranch.BranchCode,oBankBranch.BranchName,oBankBranch.Address,oBankBranch.SwiftCode,oBankBranch.PhoneNo,oBankBranch.FaxNo,oBankBranch.Note,oBankBranch.IsOwnBank,oBankBranch.IsActive,(int)eEnumDBOperation,nUserId);
        }

        public static void Delete(TransactionContext tc, BankBranch oBankBranch, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BankBranch]"
                                    + "%n, %n, %s, %s, %s, %s, %s, %s, %s, %b, %b, %n, %n",
                                     oBankBranch.BankBranchID, oBankBranch.BankID, oBankBranch.BranchCode, oBankBranch.BranchName, oBankBranch.Address, oBankBranch.SwiftCode, oBankBranch.PhoneNo, oBankBranch.FaxNo, oBankBranch.Note, oBankBranch.IsOwnBank, oBankBranch.IsActive, (int)eEnumDBOperation, nUserId);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankBranch WHERE BankBranchID=%n", nID);
        }
        public static IDataReader GetsByBank(TransactionContext tc, int nBankID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankBranch WHERE BankID=%n", nBankID);
        }
        public static IDataReader GetsByDeptAndBU(TransactionContext tc, string sDeptIDs, int BUID, string sBankName)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankBranch WHERE BankBranchID IN (SELECT BankBranchID FROM BankBranchBU WHERE BUID  = " + BUID + ") AND BankBranchID IN (SELECT BankBranchID FROM BankBranchDept WHERE OperationalDept  IN (" + sDeptIDs + ")) AND BankName Like '%" + sBankName + "%' order by BankName,BranchName");
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankBranch Order BY BankName,BranchName");
        }
        public static IDataReader GetsOwnBranchs(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankBranch WHERE IsOwnBank=1 ORDER BY BankName, BranchName");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}