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
    public class CommissionMaterialDA
    {
        public CommissionMaterialDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, CommissionMaterial oCommissionMaterial, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CommissionMaterial] %n,%s,%s,%b,%n,%n",
                   oCommissionMaterial.CMID, oCommissionMaterial.Name, oCommissionMaterial.SearchWhat, oCommissionMaterial.IsActive,
                   nUserID, nDBOperation);
        }

        public static IDataReader Activity(int nCMID, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE CommissionMaterial SET IsActive=~IsActive WHERE CMID=%n;SELECT * FROM CommissionMaterial WHERE CMID=%n", nCMID, nCMID);
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nCMID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CommissionMaterial WHERE CMID=%n", nCMID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CommissionMaterial");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
