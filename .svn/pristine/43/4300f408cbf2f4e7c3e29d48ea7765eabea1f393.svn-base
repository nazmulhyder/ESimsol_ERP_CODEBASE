using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
   
    public class DUPScheduleDA
    {
        public DUPScheduleDA() { }

        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, DUPSchedule oDUPSchedule, EnumDBOperation eEnumDBDUPSchedule, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUPSchedule]"
                                   + "%n,%s,%n,%n,%n,%n,%n,%n,%b,%D,%D,%b,%n, %s, %n, %n, %n,%n",
                                   oDUPSchedule.DUPScheduleID, oDUPSchedule.ScheduleNo, oDUPSchedule.BatchGroup, oDUPSchedule.StatusInt, oDUPSchedule.MachineID, oDUPSchedule.PSBatchNo, oDUPSchedule.LocationID, oDUPSchedule.Qty, oDUPSchedule.ScheduleType, oDUPSchedule.StartTime.ToString("dd MMM yyyy HH:mm"), oDUPSchedule.EndTime.ToString("dd MMM yyyy HH:mm"), oDUPSchedule.IsIncreaseTime, oDUPSchedule.LotID, oDUPSchedule.Note, oDUPSchedule.WorkingUnitID,nUserId, (int)eEnumDBDUPSchedule, oDUPSchedule.BUID);
        }

        public static void Delete(TransactionContext tc, DUPSchedule oDUPSchedule, EnumDBOperation eEnumDBDUPSchedule, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DUPSchedule]"
                                    + "%n,%s,%n,%n,%n,%n,%n,%n,%b,%D,%D,%b,%n, %s, %n, %n, %n,%n",
                                   oDUPSchedule.DUPScheduleID, oDUPSchedule.ScheduleNo, oDUPSchedule.BatchGroup, oDUPSchedule.StatusInt, oDUPSchedule.MachineID, oDUPSchedule.PSBatchNo, oDUPSchedule.LocationID, oDUPSchedule.Qty, oDUPSchedule.ScheduleType, oDUPSchedule.StartTime.ToString("dd MMM yyyy HH:mm"), oDUPSchedule.EndTime.ToString("dd MMM yyyy HH:mm"), oDUPSchedule.IsIncreaseTime, oDUPSchedule.LotID, oDUPSchedule.Note, oDUPSchedule.WorkingUnitID, nUserId, (int)eEnumDBDUPSchedule, oDUPSchedule.BUID);
        }

       
                                 

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUPSchedule where Activity='1'  order by StartTime ASC");
        }
        public static IDataReader Gets(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, string sLocationIDs)
        {
            return tc.ExecuteReader(" SELECT * FROM View_DUPSchedule  where (StartTime >=%D and  StartTime<%D) or (EndTime >=%D and  EndTime<%D) and Activity='1' and LocationID in (%q)  order by StartTime ASC", dStartDate, dEndDate, dStartDate, dEndDate, sLocationIDs);
        }

        public static IDataReader Gets(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, string sLocationIDs, string sDyeMachineIDs)
        {
            return tc.ExecuteReader(" SELECT * FROM View_DUPSchedule  where Activity='1' and StartTime >=%D and  EndTime<%D and LocationID in (%q) and PSSID in (%q) order by StartTime ASC", dStartDate, dEndDate, sLocationIDs, sDyeMachineIDs);
        }

        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUPSchedule WHERE DUPScheduleID=%n and Activity='1'", nID);
        }
        public static IDataReader Update_Status(TransactionContext tc, DUPSchedule oDUPSchedule)
        {
            string sSQL1 = SQLParser.MakeSQL("Update DUPSchedule Set ScheduleStatus=%n WHERE DUPScheduleID=%n", oDUPSchedule.StatusInt, oDUPSchedule.DUPScheduleID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_DUPSchedule WHERE DUPScheduleID=%n", oDUPSchedule.DUPScheduleID);
        }
        public static int GetsMax(TransactionContext tc,string sSql)
        {
            object obj = tc.ExecuteScalar(sSql);
            if (obj == null) return 0;
            int aaaa = 0;
            aaaa = Convert.ToInt32(obj);
            return aaaa;
        }

        public static IDataReader Gets(TransactionContext tc, string sSql)
        {
            return tc.ExecuteReader(sSql);
        }



        public static int GetHisEven(TransactionContext tc, int nDUPScheduleID, EnumRSState eRSState)
        {
            object obj = tc.ExecuteScalar("SELECT RSH.CurrentStatus FROM RouteSheetHistory as RSH WHERE RouteSheetID in (Select RouteSheetID from RouteSheet where RouteSheet.DUPScheduleID>0 and RouteSheet.DUPScheduleID=%n ) and RSH.CurrentStatus=%n ", nDUPScheduleID, eRSState);
            if (obj == null) return 0;
            int aaaa = 0;
            aaaa = Convert.ToInt32(obj);
            return aaaa;
        }

        #endregion

    }
}
