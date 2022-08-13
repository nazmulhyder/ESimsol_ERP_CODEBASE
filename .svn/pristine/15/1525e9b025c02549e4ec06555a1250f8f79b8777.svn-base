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
    public class ITaxRebateSchemeSlabDetailDA
    {
        public ITaxRebateSchemeSlabDetailDA() { }
        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, ITaxRebateSchemeSlabDetail oITaxRebateSchemeSlabDetail, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ITaxRebateSchemeSlabDetail] %n,%n,%n,%n,%n,%n",
                   oITaxRebateSchemeSlabDetail.ITaxRSSDID, oITaxRebateSchemeSlabDetail.ITaxRSSID,
                   oITaxRebateSchemeSlabDetail.UptoAmount, oITaxRebateSchemeSlabDetail.SlabRebateInPercent,
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
