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
    public class PTUDistributionDA
    {
        public PTUDistributionDA() { }


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_PTUDistribution] WHERE PTUDistributionID=%n", nID);
        }
        public static IDataReader Get(TransactionContext tc, int nPTUID, int nLotID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_PTUDistribution] WHERE PTUID=%n And LotID=%n", nPTUID, nLotID);
        }
     
        public static IDataReader Gets(TransactionContext tc, int nPTUID)
        {
            return tc.ExecuteReader(" select * from [View_PTUDistribution] where PTUID=%n", nPTUID);
        }
        public static IDataReader Gets(TransactionContext tc, int nPTUID, int nWUID)
        {
            return tc.ExecuteReader(" select * from [View_PTUDistribution] where PTUID=%n AND LotID IN (Select LotID FROM Lot WHERE WorkingUnitID=%n) AND Qty>0", nPTUID, nWUID);
        }
        public static IDataReader GetsByLot(TransactionContext tc, int nLotID)
        {
            return tc.ExecuteReader(" select * from [View_PTUDistribution] where LotID=%n", nLotID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(" %q ", sSQL);
        }
        public static IDataReader PTUToPTU_Transfer(TransactionContext tc, PTUDistribution oPTUDistribution, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [sp_IUD_PTUDistribution]" + "%n,%n, %n, %n, %n",
                                    oPTUDistribution.PTUID, oPTUDistribution.LotID, oPTUDistribution.Qty, oPTUDistribution.PTUID_Dest, nUserID);
        }
        #endregion

    }
}
