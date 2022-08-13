using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class RecapBillOfMaterialDA
    {
        public RecapBillOfMaterialDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, RecapBillOfMaterial oRecapBillOfMaterial, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sRecapBillOfMaterialIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RecapBillOfMaterial]" + "%n, %n, %n, %n, %n, %s, %s, %s, %n, %n, %n, %n, %n,%n,%n,%n,%n, %s, %s, %n, %n, %s",
                                    oRecapBillOfMaterial.RecapBillOfMaterialID, oRecapBillOfMaterial.OrderRecapID, oRecapBillOfMaterial.ProductID, oRecapBillOfMaterial.ColorID, oRecapBillOfMaterial.SizeID, oRecapBillOfMaterial.ItemDescription, oRecapBillOfMaterial.Reference, oRecapBillOfMaterial.Construction, oRecapBillOfMaterial.Sequence, oRecapBillOfMaterial.MUnitID, oRecapBillOfMaterial.ReqQty, oRecapBillOfMaterial.CuttingQty, oRecapBillOfMaterial.ConsumptionQty, oRecapBillOfMaterial.OrderQty, oRecapBillOfMaterial.BookingQty, oRecapBillOfMaterial.BookingConsumption, oRecapBillOfMaterial.BookingConsumptionInPercent, oRecapBillOfMaterial.POCode, oRecapBillOfMaterial.Remarks, nUserID, (int)eEnumDBOperation, sRecapBillOfMaterialIDs);
        }


        public static void Delete(TransactionContext tc, RecapBillOfMaterial oRecapBillOfMaterial, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sRecapBillOfMaterialIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_RecapBillOfMaterial]" + "%n, %n, %n, %n, %n, %s, %s, %s, %n, %n, %n, %n, %n,%n,%n,%n,%n, %s, %s, %n, %n, %s",
                                    oRecapBillOfMaterial.RecapBillOfMaterialID, oRecapBillOfMaterial.OrderRecapID, oRecapBillOfMaterial.ProductID, oRecapBillOfMaterial.ColorID, oRecapBillOfMaterial.SizeID, oRecapBillOfMaterial.ItemDescription, oRecapBillOfMaterial.Reference, oRecapBillOfMaterial.Construction, oRecapBillOfMaterial.Sequence, oRecapBillOfMaterial.MUnitID, oRecapBillOfMaterial.ReqQty, oRecapBillOfMaterial.CuttingQty, oRecapBillOfMaterial.ConsumptionQty, oRecapBillOfMaterial.OrderQty, oRecapBillOfMaterial.BookingQty, oRecapBillOfMaterial.BookingConsumption, oRecapBillOfMaterial.BookingConsumptionInPercent, oRecapBillOfMaterial.POCode, oRecapBillOfMaterial.Remarks, nUserID, (int)eEnumDBOperation, sRecapBillOfMaterialIDs);
        }

        public static void UpdateImage(TransactionContext tc, byte[] attachfile, int nRecapBillOfMaterialID, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = attachfile;
            string sSQL = SQLParser.MakeSQL("UPDATE RecapBillOfMaterial SET Attachfile=%q" + " WHERE RecapBillOfMaterialID=%n", "@file", nRecapBillOfMaterialID);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }
        public static void UpDateSequence(TransactionContext TC, int id, int Sequence)
        {
            TC.ExecuteNonQuery("Update RecapBillOfMaterial SET Sequence = %n WHERE  RecapBillOfMaterialID = %n", Sequence, id);
        }
        public static void DeleteImage(TransactionContext TC, int nBMID)
        {
            TC.ExecuteNonQuery("Update RecapBillOfMaterial SET AttachFile = null WHERE RecapBillOfMaterialID = %n", nBMID);
        }
        public static void UpdateImageFromBM(TransactionContext TC, int nOldBMID, int nNewBMID)
        {
            TC.ExecuteNonQuery("Update RecapBillOfMaterial SET AttachFile = (SELECT BM.AttachFile FROM RecapBillOfMaterial AS BM WHERE BM.RecapBillOfMaterialID = %n) WHERE RecapBillOfMaterialID = %n", nOldBMID, nNewBMID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RecapBillOfMaterialWithoutAttachment WHERE RecapBillOfMaterialID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RecapBillOfMaterialWithoutAttachment");
        }

        public static IDataReader Gets_Report(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_RecapBillOfMaterialWithoutAttachment WHERE OrderRecapID=%n ORDER BY Sequence", id);

        }

        public static IDataReader Gets(TransactionContext tc, int nOrderRecapID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RecapBillOfMaterialWithoutAttachment WHERE OrderRecapID=%n ORDER BY Sequence", nOrderRecapID);
        }
        public static IDataReader GetsWithImage(TransactionContext tc, int nOrderRecapID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RecapBillOfMaterialWithAttachment WHERE OrderRecapID=%n ORDER BY Sequence", nOrderRecapID);
        }

        public static IDataReader GetWithImage(TransactionContext tc, int nRecapBillOfMaterialID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RecapBillOfMaterialWithAttachment WHERE RecapBillOfMaterialID = %n ", nRecapBillOfMaterialID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
