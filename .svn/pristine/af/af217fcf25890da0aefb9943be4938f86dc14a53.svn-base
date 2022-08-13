using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ImportClaimDetail

    public class ImportClaimDetail : BusinessObject
    {
        public ImportClaimDetail()
        {
            ImportClaimDetailID =0;
            ImportClaimID=0;
            ProductID = 0;
            ProductName = "";
            MUnit = "";
            Qty=0;
            UnitPrice = 0;
            Note = "";
            ErrorMessage = "";
            CurrencySymbol = "";
        }

        #region Properties
        public int ImportClaimDetailID { get; set; }
        public int ImportClaimID { get; set; }
        public int ProductID  { get; set; }
        public string ProductName { get; set; }
        public string MUnit { get; set; }
        public string CurrencySymbol { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }

        #region Derived Property
        public string UnitPriceST
        {
            get 
            {
                return Global.MillionFormat(this.UnitPrice);
            }
        }
        #endregion
        public double Amount { get {return this.Qty*this.UnitPrice;} }
        public string AmountST { get { return Global.MillionFormat(this.Qty * this.UnitPrice); } }
        public string QtyST { get { return Global.MillionFormat(this.Qty)+" "+this.MUnit;} }

        #endregion

        #region Functions
        public static List<ImportClaimDetail> Gets(long nUserID)
        {
            return ImportClaimDetail.Service.Gets(nUserID);
        }
        public static List<ImportClaimDetail> Gets(int nImportInvoiceID, long nUserID)
        {
            return ImportClaimDetail.Service.Gets(nImportInvoiceID, nUserID);
        }
        public static List<ImportClaimDetail> Gets(string sSQL, long nUserID)
        {
            return ImportClaimDetail.Service.Gets(sSQL, nUserID);
        }
        public ImportClaimDetail Get(int id, long nUserID)
        {
            return ImportClaimDetail.Service.Get(id, nUserID);
        }

        public ImportClaimDetail Save(long nUserID)
        {
            return ImportClaimDetail.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return ImportClaimDetail.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IImportClaimDetailService Service
        {
            get { return (IImportClaimDetailService)Services.Factory.CreateService(typeof(IImportClaimDetailService)); }
        }
        #endregion
    }
    #endregion


    #region IImportClaimDetail interface

    public interface IImportClaimDetailService
    {
        ImportClaimDetail Get(int id, Int64 nUserID);
        List<ImportClaimDetail> Gets(string sSQL, long nUserID);
        List<ImportClaimDetail> Gets(int nImportInvoiceID, long nUserID);
        List<ImportClaimDetail> Gets(Int64 nUserID);
        string Delete(ImportClaimDetail oImportClaimDetail, Int64 nUserID);
        ImportClaimDetail Save(ImportClaimDetail oImportClaimDetail, Int64 nUserID);
    }
    #endregion
}
