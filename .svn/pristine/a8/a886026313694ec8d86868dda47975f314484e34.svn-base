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
    public class PurchaseInvoicePackingListDetail : BusinessObject
    {
        #region  Constructor
        public PurchaseInvoicePackingListDetail()
        {
            PurchaseInvoicePackingListDetailID = 0;
            WeightPerBag = 0;
            NoOfBag = 0;

        }
        #endregion

        #region Properties

        public int PurchaseInvoicePackingListDetailID { get; set; }

        public int PurchaseInvoicePackingListID { get; set; }
        public double NoOfBag { get; set; }
        public double WeightPerBag { get; set; }
        public string BagDes { get; set; }
        public string ErrorMessage { get; set; }
        public double Qty
        {
            get
            {
                return (this.NoOfBag * this.WeightPerBag);
            }
        }
        #endregion

        #region Functions
        public PurchaseInvoicePackingListDetail Get(int id, int nUserID)
        {
            return PurchaseInvoicePackingListDetail.Service.Get(id, nUserID);
        }
        public static List<PurchaseInvoicePackingListDetail> Gets( int nUserID)
        {
            return PurchaseInvoicePackingListDetail.Service.Gets( nUserID);
        }
    
        public static List<PurchaseInvoicePackingListDetail> Gets(int nPPListID, int nUserID)
        {
            return PurchaseInvoicePackingListDetail.Service.Gets(nPPListID, nUserID);
        }

        public PurchaseInvoicePackingListDetail Save(int nUserID)
        {
            return PurchaseInvoicePackingListDetail.Service.Save(this, nUserID);
        }
        public string Delete(int nUserID)
        {
            return PurchaseInvoicePackingListDetail.Service.Delete(this, nUserID);
        }
      
        #endregion

       
        #region ServiceFactory
        internal static IPurchaseInvoicePackingListDetailService Service
        {
            get { return (IPurchaseInvoicePackingListDetailService)Services.Factory.CreateService(typeof(IPurchaseInvoicePackingListDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IPurchaseInvoicePackingListDetail interface
    public interface IPurchaseInvoicePackingListDetailService
    {
        PurchaseInvoicePackingListDetail Get(int id, Int64 nUserID);
        List<PurchaseInvoicePackingListDetail> Gets(Int64 nUserID);
        List<PurchaseInvoicePackingListDetail> Gets(int nPPListID, Int64 nUserID);
        string Delete(PurchaseInvoicePackingListDetail oPurchaseInvoicePackingListDetail, Int64 nUserID);
        PurchaseInvoicePackingListDetail Save(PurchaseInvoicePackingListDetail oPurchaseInvoicePackingListDetail, Int64 nUserID);
    }
    #endregion
}
