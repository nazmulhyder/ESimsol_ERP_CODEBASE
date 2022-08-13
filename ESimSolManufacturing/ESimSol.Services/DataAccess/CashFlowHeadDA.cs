using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
namespace ESimSol.Services.DataAccess
{

    public class CashFlowHeadDA
    {
        public CashFlowHeadDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CashFlowHead oCashFlowHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CashFlowHead]" + "%n, %n, %s, %b, %s, %n, %n, %n",
                                    oCashFlowHead.CashFlowHeadID, oCashFlowHead.CashFlowHeadTypeInt, oCashFlowHead.DisplayCaption, oCashFlowHead.IsDebit, oCashFlowHead.Remarks, oCashFlowHead.Sequence, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, CashFlowHead oCashFlowHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CashFlowHead]" + "%n, %n, %s, %b, %s, %n, %n, %n",
                                    oCashFlowHead.CashFlowHeadID, oCashFlowHead.CashFlowHeadTypeInt, oCashFlowHead.DisplayCaption, oCashFlowHead.IsDebit, oCashFlowHead.Remarks, oCashFlowHead.Sequence, nUserID, (int)eEnumDBOperation);
        }
        public static void UpdateScequence(TransactionContext tc, CashFlowHead oCashFlowHead)
        {
            tc.ExecuteNonQuery("UPDATE CashFlowHead SET Sequence = %n WHERE CashFlowHeadID=%n", oCashFlowHead.Sequence, oCashFlowHead.CashFlowHeadID);
        }                          
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM CashFlowHead WHERE CashFlowHeadID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CashFlowHead AS HH ORDER BY HH.Sequence ASC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}
