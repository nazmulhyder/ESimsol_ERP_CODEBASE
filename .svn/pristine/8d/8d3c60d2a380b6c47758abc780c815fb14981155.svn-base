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
    public class ShiftBreakNameDA
    {
        public ShiftBreakNameDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ShiftBreakName oShiftBreakName, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ShiftBreakName] %n,%s,%b,%n,%n",
                   oShiftBreakName.ShiftBNID, oShiftBreakName.Name,
                   oShiftBreakName.IsActive,
                   nUserID, nDBOperation);

        }
        public static IDataReader Activity(int nShiftBNID, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE ShiftBreakName SET IsActive=~IsActive WHERE ShiftBNID=%n;SELECT * FROM ShiftBreakName WHERE ShiftBNID=%n", nShiftBNID, nShiftBNID);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nShiftBNID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ShiftBreakName WHERE ShiftBNID=%n", nShiftBNID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ShiftBreakName");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
