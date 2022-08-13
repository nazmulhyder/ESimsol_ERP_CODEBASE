using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.Utility;

namespace ICS.Core.DataAccess.SQL
{
    #region DataAccess: SQL Connection Factory
    /// <summary>
    /// Summary description for SQLFactory.
    /// </summary>
    public class SQLFactory : ConnectionFactory
    {
        private const string passwordKey = "password";
        public override System.Data.IDbConnection CreateConnection()
        {
            #region Making Connection String
            string sConnection1 = "Data Source=ICS-02; User ID=sa;password=welt12#;Initial Catalog=ESimSol_ERP";
            //string sConnection1 = "Data Source=203.190.10.36; User ID=developer;password=welt12#;Initial Catalog=ESimSol_TXERP_MITHILA"; //Mithela

            return new SqlConnection(sConnection1);
            #endregion
        }
        public SQLFactory() { }
    }
    #endregion
}