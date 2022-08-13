using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class RouteSheetDA
    {
        public RouteSheetDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, RouteSheet oRouteSheet, int nDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheet]"
                                   + "%n, %s,%d, %n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %n, %n, %n,%n,%b,%n,%n,%n,%n, %n, %n",
                                     oRouteSheet.RouteSheetID,oRouteSheet.RouteSheetNo, oRouteSheet.RouteSheetDate, oRouteSheet.MachineID, oRouteSheet.ProductID_Raw, oRouteSheet.LotID, oRouteSheet.Qty, (int)oRouteSheet.RSState, oRouteSheet.LocationID, oRouteSheet.PTUID, oRouteSheet.DUPScheduleID, oRouteSheet.Note, oRouteSheet.TtlLiquire, oRouteSheet.TtlCotton, oRouteSheet.HanksCone, oRouteSheet.NoOfHanksCone, oRouteSheet.OrderType, oRouteSheet.CopiedFrom, oRouteSheet.IsReDyeing,oRouteSheet.RSShiftID,oRouteSheet.QtyDye,oRouteSheet.QtyOmit,oRouteSheet.Label, nUserID, nDBOperation);
        }
        public static IDataReader UpdateMachine(TransactionContext tc, RouteSheet oRouteSheet, int nDBOperation, long nUserID)
        {
            tc.ExecuteNonQuery("UPDATE RouteSheet SET MachineID = %n WHERE RouteSheetID = %n", oRouteSheet.MachineID, oRouteSheet.RouteSheetID);
            return tc.ExecuteReader("SELECT * FROM View_RouteSheet WHERE RouteSheetID=%n", oRouteSheet.RouteSheetID);
        }
        public static IDataReader RouteSheetEditSave(TransactionContext tc, RouteSheet oRouteSheet, int nDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheetEdit]"
                                   + "%n,%n, %n, %n, %n, %n,%n,%n, %n, %n",
                                     oRouteSheet.RouteSheetID, oRouteSheet.MachineID, oRouteSheet.TtlLiquire, oRouteSheet.TtlCotton, oRouteSheet.RSShiftID, oRouteSheet.QtyDye,oRouteSheet.QtyOmit,oRouteSheet.Label, nUserID, nDBOperation);
        }

        #endregion

        #region Yarn Out
        public static IDataReader YarnOut(TransactionContext tc, RouteSheet oRouteSheet, int nEventEmpID, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheet_YarnOut]"
                                   + "%n, %n, %n, %s, %n, %n, %n",
                                     oRouteSheet.RouteSheetID, oRouteSheet.ProductID_Raw, oRouteSheet.LotID, oRouteSheet.Note, oRouteSheet.NoOfHanksCone, nEventEmpID, nUserID);
        }
        public static IDataReader YarnOut_Multi(TransactionContext tc, RouteSheet oRouteSheet, int nEventEmpID, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheet_YarnOut_MultiLot]"
                                   + "%n, %n, %n, %s,  %n, %n",
                                     oRouteSheet.RouteSheetID, oRouteSheet.ProductID_Raw, oRouteSheet.LotID, oRouteSheet.Note, nEventEmpID, nUserID);
        }
        #endregion

        #region QC Done
     
        public static IDataReader RSQCDOne(TransactionContext tc, RouteSheetDO oRouteSheetDO, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheet_QC]" + "%n,%n,%n,%s, %n", oRouteSheetDO.RouteSheetID, oRouteSheetDO.DyeingOrderDetailID, (int)oRouteSheetDO.RSState, oRouteSheetDO.Note, nUserID);
        }
        public static IDataReader RSQCDOneByForce(TransactionContext tc, RouteSheet oRouteSheet, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheet_QCByForce]" + "%n,%s, %n", oRouteSheet.RouteSheetID,  oRouteSheet.Note, nUserID);
        }
        public static IDataReader RSInRSInSubFinishing(TransactionContext tc, RouteSheet oRouteSheet, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RouteSheet_RSInSubFinishing]" + "%n, %n,%s,%n,%n", oRouteSheet.RouteSheetID,oRouteSheet.Qty,oRouteSheet.Note,(int)oRouteSheet.RSState, nUserID);
        }
        #endregion

        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheet WHERE RouteSheetID=%n", nID);
        }

        public static IDataReader GetByPS(TransactionContext tc, int nDUPScheduleID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RouteSheet WHERE isnull(DUPScheduleID,0)>0 and DUPScheduleID=%n", nDUPScheduleID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static double GetRouteSheetQty(TransactionContext tc, int nRouteSheetID)
        {
            object obj = tc.ExecuteScalar("Select Qty from RouteSheet WHERE RouteSheetID=%n",nRouteSheetID);
            return Convert.ToDouble(obj);
        }

        #endregion
    }
}
