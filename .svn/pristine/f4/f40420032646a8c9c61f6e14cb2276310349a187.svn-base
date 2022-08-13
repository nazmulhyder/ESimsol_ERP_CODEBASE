using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ExportPILCMapping
    public class ExportPILCMapping : BusinessObject
    {
        public ExportPILCMapping()
        {
            ExportPILCMappingID = 0;
            ExportPILCMappingLogID = 0;
            ExportPIID = 0;
            Amount = 0;
            ExportLCID = 0;
            Activity = false;
            Date = DateTime.Now;
            IssueDate = DateTime.Now;
            UDRecDate = DateTime.MinValue;
            ReviseNo = 0;
            SLNo = 0;
            Qty = 0;
            PINo = "";
            ContractorName = "";
            Flag = false;
            VersionNo = 0;
            UDRcvType = 0;
            MKTPName="";
            CPerson = "";
            CPPhone = "";
            LCReceiveDate = DateTime.Now;
            PIValue = 0;
            ReportDate = DateTime.Now;
            ShipmentDate= DateTime.Now;
            ExpiryDate = DateTime.Now;
        }

        #region Properties
        public int ExportPILCMappingID { get; set; }
        public int ExportPILCMappingLogID { get; set; }        
        public int ExportPIID { get; set; }        
        public double Amount { get; set; }        
        public int ExportLCID { get; set; }        
        public int ContractorID { get; set; }
        public int LCTermID { get; set; }
        public int BankBranchID { get; set; }  
        public bool Activity { get; set; }
        public DateTime Date { get; set; }        //// PI Attach Date as well as LC Amendment Date by User
        public DateTime LCReceiveDate { get; set; }  //// PI Issue Date
        public DateTime IssueDate { get; set; }  //// PI Issue Date
        public DateTime UDRecDate { get; set; }  //// PI Issue Date
        
        public ExportLC ExportLC { get; set; }        
        public int ReviseNo { get; set; }
        public int VersionNo { get; set; }        
        public int SLNo { get; set; }        
        public double Qty { get; set; }
        public double PIValue { get; set; }

        public DateTime ReportDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public string ReportDateST
        {
            get
            {
                return this.ReportDate.ToString("dd MMM yyyy");
            }
        }
        public string ShipmentDateST
        {
            get
            {
                return this.ShipmentDate.ToString("dd MMM yyyy");
            }
        }
        public string ExpiryDateST
        {
            get
            {
                return this.ExpiryDate.ToString("dd MMM yyyy");
            }
        }
        public double TotalQtyKG
        {
            get { return Global.GetKG(this.Qty, 2); }
        }
        private string _sActivitySt;
        public string ActivitySt
        {
            get
            {
                if (this.Activity)
                {
                    _sActivitySt = "Active";
                }
                else
                {
                    _sActivitySt = "In-Active";
                }
                return _sActivitySt;
            }
        }
        #region PINo_Full
        private string _sPINo_Full = "";
        public string PINo_Full
        {
            get
            {
                if (this.ReviseNo > 0)
                {

                    _sPINo_Full = this.PINo;
                }
                else
                {
                    _sPINo_Full = this.PINo;
                }
                return _sPINo_Full;
            }
        }
        #endregion
        public string PINo { get; set; }        
        public string Currency { get; set; }         
        public string MUName { get; set; }        
        public string LCTermsName { get; set; }
        public string BankName { get; set; }
        public string BuyerName { get; set; }
        public string MKTPName { get; set; }
        public string CPerson { get; set; }
        public string CPPhone { get; set; } 
        
        public string ErrorMessage { get; set; }        
        public bool Flag { get; set; }
        public int UDRcvType { get; set; }    /// 0=No Receive,1=Partily Receive,2=Full Receove
        public EnumPIStatus PIStatus { get; set; }
        #region AmountSt
        public string AmountSt
        {
            get
            {

                return this.Currency + "" + Global.MillionFormat(this.Amount);
            }
        }
        #endregion
        #region PIValueSt
        public string PIValueSt
        {
            get
            {

                return this.Currency + "" + Global.MillionFormat(this.PIValue);
            }
        }
        #endregion
        #region IssueDateST
        public string IssueDateST
        {
            get
            {

                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        #endregion
        #region LCReceiveDateST
        public string LCReceiveDateST
        {
            get
            {
                return this.LCReceiveDate.ToString("dd MMM yyyy");
            }
        }
        #endregion
        #region DateST  // PI Attach Date as well as LC Amendment Date
        public string DateST
        {
            get
            {

                return this.Date.ToString("dd MMM yyyy");
            }
        }
        #endregion
        #region UDRecDateST
        public string UDRecDateSt
        {
            get
            {
                if (this.UDRecDate != DateTime.MinValue)
                {
                    return this.UDRecDate.ToString("dd MMM yyyy");
                }
                else
                { return ""; }
            }
        }
        #endregion
        private string SPIStatusST = "";
        public string PIStatusST
        {
            get
            {
                if (this.PIStatus == EnumPIStatus.Initialized)
                {
                    SPIStatusST = "Waiting for req PI";
                }
                else
                {
                    SPIStatusST = this.PIStatus.ToString();
                }
                return SPIStatusST;
            }
        }
        #endregion

        #region Derived Propertiese
        public string ContractorName { get; set; }
        public string AmendmentSt 
        {
            get 
            {
                return (this.Flag == true ? "Revised" : "");
            }
        }
        public string IssueDateInString
        {
            get
            {
                if (IssueDate == DateTime.MinValue) return "";
                return IssueDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public ExportPILCMapping Save(Int64 nUserID)
        {
            return ExportPILCMapping.Service.Save(this, nUserID);            
        }
    
        public string Delete(Int64 nUserID)
        {
            return ExportPILCMapping.Service.Delete(this, nUserID);            
        }        
        public static List<ExportPILCMapping> GetsByLCID(int nExportLCID, Int64 nUserID)
        {
            return ExportPILCMapping.Service.GetsByLCID(nExportLCID, nUserID);            
        }
        public static List<ExportPILCMapping> GetsLogByLCID(int nExportLCLogID, Int64 nUserID)
        {
            return ExportPILCMapping.Service.GetsLogByLCID(nExportLCLogID, nUserID);            
        }
        public static List<ExportPILCMapping> GetsByEBillID(int nExportBillID, Int64 nUserID)
        {
            return ExportPILCMapping.Service.GetsByEBillID(nExportBillID, nUserID);
        }
        public static List<ExportPILCMapping> Gets(int nExportLCID, int nVersionNo, Int64 nUserID)
        {
            return ExportPILCMapping.Service.Gets(nExportLCID,  nVersionNo, nUserID);            
        }
        public static List<ExportPILCMapping> Gets(string sSQL, Int64 nUserID)
        {
            return ExportPILCMapping.Service.Gets(sSQL, nUserID);
        }
        public ExportPILCMapping UpdateExportPILCMapping(ExportPILCMapping oExportPILCMapping, long nUserID)
        {
            return ExportPILCMapping.Service.UpdateExportPILCMapping(oExportPILCMapping, nUserID);
        }        
        #endregion

        #region ServiceFactory
        internal static IExportPILCMappingService Service
        {
            get { return (IExportPILCMappingService)Services.Factory.CreateService(typeof(IExportPILCMappingService)); }
        }
        #endregion
    }
    #endregion

    #region IExportPILCMapping interface
    public interface IExportPILCMappingService
    {        
        List<ExportPILCMapping> GetsByLCID(int nExportLCID, Int64 nUserID);        
        List<ExportPILCMapping> GetsLogByLCID(int nExportLCLogID, Int64 nUserID);
        List<ExportPILCMapping> Gets(int nExportLCID, int nVersionNo, Int64 nUserID);
        List<ExportPILCMapping> GetsByEBillID(int nExportBillID,  Int64 nUserID);
        List<ExportPILCMapping> Gets(string sSQL, Int64 nUserID);
        string Delete(ExportPILCMapping oExportPILCMapping, Int64 nUserID);        
        ExportPILCMapping Save(ExportPILCMapping oExportPILCMapping, Int64 nUserID);
        ExportPILCMapping UpdateExportPILCMapping(ExportPILCMapping oExportPILCMapping, long nUserID);
       
    }
    #endregion
}
