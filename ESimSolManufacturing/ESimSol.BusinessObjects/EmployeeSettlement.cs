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
    #region EmployeeSettlement

    public class EmployeeSettlement : BusinessObject
    {
        public EmployeeSettlement()
        {
            EmployeeSettlementID = 0;
            EmployeeID=0;
            Reason="";
            SubmissionDate = DateTime.Now;
            EffectDate= DateTime.Now;
            SettlementType = EnumSettleMentType.None;
            IsNoticePayDeduction = false;
            ApproveBy=0;
            ApproveByDate = DateTime.Now;
            IsResigned = true;
            ErrorMessage = "";
            EncryptID = "";
            EmployeeName = "";
            EmployeeCode = "";
            DepartmentName = "";
            DesignationName = "";
            ApproveByName = "";
            WorkingStatus = EnumEmployeeWorkigStatus.None;
            PaymentDate = DateTime.Now;
            IsSalaryHold = true;
            Params = "";
            EmpIDs = "";
        }

        #region Properties
        public int EmployeeSettlementID { get; set; }
        public int EmployeeID { get; set; }
        public string Reason { get; set; }
        public string Params { get; set; }
        public string EmpIDs { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime EffectDate { get; set; }
        public EnumSettleMentType SettlementType { get; set; }
        public bool IsNoticePayDeduction { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveByDate { get; set; }
        public bool IsResigned { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsSalaryHold { get; set; }
        #endregion

        #region Derived Property
        public List<EmployeeSettlement> EmployeeSettlements { get; set; }
        public string EncryptID { get; set; }
        public string ApproveByName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string WorkingStatusInString { get { if (IsResigned)return "Resigned"; else return "-"; } }
        public string NoticePayDeductionStatusInST { get { if (IsNoticePayDeduction)return "Yes"; else return "No"; } }
        public EnumEmployeeWorkigStatus WorkingStatus { get; set; }
        public string SettlementTypeInString
        {
            get
            {
                return SettlementType.ToString();
            }
        }
        public string ApproveDateInString
        {
            get
            {
                return ApproveByDate.ToString("dd MMM yyyy");
            }
        }
        public string SubmissionDateInString
        {
            get
            {
                return SubmissionDate.ToString("dd MMM yyyy");
            }
        }
        public string EffectDateInString
        {
            get
            {
                return EffectDate.ToString("dd MMM yyyy");
            }
        }

        public string PaymentDateInString
        {
            get
            {
                if (PaymentDate> Convert.ToDateTime("01 Jan 1800"))
                { return PaymentDate.ToString("dd MMM yyyy"); }
                else { return "-"; }
                
            }
        }
        public string PaymentStatus { get { if (PaymentDate > Convert.ToDateTime("01 Jan 1800")) return "Paid"; else return "-"; } }
        public Company Company { get; set; }
        #endregion

        #region Functions
        public static EmployeeSettlement Get(int Id, long nUserID)
        {
            return EmployeeSettlement.Service.Get(Id, nUserID);
        }
        public static EmployeeSettlement Get(string sSQL, long nUserID)
        {
            return EmployeeSettlement.Service.Get(sSQL, nUserID);
        }
        public static List<EmployeeSettlement> Gets(long nUserID)
        {
            return EmployeeSettlement.Service.Gets(nUserID);
        }

        public static List<EmployeeSettlement> Gets(string sSQL, long nUserID)
        {
            return EmployeeSettlement.Service.Gets(sSQL, nUserID);
        }

        public EmployeeSettlement IUD(int nDBOperation, long nUserID)
        {
            return EmployeeSettlement.Service.IUD(this, nDBOperation, nUserID);
        }
        public List<EmployeeSettlement> Approve_Multiple(int nDBOperation, long nUserID)
        {
            return EmployeeSettlement.Service.Approve_Multiple(this, nDBOperation, nUserID);
        }

        //public static EmployeeSettlement Approve(int nId, long nUserID)
        //{
        //    return EmployeeSettlement.Service.Approve(nId, nUserID);
        //}
        public static EmployeeSettlement PaymentDone(int nId, long nUserID)
        {
            return EmployeeSettlement.Service.PaymentDone(nId, nUserID);
        }

        public static List<EmployeeSettlement> GetHierarchy(string sEmployeeIDs, long nUserID)
        {
            return EmployeeSettlement.Service.GetHierarchy(sEmployeeIDs, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IEmployeeSettlementService Service
        {
            get { return (IEmployeeSettlementService)Services.Factory.CreateService(typeof(IEmployeeSettlementService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeSettlement interface

    public interface IEmployeeSettlementService
    {
        EmployeeSettlement Get(int id, Int64 nUserID);
        EmployeeSettlement Get(string sSQL, Int64 nUserID);
        List<EmployeeSettlement> Gets(Int64 nUserID);
        List<EmployeeSettlement> Gets(string sSQL, Int64 nUserID);
        EmployeeSettlement IUD(EmployeeSettlement oEmployeeSettlement, int nDBOperation, Int64 nUserID);
        List<EmployeeSettlement> Approve_Multiple(EmployeeSettlement oEmployeeSettlement, int nDBOperation, Int64 nUserID);
        //EmployeeSettlement Approve(int nId, Int64 nUserID);
        EmployeeSettlement PaymentDone(int nId, Int64 nUserID);
        List<EmployeeSettlement> GetHierarchy(string sEmployeeIDs, Int64 nUserID);
    }
    #endregion
}
