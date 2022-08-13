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
    #region ImportPIGRNDetail
    public class ImportPIGRNDetail : BusinessObject
    {
        #region  Constructor
        public ImportPIGRNDetail()
        {
            ImportPIGRNDetailID = 0;
            ImportPIID = 0;
            ProductID = 0;
            MUnitID = 0;
            UnitPrice = 0;
            Qty = 0;
            Note = "";
            ErrorMessage = "";
            ImportPIGRNDetails = new List<ImportPIGRNDetail>();
        }
        #endregion

        #region Properties
       
        public int ImportPIGRNDetailID {get;set;}
        public int ImportPIID {get;set;}
        public int ProductID {get;set;}
        public int MUnitID { get; set; }
        public double UnitPrice { get; set; }
        public double Qty { get; set; }
        public string AcknowledgementNo { get; set; }
        public string OrderNo { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        public double InvoiceQty { get; set; }

       #region DerivedProperties
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PCNo { get; set; }
        public string MUName { get; set; }
         public string CurrencySymbol { get; set; }
        public double SmallUnitValue { get; set; }
        public string AmountSt
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(UnitPrice * Qty);
            }

        }
        public double TotalValue
        {
            get
            {
                return UnitPrice * Qty;
            }

        }
        public List<ImportPIGRNDetail> ImportPIGRNDetails { get; set; }

        #endregion

        #endregion

        #region Functions

        public ImportPIGRNDetail Get(int nImportInvoiceDetailID, int nUserID)
        {
            return ImportPIGRNDetail.Service.Get(nImportInvoiceDetailID, nUserID);
        }
        public static List<ImportPIGRNDetail> Gets(string sSQL, int nUserID)
        {
            return ImportPIGRNDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<ImportPIGRNDetail> Gets(int ImportPIGRNID, int nUserID)
        {
            return ImportPIGRNDetail.Service.Gets(ImportPIGRNID,  nUserID);
        }
        public static List<ImportPIGRNDetail> GetsByImportPIGRNID(int nImportPIGRNId, int nUserID)
        {
            return ImportPIGRNDetail.Service.GetsByImportPIGRNID(nImportPIGRNId, nUserID);
        }
        public  List<ImportPIGRNDetail> SaveImportPIGRNDetail(int nUserID)
        {
            return ImportPIGRNDetail.Service.SaveImportPIGRNDetail(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return ImportPIGRNDetail.Service.Delete(this, nUserID);
        }
        
        #endregion

        
        #region ServiceFactory
        internal static IImportPIGRNDetailService Service
        {
            get { return (IImportPIGRNDetailService)Services.Factory.CreateService(typeof(IImportPIGRNDetailService)); }
        }
        #endregion
    }
    #endregion

    

    #region IImportPIGRNDetail interface

    public interface IImportPIGRNDetailService
    {
       
        ImportPIGRNDetail Get(int nImportPIGRNDetail, Int64 nUserID);
        List<ImportPIGRNDetail> Gets(Int64 nUserID);
        List<ImportPIGRNDetail> Gets(int ImportPIGRNID, Int64 nUserID); 
        List<ImportPIGRNDetail> Gets(string sSQL, Int64 nUserID);
        List<ImportPIGRNDetail> GetsByImportPIGRNID(int nImportPIGRNId, Int64 nUserID);
        List<ImportPIGRNDetail> SaveImportPIGRNDetail(ImportPIGRNDetail oImportPIGRNDetail, Int64 nUserID);
        string Delete(ImportPIGRNDetail oImportPIGRN, Int64 nUserID);

    }
    #endregion
}
