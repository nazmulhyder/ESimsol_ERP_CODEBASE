using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportPackDA
    {
        public ImportPackDA() { }

        #region write By Mohammad Mahabub by 27 sept 2016

        #region Insert, Update, Delete

        public static IDataReader InsertUpdate(TransactionContext tc, ImportPack oImportPack, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportPack]"
                                    + "%n,%n,%s,%d,%n,%n,%n,%n,%s,%s,%s,%s,%n,%n,%n,%n,%n",
                                    oImportPack.ImportPackID,
                                    oImportPack.ImportInvoiceID,
                                    oImportPack.PackNo,
                                    oImportPack.PackDate,
                                    oImportPack.PackCountBy,
                                    oImportPack.TotalPack,
                                    oImportPack.NetWeight,
                                    oImportPack.GrossWeight,
                                    oImportPack.Origin,
                                    oImportPack.Brand,
                                    oImportPack.Remarks,
                                    oImportPack.LotNo,
                                    oImportPack.ProductID,
                                    oImportPack.UnitPrice,
                                    oImportPack.ParentPackID,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }
        public static void Delete(TransactionContext tc, ImportPack oImportPack, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportPack]"
                                    + "%n,%n,%s,%d,%n,%n,%n,%n,%s,%s,%s,%s,%n,%n,%n,%n,%n",
                                    oImportPack.ImportPackID,
                                    oImportPack.ImportInvoiceID,
                                    oImportPack.PackNo,
                                    oImportPack.PackDate,
                                    oImportPack.PackCountBy,
                                    oImportPack.TotalPack,
                                    oImportPack.NetWeight,
                                    oImportPack.GrossWeight,
                                    oImportPack.Origin,
                                    oImportPack.Brand,
                                    oImportPack.Remarks,
                                    oImportPack.LotNo,
                                    oImportPack.ProductID,
                                    oImportPack.UnitPrice,
                                    oImportPack.ParentPackID,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }
        public static void Save_FromDO(TransactionContext tc, ImportPack oImportPack, EnumDBOperation eENumDBPurchaseInvoice, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportPack_FromDO]"
                                    + "%n,%n,%s,%d,%s,%s,%n,%n,%n,%n,%n,%n",
                                    oImportPack.ImportPackID,
                                    oImportPack.ImportInvoiceID,
                                    oImportPack.PackNo,
                                    oImportPack.PackDate,
                                    oImportPack.Remarks,
                                    oImportPack.LotNo,
                                     oImportPack.PackCountBy,
                                    oImportPack.ProductID,
                                    oImportPack.UnitPrice,
                                    oImportPack.ParentPackID,
                                    nUserId,
                                    (int)eENumDBPurchaseInvoice);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(int nImportPackID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_ImportPack where ImportPackID=%n", nImportPackID);
        }

        public static IDataReader GetByInvoice(int nImportInvoiceID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_ImportPack where ImportInvoiceID=%n", nImportInvoiceID);
        }

        public static IDataReader Gets(TransactionContext tc, int nImportInvoiceID)
        {
            return tc.ExecuteReader("select * from View_ImportPack where ImportInvoiceID=%n", nImportInvoiceID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
        #endregion
    }

}
