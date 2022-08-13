using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class BankDA
    {
        public BankDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Bank oBank, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Bank]"
                                    + "%n, %s, %s, %s, %s, %n, %n, %n",
                                    oBank.BankID, oBank.Code, oBank.Name, oBank.ShortName, oBank.FaxNo, oBank.ChequeSetupID, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, Bank oBank, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Bank]"
                                    + "%n, %s, %s, %s, %s, %n, %n, %n",
                                    oBank.BankID, oBank.Code, oBank.Name, oBank.ShortName, oBank.FaxNo, oBank.ChequeSetupID, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Bank WHERE BankID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Bank Order By [Name]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_Bank
        }
        public static IDataReader GetByCategory(TransactionContext tc, bool bCategory)
        {
            return tc.ExecuteReader("SELECT * FROM View_Bank WHERE Category=%b ORDER BY [Name] ", bCategory);
        }
        public static IDataReader GetByNegoBank(TransactionContext tc, int nBankID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Bank WHERE BankID IN (SELECT DISTINCT(NegotiationBankID) FROM EXPORTLC WHERE NegotiationBankID>0 ) ", nBankID);
        }
        #endregion
    }  
}
