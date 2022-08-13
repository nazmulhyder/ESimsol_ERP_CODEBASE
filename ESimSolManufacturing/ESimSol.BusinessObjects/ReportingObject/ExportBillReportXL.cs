using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class ExportBillReportXL
    {
        public ExportBillReportXL()
        {
            SL = "";
            LCNo = "";
            ApplicantName = "";
            BankName_Advice = "";
            BankName_Nego = "";
            ExportBillNo = "";
            Amount = "";
            CurrentStatus = "";
            StartDate = "";
            SendToParty = "";
            RecdFromParty= "";
            SendToBankDate = "";
            RecedFromBankDate = "";
            LDBCNo = "";
            MaturityReceivedDate = "";
            MaturityDate = "";
            DiscountedDate = "";
            RelizationDate = "";
            BankFDDRecDate = "";
            EncashmentDate = "";
           
        }

        #region Properties
        public string SL { get; set; }
        public string LCNo { get; set; }
        public string ApplicantName { get; set; }
        public string BankName_Advice { get; set; }
        public string BankName_Nego { get; set; }
        public string ExportBillNo { get; set; }
        public string Amount { get; set; }
        public string CurrentStatus { get; set; }
        public string StartDate { get; set; }
        public string SendToParty { get; set; }
        public string RecdFromParty{ get; set; }
        public string SendToBankDate { get; set; }
        public string RecedFromBankDate { get; set; }
        public string LDBCDate { get; set; }
        public string LDBCNo { get; set; }
        public string MaturityReceivedDate { get; set; }
        public string MaturityDate { get; set; }
        public string DiscountedDate { get; set; }
        public string RelizationDate { get; set; }
        public string BankFDDRecDate { get; set; }
        public string EncashmentDate { get; set; }
       

        #endregion
    }
}
