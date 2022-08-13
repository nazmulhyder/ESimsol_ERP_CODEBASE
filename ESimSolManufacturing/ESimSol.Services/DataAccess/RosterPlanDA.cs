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
    public class RosterPlanDA
    {
        public RosterPlanDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, RosterPlan oRosterPlan, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_RosterPlan] %n, %n, %s, %n, %b, %n, %n", oRosterPlan.RosterPlanID, oRosterPlan.CompanyID, oRosterPlan.Description, oRosterPlan.RosterCycle, oRosterPlan.IsActive, nUserID, nDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM RosterPlan WHERE RosterPlanID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM RosterPlan");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static void ChangeActiveStatus(TransactionContext tc, RosterPlan oRosterPlan)
        {
            //tc.ExecuteNonQuery("Update RosterPlan SET IsActive=%b", oRosterPlan.IsActive);
            tc.ExecuteNonQuery("Update RosterPlan SET IsActive=%b WHERE RosterPlanID=%n", oRosterPlan.IsActive,oRosterPlan.RosterPlanID);
        }
        
        #endregion
    }
}
