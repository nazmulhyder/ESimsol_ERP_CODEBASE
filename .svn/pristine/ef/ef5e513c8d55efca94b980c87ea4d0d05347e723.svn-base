using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class ServiceChargeDA
    {
        public ServiceChargeDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ServiceCharge oServiceCharge, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ServiceCharge]" + "%n, %s, %s, %n"
                , oServiceCharge.ServiceChargeID
                , oServiceCharge.Name
                , oServiceCharge.Note
                , nDBOperation
            );
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM ServiceCharge WHERE ServiceChargeID="+id);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }



        #endregion
    }
}

