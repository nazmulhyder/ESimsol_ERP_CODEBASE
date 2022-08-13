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
    #region RosterPlanDetail

    public class RosterPlanDetail : BusinessObject
    {
        public RosterPlanDetail()
        {
            RosterPlanDetailID = 0;
            RosterPlanID = 0;
            ShiftID = 0;
            NextShiftID = 0;
            ToleranceTime = DateTime.Now;
            ErrorMessage = "";
        }

        #region Properties

        public int RosterPlanDetailID { get; set; }
        public int RosterPlanID { get; set; }
        public int ShiftID { get; set; }
        public DateTime ToleranceTime { get; set; }
        public int NextShiftID { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string Shift { get; set; }
        public string NextShift { get; set; }
        public List<HRMShift> Shifts { get; set; }

        public string ToleranceTimeInString
        {
            get
            {
                return ToleranceTime.ToString("HH:mm");

            }
        }

        #endregion

        #region Functions

        public RosterPlanDetail Get(int id, long nUserID)
        {
            return RosterPlanDetail.Service.Get(id, nUserID);
        }

        public static List<RosterPlanDetail> Gets(int id, long nUserID)
        {
            return RosterPlanDetail.Service.Gets(id, nUserID);
        }

        public static List<RosterPlanDetail> Gets(string sSQL, long nUserID)
        {
            return RosterPlanDetail.Service.Gets(sSQL, nUserID);
        }

        public RosterPlanDetail IUD(int nDBOperation, long nUserID)
        {
            return RosterPlanDetail.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IRosterPlanDetailService Service
        {
            get { return (IRosterPlanDetailService)Services.Factory.CreateService(typeof(IRosterPlanDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IRosterPlanDetail interface

    public interface IRosterPlanDetailService
    {
        RosterPlanDetail Get(int id, Int64 nUserID);
        List<RosterPlanDetail> Gets(int id, Int64 nUserID);
        List<RosterPlanDetail> Gets(string sSQL, Int64 nUserID);
        RosterPlanDetail IUD(RosterPlanDetail oRosterPlanDetail, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
