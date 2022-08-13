using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class RouteSheetPackingDA
    {
        public RouteSheetPackingDA() { }

        #region Insert Update Delete Function

       
        public static IDataReader IUD(TransactionContext tc, RouteSheetPacking oRouteSheetPacking, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheetPacking] %n,%n,%n,%n,   %s,%n,%s, %n,    %n,%n,%n,%n,   %n,%n,%n,%n, %n,  %n,%n",
                   oRouteSheetPacking.PackingID, oRouteSheetPacking.RouteSheetID, oRouteSheetPacking.Weight, oRouteSheetPacking.NoOfHankCone, oRouteSheetPacking.Note, oRouteSheetPacking.Warp, oRouteSheetPacking.Length, oRouteSheetPacking.PackedByEmpID, oRouteSheetPacking.DyeingOrderDetailID, (int)oRouteSheetPacking.YarnType, (int)oRouteSheetPacking.BagType, oRouteSheetPacking.BagWeight, oRouteSheetPacking.QtyGW, oRouteSheetPacking.LDPE, oRouteSheetPacking.HDPE, oRouteSheetPacking.CTN, oRouteSheetPacking.DUHardWindingID, nUserID, nDBOperation);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nRouteSheetPackingID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RoutesheetPacking WHERE PackingID=%n", nRouteSheetPackingID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static void Update_RS_BagNo(int nRouteSheetID, TransactionContext tc)
        {
            tc.ExecuteNonQuery("UPDATE DUHardWinding SET BagNo = (SELECT MAX(BagNo) FROM RouteSheetPacking WHERE RouteSheetID =  %n) FROM DUHardWinding WHERE RouteSheetID = %n", nRouteSheetID, nRouteSheetID);
        }


        #endregion
    }
}
