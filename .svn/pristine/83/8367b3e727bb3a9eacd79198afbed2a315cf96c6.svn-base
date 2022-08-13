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
    public class FabricClaimDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FabricClaim oFabricClaim, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricClaim]"
                                   + "%n,%n,%n,%s,  %s,%D,%s,  %s,%s,%s,  %n,%n",
                                   oFabricClaim.FabricClaimID, oFabricClaim.FSCID, oFabricClaim.ParentFSCID, oFabricClaim.ParentSCNo, oFabricClaim.SCNoFull, oFabricClaim.SCDate, oFabricClaim.Subject, oFabricClaim.Remarks, oFabricClaim.Note_Checked, oFabricClaim.Note_Approve, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricClaim oFabricClaim, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricClaim]"
                                   + "%n,%n,%n,%s,  %s,%D,%s,  %s,%s,%s,  %n,%n",
                                   oFabricClaim.FabricClaimID, oFabricClaim.FSCID, oFabricClaim.ParentFSCID, oFabricClaim.ParentSCNo, oFabricClaim.SCNoFull, oFabricClaim.SCDate, oFabricClaim.Subject, oFabricClaim.Remarks, oFabricClaim.Note_Checked, oFabricClaim.Note_Approve, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_FabricClaim] WHERE FabricClaimID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM [View_FabricClaim]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
