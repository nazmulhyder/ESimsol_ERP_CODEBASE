using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{

    public class FabricMachineDA
    {
        public FabricMachineDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricMachine oFabricMachine, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricMachine]"
                                    + "%n,%n,%n,%s,%s,%s,%n,%n,%b,%n,%b,%n,%n,%b,%n,%n",
                                    oFabricMachine.FMID, (int)oFabricMachine.WeavingProcess, oFabricMachine.CCID, oFabricMachine.Code, oFabricMachine.Name, oFabricMachine.Capacity, oFabricMachine.RPM, oFabricMachine.StandardEfficiency, oFabricMachine.IsActive, (int)oFabricMachine.MachineStatus, oFabricMachine.IsBeam, oFabricMachine.ChildMachineTypeID, oFabricMachine.FabricMachineGroupID, oFabricMachine.IsDirect, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, FabricMachine oFabricMachine, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricMachine]"
                                    + "%n,%n,%n,%s,%s,%s,%n,%n,%b,%n,%b,%n,%n,%b,%n,%n",
                                    oFabricMachine.FMID, (int)oFabricMachine.WeavingProcess, oFabricMachine.CCID, oFabricMachine.Code, oFabricMachine.Name, oFabricMachine.Capacity, oFabricMachine.RPM, oFabricMachine.StandardEfficiency, oFabricMachine.IsActive, (int)oFabricMachine.MachineStatus, oFabricMachine.IsBeam, oFabricMachine.ChildMachineTypeID, oFabricMachine.FabricMachineGroupID, oFabricMachine.IsDirect, (int)eEnumDBOperation, nUserID);
        }
        public static void ActiveInActive(TransactionContext tc, int id, bool bIsActive)
        {
           if(bIsActive)
           {
               tc.ExecuteNonQuery("Update FabricMachine SET IsActive = %b WHERE FMID = %n", bIsActive, id);
           }
           else
           {
               tc.ExecuteNonQuery("Update FabricMachine SET IsActive = %b, InActiveDate = %d WHERE FMID = %n", bIsActive, DateTime.Today, id);
           }
        }
        
        public static IDataReader LoomMachineRestore(TransactionContext tc,int FMID)
        {
            return tc.ExecuteReader("EXEC [SP_RestoreFabricMachine]"
                                    + "%n",
                                    FMID);
        }

        public static IDataReader HoldBeamFinishForLoomProcess(TransactionContext tc, int FMID)
        {
            return tc.ExecuteReader("EXEC [SP_HoldBeamFinishForLoomProcess]"
                                    + "%n",
                                    FMID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricMachine WHERE FMID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricMachine");
        }
        public static IDataReader Gets(int nWeavingProcess, int nMachineStatus, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricMachine WHERE WeavingProcess = %n AND MachineStatus = %n", nWeavingProcess, nMachineStatus);
        }
        public static IDataReader Gets(bool bIsBeam, int nWeavingProcess, int nMachineStatus, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricMachine WHERE IsBeam=%b AND WeavingProcess = %n AND MachineStatus = %n", bIsBeam, nWeavingProcess, nMachineStatus);
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }  
  
}
