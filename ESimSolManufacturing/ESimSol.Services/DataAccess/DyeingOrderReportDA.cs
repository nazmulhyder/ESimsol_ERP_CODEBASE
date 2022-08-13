using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DyeingOrderReportDA
    {
        public DyeingOrderReportDA() { }

        #region Generation Function

        #endregion

        #region Get & Exist Function
     
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, int nSampleInvoiceID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DyeingOrderReport WHERE SampleInvoiceID=%n and PaymentType  in (" + (int)EnumOrderPaymentType.AdjWithNextLC+","+(int)EnumOrderPaymentType.AdjWithPI+","+(int)EnumOrderPaymentType.CashOrCheque+") order by DyeingOrderID,SL,DyeingOrderDetailID", nSampleInvoiceID);
        }
        #endregion

    }
}