using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class KnittingOrderDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, KnittingOrder oKnittingOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_KnittingOrder]"
                                   + "%n,%n,%n,     %s,%d,%n,       %d,%d,%n,       %n,%n,%s,       %s,%n,%n,%n",
                                   oKnittingOrder.KnittingOrderID, oKnittingOrder.BUID, oKnittingOrder.BusinessSessionID, 
                                   oKnittingOrder.OrderNo, oKnittingOrder.OrderDate, oKnittingOrder.FactoryID, 
                                   oKnittingOrder.StartDate, oKnittingOrder.ApproxCompleteDate, oKnittingOrder.CurrencyID, 
                                   oKnittingOrder.Amount, oKnittingOrder.IssueQty, oKnittingOrder.Remarks,
                                   oKnittingOrder.KnittingInstruction, nUserID, (int)eEnumDBOperation,(int) oKnittingOrder.OrderType);
        }

        public static void Delete(TransactionContext tc, KnittingOrder oKnittingOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_KnittingOrder]"
                                   + "%n,%n,%n,     %s,%d,%n,       %d,%d,%n,       %n,%n,%s,       %s,%n,%n,%n",
                                   oKnittingOrder.KnittingOrderID, oKnittingOrder.BUID, oKnittingOrder.BusinessSessionID,
                                   oKnittingOrder.OrderNo, oKnittingOrder.OrderDate, oKnittingOrder.FactoryID,
                                   oKnittingOrder.StartDate, oKnittingOrder.ApproxCompleteDate, oKnittingOrder.CurrencyID,
                                   oKnittingOrder.Amount, oKnittingOrder.IssueQty, oKnittingOrder.Remarks,
                                   oKnittingOrder.KnittingInstruction, nUserID, (int)eEnumDBOperation, (int)oKnittingOrder.OrderType);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingOrder WHERE KnittingOrderID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnittingOrder ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
