using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class FabricBeamFinishDA
    {
        public FabricBeamFinishDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricBeamFinish oFabricBeamFinish, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBeamFinish]" + "%n, %n, %n, %n, %D, %n, %s, %s, %b, %n, %n",
                                    oFabricBeamFinish.FabricBeamFinishID, oFabricBeamFinish.FESDID, oFabricBeamFinish.DyeingOrderID, oFabricBeamFinish.FEOSID, oFabricBeamFinish.ReadyDate, oFabricBeamFinish.LengthFinish, oFabricBeamFinish.Note, oFabricBeamFinish.BeamNo, oFabricBeamFinish.IsFinish, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricBeamFinish oFabricBeamFinish, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBeamFinish]" + "%n, %n, %n, %n, %D, %n, %s, %s, %b, %n, %n",
                                    oFabricBeamFinish.FabricBeamFinishID, oFabricBeamFinish.FESDID, oFabricBeamFinish.DyeingOrderID, oFabricBeamFinish.FEOSID, oFabricBeamFinish.ReadyDate, oFabricBeamFinish.LengthFinish, oFabricBeamFinish.Note, oFabricBeamFinish.BeamNo, oFabricBeamFinish.IsFinish, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBeamFinish WHERE FabricBeamFinishID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBeamFinish ORDER BY Sequence ASC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
