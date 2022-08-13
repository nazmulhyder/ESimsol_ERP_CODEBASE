using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
using System.Linq;
namespace ESimSol.Services.DataAccess
{
    public class MachineTypeDA
    {
        public MachineTypeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, MachineType oMachineType, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_MachineType]"
                                    + "%n, %s, %s, %n, %s, %n, %n",
                                    oMachineType.MachineTypeID, oMachineType.Name, oMachineType.Note, oMachineType.BUID, oMachineType.ModuleIDs, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, MachineType oMachineType, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_MachineType]"
                                    + "%n, %s, %s, %n, %s, %n, %n",
                                    oMachineType.MachineTypeID, oMachineType.Name, oMachineType.Note, oMachineType.BUID, oMachineType.ModuleIDs, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM MachineType WHERE MachineTypeID=%n", nID);
        }
     
        public static IDataReader GetBy(TransactionContext tc, int nDyeingStepType)
        {
            return tc.ExecuteReader("SELECT * FROM MachineType WHERE DyeingStepType=%n", nDyeingStepType);
        }

        public static IDataReader GetsByModuleIDs(TransactionContext tc, string Ids)
        {
            return tc.ExecuteReader("SELECT * FROM MachineType WHERE MachineType IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID IN ( "+Ids+" ))");
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM MachineType");
        }
      
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Activate(TransactionContext tc, MachineType oMachineType)
        {
            string sSQL1 = SQLParser.MakeSQL("Update MachineType Set Activity=~Activity WHERE MachineTypeID=%n", oMachineType.MachineTypeID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM MachineType WHERE MachineTypeID=%n", oMachineType.MachineTypeID);

        }
    
     
    
        #endregion
    }
}