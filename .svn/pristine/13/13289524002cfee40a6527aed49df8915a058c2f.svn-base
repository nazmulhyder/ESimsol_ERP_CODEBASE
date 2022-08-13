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
    public class ShiftBULocConfigureDA
    {
        public ShiftBULocConfigureDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ShiftBULocConfigure oShiftBULocConfigure, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ShiftBULocConfigure]" + "%n, %n, %n, %n, %n"
                , oShiftBULocConfigure.ShiftBULocID
                , oShiftBULocConfigure.BUID
                , oShiftBULocConfigure.LocationID
                , oShiftBULocConfigure.ShiftID
                , nUserID
            );
        }
        public static void Delete(TransactionContext tc, int nBUID, int nLocationID, Int64 nUserID)
        {
            tc.ExecuteNonQuery("DELETE FROM ShiftBULocConfigure WHERE BUID = " + nBUID + " AND LocationID=" + nLocationID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}


