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
    public class RouteSheetCombineDetailDA
    {
        public RouteSheetCombineDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, RouteSheetCombineDetail oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheetCombineDetail]" + "%n, %n, %n, %n,%n,%n,%n,%n, %n, %n",
                                    oDO.RouteSheetCombineDetailID, oDO.RouteSheetCombineID, oDO.RouteSheetID, oDO.DUPScheduleID, oDO.TtlLiquire, oDO.MachineID, oDO.QtyDye,oDO.NoOfHanksCone,  nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, RouteSheetCombineDetail oDO, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_RouteSheetCombineDetail]" + "%n, %n, %n, %n,%n,%n,%n,%n, %n, %n",
                                      oDO.RouteSheetCombineDetailID, oDO.RouteSheetCombineID, oDO.RouteSheetID, oDO.DUPScheduleID, oDO.TtlLiquire, oDO.MachineID, oDO.QtyDye,oDO.NoOfHanksCone, nUserID, (int)eEnumDBOperation);
        }
        #endregion


        #region Get & Exist Function

        public static IDataReader Get(int nID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetCombineDetail WHERE RouteSheetCombineDetailID=%n", nID);
        }

        public static IDataReader GetsBy(TransactionContext tc, int nRouteSheetID)
        {
            return tc.ExecuteReader("Select * from View_RouteSheetCombineDetail where RouteSheetID=%n ", nRouteSheetID);
        }
        public static IDataReader Gets(TransactionContext tc, int nRSCID)
        {
            return tc.ExecuteReader("Select * from View_RouteSheetCombineDetail where RouteSheetCombineID=%n ", nRSCID);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
     
        #endregion
    }
}
