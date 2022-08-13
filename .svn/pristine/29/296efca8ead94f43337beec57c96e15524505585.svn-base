using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    class DUPScheduleDetailDA
    {
        
        public DUPScheduleDetailDA() { }

        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, DUPScheduleDetail oDUPScheduleDetail, EnumDBOperation eEnumDBDUPScheduleDetail, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUPScheduleDetail]"
                                    + "%n,%n,%n,%n,%s,%n,%n, %s,%n,%n",
                                   oDUPScheduleDetail.DUPScheduleDetailID, oDUPScheduleDetail.DUPScheduleID, oDUPScheduleDetail.DODID,  oDUPScheduleDetail.Qty, oDUPScheduleDetail.Remarks, oDUPScheduleDetail.QtyPerBag, oDUPScheduleDetail.BagCount, oDUPScheduleDetail.OrderLast, nUserId, (int)eEnumDBDUPScheduleDetail);
        }


        public static void Delete(TransactionContext tc, DUPScheduleDetail oDUPScheduleDetail, EnumDBOperation eEnumDBDUPScheduleDetail, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DUPScheduleDetail]"
                                   + "%n,%n,%n,%n,%s,%n,%n, %s,%n,%n",
                                   oDUPScheduleDetail.DUPScheduleDetailID, oDUPScheduleDetail.DUPScheduleID, oDUPScheduleDetail.DODID, oDUPScheduleDetail.Qty, oDUPScheduleDetail.Remarks, oDUPScheduleDetail.QtyPerBag, oDUPScheduleDetail.BagCount, oDUPScheduleDetail.OrderLast, nUserId, (int)eEnumDBDUPScheduleDetail);
        }
        public static IDataReader Swap(TransactionContext tc, DUPSchedule oDUPSchedule, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUPSchedule_Swap]"
                               + "%n,%n,%n,%D,%D,%n",
                               oDUPSchedule.DUPScheduleID, oDUPSchedule.MachineID, oDUPSchedule.LocationID, oDUPSchedule.StartTime.ToString("dd MMM yyyy HH:mm"), oDUPSchedule.EndTime.ToString("dd MMM yyyy HH:mm"), nUserId);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc,int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUPScheduleDetail where DUPScheduleID=%n", id);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUPScheduleDetail ");
        }

        public static IDataReader Gets(TransactionContext tc,string sPSIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUPScheduleDetail where DUPScheduleID in (%q) ", sPSIDs);
        }
        public static IDataReader GetsSqL(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Update_Requisition(TransactionContext tc, DUPScheduleDetail oDUPScheduleDetail)
        {
            string sSQL1 = SQLParser.MakeSQL("Update DUPScheduleDetail Set IsRequistion=%b WHERE DUPScheduleDetailID IN (" + oDUPScheduleDetail.Params + ")", oDUPScheduleDetail.IsRequistion);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM DUPScheduleDetail WHERE  DUPScheduleDetailID IN (%s)", oDUPScheduleDetail.Params);
        }
        public static IDataReader Gets_RS(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_RSReport] %s, %n", sSQL,1);
        }
        #endregion
    }
}
