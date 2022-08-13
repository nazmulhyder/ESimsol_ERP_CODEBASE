using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class MeetingSummaryDA
    {
        public MeetingSummaryDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MeetingSummary oMeetingSummary, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MeetingSummary]"
                                    + "%n, %n, %n, %s, %n, %n, %n, %n, %n, %n",
                                    oMeetingSummary.MeetingSummaryID, oMeetingSummary.MarketingScheduleID, oMeetingSummary.MeetingSummarizeBy,
                                    oMeetingSummary.MeetingSummaryText, (int)oMeetingSummary.RefType, oMeetingSummary.RefID, oMeetingSummary.CurrencyID,
                                    oMeetingSummary.Price, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, MeetingSummary oMeetingSummary, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MeetingSummary]"
                                    + "%n, %n, %n, %s, %n, %n, %n, %n, %n, %n",
                                    oMeetingSummary.MeetingSummaryID, oMeetingSummary.MarketingScheduleID, oMeetingSummary.MeetingSummarizeBy,
                                    oMeetingSummary.MeetingSummaryText, (int)oMeetingSummary.RefType, oMeetingSummary.RefID, oMeetingSummary.CurrencyID,
                                    oMeetingSummary.Price, (int)eEnumDBOperation, nUserID);
        }

        public static IDataReader SaveFromFabric(TransactionContext tc, MeetingSummary oMeetingSummary, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MeetingSummary]"
                                    + "%n, %n, %n, %s, %n, %n, %n, %n, %n, %n",
                                    oMeetingSummary.MeetingSummaryID, oMeetingSummary.MarketingScheduleID, oMeetingSummary.MeetingSummarizeBy,
                                    oMeetingSummary.MeetingSummaryText, (int)oMeetingSummary.RefType, oMeetingSummary.RefID, oMeetingSummary.CurrencyID,
                                    oMeetingSummary.Price, (int)eEnumDBOperation, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeetingSummary WHERE MeetingSummaryID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nMarketingScheduleID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeetingSummary WHERE MarketingScheduleID=" + nMarketingScheduleID + " ORDER BY MeetingSummaryID DESC");
        }
        public static IDataReader GetsByRef(TransactionContext tc, int nFabricID, int nRefType)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeetingSummary WHERE RefType = " + nRefType + " AND RefID =" + nFabricID + " ORDER BY MeetingSummaryID DESC");
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_MeetingSummary");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_MeetingSummary
        }

        
        #endregion
    }  
}
