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
    public class BlockMachineMappingDetailDA
    {
        public BlockMachineMappingDetailDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, BlockMachineMappingDetail oBlockMachineMappingDetail, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BlockMachineMappingDetail] %n,%n,%s,%b,%n,%n",
                   oBlockMachineMappingDetail.BMMDID, oBlockMachineMappingDetail.BMMID,
                    oBlockMachineMappingDetail.MachineNo, oBlockMachineMappingDetail.IsActive,
                   nUserID, nDBOperation);

        }

        public static IDataReader Activity(int nBlockMachineMappingDetailID, bool IsActive, TransactionContext tc)
        {

            return tc.ExecuteReader("UPDATE BlockMachineMappingDetail SET IsActive=%b WHERE BMMDID=%n;SELECT * FROM BlockMachineMappingDetail WHERE BMMDID=%n", IsActive, nBlockMachineMappingDetailID, nBlockMachineMappingDetailID);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nBMMDID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM BlockMachineMappingDetail WHERE BMMDID=%n", nBMMDID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM BlockMachineMappingDetail");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
