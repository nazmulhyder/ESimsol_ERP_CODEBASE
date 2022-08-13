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
    public class ITaxBasicInformationDA
    {
        public ITaxBasicInformationDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ITaxBasicInformation oITaxBasicInformation, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ITaxBasicInformation] %n,%n,%s,%s,%s,%n,%s,%s,%b,%b,%n,%n",
                   oITaxBasicInformation.ITaxBasicInformationID, oITaxBasicInformation.EmployeeID,
                   oITaxBasicInformation.TIN, oITaxBasicInformation.ETIN,
                   oITaxBasicInformation.NationalID, oITaxBasicInformation.TaxArea,
                   oITaxBasicInformation.Cercile, oITaxBasicInformation.Zone,
                   oITaxBasicInformation.IsNonResident, oITaxBasicInformation.IsSelf,
                   nUserID, nDBOperation);


        }

       
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nITaxBasicInformationID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ITaxBasicInformation WHERE ITaxBasicInformationID=%n", nITaxBasicInformationID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ITaxBasicInformation");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
