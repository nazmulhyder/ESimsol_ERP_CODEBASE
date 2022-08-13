using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class MachineDA
    {
        public MachineDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Machine oMachine, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_Machine]"
                                    + "%n,%n,%n, %s,%s,%s, %n,%b,%n, %s, %n,%n",
                                    oMachine.MachineID, oMachine.MachineTypeID, oMachine.LocationID, oMachine.Name, oMachine.Code, oMachine.Note, oMachine.Capacity, oMachine.Activity, oMachine.BUID, oMachine.Capacity2, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, Machine oMachine, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_Machine]"
                                      + "%n,%n,%n, %s,%s,%s, %n,%b,%n, %s, %n,%n",
                                    oMachine.MachineID, oMachine.MachineTypeID, oMachine.LocationID, oMachine.Name, oMachine.Code, oMachine.Note, oMachine.Capacity, oMachine.Activity, oMachine.BUID, oMachine.Capacity2, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Machine WHERE MachineID=%n", nID);
        }

        public static IDataReader GetByType(TransactionContext tc, int nMachineTypeID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Machine WHERE MachineTypeID=%n and Activity=1 ORDER BY SequenceNo", nMachineTypeID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Machine ORDER BY SequenceNo");
        }
        public static IDataReader GetsBy(TransactionContext tc,int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Machine WHERE Activity=1 and BUID=%n ORDER BY SequenceNo", nBUID);
        }
        public static IDataReader GetsByModule(TransactionContext tc, int nBUID,string sModuleIDs )
        {
            return tc.ExecuteReader("SELECT * FROM View_Machine WHERE MachineTypeID in (Select MachineTypeID from MachineType where BUID=%n and  MachineTypeID IN (SELECT MachineTypeID FROM MachineModuleMapping WHERE ModuleID IN ( " + sModuleIDs + " )))and Activity=1  ORDER BY SequenceNo", nBUID);
        }
        public static IDataReader GetsActive(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Machine WHERE Activity=1 ORDER BY SequenceNo");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Activate(TransactionContext tc, Machine oMachine)
        {
            string sSQL1 = SQLParser.MakeSQL("Update Machine Set Activity=~Activity WHERE MachineID=%n", oMachine.MachineID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_Machine WHERE MachineID=%n", oMachine.MachineID);
        }

        public static void UpdateSequence(TransactionContext tc, Machine oMachine, Int64 nUserId)
        {
            tc.ExecuteNonQuery("UPDATE Machine SET SequenceNo=%n WHERE MachineID=%n", oMachine.SequenceNo, oMachine.MachineID);
            //return tc.ExecuteReader("SELECT * FROM View_Machine WHERE MachineID=%n", oMachine.MachineID);
        }
    
        #endregion
    }
}