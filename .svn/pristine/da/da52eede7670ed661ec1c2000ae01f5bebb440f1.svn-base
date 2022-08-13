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


    public class DevelopementRecapMgtReportDA
    {


        public static IDataReader Gets(string sSql, int ReportFormat, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_DevelopementRecapMgtReport]" + "%s,%n", sSql, ReportFormat);
        }

       
 



       


    }  
    
   
}
