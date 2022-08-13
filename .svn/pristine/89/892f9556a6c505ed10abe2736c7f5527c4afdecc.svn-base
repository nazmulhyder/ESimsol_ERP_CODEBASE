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
    #region ImportInvChallanDetail
    public class ImportInvChallanDetail : BusinessObject
    {
        #region  Constructor
        public ImportInvChallanDetail()
        {
            ImportInvChallanDetailID = 0;
            ImportInvChallanID = 0;
            ProductID = 0;
            NumberOfPack = 0;
            QtyPerPack = 0;
            MUnitID = 0;
            Qty = 0;
            ProductCode ="";
            ProductName = "";
            ImportInvoiceNo = "";
            MUName = "";
            MUSymbol = "";
            ImportInvoiceDetailID = 0;
            ImportInvoiceID = 0;
            ProductID = 0;
            Qty_GRN = 0;
            Qty_IPL = 0;
            NumberOfPack_CD = 0;
        }
        #endregion

        #region Properties
        public int ImportInvChallanDetailID { get; set; }
        public int ImportInvChallanID { get; set; }
        public int ImportInvoiceDetailID { get; set; }
        public int ImportPackDetailID { get; set; }
        public int ImportInvoiceID { get; set; }
        public int ProductID { get; set; }
        public int GRNDetailID { get; set; }
        public double Qty_IPL { get; set; }
        public double QtyPerPack { get; set; }
        public double NumberOfPack { get; set; }
        public double NumberOfPack_CD { get; set; }
        public string ProductSpec { get; set; }
        public double Qty_GRN { get; set; }
        public int MUnitID { get; set; }     
        public double Qty { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public string LotNo { get; set; }
        public double InvoiceQty { get; set; }
        public string Note { get; set; }
        public string ImportInvoiceNo { get; set; }
        public string ErrorMessage { get; set; }                      
        #region DerivedProperties
        public double QtyPerBalesInKg { 
            get
            {
                if (this.QtyPerPack > 0)
                {
                    return this.QtyPerPack / 2.2046;
                }else
                {
                    return 0;
                }
                
            }
         }
      public string InvoiceQtySt
        {
            get
            {
                return this.InvoiceQty + "" + this.MUSymbol;
            }
        }
        #endregion

        #endregion

        #region Functions
        public ImportInvChallanDetail Get(int nImportInvChallanDetailID, long nUserID)
        {
            return ImportInvChallanDetail.Service.Get(nImportInvChallanDetailID, nUserID);
        }
     
        public string Delete(long nUserID)
        {
            return ImportInvChallanDetail.Service.Delete(this, nUserID);
        }

     
        public static List<ImportInvChallanDetail> Gets(int nImportInvChallanID, long nUserID)
        {
            return ImportInvChallanDetail.Service.Gets(nImportInvChallanID, nUserID);
        }

        public static List<ImportInvChallanDetail> Gets(string sSQL, long nUserID)
        {
            return ImportInvChallanDetail.Service.Gets(sSQL, nUserID);
        }
     
       
        #endregion

        #region ServiceFactory

     
        internal static IImportInvChallanDetailService Service
        {
            get { return (IImportInvChallanDetailService)Services.Factory.CreateService(typeof(IImportInvChallanDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IImportInvChallanDetail interface
    public interface IImportInvChallanDetailService
    {
        ImportInvChallanDetail Get(int nID, Int64 nUserId);
        List<ImportInvChallanDetail> Gets(int nImportInvChallanID, Int64 nUserId);
        List<ImportInvChallanDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(ImportInvChallanDetail oImportInvChallanDetail, Int64 nUserId);
        
    }
    #endregion
}
