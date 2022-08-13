using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class LeaveApplicationDA
    {
        public LeaveApplicationDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, LeaveApplication oLeaveApplication, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LeaveApplication] %n, %n, %n, %n, %n, %D, %D, %s, %s, %n, %n, %d, %n, %d, %b, %s, %s, %n,%n,%n,%n,%n,%n", 
                oLeaveApplication.LeaveApplicationID, oLeaveApplication.EmployeeID, oLeaveApplication.EmpLeaveLedgerID, (int)oLeaveApplication.ApplicationNature, (int)oLeaveApplication.LeaveType, oLeaveApplication.StartDateTime,
                oLeaveApplication.EndDateTime,oLeaveApplication.Location, oLeaveApplication.Reason, oLeaveApplication.RequestForRecommendation, oLeaveApplication.RecommendedBy, oLeaveApplication.RecommendedByDate,
                oLeaveApplication.ApproveBy, oLeaveApplication.ApproveByDate, oLeaveApplication.IsUnPaid, oLeaveApplication.RecommendationNote, oLeaveApplication.ApprovalNote, (int)oLeaveApplication.LeaveStatus, oLeaveApplication.RequestForAproval, oLeaveApplication.ResponsiblePersonID, nUserID, nDBOperation, oLeaveApplication.LeaveDuration);
        }
        //public static void Approved(TransactionContext tc, LeaveApplication oLeaveApplication, Int64 nUserID)
        //{
        //    tc.ExecuteNonQuery("UPDATE LeaveApplication SET LeaveStatus=3, IsUnPaid=%b, ApprovalNote=%s, ApproveBy=%n WHERE LeaveApplicationID=%n ",  oLeaveApplication.IsUnPaid, oLeaveApplication.ApprovalNote, nUserID, oLeaveApplication.LeaveApplicationID);
        //}
        //public static IDataReader MultipleApprove(TransactionContext tc, LeaveApplication oLeaveApplication, Int64 nUserID)
        //{
        //    return tc.ExecuteReader("UPDATE LeaveApplication SET LeaveStatus=3, IsUnPaid=%b, ApprovalNote=%s, ApproveBy=%n WHERE LeaveApplicationID=%n ; SELECT * FROM View_LeaveApplication WHERE LeaveApplicationID =%n", oLeaveApplication.IsUnPaid, oLeaveApplication.ApprovalNote, nUserID, oLeaveApplication.LeaveApplicationID, oLeaveApplication.LeaveApplicationID);
        //}
        public static IDataReader Approve(TransactionContext tc, LeaveApplication oLeaveApplication, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_LeaveApprove] %s,%n,%b,%n", oLeaveApplication.ApprovalNote, oLeaveApplication.LeaveApplicationID, oLeaveApplication.IsHRApproval, nUserID);
        }
        public static IDataReader Cancel(TransactionContext tc, LeaveApplication oLeaveApplication, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_LeaveCancel] %n, %n, %s, %n", oLeaveApplication.LeaveApplicationID, (int)oLeaveApplication.LeaveStatus, oLeaveApplication.CancelledNote, nUserID);
           // return tc.ExecuteReader("UPDATE LeaveApplication SET LeaveStatus=4, CancelledNote=%s, CancelledBy=%n,CancelledByDate=%d  WHERE LeaveApplicationID=%n ; SELECT * FROM View_LeaveApplication WHERE LeaveApplicationID =%n", oLeaveApplication.CancelledNote, nUserID,DateTime.Now, oLeaveApplication.LeaveApplicationID, oLeaveApplication.LeaveApplicationID);
        }

        public static IDataReader LeaveAdjustment(TransactionContext tc, int LeaveApplicationID, DateTime EndDate, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LeaveAdjustment] %n, %d", LeaveApplicationID, EndDate);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LeaveApplication WHERE LeaveApplicationID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_LeaveApplication");
        }
        public static IDataReader GetsEmployeeLeaveLedger(TransactionContext tc, string sSQL, int nACSID)
        {
            return tc.ExecuteReader("EXEC [SP_LeaveLedgerRegister] %s, %n", sSQL, nACSID );
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        
        #endregion
    }
}
