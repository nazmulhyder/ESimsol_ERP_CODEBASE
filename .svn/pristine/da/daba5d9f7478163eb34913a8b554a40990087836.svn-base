using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExportBillReport
    [DataContract]
    public class ExportBillReport : BusinessObject
    {

        #region  Constructor
        public ExportBillReport()
        {
            ExportBillID = 0;
            ExportBillNo = "";
            ExportLCID = 0;
            Amount = 0;
            State = EnumLCBillEvent.BOEinHand;
            StartDate = DateTime.Today;
            IsActive = true;
            MaturityDate = DateTime.Today;
            RelizationDate = DateTime.Today;
            LDBCNo="";
            ExportLCNo = "";
            LDBCID = 0;
            Params = "";
            ErrorMessage = "";
            DiscountedDate=  DateTime.Today;
            Sequence = 0;
            ShipmentDate = DateTime.Today;
            ExpiryDate = DateTime.Today;
            MKTPName = "";
            UPNo = "";
            ProductName = "";
            MasterLCNos = "";
            Qty = 0;
            Qty_Bill = 0;
            Params = "";
            ExportLCType = EnumExportLCType.None;
        }
        #endregion

        #region Properties
    
        public int ExportBillID { get; set; }
        public string ExportBillNo { get; set; }
        public string Bill { get; set; }
        public double Amount { get; set; }
        public double UnitPrice { get; set; }
        public EnumLCBillEvent State { get; set; }
        public int StateInt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DocPrepareDate { get; set; }
        public int ExportLCID { get; set; }
        public int LDBCID { get; set; }
        public bool IsActive { get; set; }
        public string UPNo { get; set; }
        public string MasterLCNos { get; set; }

        //Export LC Derive Property
        //Contractor
        public int ApplicantID { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantAddress { get; set; }
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

        //Export LC Derive Property
        public string ExportLCNo { get; set; }
        public int BankBranchID_Advice { get; set; }
        public int BankBranchID_Issue { get; set; }
        public int BankBranchID_Nego { get; set; }
        public double OverDueRate { get; set; }
        public int CreditAvailableDays { get; set; }
        public double Amount_LC { get; set; }
        public string PINo { get; set; }
        public DateTime LCOpeningDate { get; set; }
        public DateTime LCRecivedDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string AmendmentNo { get; set; }
        public int BUID { get; set; }
        public string LDBCNo { get; set; }
        public string LDBPNo { get; set; }
        public string MKTPName { get; set; }
        public double LDBPAmount { get; set; }
        public DateTime LDBCDate { get; set; }
        public DateTime AcceptanceDate { get; set; } // Doc Submit To Party Bank
        public DateTime MaturityDate { get; set; }
        public DateTime MaturityReceivedDate { get; set; } ///NegotiationDate
        public DateTime RelizationDate { get; set; }
        public DateTime BankFDDRecDate { get; set; }
        public DateTime DiscountedDate { get; set; }
        public DateTime EncashmentDate { get; set; }
        public int CurrencyID { get; set; } // as per LC Currency
        public int LCTramsID { get; set; }// Default LC , Update during Maturity Received
        [DataMember]
        public string LCTermsName { get; set; }
        public DateTime SendToParty { get; set; }
        public DateTime RecdFromParty { get; set; }
        public DateTime SendToBankDate { get; set; }
        public DateTime RecedFromBankDate { get; set; }
        public string Params { get; set; }
        public string ErrorMessage { get; set; }
        public string Currency { get; set; }
        public string ProductName { get; set; }
        public double Qty { get; set; }
        public double Qty_Bill { get; set; }
        public int Sequence { get; set; }
        
        #region Derived Properties
        #region Derived
        public EnumExportLCType ExportLCType { get; set; }
        public List<LCTerm> LCTermss { get; set; }
        public List<BankBranch> BankBranchs { get; set; }
        [DataMember]
        public List<Currency> Currencys { get; set; }
        public List<ExportBillEncashment> ExportBillEncashments { get; set; }
        public List<ExportBillRealized> ExportBillRealizeds { get; set; }
        public List<BankAccount> BankAccounts { get; set; }

        #endregion 
        public int DueDay { get { return Math.Abs(Global.DateDiff("d",this.MaturityDate , DateTime.Now)); } }
        public int DueDay_Party
        {
            get
            {
                if (this.SendToParty == DateTime.MinValue)
                {
                    return 0;
                }
                else if (this.RecdFromParty == DateTime.MinValue)
                {
                    return Global.DateDiff("d", Convert.ToDateTime(SendToParty), Convert.ToDateTime(DateTime.Now));
                }
                else
                {
                    return Global.DateDiff("d", Convert.ToDateTime(SendToParty), Convert.ToDateTime(this.RecdFromParty));
                }
            }

        }

        public string Qty_BillSt
        {
            get
            {
                return  Global.MillionFormat(this.Qty_Bill);
            }
        }
        #region DerivedStr
        public string ExportBillNoSt
        {
            get
            {
                return this.Bill + "-" + this.ExportBillNo;

            }
        }
        private int nDgvStyle = 0;
        public int DgvStyle
        {
            get
            {
                if (this.State == EnumLCBillEvent.BankAcceptedBill || this.State == EnumLCBillEvent.Discounted || this.State == EnumLCBillEvent.ReqForDiscounted)
                {
                    if (this.MaturityDate <= DateTime.Today)
                    {
                        nDgvStyle = 1;
                    }
                    else
                    {
                        nDgvStyle= 0;
                    }
                }
                else
                {
                    nDgvStyle = 0;
                }
                return nDgvStyle;

            }
        }

        #region StateST
        public string StateSt
        {
            get
            {
                return EnumObject.jGet((EnumLCBillEvent)this.State);
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
        #region LCRecivedDateSt
        public string LCRecivedDateSt
        {
            get
            {
                if (this.LCRecivedDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.LCRecivedDate.ToString("dd MMM yyyy");
                }
            }
        }

        #endregion
        #region DocPrepareDateSt
        public string DocPrepareDateSt
        {
            get
            {
                if (this.DocPrepareDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.DocPrepareDate.ToString("dd MMM yyyy");
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
        #region ShipmentDatest
        public string ShipmentDateSt
        {
            get
            {
                if (this.ShipmentDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ShipmentDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #region ExpiryDateSt
        public string ExpiryDateSt
        {
            get
            {
                if (this.ExpiryDate == DateTime.MinValue)
                {
                    return "--";
                }
                else
                {
                    return this.ExpiryDate.ToString("dd MMM yyyy");
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
        #region TimeLag1
        private int _nTimeLag1;
        public int TimeLag1
        {
            get
            {
                if (this.LCRecivedDate == DateTime.MinValue || this.StartDate == DateTime.MinValue)
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
                if (this.SendToParty == DateTime.MinValue || this.RecdFromParty == DateTime.MinValue)
                {
                    return 0;
                }
                else
                {
                    return _nTimeLag3 = Global.DateDiff("d", Convert.ToDateTime(SendToParty), Convert.ToDateTime(this.RecdFromParty));
                }
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
                    return 0;
                }
                else
                {
                    return _nTimeLag4 = Global.DateDiff("d", Convert.ToDateTime(this.RecdFromParty), Convert.ToDateTime(this.SendToBankDate));
                }
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
                    return 0;
                }
                else
                {
                    return _nTimeLag5 = Global.DateDiff("d", Convert.ToDateTime(this.SendToBankDate), Convert.ToDateTime(this.RecedFromBankDate));
                }
            }

        }
        #endregion
        #region TimeLag6
        private int _nTimeLag6;
        public int TimeLag6
        {
            get
            {
                if (this.RecedFromBankDate == DateTime.MinValue || this.LDBCDate == DateTime.MinValue)
                {
                    return 0;
                }
                else
                {
                    return _nTimeLag6 = Global.DateDiff("d", Convert.ToDateTime(this.RecedFromBankDate), Convert.ToDateTime(this.LDBCDate));
                }
            }

        }
        #endregion
        #region TimeLag7
        private int _nTimeLag7;
        public int TimeLag7
        {
            get
            {
                if (this.LDBCDate == DateTime.MinValue || this.AcceptanceDate == DateTime.MinValue)
                {
                    return 0;
                }
                else
                {
                    return _nTimeLag7 = Global.DateDiff("d", Convert.ToDateTime(this.LDBCDate), Convert.ToDateTime(this.AcceptanceDate));
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
                if (this.AcceptanceDate == DateTime.MinValue || this.MaturityReceivedDate == DateTime.MinValue)
                {
                    return 0;
                }
                else
                {
                    return _nTimeLag8 = Global.DateDiff("d", Convert.ToDateTime(this.AcceptanceDate), Convert.ToDateTime(this.MaturityReceivedDate));
                }
            }

        }
        #endregion
        #region TimeLag9
        private int _nTimeLag9;
        public int TimeLag9
        {
            get
            {
                if (this.MaturityReceivedDate == DateTime.MinValue || this.MaturityDate == DateTime.MinValue)
                {
                    return 0;
                }
                else
                {
                    return _nTimeLag8 = Global.DateDiff("d", Convert.ToDateTime(this.MaturityReceivedDate), Convert.ToDateTime(this.MaturityDate));
                }
            }

        }
        #endregion
        #region TimeLag10
        private int _nTimeLag10;
        public int TimeLag10
        {
            get
            {
                if (this.MaturityDate == DateTime.MinValue || this.DiscountedDate == DateTime.MinValue)
                {
                    return 0;
                }
                else
                {
                    return _nTimeLag8 = Global.DateDiff("d", Convert.ToDateTime(this.MaturityDate), Convert.ToDateTime(this.DiscountedDate));
                }
            }

        }
        #endregion
        #region TimeLag11
        public int TimeLag11
        {
            get
            {
                if (this.DiscountedDate == DateTime.MinValue || this.RelizationDate == DateTime.MinValue)
                {
                    return 0;
                }
                else
                {
                    return _nTimeLag8 = Global.DateDiff("d", Convert.ToDateTime(this.DiscountedDate), Convert.ToDateTime(this.RelizationDate));
                }
            }

        }
        #endregion
        #region TimeLag12
        public int TimeLag12
        {
            get
            {
                if (this.RelizationDate == DateTime.MinValue || this.BankFDDRecDate == DateTime.MinValue)
                {
                    return 0;
                }
                else
                {
                    return _nTimeLag8 = Global.DateDiff("d", Convert.ToDateTime(this.RelizationDate), Convert.ToDateTime(this.BankFDDRecDate));
                }
            }

        }
        #endregion
        #region TimeLag13
        private int _nTimeLag13;
        public int TimeLag13
        {
            get
            {
                if (this.BankFDDRecDate == DateTime.MinValue || this.EncashmentDate == DateTime.MinValue)
                {
                    return 0;
                }
                else
                {
                    return _nTimeLag8 = Global.DateDiff("d", Convert.ToDateTime(this.BankFDDRecDate), Convert.ToDateTime(this.EncashmentDate));
                }
            }

        }
        #endregion
        #region TimeLag14
        //private int _nTimeLag14;
        public int TimeLag14
        {
            get
            {
                if ( this.LDBCDate == DateTime.MinValue)
                {
                    return 0;
                }
                else if (this.LDBCDate != DateTime.MinValue && this.State == EnumLCBillEvent.NegotiatedBill)
                {
                    return  Global.DateDiff("d", Convert.ToDateTime(this.LDBCDate), Convert.ToDateTime(DateTime.Today));
                }
                else if (this.MaturityReceivedDate != DateTime.MinValue && this.LDBCDate != DateTime.MinValue)
                {
                    return  Global.DateDiff("d", Convert.ToDateTime(this.LDBCDate), Convert.ToDateTime(this.MaturityReceivedDate));
                }
                else
                {
                    return 0;
                }
            }

        }
        #endregion
        #endregion
        

        #endregion
        #endregion

        #region Functions
        public ExportBillReport Get(int nId, Int64 nUserID)
        {
            return ExportBillReport.Service.Get(nId, nUserID);
        }

        public ExportBillReport GetByLDBCNo(string sLDBCNo, Int64 nUserID)
        {
            return ExportBillReport.Service.GetByLDBCNo(sLDBCNo, nUserID);
        }
     
        #region Collecion Functions
    
        public static List<ExportBillReport> Gets(string sSQL, Int64 nUserID)
        {
            return ExportBillReport.Service.Gets(sSQL,nUserID);
        }

        #region Non DB Functions

        public static string IDInString(List<ExportBillReport> oExportBillReports)
        {
            string sReturn = "";
            foreach (ExportBillReport oItem in oExportBillReports)
            {
                sReturn = sReturn + oItem.ExportBillID.ToString() + ",";
            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }

        public static string LCIDInString(List<ExportBillReport> oExportBillReports)
        {
            string sReturn = "";
            foreach (ExportBillReport oItem in oExportBillReports)
            {
                sReturn = sReturn + oItem.ExportLCID.ToString() + ",";
            }
            if (sReturn.Length > 0) sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            else sReturn = "0";
            return sReturn;
        }
        public static int GetIndex(List<ExportBillReport> oExportBillReports, int nExportBillID)
        {
            int index = -1, i = 0;

            foreach (ExportBillReport oItem in oExportBillReports)
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
        #endregion
        #endregion


        #region ServiceFactory
     
        internal static IExportBillReportService Service
        {
            get { return (IExportBillReportService)Services.Factory.CreateService(typeof(IExportBillReportService)); }
        }

        #endregion
    }
    #endregion

    #region IExportBillReport interface
    public interface IExportBillReportService
    {
        
        ExportBillReport Get(int id, Int64 nUserID);
        ExportBillReport GetByLDBCNo(string sLDBCNo, Int64 nUserID);
        List<ExportBillReport> Gets(string sSQL, Int64 nUserID);
      
    }
    #endregion
}