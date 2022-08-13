using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DUDyeingTypeMappingDA
    {
        public DUDyeingTypeMappingDA() { }
        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, DUDyeingTypeMapping oDUDTM, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DUDyeingTypeMapping] " + " %n, %n ,%n ,%n ,%n ,%n   ", oDUDTM.DyeingTypeMappingID, (int)oDUDTM.DyeingType, oDUDTM.ProductID, oDUDTM.Unit, nUserID, nDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nDyeingTypeMappingID)
        {
            return tc.ExecuteReader("SELECT * FROM DUDyeingTypeMapping WHERE DyeingTypeMappingID=%n", nDyeingTypeMappingID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
