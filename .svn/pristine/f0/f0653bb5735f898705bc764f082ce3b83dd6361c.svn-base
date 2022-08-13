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

    public class RawmaterialStatusDA
    {
        public RawmaterialStatusDA() { }

        #region Get & Exist Function

        public static IDataReader GetYarnBySaleOrderIDs(string sSaleOrderIDs, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC[SP_YarnReport]"+ "%s", sSaleOrderIDs);
        }


        public static IDataReader GetAccessoriesBySaleOrderIDs(string sSaleOrderIDs, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC[SP_AccessoriesReport]"+ "%s", sSaleOrderIDs);

        }
      
        #endregion
    }
  
}
