using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class GRNDetailDA
    {
        public GRNDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, GRNDetail oGRNDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sGRNDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GRNDetail]"
                                    + "%n, %n, %n, %s, %n, %n, %n, %n, %n, %n, %n, %n,%n, %s, %n, %n, %n, %n, %n,%s,%n, %n,%s, %n,%n,%n, %s,%s, %n, %n, %s",
                                    oGRNDetail.GRNDetailID, oGRNDetail.GRNID, oGRNDetail.ProductID, oGRNDetail.TechnicalSpecification, oGRNDetail.MUnitID, oGRNDetail.RefQty, oGRNDetail.ReceivedQty, oGRNDetail.UnitPrice, oGRNDetail.ItemWiseLandingCost, oGRNDetail.InvoiceLandingCost, oGRNDetail.LCLandingCost, oGRNDetail.Amount, oGRNDetail.LotID, oGRNDetail.LotNo, oGRNDetail.RefTypeInt, oGRNDetail.RefObjectID, oGRNDetail.StyleID, oGRNDetail.ColorID, oGRNDetail.SizeID, oGRNDetail.ProjectName, oGRNDetail.VehicleModelID, oGRNDetail.RejectQty, oGRNDetail.Remarks,oGRNDetail.WeightPerCartoon,oGRNDetail.ConePerCartoon,oGRNDetail.RackID, oGRNDetail.Shade, oGRNDetail.Stretch_Length, (int)eEnumDBOperation, nUserID, sGRNDetailIDs);

        }

        public static void Delete(TransactionContext tc, GRNDetail oGRNDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sGRNDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_GRNDetail]"
                                    + "%n, %n, %n, %s, %n, %n, %n, %n, %n, %n, %n, %n,%n, %s, %n, %n, %n, %n, %n,%s,%n, %n,%s, %n,%n,%n, %s,%s, %n, %n, %s",
                                    oGRNDetail.GRNDetailID, oGRNDetail.GRNID, oGRNDetail.ProductID, oGRNDetail.TechnicalSpecification, oGRNDetail.MUnitID, oGRNDetail.RefQty, oGRNDetail.ReceivedQty, oGRNDetail.UnitPrice, oGRNDetail.ItemWiseLandingCost, oGRNDetail.InvoiceLandingCost, oGRNDetail.LCLandingCost, oGRNDetail.Amount, oGRNDetail.LotID, oGRNDetail.LotNo, oGRNDetail.RefTypeInt, oGRNDetail.RefObjectID, oGRNDetail.StyleID, oGRNDetail.ColorID, oGRNDetail.SizeID, oGRNDetail.ProjectName, oGRNDetail.VehicleModelID, oGRNDetail.RejectQty, oGRNDetail.Remarks, oGRNDetail.WeightPerCartoon, oGRNDetail.ConePerCartoon, oGRNDetail.RackID, oGRNDetail.Shade, oGRNDetail.Stretch_Length, (int)eEnumDBOperation, nUserID, sGRNDetailIDs);

        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GRNDetail WHERE GRNDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_GRNDetail");
        }

        public static IDataReader Gets(TransactionContext tc, int nGRNID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GRNDetail WHERE GRNID =%n", nGRNID);
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsRpt_Product(TransactionContext tc, int BUID, int WorkingUnitID, int DateYear, int ReportLayout)
        {
            return tc.ExecuteReader("EXEC SP_RPT_GRN_Product %n,%n,%n,%n", BUID, WorkingUnitID, DateYear, ReportLayout);
        }
        #endregion

    }  
}
