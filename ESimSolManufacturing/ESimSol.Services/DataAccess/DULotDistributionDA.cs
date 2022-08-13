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
    public class DULotDistributionDA
    {
        public DULotDistributionDA() { }


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_DULotDistribution] WHERE DULotDistributionID=%n", nID);
        }
     
        public static IDataReader Gets(TransactionContext tc, int nDODID, int nWUID)
        {
            return tc.ExecuteReader("select * from [View_DULotDistribution] where DODID=%n AND LotID IN (Select LotID FROM Lot WHERE WorkingUnitID=%n) AND Qty>0", nDODID, nWUID);
        }
        public static IDataReader GetsByWU(TransactionContext tc, int nDODID, string nWUIDs)
        {
            return tc.ExecuteReader("select * from [View_DULotDistribution] where DODID=%n AND LotID IN (Select LotID FROM Lot WHERE WorkingUnitID in (%q) ) AND Qty>0", nDODID, nWUIDs);
        }
        public static IDataReader GetsBy(TransactionContext tc, int nDODID)
        {
            return tc.ExecuteReader("select * from [View_DULotDistribution] where DODID=%n AND Qty>0", nDODID);
        }
        public static IDataReader GetsByLot(TransactionContext tc, int nLotID)
        {
            return tc.ExecuteReader(" select * from [View_DULotDistribution] where LotID=%n", nLotID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
           return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Save_Transfer(TransactionContext tc, DULotDistribution oDULotDistribution, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DULotDistribution]" + "%n,%n,%n, %n,%b, %n",
                                    oDULotDistribution.DODID, oDULotDistribution.DODID_Dest,oDULotDistribution.LotID, oDULotDistribution.Qty,oDULotDistribution.IsRaw,  nUserID);
        }
        //public static IDataReader Delete(TransactionContext tc, DULotDistribution oDULotDistribution, Int64 nUserID)
        //{
        //    return tc.ExecuteReader("EXEC [SP_IUD_DULotDistribution]" + "%n,%n,%n, %n,%b, %n",
        //                            oDULotDistribution.DODID, oDULotDistribution.DODID_Dest, oDULotDistribution.LotID, oDULotDistribution.Qty, oDULotDistribution.IsRaw, nUserID);
        //}
        public static IDataReader Save_Reduce(TransactionContext tc, DULotDistribution oDULotDistribution, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DULotDistributionReduce]" + "%n,%n,%n,%b, %n",
                                    oDULotDistribution.DULotDistributionID, oDULotDistribution.DODID,  oDULotDistribution.Qty, oDULotDistribution.IsRaw, nUserID);
        }
        #endregion

    }
}

