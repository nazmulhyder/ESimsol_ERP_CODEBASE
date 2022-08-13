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

    public class MasterLCSummeryDA
    {
        public MasterLCSummeryDA() { }

        #region Insert Update Delete Function
        public static IDataReader Gets(TransactionContext tc, string sIDs)
        {
            return tc.ExecuteReader("EXEC[SP_MasterLCSummery]"+ " %s",sIDs);
        }
        #endregion

    }  
    
   
}
