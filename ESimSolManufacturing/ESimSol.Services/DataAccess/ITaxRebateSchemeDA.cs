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
    public class ITaxRebateSchemeDA
    {
        public ITaxRebateSchemeDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ITaxRebateScheme oITaxRebateScheme, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ITaxRebateScheme] %n,%n,%n,%n,%n,%n,%n,%n",
                   oITaxRebateScheme.ITaxRebateSchemeID
                  ,oITaxRebateScheme.ITaxRateSchemeID
                  ,oITaxRebateScheme.ITaxRebateType
                  ,oITaxRebateScheme.MaxRebateAmount
                  , oITaxRebateScheme.PercentOfTaxIncome
                  ,oITaxRebateScheme.RebateInPercent
                  ,nUserID, nDBOperation);

        }


        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nITaxRebateSchemeID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ITaxRebateScheme WHERE ITaxRebateSchemeID=%n", nITaxRebateSchemeID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ITaxRebateScheme");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
