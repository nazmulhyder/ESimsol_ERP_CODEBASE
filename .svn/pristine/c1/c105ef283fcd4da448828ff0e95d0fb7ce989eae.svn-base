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
    public class RouteSheetCombineDA
    {
        public RouteSheetCombineDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, RouteSheetCombine oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheetCombine]" + "%n, %D, %n, %s, %n, %n,%n,%s ,%n,%n",
                                    oDO.RouteSheetCombineID, oDO.CombineRSDate, oDO.ProductionScheduleID, oDO.RSNo_Combine, oDO.TotalQty,oDO.TotalLiquor,oDO.TtlCotton, oDO.Note, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, RouteSheetCombine oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_RouteSheetCombine]" + "%n, %D, %n, %s, %n, %n,%n,%s ,%n,%n",
                                       oDO.RouteSheetCombineID, oDO.CombineRSDate, oDO.ProductionScheduleID, oDO.RSNo_Combine, oDO.TotalQty, oDO.TotalLiquor, oDO.TtlCotton,oDO.Note, nUserID, (int)eEnumDBOperation);
        }
        #endregion


        #region Get & Exist Function

        public static IDataReader Get(int nID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetCombine WHERE RouteSheetCombineID=%n", nID);
        }
        public static IDataReader GetBy(int nRSID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetCombine WHERE RouteSheetCombineID in (Select RouteSheetCombineID from RouteSheetCombineDetail where RouteSheetID=%n)", nRSID);
        }

        public static IDataReader GetsBy(TransactionContext tc, int nRouteSheetID)
        {
            return tc.ExecuteReader("Select * from View_RouteSheetCombine where RouteSheetID=%n ", nRouteSheetID);
        }
        public static IDataReader Gets(TransactionContext tc, int nPTUID)
        {
            return tc.ExecuteReader("Select * from View_RouteSheetCombine where PTUID=%n", nPTUID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
    
     
        #endregion
    }
}
