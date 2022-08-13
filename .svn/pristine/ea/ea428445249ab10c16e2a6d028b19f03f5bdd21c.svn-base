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
    #region PerformanceIncentiveMember

    public class PerformanceIncentiveMember : BusinessObject
    {
        public PerformanceIncentiveMember()
        {
            PIMemberID = 0;
            PIID = 0;
            EmployeeID = 0;
            Rate = 0;
            ApproveBy = 0;
            ApproveByDate = DateTime.Now;
            InactiveBy = 0;
            InactiveByDate = DateTime.Now;
            ErrorMessage = "";
            InactiveByName = "";
            ApproveByName = "";
            EmployeeName = "";
            EmployeeCode = "";
            PICode = "";
            PIName = "";
        }

        #region Properties
        public int PIMemberID { get; set; }
        public int PIID { get; set; }
        public int EmployeeID { get; set; }
        public double Rate { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveByDate { get; set; }
        public int InactiveBy { get; set; }
        public DateTime InactiveByDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<PerformanceIncentiveMember> PerformanceIncentiveMembers { get; set; }
        public string Activity { get { if (InactiveBy == 0)return "Active"; else return "Inactive"; } }
        public string EncryptPIMemberID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string PICode { get; set; }
        public string PIName { get; set; }
        public string InactiveByName { get; set; }
        public string ApproveByName { get; set; }
        public string ApproveDateInString
        {
            get
            {
                return ApproveByDate.ToString("dd MMM yyyy");
            }
        }
        public string InactiveDateInString
        {
            get
            {
                return InactiveByDate.ToString("dd MMM yyyy");
            }
        }

        #endregion

        #region Functions
        public static PerformanceIncentiveMember Get(int Id, long nUserID)
        {
            return PerformanceIncentiveMember.Service.Get(Id, nUserID);
        }
        public static PerformanceIncentiveMember Get(string sSQL, long nUserID)
        {
            return PerformanceIncentiveMember.Service.Get(sSQL, nUserID);
        }
        public static List<PerformanceIncentiveMember> Gets(long nUserID)
        {
            return PerformanceIncentiveMember.Service.Gets(nUserID);
        }

        public static List<PerformanceIncentiveMember> Gets(string sSQL, long nUserID)
        {
            return PerformanceIncentiveMember.Service.Gets(sSQL, nUserID);
        }

        public List<PerformanceIncentiveMember> IUD(List<PerformanceIncentiveMember> oPIMs, int nDBOperation, long nUserID)
        {
            return PerformanceIncentiveMember.Service.IUD(oPIMs, nDBOperation, nUserID);
        }

        public static PerformanceIncentiveMember InActive(int nId, long nUserID)
        {
            return PerformanceIncentiveMember.Service.InActive(nId, nUserID);
        }

        public static List<PerformanceIncentiveMember> Approve(string sPIMIDs, long nUserID)
        {
            return PerformanceIncentiveMember.Service.Approve(sPIMIDs, nUserID);
        }

        public static List<PerformanceIncentiveMember> UploadPIMXL(List<PerformanceIncentiveMember> oPerformanceIncentiveMembers, long nUserID)
        {
            return PerformanceIncentiveMember.Service.UploadPIMXL(oPerformanceIncentiveMembers, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IPerformanceIncentiveMemberService Service
        {
            get { return (IPerformanceIncentiveMemberService)Services.Factory.CreateService(typeof(IPerformanceIncentiveMemberService)); }
        }

        #endregion
    }
    #endregion

    #region IPerformanceIncentiveMember interface

    public interface IPerformanceIncentiveMemberService
    {
        PerformanceIncentiveMember Get(int id, Int64 nUserID);
        PerformanceIncentiveMember Get(string sSQL, Int64 nUserID);
        List<PerformanceIncentiveMember> Gets(Int64 nUserID);
        List<PerformanceIncentiveMember> Gets(string sSQL, Int64 nUserID);
        List<PerformanceIncentiveMember> IUD(List<PerformanceIncentiveMember> oPIMs, int nDBOperation, Int64 nUserID);
        PerformanceIncentiveMember InActive(int nId, Int64 nUserID);
        List<PerformanceIncentiveMember> Approve(string sPIMIDs, Int64 nUserID);
        List<PerformanceIncentiveMember> UploadPIMXL(List<PerformanceIncentiveMember> oPerformanceIncentiveMembers, Int64 nUserID);
    }
    #endregion
}
