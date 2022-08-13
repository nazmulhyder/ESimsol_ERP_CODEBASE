using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class MarketingScheduleDA
    {
        public MarketingScheduleDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MarketingSchedule oMarketingSchedule, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MarketingSchedule]"
                                    + "%n, %s, %n, %n, %D, %s, %s, %s, %n, %n, %n",
                                    oMarketingSchedule.MarketingScheduleID, oMarketingSchedule.ScheduleNo, oMarketingSchedule.MKTPersonID, oMarketingSchedule.BuyerID, oMarketingSchedule.ScheduleDateTime, oMarketingSchedule.MeetingLocation, oMarketingSchedule.MeetingDuration, oMarketingSchedule.Remarks, oMarketingSchedule.ScheduleAssignBy, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, MarketingSchedule oMarketingSchedule, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MarketingSchedule]"
                                    + "%n, %s, %n, %n, %D, %s, %s, %s, %n, %n, %n",
                                    oMarketingSchedule.MarketingScheduleID, oMarketingSchedule.ScheduleNo, oMarketingSchedule.MKTPersonID, oMarketingSchedule.BuyerID, oMarketingSchedule.ScheduleDateTime, oMarketingSchedule.MeetingLocation, oMarketingSchedule.MeetingDuration, oMarketingSchedule.Remarks, oMarketingSchedule.ScheduleAssignBy, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MarketingSchedule WHERE MarketingScheduleID=%n", nID);
        }
        public static IDataReader GetsByCurrentMonth(TransactionContext tc, DateTime dMonth, int nMKTPersonID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MarketingSchedule WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ScheduleDateTime,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dMonth.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dMonth.AddMonths(1).ToString("dd MMM yyyy") + "',106)) AND MKTPersonID=" + nMKTPersonID);
        }
        public static IDataReader GetsByCurrentMonth(TransactionContext tc, DateTime dMonth)
        {
            return tc.ExecuteReader("SELECT * FROM View_MarketingSchedule WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ScheduleDateTime,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dMonth.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dMonth.AddMonths(1).ToString("dd MMM yyyy") + "',106)) ");
        }
        public static IDataReader Gets(TransactionContext tc,DateTime dScheduleDateTime)
        {
            return tc.ExecuteReader("SELECT * FROM View_MarketingSchedule WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ScheduleDateTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dScheduleDateTime.ToString("dd MMM yyyy") + "',106))");
        }
        public static IDataReader Gets(TransactionContext tc, int nMKTPersonID, DateTime dScheduleDateTime)
        {
            return tc.ExecuteReader("SELECT * FROM View_MarketingSchedule WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ScheduleDateTime,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dScheduleDateTime.ToString("dd MMM yyyy") + "',106)) AND MKTPersonID=" + nMKTPersonID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_MarketingSchedule");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_MarketingSchedule
        }
        //public static IDataReader GetByFabricID(TransactionContext tc, int nBuyerID, int nFabricID, int nUserID)
        //{
        //    return tc.ExecuteReader("");
        //}

        
        #endregion
    }  
}
