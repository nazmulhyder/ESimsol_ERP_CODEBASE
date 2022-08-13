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
    public class BlockMachineMappingDA
    {
        public BlockMachineMappingDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, BlockMachineMapping oBlockMachineMapping, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BlockMachineMapping] %n,%n,%n,%s,%b,%n,%n",
                   oBlockMachineMapping.BMMID, (int)oBlockMachineMapping.ProductionProcess,
                   oBlockMachineMapping.DepartmentID, oBlockMachineMapping.BlockName, oBlockMachineMapping.IsActive,
                   nUserID, nDBOperation);

        }

        public static IDataReader Activity(int nBlockMachineMappingID, bool IsActive, TransactionContext tc)
        {

            return tc.ExecuteReader("UPDATE BlockMachineMapping SET IsActive=%b WHERE BMMID=%n;SELECT * FROM View_BlockMachineMapping WHERE BMMID=%n", IsActive, nBlockMachineMappingID, nBlockMachineMappingID);

        }

        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nBMMID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BlockMachineMapping WHERE BMMID=%n", nBMMID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BlockMachineMapping");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
