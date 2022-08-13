using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ProductionReportDA
    {
        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("Exec[SP_ProductionReport]"+"%s",sSQL);
        }
        #endregion
    }  
    
  }
