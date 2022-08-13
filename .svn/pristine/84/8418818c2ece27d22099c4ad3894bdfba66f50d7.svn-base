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
    #region Clause
    [DataContract]
    public class PurchaseInvoicePackingList : BusinessObject
    {
        #region  Constructor
        public PurchaseInvoicePackingList()
        {
            PurchaseInvoicePackingListID = 0;
            PurchaseInvoiceLCDetailID = 0;
            MUnitID = 0;
            PIPackingListDetails = new List<PurchaseInvoicePackingListDetail>();

        }
        #endregion

        #region Properties
        
        public int PurchaseInvoicePackingListID { get; set; }
        public int PurchaseInvoiceLCDetailID { get; set; }
        public int PurchaseInvoiceDetailID { get; set; }
        public int MUnitID { get; set; }
        public string LotNo { get; set; }
        public string ProductName { get; set; }
        public double Qty { get; set; }
        
        
        public string ErrorMessage { get; set; }
        public List<PurchaseInvoicePackingListDetail> PIPackingListDetails { get; set; }
        #endregion

        #region Functions
        public PurchaseInvoicePackingList Get(int id, int nUserID)
        {
            return PurchaseInvoicePackingList.Service.Get(id, nUserID);
        }

        public static List<PurchaseInvoicePackingList> Gets(int nPurchaseInvoiceLCDetailID, int nUserID)
        {
            return PurchaseInvoicePackingList.Service.Gets(nPurchaseInvoiceLCDetailID, nUserID);
        }
        public static List<PurchaseInvoicePackingList> GetsBy(int nPurchaseInvoiceLCID, int nUserID)
        {
            return PurchaseInvoicePackingList.Service.GetsBy(nPurchaseInvoiceLCID, nUserID);
        }

        public PurchaseInvoicePackingList Save(int nUserID)
        {
            return PurchaseInvoicePackingList.Service.Save(this, nUserID);
        }
        public string Delete(int nUserID)
        {
            return PurchaseInvoicePackingList.Service.Delete(this, nUserID);
        }
      
        #endregion
       
        #region ServiceFactory
        internal static IPurchaseInvoicePackingListService Service
        {
            get { return (IPurchaseInvoicePackingListService)Services.Factory.CreateService(typeof(IPurchaseInvoicePackingListService)); }
        }
        #endregion
    }
    #endregion


    #region IPurchaseInvoicePackingList interface
    public interface IPurchaseInvoicePackingListService
    {
        PurchaseInvoicePackingList Get(int id, Int64 nUserID);
        List<PurchaseInvoicePackingList> Gets(int nPurchaseInvoiceLCDetailID, Int64 nUserID);
        List<PurchaseInvoicePackingList> GetsBy(int nPurchaseInvoiceLCID, Int64 nUserID);
        string Delete(PurchaseInvoicePackingList oPurchaseInvoicePackingList, Int64 nUserID);
        PurchaseInvoicePackingList Save(PurchaseInvoicePackingList oPurchaseInvoicePackingList, Int64 nUserID);
    }
    #endregion

}
