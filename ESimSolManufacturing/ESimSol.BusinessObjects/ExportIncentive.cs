using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExportIncentive

    public class ExportIncentive : BusinessObject
    {
        public ExportIncentive()
        {
            ExportLCID=0;
            ExportLCNo="";
            FileNo="";
            OpeningDate=DateTime.MinValue;
            BankBranchID_Advice=0;
            BankBranchID_Negotiation=0;
            BankBranchID_Issue=0;
            ApplicantID=0;
            ContactPersonalID=0;
            Amount=0.0;
            CurrencyID=0;
            ShipmentDate = DateTime.MinValue;
            ExpiryDate = DateTime.MinValue;
            CurrentStatus = EnumExportLCStatus.FreshLC;
            Remark="";
            AtSightDiffered=false;
            ShipmentFrom="";
            PartialShipmentAllowed=false;
            TransShipmentAllowed = false;
            LiborRate=false;;
            BBankFDD=false;;
            OverDueRate=0.0;
            OverDuePeriod=0;
            VersionNo=0;
            LCRecivedDate = DateTime.MinValue;
            ForeignBank=false;
            IRC="";
            FrightPrepaid="";
            DarkMedium="";
            Year="";
            GetOriginalCopy=false;
            DCharge=0.0;
            Stability=false;
            LCTramsID=0;
            GarmentsQty="";
            NegoDays=0;
            HSCode="";
            AreaCode="";
            AmendmentDate = DateTime.MinValue;
            TextileUnit = EnumTextileUnit.None;
   
            DeliveryToID=0;
            BankBranchID_Forwarding=0;
            PaymentInstruction=EnumPaymentType.None;
            ERC="";
            ApplicantName="";
            CurrencyName="";
            CurrencySymbol="";
            BankName_Nego="";
            BBranchName_Nego="";
            BBranchName_Issue="";
            BBranchAddress_Issue = "";
            BankName_Issue="";
            BankName_Advice="";
            BBranchName_Advice="";
            VersionDate = DateTime.MinValue;
            Amount_BillReal=0;
            BillRelizationDate = DateTime.MinValue;
            ExportIncentiveID=0;
            ExInc_ExportLCID=0;
            ExportBillID=0;
            PRCDate = DateTime.MinValue;
            PRCCollectBy=0;
            ApplicationDate = DateTime.MinValue;
            ApplicationBy = 0;
            BTMAIssueBy = 0;
            BTMAIssueDate = DateTime.MinValue;
            AuditCertDate = DateTime.MinValue;
            AuditCertBy=0;
            RealizedDate = DateTime.MinValue;
            RealizedBy=0;
            Amount_Realized =0;
            Percentage_Incentive = 0.05;
            CurrencyID_Real = 0;

            PRCCollectByName="";
            ApplicationByName="";
            BTMAIssueByName="";
            AuditCertByName = "";
            RealizedByName = "";

            BankSubDate = DateTime.MinValue;
            BankSubByName = "";
            BankSubBy = 0;
            Remarks_BankSub = "";

            Remarks_PRC = "";
            Remarks_Application = "";
            Remarks_BTMA = "";
            Remarks_Audit= "";
            Remarks_Realized = "";
            MasterLCNo = "";
            SLNo = 0;
            IsCopyTo = false;
            Time_Lag = 0;
            ErrorMessage = "";
            //CurrencySymbol_Real="";
        }

        #region Properties
        public int ExportIncentiveID { get; set; }
        public int ExportLCID { get; set; }
        public string ExportLCNo { get; set; }
        public bool LiborRate { get; set; }
        public string FileNo { get; set; }
        public DateTime OpeningDate { get; set; }
        public int BankBranchID_Advice { get; set; }
        public int BankBranchID_Negotiation { get; set; }
        public int BankBranchID_Issue { get; set; }
        public int ApplicantID { get; set; }
        public int ContactPersonalID { get; set; }
        public double Amount { get; set; }
        public int CurrencyID { get; set; }
        public int PRCCollectBy { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime BTMAIssueDate { get; set; }
        public DateTime RealizedDate { get; set; }
        public double Amount_Realized { get; set; }
        public int CurrencyID_Real { get; set; }
        public int RealizedBy { get; set; }
        public int AuditCertBy { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public EnumExportLCStatus CurrentStatus { get; set; }
        public string Remark { get; set; }
        public bool AtSightDiffered { get; set; }
        public string ShipmentFrom { get; set; }
        public bool PartialShipmentAllowed { get; set; }
        public double OverDueRate { get; set; }
        public bool TransShipmentAllowed { get; set; }
        public bool BBankFDD { get; set; }
        public int OverDuePeriod { get; set; }
        public int VersionNo { get; set; }
        public DateTime LCRecivedDate { get; set; }
        public bool ForeignBank { get; set; }
        public string IRC { get; set; }
        public string FrightPrepaid { get; set; }
        public string DarkMedium { get; set; }
        public string Year { get; set; }
        public string MasterLCNos { get; set; }
        public string MasterLCDates { get; set; }
        public bool GetOriginalCopy { get; set; }
        public double DCharge { get; set; }
        public bool Stability { get; set; }
        public int LCTramsID { get; set; }
        public string GarmentsQty { get; set; }
        public int NegoDays { get; set; }
        public string HSCode { get; set; }
        public string AreaCode { get; set; }
        public EnumTextileUnit TextileUnit { get; set; }
        public int DeliveryToID { get; set; }
        public EnumPaymentType PaymentInstruction { get; set; }
        public string CurrencyName { get; set; }
        public string BankName_Nego { get; set; }
        public string BBranchName_Issue { get; set; }
        public string BBranchAddress_Issue { get; set; }
        public string BankName_Advice { get; set; }
        public DateTime VersionDate { get; set; }
        public DateTime BillRelizationDate { get; set; }
        public int ExportBillID { get; set; }
        public DateTime AmendmentDate { get; set; }
        public int BankBranchID_Forwarding { get; set; }
        public string ApplicantName { get; set; }
        public string ERC { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencySymbol_Real { get; set; }
        public string BBranchName_Nego { get; set; }
        public string BankName_Issue { get; set; }
        public string BBranchName_Advice { get; set; }
        public string BBranchAddress_Advice { get; set; }
        public double Amount_BillReal { get; set; }
        public int ExInc_ExportLCID { get; set; }
        public DateTime PRCDate { get; set; }
        public int ApplicationBy { get; set; }
        public DateTime AuditCertDate { get; set; }
        public int BTMAIssueBy { get; set; }
        public string Remarks_PRC { get; set; }
        public string Remarks_Application { get; set; }
        public string Remarks_Realized { get; set; }
        public string Remarks_BTMA { get; set; }
        public string Remarks_Audit { get; set; }

        public DateTime BankSubDate { get; set; }
        public string BankSubByName { get; set; }
        public string Remarks_BankSub { get; set; }
        public int BankSubBy { get; set; }

        public double Percentage_Incentive { get; set; }
        public int SLNo { get; set; }
        public bool IsCopyTo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public int Time_Lag { get; set; }
        public double Amount_Realized_PV
        {
            get
            {
                return (this.Amount * .05);
            }
        }
        public string Amount_Realized_PVST { get { if (this.Percentage_Incentive <= 0) this.Percentage_Incentive = 5; return this.CurrencySymbol + Global.MillionFormat(this.Amount * (this.Percentage_Incentive/100)); } }
        public string Amount_BillRealST { get { return this.CurrencySymbol + Global.MillionFormat(Amount_BillReal); } }
        public string Amount_ST { get { return this.CurrencySymbol + Global.MillionFormat(Amount); } }
        public string Amount_RealizedST { get { return this.CurrencySymbol_Real + Global.MillionFormat(Amount_Realized); } }
     
        public int UpdateOperation { get; set; }
        public string MasterLCNo { get; set; }
        public string PRCCollectByName { get; set; }
        public string ApplicationByName { get; set; }
        public string AuditCertByName { get; set; }
        public string BUShortName { get;set; }
        public string BUName { get; set; }
        public string RealizedByName { get; set; }
        public string BTMAIssueByName { get; set; }
        public string ApplicationDateST
        {
            get
            {
                if (this.ApplicationDate == DateTime.MinValue)
                    return "";
                else
                return ApplicationDate.ToString("dd MMM yyyy");
            }
        }
        public string BTMAIssueDateST
        {
            get
            {
                if (this.BTMAIssueDate == DateTime.MinValue)
                    return "";
                else
                return BTMAIssueDate.ToString("dd MMM yyyy");
            }
        }
        public string AuditCertDateST
        {
            get
            {
                if (this.AuditCertDate == DateTime.MinValue)
                    return "";
                else
                return AuditCertDate.ToString("dd MMM yyyy");
            }
        }
        public string RealizedDateST
        {
            get
            {
                if (this.RealizedDate == DateTime.MinValue)
                    return "";
                else
                return RealizedDate.ToString("dd MMM yyyy");
            }
        }
        public string PRCDateST
        {
            get
            {
                if (this.PRCDate == DateTime.MinValue)
                    return "";
                else
                    return PRCDate.ToString("dd MMM yyyy");
            }
        }
        public string BankSubDateST
        {
            get
            {
                if (this.BankSubDate == DateTime.MinValue)
                    return "";
                else
                    return BankSubDate.ToString("dd MMM yyyy");
            }
        }
        public string BillRelizationDateST
        {
            get
            {
                if (this.BillRelizationDate == DateTime.MinValue)
                    return "";
                else
                    return BillRelizationDate.ToString("dd MMM yyyy");
            }
        }
        //public double TimeLag
        //{
        //    get
        //    {
        //        if (this.ApplicationDate == DateTime.MinValue)
        //            return (DateTime.Now-this.BillRelizationDate).TotalDays+1;
        //        else
        //            return (DateTime.Now - this.ApplicationDate).TotalDays + 1;
        //    }
        //}
        public string Param { get; set; }
        public int CurrentStatusInInt { get { return (int)this.CurrentStatus; } }
        public int TextileUnitInInt { get { return (int)this.TextileUnit; } }
        public string TextileUnitST{ get { return EnumObject.jGet(this.TextileUnit); } }
        public string FileNoYear
        {
            get { return this.FileNo + "/" + this.Year; }

        }
        public string AmendmentDateSt
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                DateTime MinValue1 = new DateTime(0001, 01, 01);
                if (this.AmendmentDate == MinValue || this.AmendmentDate == MinValue1)
                {
                    return "-";
                }
                else
                {
                    return AmendmentDate.ToString("dd MMM yyyy");
                }
            }
        }

        public int VersionNoInInt
        {
            get
            {
                return this.VersionNo;
            }
        }
        public string LCRecivedDateST
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                if (this.LCRecivedDate == MinValue)
                {
                    return "-";
                }
                else
                {
                    return LCRecivedDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string CurrentStatusInST
        {
            get
            {
                //return ExportLCStatusObj.GetEnumExportLCStatusObjs(this.CurrentStatus);
                return "-*-";
            }
        }
        public string OpeningDateST
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                if (this.OpeningDate == MinValue)
                {
                    return "-";
                }
                else
                {
                    return OpeningDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ExpiryDateST
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                if (this.ExpiryDate == MinValue)
                {
                    return "-";
                }
                else
                {
                    return ExpiryDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ShipmentDateST
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                if (this.ShipmentDate == MinValue)
                {
                    return "-";
                }
                else
                {
                    return ShipmentDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string AmountSt
        {
            get
            {
                if (this.Amount < 0)
                {
                    return this.CurrencySymbol + " (" + Global.MillionFormat(this.Amount * (-1)) + ")";
                }
                else
                {
                    return this.CurrencySymbol + " " + Global.MillionFormat(this.Amount);
                }
            }
        }
        private string sGetOrginalCopy = "";
        public string GetOrginalCopySt
        {
            get
            {
                if (this.GetOriginalCopy)
                {
                    sGetOrginalCopy = "Yes";
                }
                else
                {
                    sGetOrginalCopy = "No";
                }

                return sGetOrginalCopy;
            }
        }
        #region AmendmentRequest

        #endregion
        #region AmendmentFullNo
        private string _sAmendmentFullNo;
        public string AmendmentFullNo
        {
            get
            {
                if (this.VersionNo == 0) return "";

                _sAmendmentFullNo = " A-" + this.VersionNo;
                return _sAmendmentFullNo;
            }
        }
        #endregion        
        #endregion

        #region Functions

        public static List<ExportIncentive> Gets(string sSQL, Int64 nUserID)
        {
            return ExportIncentive.Service.Gets(sSQL, nUserID);
        }

        public ExportIncentive Get(int nId, long nUserID)
        {
            return ExportIncentive.Service.Get(nId, nUserID);
        }
        public ExportIncentive Save(long nUserID)
        {
            return ExportIncentive.Service.Save(this, nUserID);
        }
        
        //Update_PRCDate,Update_ApplicationDate,Update_BTMAIssueDate,Update_AuditCertDate,Update_RealizedDate
        public ExportIncentive Update_PRCDate(long nUserID)
        {
            return ExportIncentive.Service.Update_PRCDate(this, nUserID);
        } 
        public ExportIncentive Update_ApplicationDate(long nUserID)
        {
            return ExportIncentive.Service.Update_ApplicationDate(this, nUserID);
        } 
        public ExportIncentive Update_BTMAIssueDate(long nUserID)
        {
            return ExportIncentive.Service.Update_BTMAIssueDate(this, nUserID);
        }
        public ExportIncentive Update_AuditCertDate(long nUserID)
        {
            return ExportIncentive.Service.Update_AuditCertDate(this, nUserID);
        }
        public ExportIncentive Update_RealizedDate(long nUserID)
        {
            return ExportIncentive.Service.Update_RealizedDate(this, nUserID);
        }
        public ExportIncentive Update_BankSubDate(long nUserID)
        {
            return ExportIncentive.Service.Update_BankSubDate(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return ExportIncentive.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IExportIncentiveService Service
        {
            get { return (IExportIncentiveService)Services.Factory.CreateService(typeof(IExportIncentiveService)); }
        }
        #endregion

    }
    #endregion

    #region IExportIncentive interface

    public interface IExportIncentiveService
    {
        ExportIncentive Get(int id, long nUserID);
        List<ExportIncentive> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        ExportIncentive Save(ExportIncentive oExportIncentive, long nUserID);
        ExportIncentive Update_PRCDate(ExportIncentive oExportIncentive, long nUserID);
        ExportIncentive Update_ApplicationDate(ExportIncentive oExportIncentive, long nUserID);
        ExportIncentive Update_BTMAIssueDate(ExportIncentive oExportIncentive, long nUserID);
        ExportIncentive Update_AuditCertDate(ExportIncentive oExportIncentive, long nUserID);
        ExportIncentive Update_RealizedDate(ExportIncentive oExportIncentive, long nUserID);
        ExportIncentive Update_BankSubDate(ExportIncentive oExportIncentive, long nUserID);
    }
    #endregion
}