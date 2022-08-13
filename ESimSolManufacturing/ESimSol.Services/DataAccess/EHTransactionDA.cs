using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.DataAccess
{
    public class EHTransactionDA
    {
        public EHTransactionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, EHTransaction oEHTransaction, int nDBOperation, Int64 nUserID, string sEHTransactionIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EHTransaction]"
                                 + "%n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %s",
                                 oEHTransaction.EHTransactionID, oEHTransaction.AccountHeadID, oEHTransaction.ExpenditureTypeInt, oEHTransaction.RefObjectID, oEHTransaction.CurrencyID, oEHTransaction.Amount, oEHTransaction.CCRate, oEHTransaction.AmountBC, oEHTransaction.Remarks, nUserID, (int)nDBOperation, sEHTransactionIDs);
        }
        public static void Delete(TransactionContext tc, EHTransaction oEHTransaction, EnumDBOperation nDBOperation, Int64 nUserID, string sEHTransactionIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_EHTransaction]"
                                 + "%n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %s",
                                 oEHTransaction.EHTransactionID, oEHTransaction.AccountHeadID, oEHTransaction.ExpenditureTypeInt, oEHTransaction.RefObjectID, oEHTransaction.CurrencyID, oEHTransaction.Amount, oEHTransaction.CCRate, oEHTransaction.AmountBC, oEHTransaction.Remarks, nUserID, (int)nDBOperation, sEHTransactionIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(int nEHTID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EHTransaction WHERE EHTID=%n", nEHTID);
        }       
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EHTransaction");
        }
        public static IDataReader Gets(TransactionContext tc, int nRefObjectID, EnumExpenditureType eExpenditureType)
        {
            return tc.ExecuteReader("SELECT * FROM View_EHTransaction WHERE RefObjectID=%n and ExpenditureType=%n", nRefObjectID, (int)eExpenditureType);
        }     
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
