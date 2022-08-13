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
    #region ExportOutstandingDetail
    public class ExportOutstandingDetail : BusinessObject
    {

        #region  Constructor
        public ExportOutstandingDetail()
        {
            ExportBillID = 0;
            ExportBillNo = "";
            ExportLCID = 0;
            Amount = 0;
            State = EnumLCBillEvent.BOEinHand;
            StartDate = DateTime.Today;
            RelizationDate = DateTime.Today;
            ExportLCNo = "";
            ErrorMessage = "";
            BBranchID_Nego = 0;
            BankName_Issue = "";
            ExportLCNo = "";
            DiscountType = 0;
            LDBCID = 0;
            TrgNote = "";
            DeliveryQty = 0;
            DeliveryValue = 0;
            Amount = 0;
            Qty = 0;
            ContractorID = 0;
            ContractorName = "";
            ApplicantAddress = "";
            BankName_Nego = "";
            BBranchName_Nego = "";
            NegoBankNickName = "";
            BankAddress_Nego = "";
            BankName_Issue = "";
            BBranchName_Issue = "";
            BankAddress_Issue = "";
            BankName_Advice = "";
            BankAddress_Advice = "";
            BankName_Advice = "";
            BankName_Advice = "";
            BBranchName_Advice = "";
            ExportLCNo = "";
            BankBranchID_Nego = 0;
            ExportOutstandingDetails = new List<ExportOutstandingDetail>();
        }
        #endregion

        #region Properties
        public int ExportBillID { get; set; }
        public string ExportBillNo { get; set; }
        public double Amount { get; set; }
        public double Qty { get; set; }
        public EnumLCBillEvent State { get; set; }
        public EnumExportLCStatus LCStatus { get; set; }
        public DateTime StartDate { get; set; }
        public int ExportLCID { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public string ApplicantAddress { get; set; }
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
        public string ExportLCNo { get; set; }
        //public int BankBranchID_Advice { get; set; }
        //public int BankBranchID_Issue { get; set; }
        public int BankBranchID_Nego { get; set; }
        public double OverDueRate { get; set; }
        public int LDBCID { get; set; }
        public string TrgNote { get; set; }
        public double Amount_LC { get; set; }
        public double DeliveryValue { get; set; }
        public double DeliveryQty { get; set; }
        public string PINo { get; set; }
        public DateTime LCOpeningDate { get; set; }
        public DateTime LCRecivedDate { get; set; }
        public string MKTPName { get; set; }
        public string LDBCNo { get; set; }
        public string LDBPNo { get; set; }
        public double LDBPAmount { get; set; }
        public DateTime LDBCDate { get; set; }
        public DateTime AcceptanceDate { get; set; } // Doc Submit To Party Bank
        public DateTime MaturityDate { get; set; }
        public DateTime MaturityReceivedDate { get; set; } ///NegotiationDate
        public DateTime RelizationDate { get; set; }
        public DateTime BankFDDRecDate { get; set; }
        public DateTime DiscountedDate { get; set; }
        public DateTime EncashmentDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string LCTermsName { get; set; }
        public DateTime SendToParty { get; set; }
        public DateTime RecdFromParty { get; set; }
        public DateTime SendToBankDate { get; set; }
        public DateTime RecedFromBankDate { get; set; }
        public string ErrorMessage { get; set; }
        public string Currency { get; set; }
        public Company Company { get; set; }
        public int DiscountType { get; set; }
        public List<ExportOutstandingDetail> ExportOutstandingDetails { get; set; }
        public int BBranchID_Nego { get; set; }
        #region Derived Properties
        #region Derived


        #endregion

        #region DerivedStr
        #region StateST
        public string StateSt
        {
            get
            {

                return this.State.ToString();

            }
        }
        #endregion
        #region LCStatusST
        public string LCStatusSt
        {
            get
            {

                return this.LCStatus.ToString();

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
        #endregion
        #region LCinHandSt
        public string LCinHandSt
        {
            get
            {
                return this.Currency + " " + Global.MillionFormat(this.Amount_LC - this.Amount);
            }
        }
        #endregion
        #region DeliveryValueSt
        public string DeliveryValueSt
        {
            get
            {
                return this.Currency + " " + Global.MillionFormat(this.DeliveryValue);
            }
        }
        #endregion
        #region QtySt
        public string QtySt
        {
            get
            {

                return " " + Global.MillionFormat(this.Qty);

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
        #region EncashmentDateStr
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
        #region DeliveryDateStr
        public string DeliveryDateSt
        {
            get
            {
                if (this.DeliveryDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DeliveryDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region TimeLagDalivery
        private int _nTimeLag_Dalivery;
        public int TimeLag_Dalivery
        {
            get
            {
                if (this.DeliveryDate == DateTime.MinValue)
                {
                    _nTimeLag_Dalivery = 0;
                }
                else if (this.DeliveryDate != DateTime.MinValue && this.StartDate == DateTime.MinValue)
                {
                    _nTimeLag_Dalivery = Global.DateDiff("d", Convert.ToDateTime(this.DeliveryDate), DateTime.Today);
                }
                else
                {
                    _nTimeLag_Dalivery = Global.DateDiff("d", Convert.ToDateTime(this.DeliveryDate), Convert.ToDateTime(this.StartDate));
                }

                return _nTimeLag8;
            }

        }
        #endregion
        #region TimeLag1
        private int _nTimeLag1;
        public int TimeLag1
        {
            get
            {
                if (this.ExportLCID <= 0)
                {
                    return 0;
                }
                else
                {
                    return _nTimeLag1 = Global.DateDiff("d", this.LCRecivedDate, Convert.ToDateTime(this.StartDate.ToString("dd MMM yyyy")));
                }
            }

        }
        #endregion
        #region TimeLag2
        private int _nTimeLag2;
        public int TimeLag2
        {
            get
            {
                if (this.StartDate == DateTime.MinValue || this.SendToParty == DateTime.MinValue)
                {
                    return 0;
                }
                else if (this.StartDate != DateTime.MinValue || this.SendToParty == DateTime.MinValue)
                {
                    return _nTimeLag2 = Global.DateDiff("d", Convert.ToDateTime(this.StartDate), Convert.ToDateTime(DateTime.Now));
                }
                else
                {
                    return _nTimeLag2 = Global.DateDiff("d", Convert.ToDateTime(this.StartDate), Convert.ToDateTime(this.SendToParty));
                }
            }

        }
        #endregion
        #region TimeLag3
        private int _nTimeLag3;
        public int TimeLag3
        {
            get
            {
                if (this.SendToParty == DateTime.MinValue)
                {
                    _nTimeLag3 = 0;
                }
                else if (this.SendToParty != DateTime.MinValue && this.RecdFromParty == DateTime.MinValue)
                {
                    _nTimeLag3 = Global.DateDiff("d", Convert.ToDateTime(SendToParty), Convert.ToDateTime(DateTime.Today));
                }
                else
                {
                    _nTimeLag3 = Global.DateDiff("d", Convert.ToDateTime(this.SendToParty), Convert.ToDateTime(this.RecdFromParty));
                }
                return _nTimeLag3;
            }

        }
        #endregion
        #region TimeLag4
        private int _nTimeLag4;
        public int TimeLag4
        {
            get
            {
                if (this.RecdFromParty == DateTime.MinValue || this.SendToBankDate == DateTime.MinValue)
                {
                    _nTimeLag4 = 0;
                }
                else if (this.RecdFromParty != DateTime.MinValue || this.SendToBankDate == DateTime.MinValue)
                {
                    _nTimeLag4 = Global.DateDiff("d", Convert.ToDateTime(this.RecdFromParty), Convert.ToDateTime(DateTime.Today));
                }
                else
                {
                    _nTimeLag4 = Global.DateDiff("d", Convert.ToDateTime(this.RecdFromParty), Convert.ToDateTime(this.SendToBankDate));
                }
                return _nTimeLag4;
            }

        }
        #endregion
        #region TimeLag5
        private int _nTimeLag5;
        public int TimeLag5
        {
            get
            {
                if (this.SendToBankDate == DateTime.MinValue || this.RecedFromBankDate == DateTime.MinValue)
                {
                    _nTimeLag5 = 0;
                }
                else if (this.SendToBankDate != DateTime.MinValue || this.RecedFromBankDate == DateTime.MinValue)
                {
                    _nTimeLag5 = Global.DateDiff("d", Convert.ToDateTime(this.SendToBankDate), Convert.ToDateTime(DateTime.Today));
                }
                else
                {
                    _nTimeLag5 = Global.DateDiff("d", Convert.ToDateTime(this.SendToBankDate), Convert.ToDateTime(this.RecedFromBankDate));
                }
                return _nTimeLag5;
            }

        }
        #endregion
        #region TimeLag6
        private int _nTimeLag6;
        public int TimeLag6
        {
            get
            {
                if (this.LDBCDate == DateTime.MinValue || this.MaturityReceivedDate == DateTime.MinValue)
                {
                    _nTimeLag6 = 0;
                }
                else if (this.LDBCDate != DateTime.MinValue || this.MaturityReceivedDate == DateTime.MinValue)
                {
                    _nTimeLag6 = Global.DateDiff("d", Convert.ToDateTime(this.LDBCDate), Convert.ToDateTime(DateTime.Today));
                }
                else
                {
                    _nTimeLag6 = Global.DateDiff("d", Convert.ToDateTime(this.LDBCDate), Convert.ToDateTime(this.MaturityReceivedDate));
                }
                return _nTimeLag6;
            }

        }
        #endregion
        #region TimeLag7
        private int _nTimeLag7;
        public int TimeLag7
        {
            get
            {
                if (this.BankFDDRecDate == DateTime.MinValue || this.EncashmentDate == DateTime.MinValue)
                {
                    return 0;
                }
                else
                {
                    return _nTimeLag7 = Global.DateDiff("d", Convert.ToDateTime(this.BankFDDRecDate), Convert.ToDateTime(this.EncashmentDate));
                }
            }

        }
        #endregion
        #region TimeLag8
        private int _nTimeLag8;
        public int TimeLag8
        {
            get
            {
                if (this.MaturityDate == DateTime.MinValue && this.RelizationDate == DateTime.MinValue)
                {
                    _nTimeLag8 = 0;
                }
                else if (this.MaturityDate != DateTime.MinValue && this.RelizationDate == DateTime.MinValue)
                {
                    _nTimeLag8 = Global.DateDiff("d", Convert.ToDateTime(this.MaturityDate), DateTime.Today);
                }
                else
                {
                    _nTimeLag8 = Global.DateDiff("d", Convert.ToDateTime(this.MaturityDate), Convert.ToDateTime(this.RelizationDate));
                }

                return _nTimeLag8;
            }

        }
        #endregion

        #endregion
        #region searching Property

        public string CurrentStateIDs { get; set; }

        public int ReportType { get; set; }

        public int DateSearchCriteria { get; set; }

        public string SelectedOption { get; set; }

        public DateTime DateofMaturityEnd { get; set; }


        #endregion

        #endregion
        #endregion

        #region Functions

        #region Collecion Functions
        public static List<ExportOutstandingDetail> Gets(long nUserID)
        {
            return ExportOutstandingDetail.Service.Gets(nUserID);
        }
        public static List<ExportOutstandingDetail> Gets(int nReportType, int nBUID, DateTime dFromDate, DateTime dToDate, int nDiscountType, long nUserID)
        {
            return ExportOutstandingDetail.Service.Gets(nReportType, nBUID, dFromDate, dToDate, nDiscountType, nUserID);
        }
        public static List<ExportOutstandingDetail> Gets(string sSQL, long nUserID)
        {
            return ExportOutstandingDetail.Service.Gets(sSQL, nUserID);
        }


        #endregion
        #endregion


        #region ServiceFactory

        internal static IExportOutstandingDetailService Service
        {
            get { return (IExportOutstandingDetailService)Services.Factory.CreateService(typeof(IExportOutstandingDetailService)); }
        }
        #endregion
    }
    #endregion



    #region IExportOutstandingDetail interface
    [ServiceContract]
    public interface IExportOutstandingDetailService
    {
        List<ExportOutstandingDetail> Gets(Int64 nUserID);
        List<ExportOutstandingDetail> Gets(int nReportType, int nBUID, DateTime dFromDate, DateTime dToDate, int nDiscountType, Int64 nUserID);
        List<ExportOutstandingDetail> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}