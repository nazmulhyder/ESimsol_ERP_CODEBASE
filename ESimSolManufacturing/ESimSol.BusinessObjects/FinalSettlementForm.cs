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
    #region FinalSettlementForm
    [DataContract]
    public class FinalSettlementForm : BusinessObject
    {
        public FinalSettlementForm()
        {
            EmployeeID = 0;
            EmployeeCode="";
            EmployeeName="";
            DepartmentName = "";
            DesignationName = "";
            DateOfBirth = DateTime.Now;
            DateOfJoin= DateTime.Now;
            DateOfConfirmation = DateTime.Now;
            SettlementType = EnumSettleMentType.None;
	        DateOfSubmission =DateTime.Now;
            DateOfEffect = DateTime.Now;
            SalaryStartDate = DateTime.Now;
            SalaryMonth = "";
            SettMonth = "";
	        TotalAbsent =0;
            //SickLeave = 0;
            //CasualLeave = 0;
            //EarnLeave = 0;
            LeaveStatus = "";
	        OT_NHR =0;
	        OT_HHR =0;
            TotalNW = 0;
	        TotalEL =0;
            EnjoyedEl = 0;
            RefCode = "";
            ErrorMessage = "";
            IsNoticePay = false;
            TotalBenefitedDays = 0;
        }

        #region Properties
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoin { get; set; }
        public DateTime DateOfConfirmation { get; set; }
        public EnumSettleMentType SettlementType  { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public DateTime DateOfEffect { get; set; }
        public DateTime SalaryStartDate { get; set; }
        public string SalaryMonth { get; set; }
        public string SettMonth { get; set; }
        public int TotalAbsent { get; set; }
        public string LeaveStatus { get; set; }
        //public double SickLeave { get; set; }
        //public double CasualLeave { get; set; }
        //public double EarnLeave { get; set; }
        public int OT_NHR { get; set; }
        public int OT_HHR { get; set; }
        public int TotalNW { get; set; }
        public int TotalEL { get; set; }
        public int EnjoyedEl { get; set; }
        public string RefCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsNoticePay { get; set; }
        public int TotalBenefitedDays { get; set; }
        #endregion

        #region Derived Property
        public string DateOfBirthInString { get { return this.DateOfBirth.ToString("dd MMM yyyy"); } }
        public string DateOfJoinInString { get { return this.DateOfJoin.ToString("dd MMM yyyy"); } }
        public string DateOfConfirmationInString { get { return this.DateOfConfirmation.ToString("dd MMM yyyy"); } }
        public string DateOfSubmissionInString { get { return this.DateOfSubmission.ToString("dd MMM yyyy"); } }
        public string DateOfEffectInString { get { return this.DateOfEffect.ToString("dd MMM yyyy"); } }
        public string SalaryStartDateInString { get { return this.SalaryStartDate.ToString("dd MMM yyyy"); } }
        public string SettlementTypeInString { get { return this.SettlementType.ToString(); } }
        //public string LeaveStatus
        //{
        //    get
        //    {
        //        string S = ""; if (this.EarnLeave > 0) { S += "EL" + this.EarnLeave.ToString() + " days"; }
        //        if (this.SickLeave > 0) { S += "SL" + this.SickLeave.ToString() + " hours"; }
        //        if (this.CasualLeave > 0) { S += "CL" + this.CasualLeave.ToString() + " days"; }
        //        return S;
        //    }
        //}

        public List<FinalSettlementForm> FinalSettlementForms { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions

        public static List<FinalSettlementForm> Gets(int nEmployeeID, long nUserID)
        {
            return FinalSettlementForm.Service.Gets(nEmployeeID, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static IFinalSettlementFormService Service
        {
            get { return (IFinalSettlementFormService)Services.Factory.CreateService(typeof(IFinalSettlementFormService)); }
        }
        #endregion
    }
    #endregion

    #region IFinalSettlementForm interface

    public interface IFinalSettlementFormService
    {
        List<FinalSettlementForm> Gets(int nEmployeeID, Int64 nUserID);
    }
    #endregion
}