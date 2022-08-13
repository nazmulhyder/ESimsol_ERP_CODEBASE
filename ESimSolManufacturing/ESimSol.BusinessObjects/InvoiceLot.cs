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
    #region InvoiceLot

    public class InvoiceLot : BusinessObject
    {
        #region  Constructor
        public InvoiceLot()
        {
            InvoiceLotID = 0;
            PurchaseInvoiceDetailID = 0;
            SerialNo = "";
            EngineNo = "";
            AlternatorNo = "";
            ModuleNo = "";
            Others = "";
            GRNDetailID = 0;
            InvoiceProductName = "";
            ErrorMessage = "";
        }
        #endregion

        #region Properties
        public int InvoiceLotID { get; set; }
        public int PurchaseInvoiceDetailID { get; set; }
        public string SerialNo { get; set; }
        public string EngineNo { get; set; }
        public string AlternatorNo { get; set; }
        public string ModuleNo { get; set; }
        public string Others { get; set; }
        public int GRNDetailID { get; set; }
        public string InvoiceProductName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions
        public InvoiceLot Get(int nInvoiceLot, long nUserID)
        {
            return InvoiceLot.Service.Get(nInvoiceLot, nUserID);
        }
        public static List<InvoiceLot> Saves(List<InvoiceLot> oInvoiceLots, long nUserID)
        {
            return InvoiceLot.Service.Saves(oInvoiceLots, nUserID);
        }
        public InvoiceLot Save(long nUserID)
        {
            return InvoiceLot.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return InvoiceLot.Service.Delete(this, nUserID);
        }
        public static List<InvoiceLot> Gets(int nInvoiceDetailID, long nUserID)
        {
            return InvoiceLot.Service.Gets(nInvoiceDetailID, nUserID);
        }

        public static List<InvoiceLot> Gets(string sSQL, long nUserID)
        {
            return InvoiceLot.Service.Gets(sSQL, nUserID);
        }
        public static List<InvoiceLot> GetsByInvoice(int nInvoiceId, long nUserID)
        {
            return InvoiceLot.Service.GetsByInvoice(nInvoiceId, nUserID);
        }

        #endregion

        #region ServiceFactory
        
        internal static IInvoiceLotService Service
        {
            get { return (IInvoiceLotService)Services.Factory.CreateService(typeof(IInvoiceLotService)); }
        }
        #endregion
    }
    #endregion

    #region IInvoiceLot interface

    public interface IInvoiceLotService
    {
        InvoiceLot Get(int nID, Int64 nUserId);
        List<InvoiceLot> Gets(int nInvoiceDetailID, Int64 nUserId);
        List<InvoiceLot> Gets(string sSQL, Int64 nUserID);
        List<InvoiceLot> GetsByInvoice(int nPurchaseInvoiceId, Int64 nUserID);
        string Delete(InvoiceLot oInvoiceLot, Int64 nUserId);
        List<InvoiceLot> Saves(List<InvoiceLot> oInvoiceLots, Int64 nUserID);
        InvoiceLot Save(InvoiceLot oInvoiceLot, Int64 nUserID);
    }
    #endregion
}
