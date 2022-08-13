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
    #region SelfServiceClearance

    public class SelfServiceClearance : BusinessObject
    {
        public SelfServiceClearance()
        {
            EmployeeSettlementID = 0;
            EmployeeID = 0;
            ESCID = 0;
            EmployeeCode = "";
            EmployeeName = "";
            SettlementType = EnumSettleMentType.None;
            SubmissionDate = DateTime.Now;
            LastWorkingDate = DateTime.Now;
            Reason = "";
            CurrentStatus = EnumESCrearance.None;
            Note = "";
            ApproveBy = 0;
            ApproveByDate = DateTime.Now;
            ErrorMessage = "";
        }

        #region Properties
        public int EmployeeSettlementID { get; set; }
        public int EmployeeID { get; set; }
        public int ESCID { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public string Reason { get; set; }
        public EnumSettleMentType SettlementType { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public EnumESCrearance CurrentStatus { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveByDate { get; set; }
        #endregion

        #region Derived Property
        public string SettlementTypeInString
        {
            get
            {
                return SettlementType.ToString();
            }
        }
        public string CurrentStatusInString
        {
            get
            {
                return CurrentStatus.ToString();
            }
        }
        public string LastWorkingDateInString
        {
            get
            {
                return LastWorkingDate.ToString("dd MMM yyyy");
            }
        }

        public string SubmissionDateInString
        {
            get
            {
                return SubmissionDate.ToString("dd MMM yyyy");
            }
        }

        public string ApproveByDateInString
        {
            get
            {
                return ApproveByDate.ToString("dd MMM yyyy");
            }
        }

        #endregion

        #region Functions
        public static List<SelfServiceClearance> Gets(int  nEmployeeID, long nUserID)
        {
            return SelfServiceClearance.Service.Gets(nEmployeeID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISelfServiceClearanceService Service
        {
            get { return (ISelfServiceClearanceService)Services.Factory.CreateService(typeof(ISelfServiceClearanceService)); }
        }

        #endregion
    }
    #endregion

    #region ISelfServiceClearance interface

    public interface ISelfServiceClearanceService
    {
        List<SelfServiceClearance> Gets(int nEmployeeID, Int64 nUserID);
    }
    #endregion
}
