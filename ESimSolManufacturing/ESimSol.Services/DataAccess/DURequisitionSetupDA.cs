using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class DURequisitionSetupDA
    {
        public DURequisitionSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DURequisitionSetup oDURequisitionSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_DURequisitionSetup]"
                                    + "%n,%n,%s ,%s,  %n, %n,  %n, %n",
                                    oDURequisitionSetup.DURequisitionSetupID, oDURequisitionSetup.InOutTypeInt, oDURequisitionSetup.Name, oDURequisitionSetup.ShortName, oDURequisitionSetup.WorkingUnitID_Issue, oDURequisitionSetup.WorkingUnitID_Receive, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, DURequisitionSetup oDURequisitionSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_DURequisitionSetup]"
                                    + "%n,%n,%s ,%s,  %n, %n,  %n, %n",
                                    oDURequisitionSetup.DURequisitionSetupID, oDURequisitionSetup.InOutTypeInt, oDURequisitionSetup.Name, oDURequisitionSetup.ShortName, oDURequisitionSetup.WorkingUnitID_Issue, oDURequisitionSetup.WorkingUnitID_Receive, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM DURequisitionSetup WHERE DURequisitionSetupID=%n", nID);
        }

        public static IDataReader GetByType(TransactionContext tc, int nInOutType)
        {
            return tc.ExecuteReader("SELECT * FROM DURequisitionSetup WHERE InOutType=%n AND Activity=1", nInOutType);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DURequisitionSetup");
        }
        public static IDataReader Gets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM DURequisitionSetup WHERE BUID=%n AND Activity=1", nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Activate(TransactionContext tc, DURequisitionSetup oDURequisitionSetup)
        {
            string sSQL1 = SQLParser.MakeSQL("Update DURequisitionSetup Set Activity=~Activity WHERE DURequisitionSetupID=%n", oDURequisitionSetup.DURequisitionSetupID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM DURequisitionSetup WHERE DURequisitionSetupID=%n", oDURequisitionSetup.DURequisitionSetupID);
        }
        #endregion
    }
}