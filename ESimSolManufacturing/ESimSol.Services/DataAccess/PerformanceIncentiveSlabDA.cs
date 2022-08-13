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
    public class PerformanceIncentiveSlabDA
    {
        public PerformanceIncentiveSlabDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, PerformanceIncentiveSlab oPerformanceIncentiveSlab, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_PerformanceIncentiveSlab] %n,%n,%n,%n,%n,%b,%n,%n",
                   oPerformanceIncentiveSlab.PISlabID, oPerformanceIncentiveSlab.PIID,
                   oPerformanceIncentiveSlab.MinPoint, oPerformanceIncentiveSlab.MaxPoint,
                   oPerformanceIncentiveSlab.Value, oPerformanceIncentiveSlab.IsPercentOfRate,
                   nUserID, nDBOperation);


        }

        public static IDataReader Activity(int nPerformanceIncentiveSlabID, bool IsActive, TransactionContext tc)
        {

            return tc.ExecuteReader("UPDATE PerformanceIncentiveSlab SET IsActive=%b WHERE PerformanceIncentiveSlabID=%n;SELECT * FROM PerformanceIncentiveSlab WHERE PerformanceIncentiveSlabID=%n", IsActive, nPerformanceIncentiveSlabID, nPerformanceIncentiveSlabID);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nPerformanceIncentiveSlabID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM PerformanceIncentiveSlab WHERE PISlabID=%n", nPerformanceIncentiveSlabID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM PerformanceIncentiveSlab");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
