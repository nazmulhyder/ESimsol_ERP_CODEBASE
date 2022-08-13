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
    public class ITaxRateSlabDA
    {
        public ITaxRateSlabDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ITaxRateSlab oITaxRateSlab, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ITaxRateSlab] %n,%n,%n,%n,%n,%n,%n",
                   oITaxRateSlab.ITaxRateSlabID, oITaxRateSlab.ITaxRateSchemeID,
                   oITaxRateSlab.SequenceType, oITaxRateSlab.Amount,oITaxRateSlab.Percents, nUserID, nDBOperation);


        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nITaxRateSlabID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ITaxRateSlab WHERE ITaxRateSlabID=%n", nITaxRateSlabID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ITaxRateSlab");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
