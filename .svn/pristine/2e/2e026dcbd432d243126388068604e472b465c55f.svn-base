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
    #region ExportBill

    public class ExportBill : BusinessObject
    {

        #region  Constructor
        public ExportBill()
        {
            ExportBillID = 0;
            ExportBillNo = "";
            ExportLCID = 0;
            Amount = 0;
            State = EnumLCBillEvent.BOEinHand;
            StartDate = new DateTime(1900, 01, 01);
            IsActive = true;
            NoOfPackages = "";
            NetWeight = "";
            GrossWeight = "";
            RelizationDate = DateTime.Today;
            ExportLCNo = "";
            LDBCID = 0;
            NoteCarry = "";
            ErrorMessage = "";
            ProductNature = EnumProductNature.Dyeing;
            Sequence = 0;
            BUID = 0;
            ExportLC = new ExportLC();
            AcceptanceRate = 0;
            EncashCurrencyID = 0;
            EncashCRate = 0;
            EncashAmountBC = 0;
            EncashRemarks = "";
            ForExAmount = 0;
            FileNo = "";
            UPNo = "";
            BankBranchID_Endorse = 0;
            BankBranchID_Ford = 0;
            BBranchName_Endorse = "";
            BankName_Endorse = "";
            BBranchName_Ford = "";
            OverDueAmount = 0;
            ExpBankAccountID = 0;
            ExpCurrencyID = 0;
            ExpAmount = 0;
            ExpCRate = 0;
            ExpAmountBC = 0;
            LDBPCSymbol = "";
            DiscountAdjAmount = 0;
            DiscountAdjCRate = 0;
            DiscountAdjAmountBC = 0;
            LDBPCCRate = 0;
            LDBPCurrcncyID = 0;
            DiscountAdjIsGain = false;
            DiscountAdjGainLossBC = 0;
            DocPrepareDate = DateTime.Now;
            DocDate = DateTime.MinValue;
            ExportBillDetails = new List<ExportBillDetail>();
            ExportBillRealizeds = new List<ExportBillRealized>();
            ExportBillEncashments = new List<ExportBillEncashment>();
            LCTermss = new List<LCTerm>();
            ExportLCType = EnumExportLCType.LC;
            ErrorMessage = "";
        }
        #endregion

        #region Properties
        // ExportBill 
        public int ExportBillID { get; set; }
        public string ExportBillNo { get; set; }
        public string Bill { get; set; }
        public double Amount { get; set; }
        public EnumLCBillEvent State { get; set; }
        public int StateInt { get; set; }
        public DateTime StartDate { get; set; }
        public string NoOfPackages { get; set; }
        public string NetWeight { get; set; }
        public string GrossWeight { get; set; }
        public int ExportLCID { get; set; }
        public int LDBCID { get; set; }
        public bool IsActive { get; set; }
        public int BUID { get; set; }
        public int ApplicantID { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantAddress { get; set; }
        public double OverDueAmount { get; set; }
        public string LDBPCSymbol { get; set; }
        //Bank
        public string BankName_Nego { get; set; }
        public string BBranchName_Nego { get; set; }
        public string NegoBankNickName { get; set; }
        public string BankAddress_Nego { get; set; }
        public string BankName_Issue { get; set; }
        public string BBranchName_Issue { get; set; }
        public string BankAddress_Issue { get; set; }
        public string BankName_Advice { get; set; }
        public string BankAddress_Advice { get; set; }
        public string BBranchName_Advice { get; set; }
        public string BankName_Ford { get; set; }
        public string BBranchName_Ford { get; set; }
        public string BankName_Endorse { get; set; }
        public string BBranchName_Endorse { get; set; }

        //Export LC Derive Property
        public string ExportLCNo { get; set; }
        public string FileNo { get; set; }
        public int BankBranchID_Advice { get; set; }
        public int BankBranchID_Issue { get; set; }
        public int BankBranchID_Nego { get; set; }
        public int BankBranchID_Bill { get; set; }/// its L/C Nego Bank 
        public int BankBranchID_Endorse { get; set; }/// its L/C Issue Bank 
        public int BankBranchID_Ford { get; set; }/// its L/C Issue Bank 
        public double OverDueRate { get; set; }
        public int ExpBankAccountID { get; set; }
        public int ExpCurrencyID { get; set; }
        public double ExpAmount { get; set; }
        public double ExpCRate { get; set; }
        public double ExpAmountBC { get; set; }
        public double DiscountAdjAmount { get; set; }
        public double DiscountAdjCRate { get; set; }
        public double DiscountAdjAmountBC { get; set; }
        public bool DiscountAdjIsGain { get; set; }
        public double DiscountAdjGainLossBC { get; set; }
        public double Amount_LC { get; set; }
        public double Qty { get; set; }
        public string PINo { get; set; }
        public int Sequence { get; set; }
        public int LDBPCurrcncyID { get; set; }
        public DateTime LCOpeningDate { get; set; }
        public DateTime LCRecivedDate { get; set; }
        public string AmendmentNo { get; set; }
        public string LDBCNo { get; set; }
        public string LDBPNo { get; set; }
        public double LDBPAmount { get; set; }
        public DateTime LDBCDate { get; set; }
        public DateTime AcceptanceDate { get; set; } // Doc Submit To Party Bank
        public double AcceptanceRate { get; set; }
        public DateTime MaturityDate { get; set; }
        public DateTime MaturityReceivedDate { get; set; } ///NegotiationDate
        public DateTime RelizationDate { get; set; }
        public DateTime BankFDDRecDate { get; set; }
        public DateTime DiscountedDate { get; set; }
        public DateTime EncashmentDate { get; set; }
        public int EncashCurrencyID { get; set; }
        public double EncashCRate { get; set; }
        public double LDBPCCRate { get; set; }
        public double EncashAmountBC { get; set; }
        public string EncashRemarks { get; set; }
        public double ForExAmount { get; set; }
        public int CurrencyID { get; set; } // as per LC Currency          
        public int LCTramsID { get; set; }// Default LC , Update during Maturity Received
        public string LCTermsName { get; set; }
        // Get from ExportBill History
        public string UPNo { get; set; }
        public DateTime SendToParty { get; set; }
        public DateTime RecdFromParty { get; set; }
        public DateTime SendToBankDate { get; set; }
        public DateTime RecedFromBankDate { get; set; }
        public string NoteCarry { get; set; }
        public DateTime DocPrepareDate { get; set; }
        public DateTime DocDate { get; set; } //DocPrint Date
        public string ErrorMessage { get; set; }
        public string Currency { get; set; }
        public EnumProductNature ProductNature { get; set; }
        public EnumExportLCType ExportLCType { get; set; }
        public ExportLC ExportLC { get; set; }
        public string Params { get; set; }


        #region Derived Properties
        #region Derived
        public List<ExportBillDetail> ExportBillDetails { get; set; }
        public List<ExportBillEncashment> ExportBillEncashments { get; set; }
        public List<ExportBillRealized> ExportBillRealizeds { get; set; }
        public List<LCTerm> LCTermss { get; set; }
        #endregion

        #region DerivedStr
        #region ExportBillNoST
        public string ExportBillNoSt
        {
            get
            {
                return this.Bill + "-" + this.ExportBillNo;

            }
        }
        #endregion
        #region StateST
        public string StateSt
        {
            get
            {
                return EnumObject.jGet(this.State);
            }
        }
        #endregion
        #region AmountSt
        public string AmountSt
        {
            get
            {

                return this.Currency + " " + Global.MillionFormat(this.Amount);

            }
        }
        public string BillAmountSt
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
        #endregion
        #region Amount_LC
        public string Amount_LCSt
        {
            get
            {

                return this.Currency + " " + Global.MillionFormat(this.Amount_LC);

            }
        }
        #endregion
        #region StartDate
        public string StartDateSt
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                if (this.StartDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.StartDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region DocDate
        public string DocDateSt
        {
            get
            {
                if (this.DocDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.DocDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region LCOpeningDatest
        public string LCOpeningDatest
        {
            get
            {
                if (this.LCOpeningDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.LCOpeningDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region SendToParty
        public string SendToPartySt
        {
            get
            {
                if (this.SendToParty == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.SendToParty.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region SendToBankDateStr
        public string SendToBankDateSt
        {
            get
            {
                if (this.SendToBankDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.SendToBankDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region RecdFromPartyStr
        public string RecdFromPartySt
        {
            get
            {
                if (this.RecdFromParty == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.RecdFromParty.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region RecedFromBankDateStr
        public string RecedFromBankDateSt
        {
            get
            {
                if (this.RecedFromBankDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.RecedFromBankDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region LDBCDateStr
        public string LDBCDateSt
        {
            get
            {
                if (this.LDBCDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.LDBCDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region AcceptanceDateSt
        public string AcceptanceDateStr /// Doc Submit To Party Bank
        {
            get
            {
                if (this.AcceptanceDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.AcceptanceDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region MaturityReceivedDateStr
        public string MaturityReceivedDateSt
        {
            get
            {
                if (this.MaturityReceivedDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.MaturityReceivedDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region MaturityDate
        public string MaturityDateSt
        {
            get
            {
                if (this.MaturityDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.MaturityDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region RelizationDateStr
        public string RelizationDateSt
        {
            get
            {
                if (this.RelizationDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.RelizationDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region BankFDDRecDateStr
        public string BankFDDRecDateSt
        {
            get
            {
                if (this.BankFDDRecDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.BankFDDRecDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region DiscountedDateStr
        public string DiscountedDateSt
        {
            get
            {
                if (this.DiscountedDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DiscountedDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region EncashmentDateSt
        public string EncashmentDateSt
        {
            get
            {
                if (this.EncashmentDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.EncashmentDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        public double LDBPAmountBD
        {
            get
            {
                return LDBPAmount * LDBPCCRate;
            }
        }
        #endregion

        #region searching Property
        public string CurrentStateIDs { get; set; }
        public string DateType { get; set; }
        public int DateSearchCriteria { get; set; }
        public int StateDateType { get; set; }
        public int DateSearchState { get; set; }
        public int SearchAmountType { get; set; }
        public double FromAmount { get; set; }
        public double ToAmount { get; set; }
        public DateTime StartDateCritaria { get; set; }
        public DateTime EndDateCritaria { get; set; }
        public DateTime StartDateState { get; set; }
        public DateTime EndDateState { get; set; }
        #endregion
        #endregion

        #endregion

        #region Functions
        public ExportBill Get(int nId, Int64 nUserID)
        {
            return ExportBill.Service.Get(nId, nUserID);
        }
        public ExportBill Save(Int64 nUserID)
        {
            return ExportBill.Service.Save(this, nUserID);
        }
        public ExportBill Save_SendToBuyer(Int64 nUserID)
        {
            return ExportBill.Service.Save_SendToBuyer(this, nUserID);
        }
        public ExportBill SaveHistory(Int64 nUserID)
        {
            return ExportBill.Service.SaveHistory(this, nUserID);
        }
        public ExportBill SaveMaturityReceived(Int64 nUserID)
        {
            return ExportBill.Service.SaveMaturityReceived(this, nUserID);
        }
        public ExportBill Save_Encashment(Int64 nUserID)
        {
            return ExportBill.Service.Save_Encashment(this, nUserID);
        }
        public ExportBill Save_BillRealized(Int64 nUserID)
        {
            return ExportBill.Service.Save_BillRealized(this, nUserID);
        }
        public ExportBill SaveSAN(Int64 nUserID)
        {
            return ExportBill.Service.SaveSAN(this, nUserID);
        }
        public ExportBill SaveDocDate(Int64 nUserID)
        {
            return ExportBill.Service.SaveDocDate(this, nUserID);
        }
        public string Delete(Int64 nUserID)
        {
            return ExportBill.Service.Delete(this, nUserID);
        }
        public static List<ExportBill> Gets(int nExportLCID, Int64 nUserID)
        {
            return ExportBill.Service.Gets(nExportLCID, nUserID);
        }
        public static List<ExportBill> GetsByPI(string nExportPIID, Int64 nUserID)
        {
            return ExportBill.Service.GetsByPI(nExportPIID, nUserID);
        }
        public static List<ExportBill> Gets(string sSQL, Int64 nUserID)
        {
            return ExportBill.Service.Gets(sSQL, nUserID);
        }
        public ExportBill Save_UpdateStatus(int nUserID)
        {
            return ExportBill.Service.Save_UpdateStatus(this, nUserID);
        }
        #endregion

        #region Non DB Functions
        public static string IDInString(List<ExportBill> oExportBills)
        {
            string sReturn = "";
            foreach (ExportBill oItem in oExportBills)
            {
                sReturn = sReturn + oItem.ExportBillID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }

        public static int GetIndex(List<ExportBill> oExportBills, int nExportBillID)
        {
            int index = -1, i = 0;

            foreach (ExportBill oItem in oExportBills)
            {
                if (oItem.ExportBillID == nExportBillID)
                {
                    index = i; break;
                }
                i++;
            }
            return index;
        }
        //--End of Modification

        #endregion

        #region ServiceFactory

        internal static IExportBillService Service
        {
            get { return (IExportBillService)Services.Factory.CreateService(typeof(IExportBillService)); }
        }

        #endregion
    }
    #endregion



    #region IExportBill interface
    [ServiceContract]
    public interface IExportBillService
    {
        ExportBill Save(ExportBill oExportBill, Int64 nUserID);
        ExportBill Save_SendToBuyer(ExportBill oExportBill, Int64 nUserID);
        ExportBill SaveHistory(ExportBill oExportBill, Int64 nUserID);
        ExportBill SaveMaturityReceived(ExportBill oExportBill, Int64 nUserID);
        ExportBill Save_Encashment(ExportBill oExportBill, Int64 nUserID);
        ExportBill Save_BillRealized(ExportBill oExportBill, Int64 nUserID);
        ExportBill SaveSAN(ExportBill oExportBill, Int64 nUserID);
        ExportBill SaveDocDate(ExportBill oExportBill, Int64 nUserID);
        ExportBill Get(int id, Int64 nUserID);
        //List<ExportBill> Gets(Int64 nUserID);
        List<ExportBill> GetsByPI(string nExportPIIDs, Int64 nUserID);

        List<ExportBill> Gets(int nExportLCID, Int64 nUserID);
        List<ExportBill> Gets(string sSQL, Int64 nUserID);
        string Delete(ExportBill oExportBill, Int64 nUserID);
        ExportBill Save_UpdateStatus(ExportBill oExportBill, Int64 nUserID);
    }
    #endregion
}