using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class LabDipSetupDA
    {
        public LabDipSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, LabDipSetup oLabDipSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_LabDipSetup]"
                                    + "%n, %s,%s,%s, %s, %n, %n,%b,%b, %n, %n",
                                    oLabDipSetup.LabDipSetupID, oLabDipSetup.OrderCode, oLabDipSetup.OrderName, oLabDipSetup.ColorNoName, oLabDipSetup.LDName, oLabDipSetup.LDNoCreateBy, oLabDipSetup.PrintNo, oLabDipSetup.IsApplyCode, oLabDipSetup.IsApplyPO, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, LabDipSetup oLabDipSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_LabDipSetup]"
                                    + "%n, %s,%s,%s, %s, %n, %n,%b,%b, %n, %n",
                                 oLabDipSetup.LabDipSetupID, oLabDipSetup.OrderCode, oLabDipSetup.OrderName, oLabDipSetup.ColorNoName, oLabDipSetup.LDName, oLabDipSetup.LDNoCreateBy, oLabDipSetup.PrintNo, oLabDipSetup.IsApplyCode, oLabDipSetup.IsApplyPO, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM LabDipSetup WHERE LabDipSetupID=%n", nID);
        }

        public static IDataReader GetByType(TransactionContext tc, int nOrderType)
        {
            return tc.ExecuteReader("SELECT * FROM LabDipSetup WHERE OrderType=%n and Activity=1", nOrderType);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM LabDipSetup");
        }
        public static IDataReader Gets(TransactionContext tc,int nBUID )
        {
            return tc.ExecuteReader("SELECT * FROM LabDipSetup WHERE BUID=%n", nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Activate(TransactionContext tc, LabDipSetup oLabDipSetup)
        {
            string sSQL1 = SQLParser.MakeSQL("Update LabDipSetup Set Activity=~Activity WHERE LabDipSetupID=%n", oLabDipSetup.LabDipSetupID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM LabDipSetup WHERE LabDipSetupID=%n", oLabDipSetup.LabDipSetupID);

        }
        #endregion
    }
}