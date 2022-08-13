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
    #region LeaveApplication

    public class LeaveApplication : BusinessObject
    {
        public LeaveApplication()
        {
            LeaveApplicationID = 0;
            EmployeeID = 0;
            EmpLeaveLedgerID = 0;
            ApplicationNature = EnumLeaveApplication .None;
            LeaveType = EnumLeaveType.None;
            StartDateTime = DateTime.Now;
            EndDateTime = DateTime.Now;
            Location = "";
            Reason = "";
            RequestForRecommendation = 0;
            RecommendedBy = 0;
            RecommendedByDate = DateTime.Now;
            ApproveBy = 0;
            ApproveByDate = DateTime.Now;
            IsUnPaid = false;
            RecommendationNote = "";
            ApprovalNote = "";
            LeaveStatus = EnumLeaveStatus.None;
            CancelledBy = 0;
            LeaveHeadShortName = "";
            CancelledByDate = DateTime.Now;
            CancelledNote = "";
            RequestForAproval = 0;
            ErrorMessage = "";
            Params="";
            EmployeeLeaveLedgers = new List<EmployeeLeaveLedger>();
            ResponsiblePersonID = 0;
            ResponsiblePersonName = "";
            DBServerDateTime = DateTime.Now;
            LeaveDuration = 0;
            DepartmentName = "";
            DesignationName = "";
            Gender = "";
            IsHRApproval = false;
            HRApproveBYName = "";
            IsApprove = false;
            JoiningDate = DateTime.Now;
            BusinessUnitName = "";
            TotalDay = 0;
            ApprovedByDepartment = "";
            ApprovedByDesignation = "";
            LDays = 0;
        }

        #region Properties
        public int LeaveApplicationID { get; set; }
        public Int64 EmployeeID { get; set; }
        public int EmpLeaveLedgerID { get; set; }
        public EnumLeaveApplication ApplicationNature { get; set; }
        public EnumLeaveType LeaveType { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Location { get; set; }
        public string LocationName { get; set; }
        public string LeaveHeadShortName { get; set; }
        public int LDays { get; set; }
        public string ApprovedByDepartment { get; set; }
        public string ApprovedByDesignation { get; set; }
        public string Reason { get; set; }
        public int RequestForRecommendation { get; set; }
        public int RecommendedBy { get; set; }
        public DateTime RecommendedByDate { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveByDate { get; set; }
        public bool IsUnPaid { get; set; }
        public string RecommendationNote { get; set; }
        public string ApprovalNote { get; set; }
        public EnumLeaveStatus LeaveStatus { get; set; }
        public int CancelledBy { get; set; }
        public DateTime CancelledByDate { get; set; }
        public string Gender { get; set; }
        public string CancelledNote { get; set; }
        public double TotalDay { get; set; }
        public int RequestForAproval { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public int ResponsiblePersonID { get; set; }

        public DateTime DBServerDateTime { get; set; }
        public int LeaveDuration { get; set; }
        public bool IsHRApproval { get; set; }
        public bool IsApprove { get; set; }
        #endregion

        #region Derived Property
        public List<EmployeeLeaveLedger> EmployeeLeaveLedgers { get; set; }
        public List<LeaveHead> LeaveHeads { get; set; }
        public string RequestedForRecommendation { get; set; }
        public string Duration { get; set; }
        public string LeaveDetails { get; set; }
        public string LastLeaveEnjoyed { get; set; }
        public bool IsPaid { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string LeaveHeadName { get; set; }
        public string RecommendedByName { get; set; }
        public string ApproveByName { get; set; }
        public List<User> Users { get; set; }
        public string UserName { get; set; }
        public string ResponsiblePersonName { get; set; }
        public int ApplicationNatureInt { get; set; }
        public string ApplicationNatureInString
        {
            get
            {
                return ApplicationNature.ToString();
            }
        }
        public int LeaveTypeInt { get; set; }
        public string LeaveTypeInString
        {
            get
            {
                return LeaveType.ToString();
            }
        }
        public int LeaveStatusInt { get; set; }
        public string LeaveStatusInString
        {
            get
            {
                return LeaveStatus.ToString();
            }
        }
        public string IsUnPaidInString
        {
            get
            {
                if (IsUnPaid == false)
                {
                    return "Paid";
                }
                else
                {
                    return "Unpaid";
                }
            }
        }
        public string StartDateTimeInString
        {
            get
            {
                return StartDateTime.ToString("dd MMM yyyy");
            }
        }
        public string EndDateTimeInString
        {
            get
            {
                return EndDateTime.ToString("dd MMM yyyy");
            }
        }
        public string ResumeDate
        {
            get
            {
                return EndDateTime.AddDays(1).ToString("dd MMM yyyy");
            }
        }
        public string RecommendedByDateInString
        {
            get
            {
                return RecommendedByDate.ToString("dd MMM yy");
            }
        }
        public DateTime JoiningDate { get; set; }
        public string JoiningDateInString
        {
            get
            {
                return JoiningDate.ToString("dd MMM yy");
            }
        }
        public string ApproveByDateInString
        {
            get
            {
                return ApproveByDate.ToString("dd MMM yy");
            }
        }
        public string EmployeeNameCode
        {
            get
            {
                return EmployeeName + "[" + EmployeeCode + "]";
            }
        }
        public string BusinessUnitName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }

        public string SubmissionDateInString
        {
            get
            {
                return DBServerDateTime.ToString("dd MMM yy");
            }
        }
        public string SubmissionDateInStringBangla
        {
            get
            {
                return DBServerDateTime.ToString("dd/MM/yyyy");
            }
        }
        public string RequestForAprovalName { get; set; }
        public string CasualBalance { get; set; }

        public string MedicalBalance { get; set; }

        public string EarnLeaveBalance { get; set; }

        public string MaternityBalance { get; set; }

        public string OccasionalBalance { get; set; }

        public int Minute
        {
            get
            {
                return this.LeaveDuration % 60;
            }
        }
        public int Hour
        {
            get
            {
                return this.LeaveDuration / 60;
            }
        }

        public double LeaveDurationInHour
        {
            get
            {
                double nLeaveDuration = 0;
                if (this.LeaveType != EnumLeaveType.Full && this.LeaveDuration != 0)
                {
                    nLeaveDuration= Convert.ToDouble(this.LeaveDuration) / 60;
                }
                else nLeaveDuration = this.LeaveDuration * 8;
                return nLeaveDuration;
            }
        }

        public string LeaveDurationInST
        {
            get
            {
                string sDuration = "";
                if (this.LeaveType != EnumLeaveType.Full && this.LeaveDuration != 0)
                {
                    sDuration = Global.MinInHourMin(this.LeaveDuration);
                }
                else 
                {
                    //sDuration = this.LeaveDuration.ToString() + " d";
                    sDuration = this.LeaveDuration + " d";
                }
                return sDuration;
            }
        }

        public string Recommend
        {
            get
            {
                return ((this.RequestForRecommendation <= 0 && this.RecommendedBy <= 0 && this.ApproveBy <= 0) ? "<b>No Recommendation Needed</b>" : this.RecommendedByName);
            }
        }

        public string HRApproveBYName { get; set; }

        public string StatusOfLeave
        {
            get
            {
                string sStatus="Applied";
                if (this.CancelledBy>0){sStatus="Cancel";}
                else {
                    if (this.IsApprove == false)
                    {
                        if (this.ApproveBy > 0) { sStatus = "Pending HR Approval"; }
                        else if (this.RecommendedBy > 0) { sStatus = "Recommended"; }
                    }
                        else{sStatus="Approved";}
                }
                return sStatus;
            }
        }

        public string DBUserName { get; set; }
        public bool IsActive { get; set; }
        public string EmployeeWorkingStatus
        {
            get
            {
                return this.IsActive ? "Continued" : "Discontinued";
            }
        }

        #endregion
        #region For Bangla Leave Register
        #region EarnLeave
        public string ELDuration
        {
            get
            {
                if (this.LeaveHeadShortName == "EL") return this.LeaveDuration.ToString();
                else return "";
            }
        }
        public string ELStartDateST
        {
            get
            {
                if (this.LeaveHeadShortName == "EL") return this.StartDateTime.ToString("dd/MM/yyyy");
                else return "";
            }
        }
        public string ELEndDateST
        {
            get
            {
                if (this.LeaveHeadShortName == "EL") return this.EndDateTime.ToString("dd/MM/yyyy");
                else return "";
            }
        }
        #endregion
        #region CasualLeave
        public string CLDuration
        {
            get
            {
                if (this.LeaveHeadShortName == "CL") return this.LeaveDuration.ToString();
                else return "";
            }
        }
        public string CLStartDateST
        {
            get
            {
                if (this.LeaveHeadShortName == "CL") return this.StartDateTime.ToString("dd/MM/yyyy");
                else return "";
            }
        }
        public string CLEndDateST
        {
            get
            {
                if (this.LeaveHeadShortName == "CL") return this.EndDateTime.ToString("dd/MM/yyyy");
                else return "";
            }
        }
        #endregion
        #region SickLeave
        public string SLDuration
        {
            get
            {
                if (this.LeaveHeadShortName == "SL") return this.LeaveDuration.ToString();
                else return "";
            }
        }
        public string SLStartDateST
        {
            get
            {
                if (this.LeaveHeadShortName == "SL") return this.StartDateTime.ToString("dd/MM/yyyy");
                else return "";
            }
        }
        public string SLEndDateST
        {
            get
            {
                if (this.LeaveHeadShortName == "SL") return this.EndDateTime.ToString("dd/MM/yyyy");
                else return "";
            }
        }
        #endregion
        #region TotalLeave
        public string TotalELDuration
        {
            get
            {
                if (this.LeaveHeadShortName == "EL") return (this.LDays- Convert.ToInt32(this.ELDuration)).ToString();
                else return "";
            }
        }
        public string TotalCLDuration
        {
            get
            {
                if (this.LeaveHeadShortName == "CL") return (this.LDays - Convert.ToInt32(this.CLDuration)).ToString();
                else return "";
            }
        }
        public string TotalSLDuration
        {
            get
            {
                if (this.LeaveHeadShortName == "SL") return (this.LDays - Convert.ToInt32(this.SLDuration)).ToString();
                else return "";
            }
        }
        #endregion
        #endregion

        #region Functions
        public static List<LeaveApplication> Gets(long nUserID)
        {
            return LeaveApplication.Service.Gets(nUserID);
        }

        public static List<LeaveApplication> Gets(string sSQL, long nUserID)
        {
            return LeaveApplication.Service.Gets(sSQL, nUserID);
        }
        public static List<LeaveApplication> GetsEmployeeLeaveLedger(string sSQL, int nACSID, long nUserID)
        {
            return LeaveApplication.Service.GetsEmployeeLeaveLedger(sSQL, nACSID, nUserID);
        }

        public LeaveApplication Get(int id, long nUserID)
        {
            return LeaveApplication.Service.Get(id, nUserID);
        }

        public static LeaveApplication Get(string sSQL, long nUserID)
        {
            return LeaveApplication.Service.Get(sSQL, nUserID);
        }

        public LeaveApplication IUD(int nDBOperation, long nUserID)
        {
            return LeaveApplication.Service.IUD(this, nDBOperation, nUserID);
        }
        public LeaveApplication Approved(long nUserID)
        {
            return LeaveApplication.Service.Approved(this,nUserID);
        }

        public static List<LeaveApplication> MultipleApprove(List<LeaveApplication> oLeaveApplications, long nUserID)
        {
            return LeaveApplication.Service.MultipleApprove(oLeaveApplications, nUserID);
        }

        public LeaveApplication Cancel(long nUserID)
        {
            return LeaveApplication.Service.Cancel(this, nUserID);
        }

        public LeaveApplication LeaveAdjustment(int LeaveApplicationID,DateTime EndDate, long nUserID)
        {
            return LeaveApplication.Service.LeaveAdjustment(LeaveApplicationID,EndDate, nUserID);
        }



        #endregion

        #region ServiceFactory
        internal static ILeaveApplicationService Service
        {
            get { return (ILeaveApplicationService)Services.Factory.CreateService(typeof(ILeaveApplicationService)); }
        }
        #endregion
    }
    #endregion

    #region ILeaveApplication interface

    public interface ILeaveApplicationService
    {
        LeaveApplication Get(int id, Int64 nUserID);
        LeaveApplication Get(string sSQL, Int64 nUserID);
        List<LeaveApplication> Gets(Int64 nUserID);
        List<LeaveApplication> Gets(string sSQL, Int64 nUserID);
        List<LeaveApplication> GetsEmployeeLeaveLedger(string sSQL, int nACSID, Int64 nUserID);
        LeaveApplication IUD(LeaveApplication oLeaveApplication, int nDBOperation, Int64 nUserID);
        LeaveApplication Approved(LeaveApplication oLeaveApplication, Int64 nUserId);
        List<LeaveApplication> MultipleApprove(List<LeaveApplication> oLeaveApplications, Int64 nUserId);
        LeaveApplication Cancel(LeaveApplication oLeaveApplication, Int64 nUserId);
        LeaveApplication LeaveAdjustment(int LeaveApplicationID,DateTime EndDate, Int64 nUserId);
        
    }
    #endregion
}
