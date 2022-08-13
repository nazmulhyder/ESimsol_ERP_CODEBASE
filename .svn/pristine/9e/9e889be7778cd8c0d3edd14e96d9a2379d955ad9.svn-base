using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SalesComPaymentDetailDA
    {
        public SalesComPaymentDetailDA() { }
        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, SalesComPaymentDetail oSalesComPaymentDetail, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SalesComPaymentDetail] " + "%n , %n ,%n , %n , %n,  %n , %n , %s , %n, %n ,%n"
                , oSalesComPaymentDetail.SalesComPaymentDetailID, oSalesComPaymentDetail.SalesComPaymentID, oSalesComPaymentDetail.SalesCommissionPayableID, oSalesComPaymentDetail.Amount, oSalesComPaymentDetail.AmountBC, oSalesComPaymentDetail.PayableAmountBC, oSalesComPaymentDetail.ActualPayable, oSalesComPaymentDetail.Note, oSalesComPaymentDetail.AdjDeduct, nUserID, nDBOperation);
                                   
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nSalesComPaymentDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SalesComPaymentDetail WHERE SalesComPaymentDetailID=%n", nSalesComPaymentDetailID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
