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
    #region ImportPIReport
    public class ImportPIReport : BusinessObject
    {
        #region  Constructor
        public ImportPIReport()
        {
             ImportPIID = 0;
			 ImportPINo="";
             ContractorID = 0;
			 ContractorName="";
			 ImportPIType = EnumImportPIType.None;
			 CurrencySymbol="";
             MUnit = "";
             ImportPIDate = DateTime.Now;
             ImportPIDetailID = 0;
             ProductID=0;
             ProductName = "";
             ProductCode = ""; 
             UnitPrice=0;
             Quantity=0;
             ErrorMessage = "";
             Company = new Company();
             PCStatus = EnumImportPIState.Initialized;

 
        }
        #endregion
        #region Properties
        public int ImportPIID { get; set; }
        public string ImportPINo { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public EnumImportPIType ImportPIType { get; set; }
        public int ImportPIReportTypeInInt { get; set; }
        public string CurrencySymbol { get; set; }
        public DateTime ImportPIDate { get; set; }
        public int ImportPIDetailID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
        public string MUnit { get; set; }
        public string ErrorMessage { get; set; }
        public EnumImportPIState PCStatus { get; set; }

        public string ImportPITypeInString
        {
            get
            {
                return ImportPIType.ToString();
            }
        }

        public string ImportPIDateInString
        {
            get
            {
                return ImportPIDate.ToString("dd MMM yyyy");
            }
        }

        public string AmountSt
        {
            get
            {
                return this.CurrencySymbol+ (this.Quantity*this.UnitPrice).ToString();
            }
        }

        public double Amount
        {
            get
            {
                return this.Quantity * this.UnitPrice;
            }
        }

        public string UPriceSt
        {
            get
            {
                return this.CurrencySymbol + this.UnitPrice.ToString();
            }
        }

        public string QtySt
        {
            get
            {
                return this.Quantity.ToString() + this.MUnit;
            }
        }

        public string PCStatusInString
        {
            get
            {
                return PCStatus.ToString();
            }
        }

        public Company Company { get; set; }

        public List<ImportPIReport> ImportPIReports { get; set; }

        #endregion

        #region Functions

        public static List<ImportPIReport> Gets(string sSQL, int nUserID)
        {
            return ImportPIReport.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IImportPIReportService Service
        {
            get { return (IImportPIReportService)Services.Factory.CreateService(typeof(IImportPIReportService)); }
        }
        #endregion
    }
    #endregion


    #region IImportPIReport interface
    public interface IImportPIReportService
    {
  
        List<ImportPIReport> Gets(string sSQL, Int64 nUserID);
      
    }
    #endregion
}