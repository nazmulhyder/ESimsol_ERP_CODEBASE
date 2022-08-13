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
    public class EmployeeCommissionMaterialDA
    {
        public EmployeeCommissionMaterialDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeCommissionMaterial oEmployeeCommissionMaterial, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeCommissionMaterial] %n,%n,%n,%s,%s,%d,%b,%n,%n",
                   oEmployeeCommissionMaterial.ECMID, oEmployeeCommissionMaterial.CMID,oEmployeeCommissionMaterial.EmployeeID
                   ,oEmployeeCommissionMaterial.SearchWhatValue, oEmployeeCommissionMaterial.Note
                   ,oEmployeeCommissionMaterial.EffectDate,oEmployeeCommissionMaterial.IsActive
                   ,nUserID, nDBOperation);
        }

        public static IDataReader Activity(int nECMID, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE EmployeeCommissionMaterial SET IsActive=0 WHERE ECMID=%n;SELECT * FROM View_EmployeeCommissionMaterial WHERE ECMID=%n", nECMID, nECMID);
        }
        public static IDataReader Approve(string sSql, TransactionContext tc)
        {
            return tc.ExecuteReader(sSql);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nECMID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeCommissionMaterial WHERE ECMID=%n", nECMID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeCommissionMaterial");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
