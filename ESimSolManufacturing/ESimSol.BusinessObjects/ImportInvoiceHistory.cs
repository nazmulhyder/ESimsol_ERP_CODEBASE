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
    #region ImportInvoiceHistory
    public class ImportInvoiceHistory : BusinessObject
    {

        #region  Constructor
        public ImportInvoiceHistory()
        {
            ////
            ImportInvoiceHistoryID = 0;
            ImportInvoiceID = 0;
            InvoiceEvent = EnumInvoiceEvent.Initialize;
            BillEvent_Prevoius = EnumInvoiceEvent.Initialize;
            Note = "";
            DBUserID = 0;
            DBServerDateTime = DateTime.Now;
            InvoiceEventInt = 0;

        }
        #endregion

        #region Properties
        public int ImportInvoiceHistoryID { get; set; }
        public int ImportInvoiceID { get; set; }
        public EnumInvoiceEvent InvoiceEvent { get; set; }
        public EnumInvoiceEvent BillEvent_Prevoius { get; set; }
        public EnumInvoiceBankStatus InvoiceBankStatus { get; set; }
        public int InvoiceBankStatusInt { get; set; }
        public int InvoiceEventInt { get; set; }
        public string Note { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }

        #endregion
        #region Derived Properties
        public string EmployeeName { get; set; }

        #endregion

        #region Functions
        public ImportInvoiceHistory Get(int nImportInvoiceHistoryID, int nUserID)
        {
            return ImportInvoiceHistory.Service.Get(nImportInvoiceHistoryID, nUserID);
        }
        public ImportInvoiceHistory Get(int nInvoiceStatus, int nBankStatus, int nPInvID, int nUserID)
        {
            return ImportInvoiceHistory.Service.Get( nInvoiceStatus,  nBankStatus,  nPInvID, nUserID);
        }
        public ImportInvoiceHistory GetbyPurchaseInvoice(int nPurchaseInvoiceID, int eEvent, int nUserID)
        {
            return ImportInvoiceHistory.Service.GetbyPurchaseInvoice(nPurchaseInvoiceID, eEvent, nUserID);
        }
     
        public ImportInvoiceHistory Save(int nUserID)
        {
            return ImportInvoiceHistory.Service.Save(this, nUserID);
        }

     
        #region  Collection Functions
        public static List<ImportInvoiceHistory> Gets(int nImportInvoiceID, int nUserID)
        {
            return ImportInvoiceHistory.Service.Gets(nImportInvoiceID, nUserID);
        }
        public static List<ImportInvoiceHistory> GetsByInvoiceEvent(int nImportInvoiceID, string sInvoiceEvent, int nUserID)
        {
            return ImportInvoiceHistory.Service.GetsByInvoiceEvent(nImportInvoiceID, sInvoiceEvent, nUserID);
        }
        
        #endregion
        #endregion

        #region ServiceFactory

        internal static IImportInvoiceHistoryService Service
        {
            get { return (IImportInvoiceHistoryService)Services.Factory.CreateService(typeof(IImportInvoiceHistoryService)); }
        }

        #endregion
    }
    #endregion



    #region IImportInvoiceHistory interface
    public interface IImportInvoiceHistoryService
    {
        ImportInvoiceHistory Get(int id, Int64 nUserID);
        ImportInvoiceHistory Get(int nInvoiceStatus, int nBankStatus, int nPInvID, Int64 nUserID);
        ImportInvoiceHistory GetbyPurchaseInvoice(int nPurchaseInvoiceID, int eEvent, Int64 nUserID);
        List<ImportInvoiceHistory> Gets(Int64 nUserID);
        List<ImportInvoiceHistory> Gets(int nImportInvoiceID, Int64 nUserID);
        List<ImportInvoiceHistory> GetsByInvoiceEvent(int nImportInvoiceID, string sInvoiceEvent, Int64 nUserID);
        ImportInvoiceHistory Save(ImportInvoiceHistory oImportInvoiceHistory, Int64 nUserID);
      


    }
    #endregion
}
