using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region RosterPlan

    public class RosterPlan : BusinessObject
    {
        public RosterPlan()
        {
            RosterPlanID = 0;
            CompanyID = 1;
            Code = "1";
            Description = "";
            RosterCycle = 0;
            IsActive = true;
            ErrorMessage = "";
        }

        #region Properties

        public int RosterPlanID { get; set; }
        public int CompanyID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int RosterCycle { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public string EncryptRPID { get; set; }

        public string CompanyName;
        public int ShiftID { get; set; }
        public int NextShiftID { get; set; }
        public List<HRMShift> Shifts { get; set; }
        public List<RosterPlanDetail> RosterPlanDetails { get; set; }
        public string Activity
        {
            get
            {
                if (IsActive) return "Active";
                return "Inactive";
            }
        }


        #endregion

        #region Functions
        public static List<RosterPlan> Gets(long nUserID)
        {
            return RosterPlan.Service.Gets(nUserID);
        }

        public static List<RosterPlan> Gets(string sSQL, long nUserID)
        {
            return RosterPlan.Service.Gets(sSQL, nUserID);
        }

        public RosterPlan Get(int id, long nUserID)
        {
            return RosterPlan.Service.Get(id, nUserID);
        }

        public RosterPlan IUD(int nDBOperation, long nUserID)
        {
            return RosterPlan.Service.IUD(this, nDBOperation, nUserID);
        }

        public string ChangeActiveStatus(RosterPlan oRosterPlan, long nUserID)
        {
            return RosterPlan.Service.ChangeActiveStatus(oRosterPlan, nUserID);
        }

        //public RosterPlan IUD(RosterPlan oRosterPlan, long nUserID, int nDBOperation)
        //{
        //    return (RosterPlan)ICSWCFServiceClient.CallMethod(ServiceType, wcfSessionid, "IUD", this,nDBOperation, oRosterPlan)[0];
        //}
        #endregion

        #region ServiceFactory
        internal static IRosterPlanService Service
        {
            get { return (IRosterPlanService)Services.Factory.CreateService(typeof(IRosterPlanService)); }
        }

        #endregion
    }
    #endregion

    #region IRosterPlan interface
    public interface IRosterPlanService
    {
        RosterPlan Get(int id, Int64 nUserID);
        List<RosterPlan> Gets(Int64 nUserID);
        List<RosterPlan> Gets(string sSQL, Int64 nUserID);
        RosterPlan IUD(RosterPlan oRosterPlan, int nDBOperation, Int64 nUserID);
        string ChangeActiveStatus(RosterPlan oRosterPlan, Int64 nUserID);
    }
    #endregion
}
