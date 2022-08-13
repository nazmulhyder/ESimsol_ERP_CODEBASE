using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class EBPRLcInHandXL
    {
        public EBPRLcInHandXL()
        {
            SLNo = "";
            ApplicantName = "";
            BankName_Nego = "";
            ExportLCNo = "";
            Amount_LCSt="";
            AmountSt="";
            LCinHandSt="";
            DeliveryValueSt="";
            LCStatusSt="";
        }

        #region Properties
        public string SLNo { get; set; }
        public string ApplicantName { get; set; }
        public string BankName_Nego { get; set; }
        public string ExportLCNo { get; set; }
        public string Amount_LCSt { get; set; }
        public string AmountSt { get; set; }
        public string LCinHandSt { get; set; }
        public string DeliveryValueSt { get; set; }
        public string LCStatusSt { get; set; }


        #endregion
    }
    public class EBPRPendingPartyAcceptanceXL
    {
        public EBPRPendingPartyAcceptanceXL()
        {
            SLNo = "";
            ExportBillNo = "";
            ApplicantName = "";
            ExportLCNo = "";
            PINo = "";
            BankName_Nego = "";
            AmountSt = "";
            MKTPName = "";
            SendToPartySt = "";
            DueDays = "";
            DeliveryDateSt = "";
            DueDate = "";
        }

        #region Properties
        public string SLNo { get; set; }
        public string ExportBillNo { get; set; }
        public string ApplicantName { get; set; }
        public string ExportLCNo { get; set; }
        public string PINo { get; set; }
        public string BankName_Nego { get; set; }
        public string AmountSt { get; set; }
        public string MKTPName { get; set; }
        public string SendToPartySt { get; set; }
        public string DueDays { get; set; }
        public string DeliveryDateSt { get; set; }
        public string DueDate { get; set; }

        #endregion
    }
    public class EBPRPendingSubmitToBankXL
    {
        public EBPRPendingSubmitToBankXL()
        {
            SLNo = "";
            ExportBillNo = "";
            ApplicantName = "";
            ExportLCNo = "";
            PINo = "";
            BankName_Nego = "";
            AmountSt = "";
            BankName_Issue="";
            PrepareDate="";
            SendToPartySt = "";
            ReceiveFromParty="";
            DueDays = "";
        }

        #region Properties
        public string SLNo { get; set; }
        public string ExportBillNo { get; set; }
        public string ApplicantName { get; set; }
        public string ExportLCNo { get; set; }
        public string PINo { get; set; }
        public string BankName_Nego { get; set; }
        public string AmountSt { get; set; }
        public string BankName_Issue { get; set; }
        public string PrepareDate { get; set; }
        public string SendToPartySt { get; set; }
        public string ReceiveFromParty { get; set; }
        public string DueDays { get; set; }

        #endregion
    }
    public class EBPRPendingLDBCXL
    {
        public EBPRPendingLDBCXL()
        {
            SLNo = "";
            ExportBillNo = "";
            ApplicantName = "";
            ExportLCNo = "";
            PINo = "";
            BankName_Nego = "";
            AmountSt = "";
            BankName_Issue = "";
            PrepareDate = "";
            SendToPartySt = "";
            ReceiveFromParty = "";
            SendToBankDateSt = "";
            DueDays = "";
        }

        #region Properties
        public string SLNo { get; set; }
        public string ExportBillNo { get; set; }
        public string ApplicantName { get; set; }
        public string ExportLCNo { get; set; }
        public string PINo { get; set; }
        public string BankName_Nego { get; set; }
        public string AmountSt { get; set; }
        public string BankName_Issue { get; set; }
        public string PrepareDate { get; set; }
        public string SendToPartySt { get; set; }
        public string ReceiveFromParty { get; set; }
        public string SendToBankDateSt { get; set; }
        public string DueDays { get; set; }

        #endregion
    }
    public class EBPRPendingMaturityXL
    {
        public EBPRPendingMaturityXL()
        {
            SLNo = "";
            ExportBillNo = "";
            ApplicantName = "";
            ExportLCNo = "";
            PINo = "";
            BankName_Nego = "";
            AmountSt = "";
            BankName_Issue = "";
            LDBCNo = "";
            LDBCDateSt = "";
            MaturityRecDate="";
            MaturityDate = "";
            ORate = "";
            ODue = "";
            LastDeliveryDate="";
            DueDays = "";
        }

        #region Properties
        public string SLNo { get; set; }
        public string ExportBillNo { get; set; }
        public string ApplicantName { get; set; }
        public string ExportLCNo { get; set; }
        public string PINo { get; set; }
        public string BankName_Nego { get; set; }
        public string AmountSt { get; set; }
        public string BankName_Issue { get; set; }
        public string LDBCNo { get; set; }
        public string LDBCDateSt { get; set; }
        public string MaturityRecDate { get; set; }
        public string MaturityDate { get; set; }
        public string ORate { get; set; }
        public string ODue { get; set; }
        public string LastDeliveryDate { get; set; }
        public string DueDays { get; set; }

        #endregion
    }
    public class EBPROverdueMaturityXL
    {
        public EBPROverdueMaturityXL()
        {
            SLNo = "";
            ExportBillNo = "";
            ApplicantName = "";
            ExportLCNo = "";
            PINo = "";
            BankName_Nego = "";
            AmountSt = "";
            BankName_Issue = "";
            Status="";
            LDBCNo = "";
            LDBCDateSt = "";
            MaturityRecDate = "";
            MaturityDate = "";
            ORate = "";
            ODue = "";
            LastDeliveryDate = "";
            DueDays = "";
        }

        #region Properties
        public string SLNo { get; set; }
        public string ExportBillNo { get; set; }
        public string ApplicantName { get; set; }
        public string ExportLCNo { get; set; }
        public string PINo { get; set; }
        public string BankName_Nego { get; set; }
        public string AmountSt { get; set; }
        public string BankName_Issue { get; set; }
        public string Status { get; set; }
        public string LDBCNo { get; set; }
        public string LDBCDateSt { get; set; }
        public string MaturityRecDate { get; set; }
        public string MaturityDate { get; set; }
        public string ORate { get; set; }
        public string ODue { get; set; }
        public string LastDeliveryDate { get; set; }
        public string DueDays { get; set; }

        #endregion
    }

    public class EBPRPendingPaymentXL
    {
        public EBPRPendingPaymentXL()
        {
            SLNo = "";
            ExportBillNo = "";
            ApplicantName = "";
            ExportLCNo = "";
            PINo = "";
            BankName_Nego = "";
            AmountSt = "";
            BankName_Issue = "";
            Status = "";
            LDBCNo = "";
            LDBCDateSt = "";
            MaturityRecDate = "";
            MaturityDate = "";
            ORate = "";
            ODue = "";
            LastDeliveryDate = "";
            DueDays = "";
        }

        #region Properties
        public string SLNo { get; set; }
        public string ExportBillNo { get; set; }
        public string ApplicantName { get; set; }
        public string ExportLCNo { get; set; }
        public string PINo { get; set; }
        public string BankName_Nego { get; set; }
        public string AmountSt { get; set; }
        public string BankName_Issue { get; set; }
        public string Status { get; set; }
        public string LDBCNo { get; set; }
        public string LDBCDateSt { get; set; }
        public string MaturityRecDate { get; set; }
        public string MaturityDate { get; set; }
        public string ORate { get; set; }
        public string ODue { get; set; }
        public string LastDeliveryDate { get; set; }
        public string DueDays { get; set; }

        #endregion
    }
}
