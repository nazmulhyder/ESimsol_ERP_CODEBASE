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

    public class TAPDetailDA
    {
        public TAPDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TAPDetail oTAPDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sTAPDetailIDs)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_TAPDetail]"
                                    + "%n,%n,%n,%n,%d,%d,%n,%n,%s, %s,%n,%n,%s", oTAPDetail.TAPDetailID, oTAPDetail.TAPID, oTAPDetail.OrderStepID, oTAPDetail.Sequence, oTAPDetail.ApprovalPlanDate, oTAPDetail.SubmissionDate, oTAPDetail.ReqSubmissionDays, oTAPDetail.ReqBuyerApprovalDays, oTAPDetail.SubStepName, oTAPDetail.Remarks, nUserID, (int)eEnumDBOperation, sTAPDetailIDs);
        }

        public static void Delete(TransactionContext tc, TAPDetail oTAPDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sTAPDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_TAPDetail]"
                                    + "%n,%n,%n,%n,%d,%d,%n,%n,%s, %s,%n,%n,%s", oTAPDetail.TAPDetailID, oTAPDetail.TAPID, oTAPDetail.OrderStepID, oTAPDetail.Sequence, oTAPDetail.ApprovalPlanDate, oTAPDetail.SubmissionDate, oTAPDetail.ReqSubmissionDays, oTAPDetail.ReqBuyerApprovalDays, oTAPDetail.SubStepName, oTAPDetail.Remarks, nUserID, (int)eEnumDBOperation, sTAPDetailIDs);
        }

        #endregion
        #region Insert Update Delete Function For Factory TAP
        public static IDataReader InsertUpdateFactoryTAP(TransactionContext tc, TAPDetail oTAPDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sTAPDetailIDs)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_FactoryTAPDetail]"
                                    + "%n,%n,%n,%n,%d,%s,%n,%n,%s", oTAPDetail.TAPDetailID, oTAPDetail.TAPID, oTAPDetail.OrderStepID, oTAPDetail.Sequence, oTAPDetail.ApprovalPlanDate, oTAPDetail.Remarks, nUserID, (int)eEnumDBOperation, "");
        }

        public static void DeleteFactoryTAP(TransactionContext tc, TAPDetail oTAPDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sTAPDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_FactoryTAPDetail]" +
                                    "%n,%n,%n,%n,%d,%s,%n,%n,%s", oTAPDetail.TAPDetailID, oTAPDetail.TAPID, oTAPDetail.OrderStepID, oTAPDetail.Sequence, oTAPDetail.ApprovalPlanDate, oTAPDetail.Remarks, nUserID, (int)eEnumDBOperation, sTAPDetailIDs);
        }

        #endregion

        #region Set Sequence
        public static void UpDown(TransactionContext tc, TAPDetail oTAPDetail, bool bIsREfresh)//Refresh sequence of TAP Detail
        {
            tc.ExecuteNonQuery("EXEC[SP_TAPDetail_UPDown]" + "%n,%n,%b,%b", oTAPDetail.TAPID, oTAPDetail.TAPDetailID, bIsREfresh, oTAPDetail.bIsUp);
        }
        public static void UpdateApprovePlanDate(TransactionContext tc, TAPDetail oTAPDetail)
        {
            tc.ExecuteNonQuery("Update TAPDetail SET ApprovalPlanDate = '"+oTAPDetail.ApprovalPlanDate+"' WHERE TAPDetailID = "+oTAPDetail.TAPDetailID);
        }

        public static void UpDownFactoryTAP(TransactionContext tc, TAPDetail oTAPDetail, bool bIsREfresh)//Refresh sequence of Factory TAP Detail
        {
            tc.ExecuteNonQuery("EXEC[SP_FactoryTAPDetail_UPDown]" + "%n,%n,%b,%b", oTAPDetail.TAPID, oTAPDetail.TAPDetailID, bIsREfresh, oTAPDetail.bIsUp);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TAPDetail WHERE TAPDetailID=%n", nID);
        }
        public static IDataReader Gets(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TAPDetail where TAPID = %n  Order By Sequence", id);
        }

        public static IDataReader FactoryTAPGets(int id, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FactoryTAPDetail where TAPID = %n  Order By Sequence", id);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TAPDetail Order By OrderStepID");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
    
    
}
