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
    public class ITaxRebateItemDA
    {
        public ITaxRebateItemDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ITaxRebateItem oITaxRebateItem, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ITaxRebateItem] %n,%n,%u,%n,%n",
                   oITaxRebateItem.ITaxRebateItemID, oITaxRebateItem.ITaxRebateType, oITaxRebateItem.Description, nUserID, nDBOperation);

        }

        public static IDataReader Activity(int nITaxRebateItemID, bool IsActive, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE ITaxRebateItem SET IsActive=%b WHERE ITaxRebateItemID=%n;SELECT * FROM ITaxRebateItem WHERE ITaxRebateItemID=%n", IsActive, nITaxRebateItemID, nITaxRebateItemID);
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nITaxRebateItemID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ITaxRebateItem WHERE ITaxRebateItemID=%n", nITaxRebateItemID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ITaxRebateItem");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
