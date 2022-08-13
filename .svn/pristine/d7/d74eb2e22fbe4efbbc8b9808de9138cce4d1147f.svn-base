using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class RouteSheetGraceDA
    {
        public RouteSheetGraceDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, RouteSheetGrace oRouteSheetGrace, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheetGrace]"
                                    + "%n, %n, %n, %n, %n, %s, %n, %d, %s, %n, %d, %n, %n",
                                    oRouteSheetGrace.RouteSheetGraceID, oRouteSheetGrace.RouteSheetID, oRouteSheetGrace.DyeingOrderDetailID, oRouteSheetGrace.QtyGrace,
                                    oRouteSheetGrace.GraceCount, oRouteSheetGrace.Note, oRouteSheetGrace.ApprovedByID, oRouteSheetGrace.ApproveDate,
                                    oRouteSheetGrace.NoteApp, oRouteSheetGrace.LastUpdateBy, oRouteSheetGrace.LastUpdateDateTime, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, RouteSheetGrace oRouteSheetGrace, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_RouteSheetGrace]"
                                    + "%n, %n, %n, %n, %n, %s, %n, %d, %s, %n, %d, %n, %n",
                                    oRouteSheetGrace.RouteSheetGraceID, oRouteSheetGrace.RouteSheetID, oRouteSheetGrace.DyeingOrderDetailID, oRouteSheetGrace.QtyGrace,
                                    oRouteSheetGrace.GraceCount, oRouteSheetGrace.Note, oRouteSheetGrace.ApprovedByID, oRouteSheetGrace.ApproveDate,
                                    oRouteSheetGrace.NoteApp, oRouteSheetGrace.LastUpdateBy, oRouteSheetGrace.LastUpdateDateTime, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetGrace WHERE RouteSheetGraceID=%n", nID);
        }
        public static IDataReader GetByRS(TransactionContext tc, long nRSID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetGrace WHERE RouteSheetID=%n", nRSID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetGrace where isnull(ApprovedByID,0)=0");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
}


