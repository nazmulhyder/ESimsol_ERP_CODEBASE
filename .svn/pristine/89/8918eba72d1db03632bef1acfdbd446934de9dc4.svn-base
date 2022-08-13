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
    #region ImportPIDetail
    public class ImportPIDetail : BusinessObject
    {
        #region  Constructor
        public ImportPIDetail()
        {
            ImportPIDetailID = 0;
            ImportPIID = 0;
            ImportPIDetailLogID = 0;
            ImportPILogID = 0;
            ProductID = 0;
            MUnitID = 0;
            UnitPrice = 0;
            FreightRate = 0;
            Qty = 0;
            Amount = 0;
            Note = "";
            RateUnit = 1;
            ProductUnitType = EnumUniteType.None;
            ProductUnitTypeInInt = 0;
            YetToGRNQty = 0;
            CRate = 0;
            TechnicalSheetID = 0;
            StyleNo = "";
            BuyerName = "";
            Shade = "";
            ErrorMessage = "";
        }
        #endregion

        #region Properties
        public int ImportPILogID { get; set; }
        public int ImportPIDetailLogID { get; set; }
        public int ImportPIDetailID {get;set;}
        public int ImportPIID {get;set;}
        public int ProductID {get;set;}
        public int MUnitID { get; set; }
        public double UnitPrice { get; set; }
        public double FreightRate { get; set; }
        public double Qty { get; set; }
        public string Shade { get; set; }
        public string AcknowledgementNo { get; set; }
        public string OrderNo { get; set; }
        public string Note { get; set; }
        public double InvoiceQty { get; set; }
        public double Amount { get; set; }
        public int TechnicalSheetID { get; set; }
        public string StyleNo { get; set; }
        public EnumUniteType ProductUnitType { get; set; }
        public int ProductUnitTypeInInt { get; set; }
        public double YetToGRNQty { get; set; }
        public double CRate { get; set; }
        public string BuyerName { get; set; }
        public string ErrorMessage { get; set; }

       #region DerivedProperties
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PCNo { get; set; }
        public string MUName { get; set; }
         public string CurrencySymbol { get; set; }
        public double SmallUnitValue { get; set; }
        public int RateUnit { get; set; }
        public string AmountSt
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(Amount);
            }

        }
       

        #endregion

        #endregion

        #region Functions

        public ImportPIDetail Get(int nImportInvoiceDetailID, int nUserID)
        {
            return ImportPIDetail.Service.Get(nImportInvoiceDetailID, nUserID);
        }


        public static List<ImportPIDetail> Gets(string sSQL, int nUserID)
        {
            return ImportPIDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<ImportPIDetail> Gets(int ImportPIID, int nUserID)
        {
            return ImportPIDetail.Service.Gets(ImportPIID,  nUserID);
        }
        public static List<ImportPIDetail> GetsByImportPIID(int nImportPIId, int nUserID)
        {
            return ImportPIDetail.Service.GetsByImportPIID(nImportPIId, nUserID);
        }

        public string Delete(long nUserID)
        {
            return ImportPIDetail.Service.Delete(this, nUserID);
        }
        
        #endregion

        
        #region ServiceFactory
        internal static IImportPIDetailService Service
        {
            get { return (IImportPIDetailService)Services.Factory.CreateService(typeof(IImportPIDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IImportPIDetail interface

    public interface IImportPIDetailService
    {
       
        ImportPIDetail Get(int nImportPIDetail, Int64 nUserID);
        List<ImportPIDetail> Gets(Int64 nUserID);
        List<ImportPIDetail> Gets(int ImportPIID, Int64 nUserID); 
        List<ImportPIDetail> Gets(string sSQL, Int64 nUserID);
        List<ImportPIDetail> GetsByImportPIID(int nImportPIId, Int64 nUserID);
        string Delete(ImportPIDetail oImportPI, Int64 nUserID);

    }
    #endregion
}
