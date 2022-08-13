using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    class DUDyeingTypeDA
    {
        public DUDyeingTypeDA() { }
        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, DUDyeingType oDUDT, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUDyeingType] " + " %n, %n ,%s,%n ,%b ,%n ,%n  ", oDUDT.DUDyeingTypeID, (int)oDUDT.DyeingType, oDUDT.Name,oDUDT.Capacity, oDUDT.Activity, nUserID, nDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nDUDyeingTypeID)
        {
            return tc.ExecuteReader("SELECT * FROM DUDyeingType WHERE DUDyeingTypeID=%n", nDUDyeingTypeID);
        }
        public static IDataReader GetsActivity(TransactionContext tc, bool bActivity)
        {
            return tc.ExecuteReader("SELECT * FROM DUDyeingType WHERE Activity =%b", bActivity);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
