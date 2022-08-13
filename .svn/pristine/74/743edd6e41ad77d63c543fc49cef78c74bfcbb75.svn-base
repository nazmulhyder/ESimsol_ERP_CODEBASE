using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class RouteSheetSetupDA
    {
        public RouteSheetSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, RouteSheetSetup oRouteSheetSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_RouteSheetSetup]"
                                    + "%n,%s,%s,%s, %n, %n, %b,%s,%n, %n,   %n,%n,%n,%n, %b,%b, %b, %D, %s, %n,%n,%b, %n, %n,%n,%n,%n,%n,%b,%n,%n",
                                    oRouteSheetSetup.RouteSheetSetupID, oRouteSheetSetup.RSName, oRouteSheetSetup.RSName_Print, oRouteSheetSetup.RSShortName, oRouteSheetSetup.MUnitID, oRouteSheetSetup.MUnitID_Two, oRouteSheetSetup.Activity, oRouteSheetSetup.Note, oRouteSheetSetup.SmallUnit_Cal, oRouteSheetSetup.GracePercentage, oRouteSheetSetup.LossPercentage, oRouteSheetSetup.GainPercentage, oRouteSheetSetup.PrintNo, oRouteSheetSetup.RestartBy, oRouteSheetSetup.IsLotMandatory, oRouteSheetSetup.IsShowBuyer, oRouteSheetSetup.IsApplyHW, oRouteSheetSetup.BatchTime, oRouteSheetSetup.BatchCode, oRouteSheetSetup.MachinePerDoc, oRouteSheetSetup.FontSize, oRouteSheetSetup.IsGraceApplicable, oRouteSheetSetup.NumberOfAddition, oRouteSheetSetup.DyesChemicalViewTypeInt, oRouteSheetSetup.WorkingUnitIDWIP, oRouteSheetSetup.DCEntryValType, oRouteSheetSetup.DCOutValType, oRouteSheetSetup.RSStateForCost, oRouteSheetSetup.IsRateOnUSD, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, RouteSheetSetup oRouteSheetSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_RouteSheetSetup]"
                                    + "%n,%s,%s,%s, %n, %n, %b,%s,%n, %n,   %n,%n,%n,%n, %b,%b, %b, %D, %s, %n,%n,%b, %n, %n,%n,%n,%n,%n,%n,%n",
                                    oRouteSheetSetup.RouteSheetSetupID, oRouteSheetSetup.RSName, oRouteSheetSetup.RSName_Print, oRouteSheetSetup.RSShortName, oRouteSheetSetup.MUnitID, oRouteSheetSetup.MUnitID_Two, oRouteSheetSetup.Activity, oRouteSheetSetup.Note, oRouteSheetSetup.SmallUnit_Cal, oRouteSheetSetup.GracePercentage, oRouteSheetSetup.LossPercentage, oRouteSheetSetup.GainPercentage, oRouteSheetSetup.PrintNo, oRouteSheetSetup.RestartBy, oRouteSheetSetup.IsLotMandatory, oRouteSheetSetup.IsShowBuyer, oRouteSheetSetup.IsApplyHW, oRouteSheetSetup.BatchTime, oRouteSheetSetup.BatchCode, oRouteSheetSetup.MachinePerDoc, oRouteSheetSetup.FontSize, oRouteSheetSetup.IsGraceApplicable, oRouteSheetSetup.NumberOfAddition, oRouteSheetSetup.DyesChemicalViewTypeInt, oRouteSheetSetup.WorkingUnitIDWIP, oRouteSheetSetup.DCEntryValType, oRouteSheetSetup.DCOutValType, oRouteSheetSetup.RSStateForCost,oRouteSheetSetup.IsRateOnUSD, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetSetup WHERE RouteSheetSetupID=%n", nID);
        }
        public static IDataReader GetBy(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT top(1)* FROM View_RouteSheetSetup ");
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetSetup");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
       
    
     
    
        #endregion
    }
}