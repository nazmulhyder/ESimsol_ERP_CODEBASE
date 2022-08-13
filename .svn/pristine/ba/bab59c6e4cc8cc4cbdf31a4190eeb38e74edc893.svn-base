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

    public class PTUUnit2LogDA
    {
        #region Insert Update Delete Function

       

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PTUUnit2Log WHERE PTUUnit2LogID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nPTUUnit2ID, int eRefType)
        {
            return tc.ExecuteReader("SELECT * FROM View_PTUUnit2Log WHERE PTUUnit2ID=%n AND PTUUnit2RefType = %n", nPTUUnit2ID, eRefType);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
 
}
