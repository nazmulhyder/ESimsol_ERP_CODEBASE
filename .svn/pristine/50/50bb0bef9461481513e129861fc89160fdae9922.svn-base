using System;
using System.Data;
using ICS.Core.DataAccess.OleDb;
using ICS.Core.DataAccess.SQL;

namespace ICS.Core.DataAccess
{
    #region DataAccess: Connection Facory
    public abstract class ConnectionFactory
    {
        private static ConnectionFactory _default;
        public static ConnectionFactory Default
        {
            get
            {
                if (_default == null)
                {
                    
                    //_default = new OleDbFactory();
                    //aaa111
                    _default = new  SQLFactory();
                }

                return _default;
            }
        }

        public abstract IDbConnection CreateConnection();
    }
    #endregion
}
