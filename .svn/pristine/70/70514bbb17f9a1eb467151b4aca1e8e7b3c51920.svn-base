using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class BankPersonnelDA
    {
        public BankPersonnelDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BankPersonnel oBankPersonnel, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BankPersonnel]"
                                    + "%n, %n, %n, %s, %s, %s, %s, %s, %n, %n",
                                    oBankPersonnel.BankPersonnelID, oBankPersonnel.BankID, oBankPersonnel.BankBranchID, oBankPersonnel.Name, oBankPersonnel.Address, oBankPersonnel.Phone, oBankPersonnel.Email, oBankPersonnel.Note, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, BankPersonnel oBankPersonnel, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BankPersonnel]"
                                    + "%n, %n, %n, %s, %s, %s, %s, %s, %n, %n",
                                    oBankPersonnel.BankPersonnelID, oBankPersonnel.BankID, oBankPersonnel.BankBranchID, oBankPersonnel.Name, oBankPersonnel.Address, oBankPersonnel.Phone, oBankPersonnel.Email, oBankPersonnel.Note, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM BankPersonnel WHERE BankPersonnelID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM BankPersonnel");
        }
        public static IDataReader GetsByBank(TransactionContext tc, int nBankID)
        {
            return tc.ExecuteReader("SELECT * FROM BankPersonnel WHERE BankID=%n", nBankID);
        }
        public static IDataReader GetsByBankBranch(TransactionContext tc, int nBankBranchID)
        {
            return tc.ExecuteReader("SELECT * FROM BankPersonnel WHERE BankBranchID=%n", nBankBranchID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }    
}
