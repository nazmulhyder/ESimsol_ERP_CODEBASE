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
    public class BlockMachineMappingSupervisorDA
    {
        public BlockMachineMappingSupervisorDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, BlockMachineMappingSupervisor oBlockMachineMappingSupervisor, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BlockMachineMappingSupervisor] %n,%n,%n,%d,%b,%n,%n",
                   oBlockMachineMappingSupervisor.BMMSID, oBlockMachineMappingSupervisor.BMMID,oBlockMachineMappingSupervisor.EmployeeID,
                   oBlockMachineMappingSupervisor.StartDate, oBlockMachineMappingSupervisor.IsActive,
                   nUserID, nDBOperation);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nBMMSID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BlockMachineMappingSupervisor WHERE BMMSID=%n", nBMMSID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BlockMachineMappingSupervisor");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
