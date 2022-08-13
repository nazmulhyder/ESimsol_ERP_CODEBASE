using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DUProductionStatusReportDA
    {
        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, EnumReportLayout eEnumReportLayout, string sSQL)
        {
            return tc.ExecuteReader("Exec[SP_ProductionStatusReport] %s, %n", sSQL, (int)eEnumReportLayout);
        }
        #endregion
    }  
  }
