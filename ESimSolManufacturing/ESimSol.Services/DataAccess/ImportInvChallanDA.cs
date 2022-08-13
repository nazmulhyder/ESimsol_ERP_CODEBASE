using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportInvChallanDA
    {
        public ImportInvChallanDA() { }

        #region 

        #region Insert, Update, Delete

        public static IDataReader InsertUpdate(TransactionContext tc, ImportInvChallan oImportInvChallan, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportInvChallan]"
                                    + "%n,%s,%D,%s,%s,%s,%s,%n,%n",
                                    oImportInvChallan.ImportInvChallanID,
                                    oImportInvChallan.ChallanNo,
                                    oImportInvChallan.ChallanDate,
                                    oImportInvChallan.DriverName,
                                    oImportInvChallan.VehicleInfo,
                                    oImportInvChallan.CotractNo,
                                    oImportInvChallan.Note,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }
        public static void Delete(TransactionContext tc, ImportInvChallan oImportInvChallan, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportInvChallan]"
                                    + "%n,%s,%D,%s,%s,%s,%s,%n,%n",
                                    oImportInvChallan.ImportInvChallanID,
                                    oImportInvChallan.ChallanNo,
                                    oImportInvChallan.ChallanDate,
                                    oImportInvChallan.DriverName,
                                    oImportInvChallan.VehicleInfo,
                                    oImportInvChallan.CotractNo,
                                    oImportInvChallan.Note,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }
       
       

      

        #endregion



        #region Get & Exist Function


        public static IDataReader Get(int nImportInvChallanID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_ImportInvChallan where ImportInvChallanID=%n", nImportInvChallanID);
        }

        public static IDataReader GetByInvoice(int nImportInvoiceID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_ImportInvChallan where ImportInvoiceID=%n", nImportInvoiceID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportInvChallan where IsGRN=0");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
        #endregion
    }

}
