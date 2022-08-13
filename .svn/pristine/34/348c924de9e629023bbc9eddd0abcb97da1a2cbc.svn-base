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
    public class LeaveHeadDA
    {
        public LeaveHeadDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, LeaveHead oLeaveHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LeaveHead]"
                                    + "%n, %n, %s, %s,%n,%n, %b, %n, %n,%s,%b,%b, %u", oLeaveHead.LeaveHeadID, oLeaveHead.Code, oLeaveHead.Name, oLeaveHead.Description, oLeaveHead.TotalDay, (int)oLeaveHead.RequiredFor, oLeaveHead.IsActive, nUserID, (int)eEnumDBOperation, oLeaveHead.ShortName, oLeaveHead.IsLWP, oLeaveHead.IsHRApproval, oLeaveHead.NameInBangla);
        }

        public static void Delete(TransactionContext tc, LeaveHead oLeaveHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_LeaveHead]"
                                    + "%n, %n, %s, %s,%n,%n, %b, %n, %n,%s,%b,%b, %u", oLeaveHead.LeaveHeadID, oLeaveHead.Code, oLeaveHead.Name, oLeaveHead.Description, oLeaveHead.TotalDay, (int)oLeaveHead.RequiredFor, oLeaveHead.IsActive, nUserID, (int)eEnumDBOperation, oLeaveHead.ShortName, oLeaveHead.IsLWP,oLeaveHead.IsHRApproval, oLeaveHead.NameInBangla);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM LeaveHead WHERE LeaveHeadID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM LeaveHead");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static void ChangeActiveStatus(TransactionContext tc, LeaveHead oLeaveHead)
        {
            tc.ExecuteNonQuery("Update LeaveHead SET IsActive=%b WHERE LeaveHeadID=%n", oLeaveHead.IsActive, oLeaveHead.LeaveHeadID);
        }
        #endregion
    }
}
