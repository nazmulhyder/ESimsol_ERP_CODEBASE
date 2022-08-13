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
    #region DisciplinaryAction

    public class DisciplinaryAction : BusinessObject
    {
        public DisciplinaryAction()
        {
            DisciplinaryActionID = 0;
            ActionType = EnumEnumAllowanceType.Deduction;
            EmployeeID = 0;
            PaymentCycle = EnumPaymentCycle.None;
            Description = "";
            Amount = 0;
            CompAmount = 0;
            ExecutedOn = DateTime.Now;
            ApproveBy = 0;
            ApproveByName = "";
            ApproveByDate = DateTime.Now;
            ProcessID = 0;
            IsLock = true;
            EmployeeName = "";
            EmployeeCode = "";
            OfficialInfo = "";
            SalaryHeadID = 0;
            SalaryHeadName = "";
            SalaryHeadType = EnumSalaryHeadType.None;
            ErrorMessage = "";
            EncryptDAID = "";
            sIDs = "";
            bUpload = false;
        }


        #region Properties
        public int DisciplinaryActionID { get; set; }
        public bool bUpload { get; set; }
        public EnumEnumAllowanceType ActionType { get; set; }
        public int SalaryHeadID { get; set; }
        public int EmployeeID { get; set; }
        public EnumPaymentCycle PaymentCycle { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public double CompAmount { get; set; }
        public DateTime ExecutedOn { get; set; }
        public int ApproveBy { get; set; }
        public string ApproveByName { get; set; }
        public DateTime ApproveByDate { get; set; }
        public int ProcessID { get; set; }
        public bool IsLock { get; set; }
        public string OfficialInfo { get; set; }
        public string ErrorMessage { get; set; }
        public string EncryptDAID { get; set; }

        #endregion

        #region Derived Property
        public string SalaryHeadName { get; set; }
        public EnumSalaryHeadType SalaryHeadType { get; set; }
        public double ProductionAmount { get; set; }
        public string Remark { get; set; }
        public DateTime JoiningDate { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string LocationName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public List<DisciplinaryAction> DisciplinaryActions { get; set; }
        public Company Company { get; set; }
        public string sIDs { get; set; }
        public string EmployeeNameCode
        {
            get
            {
                string S = "";
                if (this.EmployeeName !="")
                {
                    S += this.EmployeeName;
                }
                if (this.EmployeeCode != "")
                {
                    S += "["+this.EmployeeCode+"]";
                }
                return S;
            }
        }
        public string ExecutedOnInString
        {
            get
            {
                return ExecutedOn.ToString("dd MMM yyyy");
            }
        }
        public string ApproveByDateInString
        {
            get
            {
                return ApproveByDate.ToString();
            }
        }
        public string JoiningDateInString
        {
            get
            {
                return JoiningDate.ToString("dd MMM yyyy");
            }
        }

        public double NetPayment
        {
            get
            {
                if (this.ProductionAmount % 1000 >= 500)
                    return ProductionAmount - ProductionAmount % 1000 + 1000;
                else if (this.ProductionAmount % 1000 < 500)
                    return ProductionAmount - ProductionAmount % 1000;
                else if (this.ProductionAmount % 1000 == 0)
                    return ProductionAmount;
                else
                    return 0;
            }
        }

        public int SalaryHeadTypeInt { get; set; }
        public string SalaryHeadTypeInString
        {
            get
            {
                return SalaryHeadType.ToString();
            }
        }
        public int PaymentCycleInt { get; set; }
        public string PaymentCycleInString
        {
            get
            {
                return PaymentCycle.ToString();
            }
        }
        #endregion

        #region Functions

        public static List<DisciplinaryAction> Gets(string sSQL, long nUserID)
        {
            return DisciplinaryAction.Service.Gets(sSQL, nUserID);
        }

        public DisciplinaryAction Get(int id, long nUserID)
        {
            return DisciplinaryAction.Service.Get(id, nUserID);
        }

        public List<DisciplinaryAction> IUD(int nDBOperation, long nUserID)
        {
            return DisciplinaryAction.Service.IUD(this, nDBOperation, nUserID);
        }
        public List<DisciplinaryAction> IUD_List(List<DisciplinaryAction> oDAs, long nUserID)
        {
            return DisciplinaryAction.Service.IUD_List(oDAs, nUserID);
        }
        public static List<DisciplinaryAction> GetsForDAProcess(string sParams, long nUserID)
        {
            return DisciplinaryAction.Service.GetsForDAProcess(sParams, nUserID);
        }

        public static List<DisciplinaryAction> UploadXL(List<DisciplinaryAction> oDAs, long nUserID)
        {
            return DisciplinaryAction.Service.UploadXL(oDAs, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDisciplinaryActionService Service
        {
            get { return (IDisciplinaryActionService)Services.Factory.CreateService(typeof(IDisciplinaryActionService)); }
        }

        #endregion
    }
    #endregion

    #region IDisciplinaryAction interface
    public interface IDisciplinaryActionService
    {
        DisciplinaryAction Get(int id, Int64 nUserID);
        List<DisciplinaryAction> Gets(string sSQL, Int64 nUserID);
        List<DisciplinaryAction> IUD(DisciplinaryAction oDisciplinaryAction, int nDBOperation, Int64 nUserID);
        List<DisciplinaryAction> IUD_List(List<DisciplinaryAction> oDisciplinaryActions, Int64 nUserID);
        List<DisciplinaryAction> GetsForDAProcess(string sParams, Int64 nUserID);
        List<DisciplinaryAction> UploadXL(List<DisciplinaryAction> oDAs, Int64 nUserID);

    }
    #endregion
}
