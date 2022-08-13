using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{

    #region MasterLC

    public class MasterLC : BusinessObject
    {
        public MasterLC()
        {
            MasterLCID = 0;
            MasterLCLogID = 0;
            FileNo = "";
            LCStatus = EnumLCStatus.None;
            MasterLCNo = "";
            Applicant = 0;
            LastDateofShipment = new DateTime(1900, 01, 01);
            ExpireDate = new DateTime(1900, 01, 01);
            MasterLCDate = DateTime.MinValue;
            ReceiveDate = DateTime.Now;
            Beneficiary = 0;
            IssueBankID = 0;
            AdviceBankID = 0;
            CurrencyID = 0;
            LCValue = 0;
            Remark = "";
            ApprovedBy = 0;
            ActualAdvBankID = 0;
            ActualAdvBankName = "";
            ShipmentPort = "";
            PartialShipmentAllow = EnumPartialShipmentAllow.None;
            Transferable = EnumTransferable.None;
            LCType = EnumLCType.At_Sight;
            DeferredFrom = EnumDefferedFrom.None;
            DefferedDaysCount = 0;
            DiscrepancyCharge = 0;
            CurrencyName = "";
            CurrencySymbol = "";
            ApplicantName = "";
            BeneficiaryName = "";
            IssueBankName = "";
            AdviceBankName = "";
            ApprovedByName = "";
            MasterLCActionType = EnumMasterLCActionType.None;
            YetToTransferValue = 0;
            LCQty = 0;
            Consignee = 0;
            NotifyParty = 0;
            ConsigneeName = "";
            NotifyPartyName = "";
            MasterLCType = EnumMasterLCType.MasterLC;
            MasterLCTypeInInt = 0;
            Country = "";
            ProductDesc = "";
            BUID = 0;
            ReviseRequest = new BusinessObjects.ReviseRequest();
            ApprovalRequest = new BusinessObjects.ApprovalRequest();
            LCTransfers = new List<LCTransfer>();
            BusinessUnits = new List<BusinessUnit>();
            MasterLCDetails = new List<MasterLCDetail>();
            YetToInvoiceAmount = 0;
            MLCWithOrder = false;
            ErrorMessage = "";
        }

        #region Properties

        public int MasterLCID { get; set; }

        public int MasterLCLogID { get; set; }
        public string FileNo { get; set; }

        public EnumLCStatus LCStatus { get; set; }

        public string MasterLCNo { get; set; }

        public int Applicant { get; set; }

        public DateTime ReceiveDate { get; set; }

        public DateTime MasterLCDate { get; set; }

        public DateTime LastDateofShipment { get; set; }

        public DateTime ExpireDate { get; set; }

        public int Beneficiary { get; set; }

        public int IssueBankID { get; set; }

        public int AdviceBankID { get; set; }
        public EnumMasterLCType MasterLCType { get; set; }
        public int MasterLCTypeInInt { get; set; }
        public string Country { get; set; }
        public string ProductDesc { get; set; }
        public double LCValue { get; set; }

        public string ShipmentPort { get; set; }

        public EnumPartialShipmentAllow PartialShipmentAllow { get; set; }

        public EnumTransferable Transferable { get; set; }

        public EnumLCType LCType { get; set; }

        public double LCQty { get; set; }

        public int CurrencyID { get; set; }

        public string CurrencyName { get; set; }

        public EnumDefferedFrom DeferredFrom { get; set; }

        public int DefferedDaysCount { get; set; }

        public double DiscrepancyCharge { get; set; }

        public string Remark { get; set; }

        public int ApprovedBy { get; set; }

        public string ApprovedByName { get; set; }

        public int MasterLCHistoryID { get; set; }

        public double Quantity { get; set; }

        public double Amount { get; set; }

        public double YetToTransferValue { get; set; }

        public string CurrencySymbol { get; set; }

        public string ApplicantName { get; set; }

        public string BeneficiaryName { get; set; }

        public string IssueBankName { get; set; }

        public string AdviceBankName { get; set; }
        public int Consignee { get; set; }
        public int NotifyParty { get; set; }
        public string ConsigneeName { get; set; }
        public string NotifyPartyName { get; set; }
        public EnumMasterLCActionType MasterLCActionType { get; set; }
        public double YetToInvoiceAmount { get; set; }
       public int  ActualAdvBankID { get; set; }
       public string ActualAdvBankName { get; set; }
        public bool MLCWithOrder { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<MasterLC> MasterLCList { get; set; }
        public List<MasterLCDetail> MasterLCDetails { get; set; }
        public List<MasterLCHistory> MasterLCHistories { get; set; }
        public List<Employee> Employees { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        public List<VOrder> SaleOrderList { get; set; }
        public List<BusinessSession> BussinessSessions { get; set; }
        public List<User> Users { get; set; }
        public List<MasterLCTermsAndCondition> MasterLCTermsAndConditions { get; set; }
        public List<BankAccount> BankAccounts { get; set; }
        public List<Company> Companies { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<LCTransfer> LCTransfers { get; set; }
        public List<MeasurementUnit> MeasurementUnits { get; set; }
        public List<ReportLayout> ReportLayouts { get; set; }
        public Company Company { get; set; }
        public ContactPersonnel ContactPersonnel { get; set; }
        public ApprovalRequest ApprovalRequest { get; set; }
        public ReviseRequest ReviseRequest { get; set; }
        public Contractor Contractor { get; set; }

        public int OperationBy { get; set; }
        public int LCStatusInInt { get; set; }
        public int PartialShipmentAllowInInt { get; set; }
        public int TransferableInInt { get; set; }
        public int LCTypeInInt { get; set; }
        public int DeferredFromInInt { get; set; }
        public int BUID { get; set; }
        public string ActionTypeExtra { get; set; }
        public string PinCode { get; set; }
        public string sRemark { get; set; }
        public int ContractorID { get; set; }
        public double OrderTagAmount { get; set; }
        public double YetToOrderTagAmount { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        public string OrderRecapNo { get; set; }
        public string StyleNo { get; set; }
        public string SearchingData { get; set; }

        public string MasterLCTypeInString
        {
            get
            {
                return this.MasterLCType.ToString();
            }
        }
        
        public string LCStatusInString
        {
            get
            {
                return this.LCStatus.ToString();
            }

        }
        public string PartialShipmentAllowInString
        {
            get
            {
                return this.PartialShipmentAllow.ToString();
            }

        }
        public string TransferableInString
        {
            get
            {
                return this.Transferable.ToString();
            }

        }
        public string LCTypeInString
        {
            get
            {
                return this.LCType.ToString();
            }

        }
        public string DeferredFromInString
        {
            get
            {
                return this.DeferredFrom.ToString();
            }

        }

       
        public string ReceiveDateInString
        {
            get
            {
                return ReceiveDate.ToString("dd MMM yyyy");

            }
        }

        public string LastDateofShipmentSt
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                if (this.LastDateofShipment == MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.LastDateofShipment.ToString("dd MMM yyyy");
                }
            }
        }
        public string ExpireDateSt
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                if (this.ExpireDate == MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.ExpireDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string MasterLCDateSt
        {
            get
            {

                if (this.MasterLCDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.MasterLCDate.ToString("dd MMM yyyy");
                }
            }
        }


        #endregion

        #region Functions

        public static List<MasterLC> Gets(int buid, long nUserID)
        {
            return MasterLC.Service.Gets(buid,nUserID);
        }
        public static List<MasterLC> GetsByLCID(int nExportLCID, Int64 nUserID)
        {
            return MasterLC.Service.GetsByLCID(nExportLCID, nUserID);
        }
        public static List<MasterLC> GetsMasterLCLog(int id, long nUserID) // id is Master LC ID
        {
            return MasterLC.Service.GetsMasterLCLog(id, nUserID);
        }
        public static List<MasterLC> Gets(string sSQL, long nUserID)
        {

            return MasterLC.Service.Gets(sSQL, nUserID);
        }
        public MasterLC Get(int id, long nUserID)
        {
            return MasterLC.Service.Get(id, nUserID);
        }
        public MasterLC GetBySaleOrder(int id, long nUserID) // Sale Order ID
        {
            return MasterLC.Service.GetBySaleOrder(id, nUserID);
        }
        public MasterLC GetLog(int id, long nUserID) // id is PI Log ID
        {

            return MasterLC.Service.GetLog(id, nUserID);
        }
        public MasterLC Save(long nUserID)
        {
            return MasterLC.Service.Save(this, nUserID);
        }
        public MasterLC AcceptMasterLCAmmendment(long nUserID)
        {
            return MasterLC.Service.AcceptMasterLCAmmendment(this, nUserID);
        }
        public MasterLC ChangeStatus(long nUserID)
        {

            return MasterLC.Service.ChangeStatus(this, nUserID);
        }
        public string Delete(int nMasterLCID, long nUserID)
        {
            return MasterLC.Service.Delete(nMasterLCID, nUserID);
        }
        public static List<MasterLC> GetsByContractorID(int nContractorID, Int64 nUserID)
        {
            return MasterLC.Service.GetsByContractorID(nContractorID, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IMasterLCService Service
        {
            get { return (IMasterLCService)Services.Factory.CreateService(typeof(IMasterLCService)); }
        }
        #endregion
    }
    #endregion

    #region IMasterLC interface

    public interface IMasterLCService
    {
        MasterLC Get(int id, Int64 nUserID);
        List<MasterLC> GetsByLCID(int nExportLCID, Int64 nUserID);
        MasterLC GetBySaleOrder(int id, Int64 nUserID);
        MasterLC GetLog(int id, Int64 nUserID);
        List<MasterLC> Gets(int buid, Int64 nUserID);
        List<MasterLC> GetsMasterLCLog(int id, Int64 nUserID);
        List<MasterLC> Gets(string sSQL, Int64 nUserID);
        MasterLC Save(MasterLC oMasterLC, Int64 nUserID);
        MasterLC AcceptMasterLCAmmendment(MasterLC oMasterLC, Int64 nUserID);
        MasterLC ChangeStatus(MasterLC oMasterLC, Int64 nUserID);
        string Delete(int nMasterLCID, Int64 nUserID);
        List<MasterLC> GetsByContractorID(int nContractorID, Int64 nUserID);
    }
    #endregion


}
