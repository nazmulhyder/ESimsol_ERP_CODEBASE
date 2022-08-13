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
    #region ImportPackDetail
    public class ImportPackDetail : BusinessObject
    {
        #region  Constructor
        public ImportPackDetail()
        {
            ImportPackDetailID = 0;
            ImportPackID = 0;
            ProductID = 0;
            NumberOfPack = 0;
            QtyPerPack = 0;
            MUnitID = 0;
            Qty = 0;
            ProductCode ="";
            ProductName = "";
            MUName = "";
            ImportInvoiceDetailID = 0;
            IsSerialNoApply = true;
            UnitPriceBC = 0;
            LCLandingCost = 0;
            InvoiceLandingCost = 0;
            ImportInvoiceID = 0;
            ProductID = 0;
            Origin = "";
            Brand = "";
            TechnicalSheetID = 0;
            StyleNo = "";
            UnitConValue = 0;
            MUNameTwo = "";
            MURateTwo = 0;
            MUnitIDTwo = 0;
            Shade = "";
        }
        #endregion

        #region Properties
        public int ImportPackDetailID { get; set; }
        public int ImportPackID { get; set; }
        public int ImportInvoiceDetailID { get; set; }
        public int ImportInvoiceID { get; set; }
        public int ProductID { get; set; }
        public bool IsSerialNoApply { get; set; }
        public double QtyPerPack { get; set; }
        public double NumberOfPack { get; set; }
        public string ProductSpec { get; set; }
        public double UnitPriceBC { get; set; }
        public double LCLandingCost { get; set; }
        public double InvoiceLandingCost { get; set; }
        public double YetToGRNQty { get; set; }
        public int MUnitID { get; set; }
        public int MUnitIDTwo { get; set; }     
        public double Qty { get; set; }
        public double MURate { get; set; }
        public double MURateTwo { get; set; }
        public string Shade { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
        public string MUNameTwo { get; set; }
        public double UnitConValue { get; set; }
        public string LotNo { get; set; }
        public double InvoiceQty { get; set; }
        public string Remarks { get; set; }
        public string Origin { get; set; }
        public string Brand { get; set; }
        public int TechnicalSheetID { get; set; }
        public string StyleNo { get; set; }
        public string ErrorMessage { get; set; }                      
        #region DerivedProperties
        public double QtyPerPackTwo
        { 
            get
            {
                if (this.MURate > 0)
                {
                    return this.QtyPerPack / this.MURate;
                }else
                {
                    return 0;
                }
                
            }
         }
        public double QtyTwo
        {
            get
            {
                if (this.Qty > 0 && this.MURate>0)
                {
                    return this.Qty/this.MURate;
                }
                else
                {
                    return 0;
                }

            }
        }
      public string InvoiceQtySt
        {
            get
            {
                return this.InvoiceQty + "" + this.MUName;
            }
        }
        #endregion

        #endregion

        #region Functions
        public ImportPackDetail Get(int nImportPackDetailID, long nUserID)
        {
            return ImportPackDetail.Service.Get(nImportPackDetailID, nUserID);
        }
     
        public string Delete(long nUserID)
        {
            return ImportPackDetail.Service.Delete(this, nUserID);
        }

     
        public static List<ImportPackDetail> Gets(int nImportPackID, long nUserID)
        {
            return ImportPackDetail.Service.Gets(nImportPackID, nUserID);
        }

        public static List<ImportPackDetail> GetsByInvioce(int nImportInvoiceID, long nUserID)
        {
            return ImportPackDetail.Service.GetsByInvioce(nImportInvoiceID, nUserID);
        }
        public static List<ImportPackDetail> Gets(string sSQL, long nUserID)
        {
            return ImportPackDetail.Service.Gets(sSQL, nUserID);
        }
       
        #endregion

        #region ServiceFactory

     
        internal static IImportPackDetailService Service
        {
            get { return (IImportPackDetailService)Services.Factory.CreateService(typeof(IImportPackDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IImportPackDetail interface
    public interface IImportPackDetailService
    {
        ImportPackDetail Get(int nID, Int64 nUserId);
        List<ImportPackDetail> Gets(int nImportPackID, Int64 nUserId);
        List<ImportPackDetail> GetsByInvioce(int nImportInvoiceID, Int64 nUserId);
        List<ImportPackDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(ImportPackDetail oImportPackDetail, Int64 nUserId);
        
    }
    #endregion
}
