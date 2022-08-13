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

    public class MerchandiserDashboardDA
    {
        public static IDataReader Gets(TransactionContext tc, string sMainSQL, string sPOSQL, string sORSQL, string sCSSQL, string sPESQL, string sPendingSQL, string sCompleteSQL)
        {
            return tc.ExecuteReader("EXEC [dbo].[SP_MerchandiserDashboard]" + "%s,%s,%s,%s,%s,%s,%s", sMainSQL, sPOSQL, sORSQL, sCSSQL, sPESQL, sPendingSQL, sCompleteSQL);
        } 
    }
    
    
  
}
