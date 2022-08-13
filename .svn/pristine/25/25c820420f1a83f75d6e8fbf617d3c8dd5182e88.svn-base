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
    public class VOrderRegisterDA
    {
        public VOrderRegisterDA() { }        

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, VOrderRegister oVOrderRegister, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_VOrderRegister]" + "%n, %b, %d, %d, %s,%n", oVOrderRegister.VOrderRefTypeInt, oVOrderRegister.IsDateApply, Convert.ToDateTime(oVOrderRegister.StartDate), Convert.ToDateTime(oVOrderRegister.EndDate), sSQL, oVOrderRegister.ReportLayoutInt );
        }
        #endregion
    }  
}
