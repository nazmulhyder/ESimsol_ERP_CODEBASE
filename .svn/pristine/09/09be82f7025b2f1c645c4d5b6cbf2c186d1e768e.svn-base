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
    public class ITaxRateSchemeDA
    {
        public ITaxRateSchemeDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ITaxRateScheme oITaxRateScheme, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ITaxRateScheme] %n,%n,%n,%n,%n,%n,%n,%n",
                   oITaxRateScheme.ITaxRateSchemeID,
                   oITaxRateScheme.ITaxAssessmentYearID,
                   oITaxRateScheme.SalaryHeadID,
                   oITaxRateScheme.TaxPayerType,
                   oITaxRateScheme.TaxArea,
                   oITaxRateScheme.MinimumTax,
                   nUserID, nDBOperation);


        }

        public static IDataReader Activity(int nITaxRateSchemeID, bool IsActive, TransactionContext tc)
        {

            return tc.ExecuteReader("UPDATE ITaxRateScheme SET IsActive=%b WHERE ITaxRateSchemeID=%n;SELECT * FROM ITaxRateScheme WHERE ITaxRateSchemeID=%n", IsActive, nITaxRateSchemeID, nITaxRateSchemeID);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nITaxRateSchemeID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ITaxRateScheme WHERE ITaxRateSchemeID=%n", nITaxRateSchemeID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ITaxRateScheme");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
