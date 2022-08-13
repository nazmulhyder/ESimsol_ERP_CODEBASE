using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportBLDA
    {
        public ImportBLDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, ImportBL oImportBL, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportBL]" + "%n, %n, %s, %d,%d, %n,%n, %n, %n, %n,%d, %n,%d, %s, %n, %n, %n",
                                            oImportBL.ImportBLID, oImportBL.BLType, oImportBL.BLNo, oImportBL.BLDate, oImportBL.ETA, oImportBL.BLQuantity, oImportBL.ShippingLine, oImportBL.LandingPort, oImportBL.DestinationPort, oImportBL.PlaceOfIssue, oImportBL.IssueDate, oImportBL.ContainerCount, oImportBL.ShipmentDate, oImportBL.VesselInfo, oImportBL.ImportInvoiceID, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, ImportBL oImportBL, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportBL]" + "%n, %n, %s, %d,%d, %n,%n, %n, %n, %n,%d, %n,%d, %s, %n, %n, %n",
                                    oImportBL.ImportBLID, oImportBL.BLType, oImportBL.BLNo, oImportBL.BLDate, oImportBL.ETA, oImportBL.BLQuantity, oImportBL.ShippingLine, oImportBL.LandingPort, oImportBL.DestinationPort, oImportBL.PlaceOfIssue, oImportBL.IssueDate, oImportBL.ContainerCount, oImportBL.ShipmentDate, oImportBL.VesselInfo, oImportBL.ImportInvoiceID,  nUserID, (int)eEnumDBOperation);
        }

      
        #endregion

        #region Delete Function
        //public static void Delete(TransactionContext tc, int nID)
        //{
        //    tc.ExecuteNonQuery("DELETE FROM ImportBL WHERE ImportBLID=%n", nID);
        //}
        //public static void DeleteWithInvoice(TransactionContext tc, int nID)
        //{
        //    tc.ExecuteNonQuery("DELETE FROM ImportBL WHERE ImportInvoiceID=%n", nID);
        //}
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("ImportBL", "ImportBLID");
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportBL WHERE ImportBLID=%n", nID);
        }
        public static IDataReader GetByInvoiceID(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportBL WHERE ImportInvoiceID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportBL Order By [BLNo]");
        }
     
        public static bool IsExist(TransactionContext tc, int nImportBLID, string sBLNo)
        {
            object objImportBLID = tc.ExecuteScalar("Select ImportBLID from View_ImportBL where [BLNo]=%s", sBLNo);
            int nDBContractorID = 0;
            if (DBNull.Value == objImportBLID || objImportBLID == null) return false;
            else nDBContractorID = Convert.ToInt32(objImportBLID);

            return (nDBContractorID != nImportBLID);
        }
        #endregion
    }
}
