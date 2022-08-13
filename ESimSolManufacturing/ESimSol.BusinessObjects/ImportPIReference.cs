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
    #region ImportPIReference
    public class ImportPIReference : BusinessObject
    {
        #region  Constructor
        public ImportPIReference()
        {
            ImportPIReferenceID = 0;
            ImportPIID = 0;
            ImportPIReferenceLogID = 0;
            ImportPILogID = 0;
            ReferenceID = 0;
            BUID = 0;
            Amount = 0;
            ReferenceNo = "";
            BillNo = "";
            RateUnit = 1;
            YetToReferenceAmount = 0;
            ReferenceAmount = 0;
            ConvertionRate = 0;
            AmountInBaseCurrency = 0;
            CurrencyID = 0;
            ErrorMessage = "";
        }
        #endregion

        #region Properties
        public int ImportPILogID { get; set; }
        public int ImportPIReferenceLogID { get; set; }
        public int ImportPIReferenceID {get;set;}
        public int ImportPIID {get;set;}
        public int ReferenceID { get; set; }
        public int BUID { get; set; }
        public string ImportPINo { get; set; }
        public string ReferenceNo { get; set; }
        public string BillNo { get; set; }
        public double InvoiceQty { get; set; }
        public double Amount { get; set; }
        public double ConvertionRate { get; set; }
        public double AmountInBaseCurrency { get; set; }
        public double YetToReferenceAmount { get; set; }
        public double ReferenceAmount { get; set; }
        public DateTime ReferenceDate { get; set; }
        public int CurrencyID { get; set; }
        public string ErrorMessage { get; set; }

       #region DerivedProperties
        public string CurrencySymbol { get; set; }
        public int RateUnit { get; set; }
        public string ReferenceAmountInString
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.ReferenceAmount,4);
            }
        }
        public string YetToReferenceAmountInString
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.YetToReferenceAmount,4);
            }
        }
       
        public string AmountSt
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(Amount);
            }

        }
        public string ReferenceDateSt
        {
            get
            {
                return this.ReferenceDate.ToString("dd MMM yyyy");
            }

        }
        #endregion

        #endregion

        #region Functions

        public ImportPIReference Get(int nImportInvoiceDetailID, int nUserID)
        {
            return ImportPIReference.Service.Get(nImportInvoiceDetailID, nUserID);
        }


        public static List<ImportPIReference> Gets(string sSQL, int nUserID)
        {
            return ImportPIReference.Service.Gets(sSQL, nUserID);
        }
        public static List<ImportPIReference> Gets(int ImportPIID, int nUserID)
        {
            return ImportPIReference.Service.Gets(ImportPIID,  nUserID);
        }
        public static List<ImportPIReference> GetsByImportPIID(int nImportPIId, int nUserID)
        {
            return ImportPIReference.Service.GetsByImportPIID(nImportPIId, nUserID);
        }

        public string Delete(long nUserID)
        {
            return ImportPIReference.Service.Delete(this, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static IImportPIReferenceService Service
        {
            get { return (IImportPIReferenceService)Services.Factory.CreateService(typeof(IImportPIReferenceService)); }
        }
        #endregion
    }
    #endregion

    #region IImportPIReference interface

    public interface IImportPIReferenceService
    {
       
        ImportPIReference Get(int nImportPIReference, Int64 nUserID);
        List<ImportPIReference> Gets(Int64 nUserID);
        List<ImportPIReference> Gets(int ImportPIID, Int64 nUserID); 
        List<ImportPIReference> Gets(string sSQL, Int64 nUserID);
        List<ImportPIReference> GetsByImportPIID(int nImportPIId, Int64 nUserID);
        string Delete(ImportPIReference oImportPI, Int64 nUserID);

    }
    #endregion
}
