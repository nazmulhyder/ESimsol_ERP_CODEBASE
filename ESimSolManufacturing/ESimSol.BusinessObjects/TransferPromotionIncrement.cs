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
    #region TransferPromotionIncrement

    public class TransferPromotionIncrement : BusinessObject
    {
        public TransferPromotionIncrement()
        {
            TPIID = 0;
            EmployeeID = 0;
            DesignationID = 0;
            DRPID = 0;
            ASID = 0;
            SalarySchemeID = 0;
            GrossSalary = 0;
            IsTransfer = false;
            IsPromotion = false;
            IsIncrement = false;
            TPIDesignationID = 0;
            TPIDRPID = 0;
            TPIASID = 0;
            TPISalarySchemeID = 0;
            TPIShiftID = 0;
            TPIGrossSalary = 0;
            TPIIsFixedAmount = false;
            JoiningDate = DateTime.Now;
            Note = "";
            EffectedDate = DateTime.Now;
            ActualEffectedDate = DateTime.Now;
            RecommendedBy = 0;
            RecommendedByDate = DateTime.Now;
            ApproveBy = 0;
            ApproveByDate = DateTime.Now;
            EmployeeCode = "";
            EmployeeName = "";
            LocationName = "";
            BUName = "";
            DepartmentName = "";
            DesignationName = "";
            TPILocationName = "";
            TPIDepartmentName = "";
            TPIDesignationName = "";
            EmployeeIDs = "";
            ErrorMessage = "";
            EncryptTPIID = "";
            IDs = "";
            EmployeeTypeID = 0;
            TPIEmployeeTypeID = 0;

            SalarySchemeName = "";
            SalaryHeadNames = "";
            CompTPIGrossSalary = 0.0;
            CompGrossSalary = 0.0;
            nCompSHField = 0;
            BUCode = "";
            LocCode = "";
            DeptCode = "";
            AttSchemeName = "";
            PresentDesignationName = "";
            ShiftCode ="";
            DesgCode="";
            EmpTypeName = "";
            IsNoHistory = false;
            IsCashFixed = false;
            CashAmount = 0;

            BasicAmount = 0.0;
            TPIBasicAmount = 0.0;
            DOJ = DateTime.MinValue;
            IndexNo = 0;
            TransferPromotionIncrements = new List<TransferPromotionIncrement>();
        }

        #region Properties

        public string BUName { get; set; }
        public int IndexNo { get; set; }
        public DateTime DOJ { get; set; }
        public double BasicAmount { get; set; }
        public double TPIBasicAmount { get; set; }
        public bool IsCashFixed { get; set; }
        public double CashAmount { get; set; }
        public string BUCode { get; set; }
        public string LocCode { get; set; }
        public string DeptCode { get; set; }
        public string AttSchemeName { get; set; }
        public string ShiftCode { get; set; }
        public string DesgCode { get; set; }
        public string EmpTypeName { get; set; }
        public int TPIID { get; set; }
        public int EmployeeTypeID { get; set; }
        public int TPIEmployeeTypeID { get; set; }
        public int EmployeeID { get; set; }
        public int DesignationID { get; set; }
        public int DRPID { get; set; }
        public int ASID { get; set; }
        public int SalarySchemeID { get; set; }
        public double GrossSalary { get; set; }
        public double CompTPIGrossSalary { get; set; }
        public double CompGrossSalary { get; set; }
        public bool IsNoHistory { get; set; }
        public bool IsTransfer { get; set; }
        public bool IsPromotion { get; set; }
        public bool IsIncrement { get; set; }
        public int TPIDesignationID { get; set; }
        public int TPIDRPID { get; set; }
        public int TPIASID { get; set; }
        public int TPISalarySchemeID { get; set; }
        public int TPIShiftID { get; set; }
        public double TPIGrossSalary { get; set; }
        public bool TPIIsFixedAmount { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Note { get; set; }
        public DateTime EffectedDate { get; set; }
        public DateTime ActualEffectedDate { get; set; }
        public int RecommendedBy { get; set; }
        public DateTime RecommendedByDate { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveByDate { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string EncryptTPIID { get; set; }
        public string EmployeeIDs { get; set; }
        public string EmployeeName { get; set; }
        public string PresentDesignationName { get; set; }
        public string EmployeeCode { get; set; }
        public string LocationName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string TPILocationName { get; set; }
        public string TPIDepartmentName { get; set; }
        public string TPIDesignationName { get; set; }
        public string AttendanceSchemeName { get; set; }
        public string TPIASName { get; set; }
        public string SalarySchemeName { get; set; }
        public string TPISSName { get; set; }
        public string TPIShiftName { get; set; }
        public string RecommendBYName { get; set; }
        public string ApprovedBYName { get; set; }
        public string IDs { get; set; }
        public int nCompSHField { get; set; }
        public string SalaryHeadNames { get; set; }
        public string CompSalaryHeadNames { get; set; }
        public string Designation
        {
            get
            {
                if (TPIDesignationName == "") return DesignationName; else return TPIDesignationName;
            }
        }

        public string EmployeeofficialInString
        {
            get
            {
                return this.Designation + "," + this.DepartmentName + "," + this.LocationName;
            }
        }
        public string TPIEmployeeofficialInString
        {
            get
            {
                return (this.TPIDesignationName == "" ? "" : this.TPIDesignationName + ",") + this.TPIDepartmentName + "," + this.TPILocationName;
            }
        }

        public string ActionInString
        {
            get
            {
                if (this.IsTransfer == true && this.IsPromotion == true && this.IsIncrement == true) return "Transfer with Promotion and Increment";
                else if (this.IsTransfer == true && this.IsPromotion == true && this.IsIncrement == false) return "Transfer with Promotion";
                else if (this.IsTransfer == true && this.IsPromotion == false && this.IsIncrement == true) return "Transfer With Increment";
                else if (this.IsTransfer == false && this.IsPromotion == true && this.IsIncrement == true) return "Promotion with Increment";
                else if (this.IsTransfer == true && this.IsPromotion == false && this.IsIncrement == false) return "Transfer";
                else if (this.IsTransfer == false && this.IsPromotion == true && this.IsIncrement == false) return "Promotion";
                else return "Increment";
            }
        }

        public string JoiningDateInString
        {
            get
            {
                return JoiningDate.ToString("dd MMM yyyy");
            }
        }


        public string EffectedDateInString
        {
            get
            {
                return EffectedDate.ToString("dd MMM yyyy");
            }
        }

        public string ActualEffectedDateInString
        {
            get
            {
                if (ActualEffectedDate.Year < 1900)
                    return "-";
                else
                return ActualEffectedDate.ToString("dd MMM yyyy");
            }
        }

        public string RecommendedByDateInString
        {
            get
            {
                return RecommendedByDate.ToString("dd MMM yyyy");
            }
        }

        public List<TransferPromotionIncrement> TransferPromotionIncrements { get; set; }
        public Company Company { get; set; }
        public string ApproveByDateInString
        {
            get
            {
                return ApproveByDate.ToString("dd MMM yyyy");
            }
        }

        public List<EmployeeSalaryStructureDetail> EmployeeSalaryStructureDetails { get; set; }

        #endregion

        #region Functions
        
        public static List<TransferPromotionIncrement> Gets(long nUserID)
        {
            return TransferPromotionIncrement.Service.Gets(nUserID);
        }


        public static List<TransferPromotionIncrement> GetsAutoTPI(long nUserID)
        {
            return TransferPromotionIncrement.Service.GetsAutoTPI(nUserID);
        }
        public static List<TransferPromotionIncrement> Gets(string sSQL, long nUserID)
        {
            return TransferPromotionIncrement.Service.Gets(sSQL, nUserID);
        }
        public static List<TransferPromotionIncrement> GetsIncrementByPercent(string sEmployeeIDs, int nSalaryHeadID, int nPercent, string sMonthIDs, string sYearIDs, string sBUIDs, string sLocationIDs, long nUserID)
        {
            return TransferPromotionIncrement.Service.GetsIncrementByPercent(sEmployeeIDs, nSalaryHeadID, nPercent, sMonthIDs, sYearIDs, sBUIDs, sLocationIDs, nUserID);
        }
        public static TransferPromotionIncrement Get(int id, long nUserID)
        {
            return TransferPromotionIncrement.Service.Get(id, nUserID);
        }

        public TransferPromotionIncrement IUD(int nDBOperation, long nUserID)
        {
            return TransferPromotionIncrement.Service.IUD(this, nDBOperation, nUserID);
        }
        public static List<TransferPromotionIncrement> IUD_Multiple(string sEmployeeIDs, int nSalaryHeadID, int nPercent, string sMonthIDs, string sYearIDs, string sBUIDs, string sLocationIDs, long nUserID)
        {
            return TransferPromotionIncrement.Service.IUD_Multiple(sEmployeeIDs, nSalaryHeadID, nPercent, sMonthIDs, sYearIDs, sBUIDs, sLocationIDs, nUserID);
        }
        public TransferPromotionIncrement AttScheme(int nDBOperation, long nUserID)
        {
            return TransferPromotionIncrement.Service.AttScheme(this, nDBOperation, nUserID);
        }
        public TransferPromotionIncrement IUDQuick(int nDBOperation, long nUserID)
        {
            return TransferPromotionIncrement.Service.IUDQuick(this, nDBOperation, nUserID);
        }
        public static TransferPromotionIncrement Recommend(int nId, long nUserID)
        {
            return TransferPromotionIncrement.Service.Recommend(nId, nUserID);
        }
        public static TransferPromotionIncrement Approve(int nId, long nUserID)
        {
            return TransferPromotionIncrement.Service.Approve(nId, nUserID);
        }
        public static List<TransferPromotionIncrement> MultipleApprove(TransferPromotionIncrement oTransferPromotionIncrement, long nUserID)
        {
            return TransferPromotionIncrement.Service.MultipleApprove(oTransferPromotionIncrement, nUserID);
        }
        public TransferPromotionIncrement Effect(long nUserID)
        {
            return TransferPromotionIncrement.Service.Effect(this, nUserID);
        }
        public static List<TransferPromotionIncrement> UploadXL(List<TransferPromotionIncrement> oTPIs, long nUserID)
        {
            return TransferPromotionIncrement.Service.UploadXL(oTPIs, nUserID);
        }
        public static List<TransferPromotionIncrement> UploadXLAsPerScheme(List<TransferPromotionIncrement> oTPIs, long nUserID)
        {
            return TransferPromotionIncrement.Service.UploadXLAsPerScheme(oTPIs, nUserID);
        }
        public static List<TransferPromotionIncrement> UploadXLTP(List<TransferPromotionIncrement> oTPIs, long nUserID)
        {
            return TransferPromotionIncrement.Service.UploadXLTP(oTPIs, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ITransferPromotionIncrementService Service
        {
            get { return (ITransferPromotionIncrementService)Services.Factory.CreateService(typeof(ITransferPromotionIncrementService)); }
        }

        #endregion
    }
    #endregion

    #region ITransferPromotionIncrement interface

    public interface ITransferPromotionIncrementService
    {
        TransferPromotionIncrement AttScheme(TransferPromotionIncrement oTransferPromotionIncrement, int nDBOperation, Int64 nUserID);
        List<TransferPromotionIncrement> Gets(Int64 nUserID);
        List<TransferPromotionIncrement> GetsAutoTPI(Int64 nUserID);
        List<TransferPromotionIncrement> Gets(string sSQL, Int64 nUserID);
        List<TransferPromotionIncrement> GetsIncrementByPercent(string sEmployeeIDs, int nSalaryHeadID, int nPercent, string sMonthIDs, string sYearIDs, string sBUIDs, string sLocationIDs, Int64 nUserID);
        TransferPromotionIncrement Get(int id, Int64 nUserID);
        TransferPromotionIncrement IUD(TransferPromotionIncrement oTransferPromotionIncrement, int nDBOperation, Int64 nUserID);
        List<TransferPromotionIncrement> IUD_Multiple(string sEmployeeIDs, int nSalaryHeadID, int nPercent, string sMonthIDs, string sYearIDs, string sBUIDs, string sLocationIDs, Int64 nUserID);
        TransferPromotionIncrement IUDQuick(TransferPromotionIncrement oTransferPromotionIncrement, int nDBOperation, Int64 nUserID);
        TransferPromotionIncrement Recommend(int nId, Int64 nUserID);
        TransferPromotionIncrement Approve(int nId, Int64 nUserID);
        List<TransferPromotionIncrement> MultipleApprove(TransferPromotionIncrement oTransferPromotionIncrement, Int64 nUserID);
        TransferPromotionIncrement Effect(TransferPromotionIncrement oTransferPromotionIncrement, Int64 nUserID);
        List<TransferPromotionIncrement> UploadXL(List<TransferPromotionIncrement> oTPIs, Int64 nUserID);
        List<TransferPromotionIncrement> UploadXLAsPerScheme(List<TransferPromotionIncrement> oTPIs, Int64 nUserID);
        
        List<TransferPromotionIncrement> UploadXLTP(List<TransferPromotionIncrement> oTPIs, Int64 nUserID);
    }
    #endregion
}
