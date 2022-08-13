using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class FabricMachineTypeDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FabricMachineType oFabricMachineType, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricMachineType]"
                                   + "%n,%s,%s,%n,%s,%n,%n",
                                   oFabricMachineType.FabricMachineTypeID, oFabricMachineType.Name, oFabricMachineType.Brand, oFabricMachineType.ParentID, oFabricMachineType.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricMachineType oFabricMachineType, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricMachineType]"
                                    + "%n,%s,%s,%n,%s,%n,%n",
                                    oFabricMachineType.FabricMachineTypeID, oFabricMachineType.Name, oFabricMachineType.Brand, oFabricMachineType.ParentID, oFabricMachineType.Note, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricMachineType WHERE FabricMachineTypeID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM FabricMachineType");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
