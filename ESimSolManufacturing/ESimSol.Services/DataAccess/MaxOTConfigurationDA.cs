using ESimSol.BusinessObjects;
using ICS.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.DataAccess
{
    class MaxOTConfigurationDA
    {
        public MaxOTConfigurationDA() { }
        public static IDataReader IUD(TransactionContext tc, MaxOTConfiguration oMaxOTConfiguration, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_MaxOTConfiguration]"
                                    + "%n, %n, %n,%n,%s,  %b, %b,%n, %b,%n, %n",
                                    oMaxOTConfiguration.MOCID
                                    ,oMaxOTConfiguration.MaxBeforeInMin
                                    , oMaxOTConfiguration.MaxOTInMin
                                    ,oMaxOTConfiguration.Sequence
                                    , oMaxOTConfiguration.TimeCardName
                                    , oMaxOTConfiguration.IsPresentOnDayOff
                                    , oMaxOTConfiguration.IsPresentOnHoliday
                                    ,oMaxOTConfiguration.SourceTimeCardID
                                    ,oMaxOTConfiguration.IsActive
                                    , (int)eEnumDBOperation
                                    , nUserID);
        }

        public static void Delete(TransactionContext tc, MaxOTConfiguration oMaxOTConfiguration, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_MaxOTConfiguration]"
                                    + "%n, %n, %n,%n,%s,  %b, %b,%n, %b,%n, %n",
                                    oMaxOTConfiguration.MOCID
                                    , oMaxOTConfiguration.MaxBeforeInMin
                                    , oMaxOTConfiguration.MaxOTInMin
                                    ,oMaxOTConfiguration.Sequence
                                    , oMaxOTConfiguration.TimeCardName
                                    , oMaxOTConfiguration.IsPresentOnDayOff
                                    , oMaxOTConfiguration.IsPresentOnHoliday
                                    ,oMaxOTConfiguration.SourceTimeCardID
                                    ,oMaxOTConfiguration.IsActive
                                    , (int)eEnumDBOperation
                                    , nUserId);
                                     
        }

        public static void Activity(TransactionContext tc, MaxOTConfiguration oMaxOTConfiguration, Int64 nUserId)
        {
            tc.ExecuteNonQuery("UPDATE MaxOTConfiguration SET IsActive = %b, LastUpdateBy = %n, LastUpdateDateTime = GETDATE() WHERE MOCID = %n", oMaxOTConfiguration.IsActive, nUserId, oMaxOTConfiguration.MOCID);
        }
        public static IDataReader Get(TransactionContext tc, int nMOCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_MaxOTConfiguration AS HH WHERE HH.MOCID=%n", nMOCID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_MaxOTConfiguration");
        }
        public static IDataReader Gets(TransactionContext tc, bool bIsActive)
        {
            return tc.ExecuteReader("select * from View_MaxOTConfiguration WHERE IsActive =%b", bIsActive);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsDayoff(TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_MaxOTConfiguration WHERE IsPresentOnDayOff = 1");
        }
        public static IDataReader GetsByUser(TransactionContext tc, Int64 id)
        {
            return tc.ExecuteReader("SELECT * FROM View_MaxOTConfiguration AS MOT WHERE MOT.MOCID IN(SELECT HH.MOCID FROM MaxOTConfigurationUser AS HH WHERE HH.UserID = %n) ORDER BY MOT.MOCID ASC", id);
        }
    }
}

