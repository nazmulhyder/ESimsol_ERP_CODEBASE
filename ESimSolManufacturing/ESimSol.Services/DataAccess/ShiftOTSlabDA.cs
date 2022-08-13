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
    public class ShiftOTSlabDA
    {
        public ShiftOTSlabDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ShiftOTSlab oShiftOTSlab, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ShiftOTSlab] %n,%n,%n,%n,%n,%b,%n,%n,%n,%n,%n,%b",
                   oShiftOTSlab.ShiftOTSlabID, oShiftOTSlab.ShiftID, oShiftOTSlab.MinOTInMin,
                   oShiftOTSlab.MaxOTInMin, oShiftOTSlab.AchieveOTInMin, oShiftOTSlab.IsActive,
                   nUserID, nDBOperation,oShiftOTSlab.CompMinOTInMin,
                   oShiftOTSlab.CompMaxOTInMin, oShiftOTSlab.CompAchieveOTInMin, oShiftOTSlab.IsCompActive);

        }
        public static IDataReader Activity(int nShiftOTSlabID, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE ShiftOTSlab SET IsActive=~IsActive WHERE ShiftOTSlabID=%n;SELECT * FROM ShiftOTSlab WHERE ShiftOTSlabID=%n", nShiftOTSlabID, nShiftOTSlabID);
        }
        public static IDataReader ActivityComp(int nShiftOTSlabID, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE ShiftOTSlab SET IsCompActive=~IsCompActive WHERE ShiftOTSlabID=%n;SELECT * FROM ShiftOTSlab WHERE ShiftOTSlabID=%n", nShiftOTSlabID, nShiftOTSlabID);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nShiftOTSlabID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ShiftOTSlab WHERE ShiftOTSlabID=%n", nShiftOTSlabID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ShiftOTSlab");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
