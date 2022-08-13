using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class FabricExecutionOrderDA
    {
        public FabricExecutionOrderDA() { }

        #region IUD
        public static IDataReader IUD(TransactionContext tc, FabricExecutionOrder oFEO, int nDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricExecutionOrder] %n, %n, %n, %n, %n, %s, %n, %s, %s, %s, %s, %n, %s, %s, %s, %s, %s, %s, %n, %n, %n, %s, %d, %d, %d, %b, %n, %d, %s, %b, %n, %d, %d, %n, %n, %n, %n, %n, %n",
            oFEO.FEOID, oFEO.FabricID, oFEO.OrderType, oFEO.BuyerID, oFEO.ContractorPersonalID, oFEO.StyleNo, oFEO.MktPersonID,
            oFEO.OrderRef, oFEO.BuyerRef, oFEO.Process, oFEO.Emirzing, oFEO.ReqFinishedGSM, oFEO.GarmentWash, oFEO.TestStandard, oFEO.FinalInspection,
            oFEO.FinishWidth, oFEO.CW, oFEO.EndUse, oFEO.Qty, oFEO.PPSampleQty, oFEO.TestSampleQty, oFEO.Note, oFEO.OrderDate, oFEO.ExpectedDeliveryDate, oFEO.ExpDelEndDate, oFEO.IsInHouse, oFEO.SaleOrderID, oFEO.ApproveDate, oFEO.FEOColor, oFEO.PreShipmentSampleReq,
            oFEO.LightSourceID, oFEO.PPSampleDate, oFEO.PreSampleDate, oFEO.GreyQty, oFEO.ProcessType, oFEO.FabricWeave, oFEO.FinishType, nUserId, nDBOperation);
        }

        public static IDataReader Copy(TransactionContext tc, FabricExecutionOrder oFEO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_CopyFabricExecutionOrder]" + "%n, %n", oFEO.FEOID, nUserID);
        }
        public static IDataReader SaveLog(TransactionContext tc, FabricExecutionOrder oFEO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricExecutionOrderLog]" + "%n, %n, %n", oFEO.FEOID, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Unapprove FEO
        public static IDataReader UnapproveFEO(TransactionContext tc, FabricExecutionOrder oFEO, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_Fabric_UndoApprove] %n,%n", oFEO.FEOID, nUserId);
        }
        #endregion

        #region Process Dyed Yarn

        public static IDataReader ProcessYarnDyed(TransactionContext tc, int nFEOID, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_ProcessYarnDyed] %n, %n ", nFEOID, nUserId);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFEOID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricExecutionOrder WHERE FEOID=%n", nFEOID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL, Int64 nUserId)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetByFEONo(TransactionContext tc, int nIsInHouse, string sFEONo, int nYear)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricExecutionOrder WHERE IsInHouse = " + nIsInHouse + " AND FEONo = %s AND year(OrderDate) = 20%n", sFEONo, nYear);
        }
        public static IDataReader UpdateFinish(TransactionContext tc, int nFEOID, bool bIsFinish, Int64 nUserId)
        {
            tc.ExecuteNonQuery("UPDATE FabricExecutionOrder SET IsFinish=%b WHERE FEOID=%n", bIsFinish, nFEOID);
            return tc.ExecuteReader("SELECT * FROM View_FabricExecutionOrder WHERE FEOID=%n", nFEOID);
        }
        #endregion

    }
}
