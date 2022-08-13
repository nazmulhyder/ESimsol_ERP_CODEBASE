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
            //string sConnection1 = "Data Source=ICS-02; User ID=sa;password=welt12#;Initial Catalog=ESimSol_ERP";
            //string sConnection1 = "Data Source= 203.190.10.36;User ID=esimsolsa; password=1esimsoliC$sa;Initial Catalog=ESimSol_GERP_T007";
           // string sConnection1 = "Data Source=ICS-Server; User ID=sa;password=welt12#;Initial Catalog=ESimSol_T007";
            //string sConnection1 = "Data Source=ICS-Server; User ID=sa;password=welt12#;Initial Catalog=ESimSol_ERP_TEST";
            
            //live database be carefull
            string sConnection1 = "Data Source=203.190.10.36;User ID=B007TESTUser;password=welt12#;Initial Catalog=B007Test";
            //string sConnection1 = "Data Source=203.202.242.155;User ID=esimsolsa;password=1esimsol@W9sa;Initial Catalog=ESimSol_GERP_TNZ";
            return new SqlConnection(sConnection1);
            #endregion
        }
        public SQLFactory() { }
    }
    #endregion
}