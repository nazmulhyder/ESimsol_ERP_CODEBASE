using ESimSol.BusinessObjects;
using ICS.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.DataAccess
{
    class GeneralWorkingDayDA
    {
        public GeneralWorkingDayDA() { }
        public static IDataReader InsertUpdate(TransactionContext tc, GeneralWorkingDay oGeneralWorkingDay, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GeneralWorkingDay]"
                                    + "%n, %s, %d, %n, %s, %n, %n",
                                    oGeneralWorkingDay.GWDID, oGeneralWorkingDay.GWDTitle, oGeneralWorkingDay.AttendanceDate, oGeneralWorkingDay.GWDApplyOnInt, oGeneralWorkingDay.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, GeneralWorkingDay oGeneralWorkingDay, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_GeneralWorkingDay]"
                                    + "%n, %s, %d, %n, %s, %n, %n",
                                    oGeneralWorkingDay.GWDID, oGeneralWorkingDay.GWDTitle, oGeneralWorkingDay.AttendanceDate, oGeneralWorkingDay.GWDApplyOnInt, oGeneralWorkingDay.Remarks, (int)eEnumDBOperation, nUserID);

        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("select * from GeneralWorkingDay");
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("select * from View_GeneralWorkingDay WHERE GWDID=%n", nID);
        }

        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
    }
}
