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
    public class FabricClaimDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FabricClaimDetail oFabricClaimDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string FabricClaimDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricClaimDetail]"
                                   + "%n,%n,%n,  %n,%n,%s,  %n,%s,%n,%n,%s",
                                   oFabricClaimDetail.FabricClaimDetailID, oFabricClaimDetail.FabricClaimID, oFabricClaimDetail.ClaimSettlementType, oFabricClaimDetail.FSCDID, oFabricClaimDetail.ParentFSCDID, oFabricClaimDetail.ParentExeNo, oFabricClaimDetail.QtyInPercent, oFabricClaimDetail.Remarks, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, FabricClaimDetail oFabricClaimDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string FabricClaimDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricClaimDetail]"
                                   + "%n,%n,%n,  %n,%n,%s,  %n,%s,%n,%n,%s",
                                   oFabricClaimDetail.FabricClaimDetailID, oFabricClaimDetail.FabricClaimID, oFabricClaimDetail.ClaimSettlementType, oFabricClaimDetail.FSCDID, oFabricClaimDetail.ParentFSCDID, oFabricClaimDetail.ParentExeNo, oFabricClaimDetail.QtyInPercent, oFabricClaimDetail.Remarks, nUserID, (int)eEnumDBOperation, FabricClaimDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_FabricClaimDetail] WHERE FabricClaimDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM [View_FabricClaimDetail]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
