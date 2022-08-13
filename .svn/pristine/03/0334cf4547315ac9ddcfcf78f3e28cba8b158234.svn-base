using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricSalesContractDetailDA
    {
        public FabricSalesContractDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricSalesContractDetail oFSCDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID,string sIDs )
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricSalesContractDetail]" + "%n, %n, %n, %n, %n, %n, %n, %s,%s,%s, %s,%s,  %s,%n,%n,%n,%n, %s,%s,%s,%b,%n,%n,%n,%s,%s,%n,%n,%s,%b,%s,%n,%n,%s",
                                    oFSCDetail.FabricSalesContractDetailID, oFSCDetail.FabricSalesContractID, oFSCDetail.FabricID, oFSCDetail.ProductID, oFSCDetail.Qty, oFSCDetail.MUnitID, oFSCDetail.UnitPrice, oFSCDetail.StyleNo, oFSCDetail.BuyerReference, oFSCDetail.ColorInfo, oFSCDetail.Size, oFSCDetail.Note, oFSCDetail.HLReference, oFSCDetail.FabricWeave, oFSCDetail.FinishType, oFSCDetail.ProcessType, oFSCDetail.FabricDesignID, oFSCDetail.DesignPattern, oFSCDetail.Construction, oFSCDetail.FabricWidth, oFSCDetail.IsProduction, oFSCDetail.ExportPIDetailID, oFSCDetail.FNLabdipDetailID, oFSCDetail.ShadeID, oFSCDetail.Shrinkage, oFSCDetail.Weight, oFSCDetail.SLNo, (int)oFSCDetail.SCDetailTypeInt, oFSCDetail.PantonNo, oFSCDetail.IsBWash, oFSCDetail.YarnType,nUserID, (int)eEnumDBOperation, sIDs);
        }
        public static void Delete(TransactionContext tc, FabricSalesContractDetail oFSCDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricSalesContractDetail]" + "%n, %n, %n, %n, %n, %n, %n, %s,%s,%s, %s,%s, %s,%n,%n,%n,%n, %s,%s,%s,%b,%n,%n,%n,%s,%s,%n,%n,%s,%b, %s, %n,%n,%s",
                                         oFSCDetail.FabricSalesContractDetailID, oFSCDetail.FabricSalesContractID, oFSCDetail.FabricID, oFSCDetail.ProductID, oFSCDetail.Qty, oFSCDetail.MUnitID, oFSCDetail.UnitPrice, oFSCDetail.StyleNo, oFSCDetail.BuyerReference, oFSCDetail.ColorInfo, oFSCDetail.Size, oFSCDetail.Note, oFSCDetail.HLReference, oFSCDetail.FabricWeave, oFSCDetail.FinishType, oFSCDetail.ProcessType, oFSCDetail.FabricDesignID, oFSCDetail.DesignPattern, oFSCDetail.Construction, oFSCDetail.FabricWidth, oFSCDetail.IsProduction, oFSCDetail.ExportPIDetailID, oFSCDetail.FNLabdipDetailID, oFSCDetail.ShadeID, oFSCDetail.Shrinkage, oFSCDetail.Weight, oFSCDetail.SLNo, (int)oFSCDetail.SCDetailTypeInt, oFSCDetail.PantonNo, oFSCDetail.IsBWash, oFSCDetail.YarnType, nUserID, (int)eEnumDBOperation, sIDs);
        }

        public static IDataReader Update_Revise(TransactionContext tc, FabricSalesContractDetail oFSCDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricSalesContractDetailLog]" + "%n, %n, %n, %n, %n, %n, %n, %s,%s,%s, %s,%s,  %s,%n,%n,%n,%n, %s,%s,%s,%b,%n,%n,%n,%s,%s,%n,%n, %b,  %n,%n,%s",
                                    oFSCDetail.FabricSalesContractDetailID, oFSCDetail.FabricSalesContractID, oFSCDetail.FabricID, oFSCDetail.ProductID, oFSCDetail.Qty, oFSCDetail.MUnitID, oFSCDetail.UnitPrice, oFSCDetail.StyleNo, oFSCDetail.BuyerReference, oFSCDetail.ColorInfo, oFSCDetail.Size, oFSCDetail.Note, oFSCDetail.HLReference, oFSCDetail.FabricWeave, oFSCDetail.FinishType, oFSCDetail.ProcessType, oFSCDetail.FabricDesignID, oFSCDetail.DesignPattern, oFSCDetail.Construction, oFSCDetail.FabricWidth, oFSCDetail.IsProduction, oFSCDetail.ExportPIDetailID, oFSCDetail.FNLabdipDetailID, oFSCDetail.ShadeID, oFSCDetail.Shrinkage, oFSCDetail.Weight, oFSCDetail.SLNo, (int)oFSCDetail.SCDetailTypeInt, true, nUserID, (int)eEnumDBOperation, sIDs);
        }
        public static IDataReader InsertUpdateExtra(TransactionContext tc, FabricSalesContractDetail oFSCDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricSalesContractDetail_Extra]" + "%n, %n, %n, %n",
                                    oFSCDetail.FabricSalesContractDetailID, oFSCDetail.FabricSalesContractID,  nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader SetHandLoomNo(TransactionContext tc, int nFSCDID, string sExe, string sOptionNo)
        {
            return tc.ExecuteReader("EXEC [SP_SetHandLoomNo]" + "%n, %s, %s", nFSCDID, sExe, sOptionNo);
        }
       
     
        public static IDataReader OrderHold(TransactionContext tc, FabricSalesContractDetail oFSCDetail, EnumDBOperation eEnumDBOperation,Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricSCHistory]" + "%n, %n, %n, %n, %s, %n,%n",
                                   oFSCDetail.FabricSalesContractID, oFSCDetail.FabricSalesContractDetailID, (int)EnumPOState.None, (int)oFSCDetail.Status, oFSCDetail.Note, nUserId, (int)EnumDBOperation.Insert);

        }
     
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSalesContractDetail WHERE FabricSalesContractDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSalesContractDetail");
        }
        public static IDataReader Gets(TransactionContext tc, int nFabricSalesContractID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSalesContractDetail WHERE isnull(SCDetailType,0) in (" + (int)EnumSCDetailType.None + "," + (int)EnumSCDetailType.AddCharge + "," + (int)EnumSCDetailType.DeductCharge + ") and  FabricSalesContractID=%n order by  Convert(int, dbo.SplitedStringGet(FabricNum,'-',0)) ASC,SCDetailType", nFabricSalesContractID);
        }
    
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsLog(TransactionContext tc, int nPIID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_FabricSalesContractDetailLog] WHERE FabricSalesContractLogID=%n", nPIID);
        }
      


        //public static IDataReader Save_UpdateDispoNo(TransactionContext tc, FabricSalesContractDetail oFabricSalesContractDetail)
        //{
        //    string sSQL1 = SQLParser.MakeSQL("Update FabricSalesContractDetail set ExeNo=%s where FabricSalesContractDetailID=%n",oFabricSalesContractDetail.ExeNo, oFabricSalesContractDetail.FabricSalesContractDetailID);
        //    tc.ExecuteNonQuery(sSQL1);
        //    return tc.ExecuteReader("SELECT * FROM View_FabricSalesContractDetail WHERE FabricSalesContractDetailID=%n", oFabricSalesContractDetail.FabricSalesContractDetailID);

        //}
        public static void Save_UpdateDispoNo(TransactionContext tc, FabricSalesContractDetail oFabricSalesContractDetail)
        {
            tc.ExecuteNonQuery("Update FabricSalesContractDetail set ExeNo=%s where FabricSalesContractDetailID=%n", oFabricSalesContractDetail.ExeNo, oFabricSalesContractDetail.FabricSalesContractDetailID);
        }
        public static void Save_SLNo(TransactionContext tc, FabricSalesContractDetail oFabricSalesContractDetail)
        {
            tc.ExecuteNonQuery("Update FabricSalesContractDetail set SLNo=%s where FabricSalesContractDetailID=%n", oFabricSalesContractDetail.SLNo, oFabricSalesContractDetail.FabricSalesContractDetailID);
        }
        public static IDataReader SaveLDNo(TransactionContext tc, FabricSalesContractDetail oFabricSalesContractDetail)
        {
            //tc.ExecuteNonQuery("DELETE FROM FNRecipe WHERE FSCDID = " + oFabricSalesContractDetail.FabricSalesContractDetailID + " AND ShadeID !=" + oFabricSalesContractDetail.ShadeID);
            return tc.ExecuteReader("Update FabricSalesContractDetail set FNLabdipDetailID='" + oFabricSalesContractDetail.FNLabdipDetailID + "', ShadeID = " + oFabricSalesContractDetail.ShadeID + " where FabricSalesContractDetailID='" + oFabricSalesContractDetail.FabricSalesContractDetailID + "'");
        }
        public static IDataReader GetsReport(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_FNExecutionOrderStatus]" + "%s", sSQL);
        }

        public static IDataReader UpdateStatus(TransactionContext tc, FabricSalesContractDetail oFabricSalesContractDetail)
        {
            string sSQL1 = SQLParser.MakeSQL("Update FabricSalesContractDetail Set Status=%n WHERE FabricSalesContractDetailID=%n", oFabricSalesContractDetail.Status, oFabricSalesContractDetail.FabricSalesContractDetailID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_FabricSalesContractDetail WHERE FabricSalesContractDetailID=%n", oFabricSalesContractDetail.FabricSalesContractDetailID);
        }

        #endregion
    }
}
