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
    public class ITaxRebateSchemeSlabDA
    {
        public ITaxRebateSchemeSlabDA() { }
        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, ITaxRebateSchemeSlab oITaxRebateSchemeSlab, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ITaxRebateSchemeSlab] %n,%n,%n,%n,%n,%n",
                   oITaxRebateSchemeSlab.ITaxRSSID, oITaxRebateSchemeSlab.ITaxRebateSchemeID,
                   oITaxRebateSchemeSlab.MinAmount, oITaxRebateSchemeSlab.MaxAmount,
                   nUserID, nDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
