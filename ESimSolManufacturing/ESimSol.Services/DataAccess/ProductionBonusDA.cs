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
    public class ProductionBonusDA
    {
        public ProductionBonusDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ProductionBonus oProductionBonus, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductionBonus] %n,%n,%n,%n,%n,%b,%b,%n,%n",
                   oProductionBonus.ProductionBonusID, oProductionBonus.SalarySchemeID,
                   oProductionBonus.MinAmount, oProductionBonus.MaxAmount,
                   oProductionBonus.Value, oProductionBonus.IsPercent, oProductionBonus.IsActive, nUserID, nDBOperation);

        }
        public static IDataReader ActivityStatus(int nProductionBonusID, bool IsActive, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE ProductionBonus SET IsActive=%b WHERE ProductionBonusID=%n;SELECT * FROM ProductionBonus WHERE ProductionBonusID=%n", IsActive, nProductionBonusID, nProductionBonusID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
