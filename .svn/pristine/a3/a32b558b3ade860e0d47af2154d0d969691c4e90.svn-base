using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class DUReturnChallanDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, DUReturnChallan oDUReturnChallan, short nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUReturnChallan]"
                                   + "%n,%s,%d,%n,%n,%s,%s,%s,%n,%n,%n",
                                   oDUReturnChallan.DUReturnChallanID, oDUReturnChallan.DUReturnChallanNo, oDUReturnChallan.ReturnDate, oDUReturnChallan.DUDeliveryChallanID, oDUReturnChallan.BUID,  oDUReturnChallan.Note, oDUReturnChallan.RefChallanNo, oDUReturnChallan.VehicleInfo,  oDUReturnChallan.WorkingUnitID, nUserID, nDBOperation);
        }

        public static void Delete(TransactionContext tc, DUReturnChallan oDUReturnChallan, short nDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DUReturnChallan]"
                                     + "%n,%s,%d,%n,%n,%s,%s,%s,%n,%n,%n",
                                   oDUReturnChallan.DUReturnChallanID, oDUReturnChallan.DUReturnChallanNo, oDUReturnChallan.ReturnDate, oDUReturnChallan.DUDeliveryChallanID, oDUReturnChallan.BUID, oDUReturnChallan.Note, oDUReturnChallan.RefChallanNo, oDUReturnChallan.VehicleInfo, oDUReturnChallan.WorkingUnitID, nUserID, nDBOperation);
        }
        public static IDataReader Receive(TransactionContext tc, int nDUReturnChallanID, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_CommitDUReturnChallan]"
                                 + "%n,%n",nDUReturnChallanID, nUserID);

        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nDUReturnChallanID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DUReturnChallan WHERE DUReturnChallanID=%n", nDUReturnChallanID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
