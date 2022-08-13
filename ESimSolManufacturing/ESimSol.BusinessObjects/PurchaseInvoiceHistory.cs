using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region PurchaseInvoiceHistory
    public class PurchaseInvoiceHistory : BusinessObject
    {
        #region  Constructor
        public PurchaseInvoiceHistory()
        {            
            PurchaseInvoiceHistoryID = 0;
            PurchaseInvoiceID = 0;
            CurrentStatus = EnumPurchaseInvoiceEvent.Initialized;
            CurrentStatusInt = 0;
            PrevoiusStatus = EnumPurchaseInvoiceEvent.Initialized;
            PrevoiusStatusInt = 0;
            Note="";
            DBUserID = 0;
            DBServerDateTime=DateTime.Now;           
        }
        #endregion

        #region Properties
        public int PurchaseInvoiceHistoryID { get; set; }        
        public int PurchaseInvoiceID { get; set; }
        public EnumPurchaseInvoiceEvent CurrentStatus  { get; set; }
        public int CurrentStatusInt { get; set; }
        public EnumPurchaseInvoiceEvent PrevoiusStatus { get; set; }
        public int PrevoiusStatusInt { get; set; }        
        public string Note { get; set; }
        public int DBUserID { get; set; }        
        public DateTime DBServerDateTime { get; set; }
        #endregion
                
        #region Functions
        public PurchaseInvoiceHistory Get(int nPurchaseInvoiceHistoryID, long nUserID)
        {
            return PurchaseInvoiceHistory.Service.Get(nPurchaseInvoiceHistoryID, nUserID);
        }
        public PurchaseInvoiceHistory Get(int nInvoiceStatus, int nBankStatus, int nPInvID, long nUserID)
        {
            return PurchaseInvoiceHistory.Service.Get( nInvoiceStatus,  nBankStatus,  nPInvID, nUserID);
        }
        public PurchaseInvoiceHistory GetbyPurchaseInvoice(int nPurchaseInvoiceID, int eEvent, long nUserID)
        {
            return PurchaseInvoiceHistory.Service.GetbyPurchaseInvoice( nPurchaseInvoiceID,  eEvent, nUserID);
        }
        public PurchaseInvoiceHistory Save(long nUserID)
        {
            return PurchaseInvoiceHistory.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return PurchaseInvoiceHistory.Service.Delete(this, nUserID);
        }
        
        #region  Collection Functions

        public static List<PurchaseInvoiceHistory> Gets(long nUserID)
        {
            return PurchaseInvoiceHistory.Service.Gets(nUserID);
        }
        public static List<PurchaseInvoiceHistory> Gets(int nPurchaseInvoiceID, long nUserID)
        {
            return PurchaseInvoiceHistory.Service.Gets(nPurchaseInvoiceID, nUserID);
        }

        #region Non DB Members
        public PurchaseInvoiceHistory GetHistoryByEvent(EnumPurchaseInvoiceEvent eEvent, List<PurchaseInvoiceHistory> oPurchaseInvoiceHistorys)
        {
            foreach (PurchaseInvoiceHistory oitem in oPurchaseInvoiceHistorys)
            {
                if (oitem.CurrentStatus == eEvent) return oitem;
            }
            PurchaseInvoiceHistory oReturn = new PurchaseInvoiceHistory();
            oReturn.CurrentStatus = eEvent;
            return oReturn;
        }

        #endregion
        #endregion
        #endregion

        #region ServiceFactory

      
        internal static IPurchaseInvoiceHistoryService Service
        {
            get { return (IPurchaseInvoiceHistoryService)Services.Factory.CreateService(typeof(IPurchaseInvoiceHistoryService)); }
        }
       
        #endregion
    }
    #endregion

    #region IPurchaseInvoiceHistory interface

    public interface IPurchaseInvoiceHistoryService
    {
        PurchaseInvoiceHistory Get(int id, Int64 nUserID);
        PurchaseInvoiceHistory Get(int nInvoiceStatus, int nBankStatus, int nPInvID, Int64 nUserID);
        PurchaseInvoiceHistory GetbyPurchaseInvoice(int nPurchaseInvoiceID, int eEvent, Int64 nUserID);
        List<PurchaseInvoiceHistory> Gets(Int64 nUserID);
        List<PurchaseInvoiceHistory> Gets(int nPurchaseInvoiceID, Int64 nUserID);
        PurchaseInvoiceHistory Save(PurchaseInvoiceHistory oPurchaseInvoiceHistory, Int64 nUserID);      
        string Delete(PurchaseInvoiceHistory oPurchaseInvoiceHistory, Int64 nUserID);
        
       
    }
    #endregion
}
