using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class POTandCSetupDA
    {
        public POTandCSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, POTandCSetup oPOTandCSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_POTandCSetup]"
                                    + "%n, %n, %s, %n,%s, %n,%n, %n",
                                     oPOTandCSetup.POTandCSetupID, oPOTandCSetup.ClauseType, oPOTandCSetup.Clause, oPOTandCSetup.Activity, oPOTandCSetup.Note,oPOTandCSetup.BUID, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, POTandCSetup oPOTandCSetup, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_POTandCSetup]"
                                    + "%n, %n, %s, %n,%s, %n,%n, %n",
                                     oPOTandCSetup.POTandCSetupID, oPOTandCSetup.ClauseType, oPOTandCSetup.Clause, oPOTandCSetup.Activity, oPOTandCSetup.Note, oPOTandCSetup.BUID, nUserId, (int)eEnumDBOperation);
        }
        public static void ActivatePOTandCSetup(TransactionContext tc, POTandCSetup oPOTandCSetup)
        {
            tc.ExecuteNonQuery("Update POTandCSetup Set Activity=Activity^1 Where POTandCSetupID=%n", oPOTandCSetup.POTandCSetupID);

        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nid)
        {
            return tc.ExecuteReader("SELECT * FROM POTandCSetup WHERE POTandCSetupID=%n", nid);
        }


        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM POTandCSetup order by ClauseType");
        }
        public static IDataReader Gets(TransactionContext tc, string sClauseType)
        {
            return tc.ExecuteReader("SELECT * FROM POTandCSetup WHERE Activity=1 and ClauseType in (%q)", sClauseType);
        }
        public static IDataReader Gets(TransactionContext tc, bool bActivity)
        {
            return tc.ExecuteReader("SELECT * FROM POTandCSetup Where Activity=%b", bActivity);
        }
        public static IDataReader GetsByBU(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM POTandCSetup Where BUID=%n", nBUID);
        }
        //
        //public static IDataReader Gets(TransactionContext tc, string sSQL)
        //{
        //    return tc.ExecuteReader(sSQL);
        //}
        #endregion
    }
}
