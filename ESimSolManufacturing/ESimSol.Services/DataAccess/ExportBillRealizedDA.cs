using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportBillRealizedDA
    {
        public ExportBillRealizedDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportBillRealized oExportBillRealized, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportBillRealized]" + "%n, %n, %n, %n, %n, %n, %n,%n, %n",
                                    oExportBillRealized.ExportBillRealizedID, oExportBillRealized.ExportBillID, oExportBillRealized.ExportBillParticularID, (int)oExportBillRealized.InOutType, oExportBillRealized.Amount, oExportBillRealized.CurrencyID,oExportBillRealized.CCRate, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, ExportBillRealized oExportBillRealized, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportBillRealized]" + "%n, %n, %n, %n, %n, %n, %n,%n, %n",
                                     oExportBillRealized.ExportBillRealizedID, oExportBillRealized.ExportBillID, oExportBillRealized.ExportBillParticularID, (int)oExportBillRealized.InOutType, oExportBillRealized.Amount, oExportBillRealized.CurrencyID, oExportBillRealized.CCRate, nUserID, (int)eEnumDBOperation);
        }
       
       
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBillRealized WHERE ExportBillRealizedID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc,int nEBillID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportBillRealized where ExportBillID=%n", nEBillID);
        }
           
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
