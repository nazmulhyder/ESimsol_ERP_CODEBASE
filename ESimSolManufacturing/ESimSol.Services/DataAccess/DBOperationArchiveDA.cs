using ESimSol.BusinessObjects;
using ICS.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.DataAccess
{
    public class DBOperationArchiveDA
    {
        public DBOperationArchiveDA() { }

        #region Insert Function
        public static void Insert(TransactionContext tc, EnumDBOperation eDBOperationType, EnumModuleName eModuleName, int nDBRefObjID, string sTableName, string sPKColName, string sBUColName, string sRefTextColName, long nDBUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_DBOperationArchive] %n, %n, %n, %s, %s, %s, %s, %n",
                                    eDBOperationType, eModuleName, nDBRefObjID, sTableName, sPKColName, sBUColName, sRefTextColName, nDBUserID);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
