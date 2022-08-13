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
    public class RosterPlanDetailDA
    {
        public RosterPlanDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, RosterPlanDetail oRosterPlanDetail, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RosterPlanDetail]" 
                +"%n, %n, %n, %n, %n, %n", 
                oRosterPlanDetail.RosterPlanDetailID, oRosterPlanDetail.RosterPlanID, oRosterPlanDetail.ShiftID, oRosterPlanDetail.NextShiftID, nUserID, nDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_RosterPlanDetail WHERE RosterPlanDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RosterPlanDetail");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
