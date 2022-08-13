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
    public class EmployeeTINInformationDA
    {
        public EmployeeTINInformationDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, EmployeeTINInformation oEmployeeTINInformation, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeTINInformation] %n,%n,%s,%s,%n,%s,%s,%b,%n,%n",
                   oEmployeeTINInformation.ETINID,
                   oEmployeeTINInformation.EmployeeID,
                   oEmployeeTINInformation.TIN,
                   oEmployeeTINInformation.ETIN,
                   oEmployeeTINInformation.TaxArea,
                   oEmployeeTINInformation.Circle,
                   oEmployeeTINInformation.Zone,
                   oEmployeeTINInformation.IsNonResident,
                   nUserID, nDBOperation);


        }

        public static void Upload(TransactionContext tc, EmployeeTINInformation oEmployeeTINInformation, Int64 nUserID, int nDBOperation)
        {
                   tc.ExecuteNonQuery("EXEC [SP_IUD_EmployeeTINInformation] %n,%n,%s,%s,%n,%s,%s,%b,%n,%n",
                   oEmployeeTINInformation.ETINID,
                   oEmployeeTINInformation.EmployeeID,
                   oEmployeeTINInformation.TIN,
                   oEmployeeTINInformation.ETIN,
                   oEmployeeTINInformation.TaxArea,
                   oEmployeeTINInformation.Circle,
                   oEmployeeTINInformation.Zone,
                   oEmployeeTINInformation.IsNonResident,
                   nUserID, nDBOperation);


        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(int nEMpID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeTINInformation WHERE EmployeeID=%n", nEMpID);
        }

        #endregion
    }
}
