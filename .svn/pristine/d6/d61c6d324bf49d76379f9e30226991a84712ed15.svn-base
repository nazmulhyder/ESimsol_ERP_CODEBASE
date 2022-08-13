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
    public class BillOfMaterialDA
    {
        public BillOfMaterialDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BillOfMaterial oBillOfMaterial, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sBillOfMaterialIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BillOfMaterial]" + "%n, %n, %n, %n, %n, %s, %s, %s, %n, %n, %n, %n, %n,%n,%n,%n,%n, %s, %s, %n, %n, %s",
                                    oBillOfMaterial.BillOfMaterialID, oBillOfMaterial.TechnicalSheetID, oBillOfMaterial.ProductID, oBillOfMaterial.ColorID, oBillOfMaterial.SizeID, oBillOfMaterial.ItemDescription, oBillOfMaterial.Reference, oBillOfMaterial.Construction, oBillOfMaterial.Sequence, oBillOfMaterial.MUnitID, oBillOfMaterial.ReqQty, oBillOfMaterial.CuttingQty, oBillOfMaterial.ConsumptionQty, oBillOfMaterial.OrderQty, oBillOfMaterial.BookingQty, oBillOfMaterial.BookingConsumption, oBillOfMaterial.BookingConsumptionInPercent, oBillOfMaterial.POCode, oBillOfMaterial.Remarks, nUserID, (int)eEnumDBOperation, sBillOfMaterialIDs);
        }


        public static void Delete(TransactionContext tc, BillOfMaterial oBillOfMaterial, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sBillOfMaterialIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BillOfMaterial]" + "%n, %n, %n, %n, %n, %s, %s, %s, %n, %n, %n, %n, %n,%n,%n,%n,%n, %s, %s, %n, %n, %s",
                                    oBillOfMaterial.BillOfMaterialID, oBillOfMaterial.TechnicalSheetID, oBillOfMaterial.ProductID, oBillOfMaterial.ColorID, oBillOfMaterial.SizeID, oBillOfMaterial.ItemDescription, oBillOfMaterial.Reference, oBillOfMaterial.Construction, oBillOfMaterial.Sequence, oBillOfMaterial.MUnitID, oBillOfMaterial.ReqQty, oBillOfMaterial.CuttingQty, oBillOfMaterial.ConsumptionQty, oBillOfMaterial.OrderQty, oBillOfMaterial.BookingQty, oBillOfMaterial.BookingConsumption, oBillOfMaterial.BookingConsumptionInPercent, oBillOfMaterial.POCode, oBillOfMaterial.Remarks, nUserID, (int)eEnumDBOperation, sBillOfMaterialIDs);
        }




        public static void UpdateImage(TransactionContext tc, byte[] attachfile,int nBillOfMaterialID, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = attachfile;
            string sSQL = SQLParser.MakeSQL("UPDATE BillOfMaterial SET Attachfile=%q" + " WHERE BillOfMaterialID=%n", "@file", nBillOfMaterialID);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }
        public static void UpDateSequence (TransactionContext TC, int id , int Sequence)
        {
            TC.ExecuteNonQuery("Update BillOfMaterial SET Sequence = %n WHERE  BillOfMaterialID = %n", Sequence, id);
        }
        public static void DeleteImage(TransactionContext TC, int nBMID)
        {
            TC.ExecuteNonQuery("Update BillOfMaterial SET AttachFile = null WHERE BillOfMaterialID = %n", nBMID);
        }
        public static void UpdateImageFromBM(TransactionContext TC, int nOldBMID, int nNewBMID)
        {
            TC.ExecuteNonQuery("Update BillOfMaterial SET AttachFile = (SELECT BM.AttachFile FROM BillOfMaterial AS BM WHERE BM.BillOfMaterialID = %n) WHERE BillOfMaterialID = %n", nOldBMID, nNewBMID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BillOfMaterialWithoutAttachment WHERE BillOfMaterialID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BillOfMaterialWithoutAttachment");
        }

        public static IDataReader Gets_Report(TransactionContext tc, int id)
        {
           // return tc.ExecuteReader("SELECT * FROM BillOfMaterial WHERE TechnicalSheetID=%n", id);
            return tc.ExecuteReader("SELECT * FROM View_BillOfMaterialWithoutAttachment WHERE TechnicalSheetID=%n ORDER BY Sequence", id);
  
        }

        public static IDataReader Gets(TransactionContext tc, int nTechnicalSheetID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BillOfMaterialWithoutAttachment WHERE TechnicalSheetID=%n ORDER BY Sequence", nTechnicalSheetID);
        }
        public static IDataReader GetsWithImage(TransactionContext tc, int nTechnicalSheetID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BillOfMaterialWithAttachment WHERE TechnicalSheetID=%n ORDER BY Sequence", nTechnicalSheetID);
        }

        public static IDataReader GetWithImage(TransactionContext tc, int nBillOfMaterialID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BillOfMaterialWithAttachment WHERE BillOfMaterialID = %n ", nBillOfMaterialID);
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
