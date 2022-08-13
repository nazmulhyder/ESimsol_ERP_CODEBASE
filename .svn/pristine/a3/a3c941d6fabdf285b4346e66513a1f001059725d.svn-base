using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class RouteSheetHistoryDA
    {
        public RouteSheetHistoryDA() { }


        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, RouteSheetHistory oRSH, int nDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheetHistory]"
                                   + "%n, %D, %n, %n, %n, %s, %n",
                                      oRSH.RouteSheetID, oRSH.EventTime, oRSH.EventEmpID, (int)oRSH.CurrentStatus, (int)oRSH.PreviousState, oRSH.Note, nUserID);
        }
        public static IDataReader IUD_Process(TransactionContext tc, RouteSheetHistory oRSH, int nDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheetProgressHistory]"
                                   + "%n, %s, %n, %n, %n, %s, %n,  %n, %n, %n,  %n, %n, %n, %n, %n",
                                      oRSH.RouteSheetID,    oRSH.EventTime.ToString("dd MMM yyyy HH:mm"),       oRSH.EventEmpID,      (int)oRSH.CurrentStatus,    (int)oRSH.PreviousState, oRSH.Note,
                                      oRSH.ShadePercentage, oRSH.MachineID_Hydro, oRSH.MachineID_Dryer, oRSH.Value_Dyes, oRSH.Value_Chemcial, oRSH.Value_Yarn, oRSH.MachineSpeed, oRSH.RBSpeed, nUserID);
        }

        public static IDataReader UpdateEventTime(TransactionContext tc, RouteSheetHistory oRouteSheetHistory)
        {
            string sSQL1 = SQLParser.MakeSQL("UPDATE RouteSheetHistory SET EventTime = %s,Note=%s WHERE RouteSheetHistoryID = %n", oRouteSheetHistory.EventTimeStr, oRouteSheetHistory.Note, oRouteSheetHistory.RouteSheetHistoryID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetHistory WHERE RouteSheetHistoryID=%n", oRouteSheetHistory.RouteSheetHistoryID);
        }
        #endregion
        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetHistory WHERE RouteSheetHistoryID=%n", nID);
        }
        public static IDataReader GetBy(TransactionContext tc, int nRSID, int nRSStatus)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetHistory WHERE RouteSheetID=%n and CurrentStatus=%n", nRSID, nRSStatus);
        }
        public static IDataReader Gets(TransactionContext tc, int nRSID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetHistory WHERE RouteSheetID=%n order By DBServerDateTime", nRSID);
        }
        public static IDataReader GetRSDyeingProgress(TransactionContext tc, RouteSheetHistory oRouteSheetHistory)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheetDyeingProgress WHERE RouteSheetID=%n", oRouteSheetHistory.RouteSheetID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
