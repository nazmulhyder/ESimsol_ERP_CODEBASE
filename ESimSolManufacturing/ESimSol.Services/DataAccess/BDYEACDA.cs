using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class BDYEACDA
    {
        public BDYEACDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BDYEAC oBDYEAC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BDYEAC]"
                                    + "%n, %n, %n, %s, %s, %s, %s, %d, %d, %s, %s ,%d,%n, %n, %n",
                                    oBDYEAC.BDYEACID, oBDYEAC.ExportLCID, oBDYEAC.ExportBillID, oBDYEAC.MasterLCNos, oBDYEAC.MasterLCDates, oBDYEAC.GarmentsQty,  oBDYEAC.BankName, oBDYEAC.InvoiceDate, oBDYEAC.DeliveryDate,oBDYEAC.SupplierName,oBDYEAC.ImportLCNo,oBDYEAC.ImportLCDate, oBDYEAC.Amount, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, BDYEAC oBDYEAC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BDYEAC]"
                                    + "%n, %n, %n, %s, %s, %s, %s, %d, %d, %s, %s ,%d,%n, %n, %n",
                                    oBDYEAC.BDYEACID, oBDYEAC.ExportLCID, oBDYEAC.ExportBillID, oBDYEAC.MasterLCNos, oBDYEAC.MasterLCDates, oBDYEAC.GarmentsQty, oBDYEAC.BankName, oBDYEAC.InvoiceDate, oBDYEAC.DeliveryDate, oBDYEAC.SupplierName, oBDYEAC.ImportLCNo, oBDYEAC.ImportLCDate, oBDYEAC.Amount, (int)eEnumDBOperation, nUserID);
        }

        
        public static void CreatePrint(TransactionContext tc, BDYEAC oBDYEAC)
        {
            tc.ExecuteNonQuery("Update BDYEAC SET IsPrint = %b WHERE BDYEACID = %n",oBDYEAC.IsPrint, oBDYEAC.BDYEACID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BDYEAC WHERE BDYEACID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BDYEAC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_BDYEAC
        }

       
        #endregion
    }  
}
