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
    #region LeaveHead

    public class LeaveHead : BusinessObject
    {
        public LeaveHead()
        {
            LeaveHeadID = 0;
            Code = 0;
            Name = "";
            Description = "";
            TotalDay = 1;
            RequiredFor = EnumLeaveRequiredFor.All;
            IsActive = true;
            ErrorMessage = "";
            ShortName = "";
            IsLWP = false;
            IsHRApproval = false;
            NameInBangla = "";
            LHRules = new List<LHRule>();
        }

        #region Properties

        public int LeaveHeadID { get; set; }
        public int Code { get; set; }
        public String Name { get; set; }
        public String NameInBangla { get; set; }
        public string Description { get; set; }
        public int TotalDay { get; set; }
        public EnumLeaveRequiredFor RequiredFor { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        public string ShortName { get; set; }
        public bool IsLWP { get; set; }
        public bool IsHRApproval { get; set; }
        public List<LHRule> LHRules { get; set; }

        #endregion

        #region Derived Property
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public List<LeaveHead> LeaveHeads { get; set; }
        #endregion

        #region Functions
        public static List<LeaveHead> Gets(long nUserID)
        {
            return LeaveHead.Service.Gets(nUserID);
        }

        public static List<LeaveHead> Gets(string sSQL, long nUserID)
        {
            return LeaveHead.Service.Gets(sSQL,nUserID);
        }

        public LeaveHead Get(int id, long nUserID)
        {
            return LeaveHead.Service.Get(id, nUserID);
        }

        public LeaveHead Save(long nUserID)
        {
            return LeaveHead.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return LeaveHead.Service.Delete(id, nUserID);
        }

        public string ChangeActiveStatus(LeaveHead oLeaveHead, long nUserID)
        {
            return LeaveHead.Service.ChangeActiveStatus(oLeaveHead, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILeaveHeadService Service
        {
            get { return (ILeaveHeadService)Services.Factory.CreateService(typeof(ILeaveHeadService)); }
        }

        #endregion
    }
    #endregion

    #region ILeaveHead interface

    public interface ILeaveHeadService
    {
        LeaveHead Get(int id, Int64 nUserID);
        List<LeaveHead> Gets(string sSQL, Int64 nUserID);
        List<LeaveHead> Gets(Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        LeaveHead Save(LeaveHead oLeaveHead, Int64 nUserID);
        string ChangeActiveStatus(LeaveHead oLeaveHead, Int64 nUserID);
    }
    #endregion
}
