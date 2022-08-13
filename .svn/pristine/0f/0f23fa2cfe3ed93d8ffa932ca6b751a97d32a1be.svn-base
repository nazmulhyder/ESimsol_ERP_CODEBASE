using System;
using System.Data;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ICS.Core.DataAccess.OleDb
{
    #region DataAccess: OleDb Connection Factory
    public class OleDbFactory : ConnectionFactory
    {
        private const string passwordKey = "password";        
        public override System.Data.IDbConnection CreateConnection()
        {
            #region Making Connection String
            string password = "", keyValue = System.Configuration.ConfigurationSettings.AppSettings["ICS.Data.ConntectionString"];
            string[] tokens = keyValue.Split(';'); keyValue = "";
            foreach (string token in tokens)
            {
                string[] tokenValues = token.Split('=');
                if (tokenValues.Length > 1)
                {
                    if (tokenValues[0] == passwordKey)
                    {
                        password = Global.Decrypt(tokenValues[1]);
                        keyValue = keyValue + ";" + passwordKey + "=" + password;
                    }
                    else
                    {
                        if (keyValue.Length > 0)
                        {
                            keyValue = keyValue + ";";
                        }
                        keyValue = keyValue + token;
                    }
                }
            }
            #endregion
            return new OleDbConnection(keyValue);
        }

        public OleDbFactory() { }
    }
    #endregion
}
