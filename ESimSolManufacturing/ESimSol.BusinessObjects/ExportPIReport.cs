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
    #region PIReport
    
    public class ExportPIReport : BusinessObject
    {
        public ExportPIReport()
        {
            ExportPIID = 0;
            PINo = "";
            //PIYear = DateTime.Today.ToString("yy");
            //PICode = "";
            IssueDate = DateTime.Now;
            ContractorID = 0;
            ContractorName = "";
            BuyerID = 0;
            BuyerName = "";
            MKTPName = "";
            PIStatus = EnumPIStatus.Initialized;
            LCNo = "";
            Currency = "";
            ProductName = "";
            BankName = "";
            Qty = 0;
            UnitPrice = 0;
            AdjQty = 0;
            AdjRate = 0;
            CRate = 0;
            CRateTwo = 0;
            AdjValue = 0;
            DocCharge = 0;
            MUName = "";
            RateUnit = 1;
            ErrorMessage = "";
            QtyCom = 0;
            ProductNature = EnumProductNature.Dyeing;
            FileNo = "";
        }

        #region Properties
        public int ExportPIID { get; set; }
        public string PINo { get; set; }
        public int ContractorID { get; set; }
        public DateTime IssueDate { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public double AdjQty { get; set; }
        public double AdjRate { get; set; }
        public double AdjValue { get; set; }
        public double DocCharge { get; set; }
        public double CRate { get; set; }
        public double QtyCom { get; set; }
        public string ContractorName { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public string MKTPName { get; set; }
        public EnumPIStatus PIStatus { get; set; }
        public string LCNo { get; set; }
        public string Currency { get; set; }
        public string ProductName { get; set; }
        public string BankName { get; set; }
     
        public string MUName { get; set; }
        public int BUID { get; set; }
        public EnumProductNature ProductNature { get; set; }
        public int CurrentStatus_LC { get; set; }
        public int AmendmentNo { get; set; }
        public bool AmendmentRequired { get; set; }
        public DateTime AmendmentDate { get; set; }
        public string CPersonName { get; set; }
        public int RateUnit { get; set; }
        public double Amount_Accep { get; set; }
        public double Amount_Maturity { get; set; }
        public string FabricNo { get; set; }
        public string FileNo { get; set; }
        public string Construction { get; set; }
        public double CRateTwo { get; set; }
        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public bool IsApplySizer { get; set; }
        public bool IsDeduct { get; set; }
        public string ReferenceCaption { get; set; }
        public string ProductDescription { get; set; }
        public string SizeName { get; set; }
        public int ModelReferenceID { get; set; }
        public string ModelReferenceName { get; set; }
        #endregion

        #region Derive Property

        public string ErrorMessage { get; set; }
        public double nAmount
        {
            get { return this.UnitPrice*this.Qty; }
        }
        public double Amount_Com
        {
            get { return this.CRate * this.QtyCom; }
        }
        public string AmountST
        {
            get
            {
                if (this.RateUnit <= 1)
                {
                    return this.Currency + Global.MillionFormat(this.UnitPrice * this.Qty);
                }
                else
                {
                    return this.Currency + Global.MillionFormat((this.Qty/this.RateUnit)*this.UnitPrice);
                }
            }
        }
        public string QtySt
        {
            get { return Global.MillionFormat(this.Qty); }
        }
        public string UPriceSt
        {
            get { return this.Currency + Global.MillionFormat(this.UnitPrice); }
        }

        public string UnitPriceSt
        {
            get
            {
                if (this.RateUnit <= 1)
                {
                    return  this.Currency + Global.MillionFormat(this.UnitPrice);
                }
                else
                {
                    return this.Currency + Global.MillionFormat(this.UnitPrice) + "/" + this.RateUnit.ToString();
                }
            }
        }
        public string IssueDateSt
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
      
   

        public string PIStatusSt
        {
            get
            {
                return this.PIStatus.ToString();
            }
        }
        public string LCStatusSt
        {
            get
            {
                if (this.AmendmentRequired)
                {
                    return "AmendmentRequired";
                }
                else
                {
                    return ((EnumExportLCStatus)this.CurrentStatus_LC).ToString();
                }
            }
        }

        #endregion


    #endregion


        #region Functions

 
        public static List<ExportPIReport> Gets(string sSQL, Int64 nUserID)
        {
            return ExportPIReport.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IExportPIReportService Service
        {
            get { return (IExportPIReportService)Services.Factory.CreateService(typeof(IExportPIReportService)); }
        }
        #endregion

        
    }

    #region IPIReport interface
    [ServiceContract]
    public interface IExportPIReportService
    {
     
        [OperationContract]
        List<ExportPIReport> Gets(string sSQL, Int64 nUserID);
        
    }
    #endregion
}
