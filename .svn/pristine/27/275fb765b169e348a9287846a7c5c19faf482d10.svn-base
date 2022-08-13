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
    public class GeneralWorkingDayShiftDA
    {
        public GeneralWorkingDayShiftDA() { }

        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, GeneralWorkingDayShift oGeneralWorkingDayShift, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GeneralWorkingDayShift]" + "%n, %n, %n, %n, %n, %s",
                   oGeneralWorkingDayShift.GWDSID, oGeneralWorkingDayShift.GWDID, oGeneralWorkingDayShift.ShiftID, (int)eEnumDBOperation, nUserID, sIDs);
        }

        public static void Delete(TransactionContext tc, GeneralWorkingDayShift oGeneralWorkingDayShift, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_GeneralWorkingDayShift]" + "%n, %n, %n, %n, %n, %s",
                   oGeneralWorkingDayShift.GWDSID, oGeneralWorkingDayShift.GWDID, oGeneralWorkingDayShift.ShiftID, (int)eEnumDBOperation, nUserID, sIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_GeneralWorkingDayShift WHERE GWDID = %n ORDER BY GWDSID", id);
        }
        #endregion
    }
}
