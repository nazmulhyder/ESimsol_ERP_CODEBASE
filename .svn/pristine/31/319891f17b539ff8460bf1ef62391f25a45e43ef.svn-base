using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SampleInvoiceDetailDA
    {

        public SampleInvoiceDetailDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, SampleInvoiceDetail oSampleInvoiceDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SampleInvoiceDetail]"
                                    + "%n, %n, %n,%n, %n,%n, %n, %s,%n, %n",
                                    oSampleInvoiceDetail.SampleInvoiceDetailID, oSampleInvoiceDetail.SampleInvoiceID, oSampleInvoiceDetail.DyeingOrderID,oSampleInvoiceDetail.ProductID,  oSampleInvoiceDetail.Qty,oSampleInvoiceDetail.UnitPrice, oSampleInvoiceDetail.Amount,  oSampleInvoiceDetail.Description,  nUserId, (int)eEnumDBOperation );
        }
        public static IDataReader InsertUpdate_Adj(TransactionContext tc, SampleInvoiceDetail oSampleInvoiceDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SampleInvoiceDetail_Adj]"
                                    + "%n, %n, %n, %n,%n, %n, %s,%n, %n",
                                    oSampleInvoiceDetail.SampleInvoiceDetailID, oSampleInvoiceDetail.SampleInvoiceID, oSampleInvoiceDetail.ProductID, oSampleInvoiceDetail.Qty, oSampleInvoiceDetail.UnitPrice, oSampleInvoiceDetail.Amount, oSampleInvoiceDetail.Description, nUserId, (int)eEnumDBOperation);
        }

        #endregion
        

        #region Delete Function
        public static void Delete(TransactionContext tc, SampleInvoiceDetail oSampleInvoiceDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SampleInvoiceDetail]"
                                    + "%n, %n, %n,%n, %n,%n, %n, %s,%n, %n",
                                    oSampleInvoiceDetail.SampleInvoiceDetailID, oSampleInvoiceDetail.SampleInvoiceID, oSampleInvoiceDetail.DyeingOrderID,oSampleInvoiceDetail.ProductID, oSampleInvoiceDetail.Qty, oSampleInvoiceDetail.UnitPrice, oSampleInvoiceDetail.Amount, oSampleInvoiceDetail.Description, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete_Adj(TransactionContext tc, SampleInvoiceDetail oSampleInvoiceDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SampleInvoiceDetail_Adj]"
                                    + "%n, %n, %n, %n,%n, %n, %s,%n, %n",
                                    oSampleInvoiceDetail.SampleInvoiceDetailID, oSampleInvoiceDetail.SampleInvoiceID, oSampleInvoiceDetail.ProductID, oSampleInvoiceDetail.Qty, oSampleInvoiceDetail.UnitPrice, oSampleInvoiceDetail.Amount, oSampleInvoiceDetail.Description, nUserId, (int)eEnumDBOperation);
        }
        public static void UpdateInvoiceID(TransactionContext tc, int nSampleInvoiceID,int nDOID)
        {
            string sSQL1 = SQLParser.MakeSQL("Update DyeingOrder Set SampleInvoiceID=%n Where DyeingOrderID>0 and DyeingOrderID=%n", nSampleInvoiceID, nDOID);

            tc.ExecuteNonQuery(sSQL1);
            
        }
        public static IDataReader InsertUpdate_AddDO(TransactionContext tc, SampleInvoiceDetail oSampleInvoiceDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SampleInvoiceDetail_AddDyeingOrder]"
                                    + "%n, %n, %n,%n",
                                    oSampleInvoiceDetail.SampleInvoiceID,  oSampleInvoiceDetail.DyeingOrderID, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Generation Function
    
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleInvoiceDetail WHERE SampleInvoiceDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleInvoiceDetail");
        }
        public static IDataReader Gets(TransactionContext tc, int nPaymentContractID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleInvoiceDetail WHERE  SampleInvoiceID=%n order by OrderNo,ProductName", nPaymentContractID);
        }
        public static IDataReader Gets(TransactionContext tc, string sql)
        {
            return tc.ExecuteReader(sql);
        }
        public static IDataReader GetsBy(TransactionContext tc, string sPCIDs,string nPIID)
        {
            return tc.ExecuteReader("EXEC [sp_PaymentContractDetail]" + "%s,%s", sPCIDs, nPIID);

        }
        #endregion
    }
}
