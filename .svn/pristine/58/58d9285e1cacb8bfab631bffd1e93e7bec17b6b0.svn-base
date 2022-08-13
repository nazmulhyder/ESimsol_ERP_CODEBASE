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
    public class ExportBillDetailDA
    {
        public ExportBillDetailDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportBillDetail oEBillDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportBillDetail]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n,%n,%n",
                                    oEBillDetail.ExportBillDetailID, oEBillDetail.ExportBillID, oEBillDetail.ProductID, oEBillDetail.Qty, oEBillDetail.UnitPrice, oEBillDetail.NoOfBag, oEBillDetail.WtPerBag, oEBillDetail.ExportPIDetailID, oEBillDetail.MUnitID, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, ExportBillDetail oEBillDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportBillDetail]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n,%n,%n",
                                   oEBillDetail.ExportBillDetailID, oEBillDetail.ExportBillID, oEBillDetail.ProductID, oEBillDetail.Qty, oEBillDetail.UnitPrice, oEBillDetail.NoOfBag, oEBillDetail.WtPerBag, oEBillDetail.ExportPIDetailID, oEBillDetail.MUnitID, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM ExportBillDetail WHERE ExportBillDetailID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("ExportBillDetail", "ExportBillDetailID");
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBillDetail WHERE ExportBillDetailID=%n", nID);
        }
        public static IDataReader GetPIP(TransactionContext tc, int nPIProductID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBillDetail WHERE PIProductID=%n", nPIProductID);
        }
        public static IDataReader Gets(TransactionContext tc,string sExportLCIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBillDetail where LCBillID in(Select LCBill.LCBillID From LCBill where ExportLCID in (%q))", sExportLCIDs);
        }

        public static IDataReader Gets(TransactionContext tc, int nExportBillID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBillDetail WHERE ExportBillID=%n", nExportBillID);
        }
        public static IDataReader Gets_GroupBy(TransactionContext tc, int nExportBillID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBillDetailForDoc WHERE ExportBillID=%n", nExportBillID);
        }
      

        public static IDataReader GetsBySQL(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion

    }
}
