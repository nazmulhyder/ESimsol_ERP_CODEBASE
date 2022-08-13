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
    public class ShipmentRegisterDA
    {

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ShipmentRegister WHERE ShipmentDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nShipmentID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ShipmentRegister WHERE ShipmentDetailID =%n", nShipmentID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
