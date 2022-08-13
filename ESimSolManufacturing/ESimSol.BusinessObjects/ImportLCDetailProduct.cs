using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region PurchasePaymentContractDetail
    public class ImportLCDetailProduct : BusinessObject
    {
        #region  Constructor
        public ImportLCDetailProduct()
        {
            ImportLCDetailProductID = 0;
            ImportLCDetailID = 0;
            ProductID = 0;
            Quantity = 0;
            MeasurementUnitID = 0;
            GrossOrNetWeight = EnumGrossOrNetWeight.None;
            UnitPrice = 0;
            PackingQty = 0;
            Note = "";
        }
        #endregion

        #region Properties
        public int ImportLCDetailProductID { get; set; }
        public int ImportLCDetailID { get; set; }
        public int ProductID { get; set; }
        public double Quantity { get; set; }
        public int MeasurementUnitID { get; set; }
        public EnumGrossOrNetWeight GrossOrNetWeight { get; set; }
        public double UnitPrice { get; set; }
        public double PackingQty { get; set; }
        public double InvoiceQty { get; set; }
        public string Note { get; set; }
        public int PCDetailID { get; set; }

        #region Derive poperty
        public double TotalValue { get { return UnitPrice * Quantity; } }

        #region Purchase Invoice Section

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUnitName { get; set; }
        public double RemainingQuantity { get; set; }
        public double ReceiveQty { get; set; }
        public double Value { get { return UnitPrice * RemainingQuantity; } }

        #endregion

        #endregion

        #endregion

        #region Functions
        public ImportLCDetailProduct Get(int nPPCDetailID, int nUserID)
        {
            return ImportLCDetailProduct.Service.Get(nPPCDetailID, nUserID);
        }
        public static List<ImportLCDetailProduct> Gets(int nPPCID, int nUserID)
        {
            return ImportLCDetailProduct.Service.Gets(nPPCID, nUserID);
        }
        public static List<ImportLCDetailProduct> GetsByLCID(int nPPCID, int nUserID)
        {
            return ImportLCDetailProduct.Service.GetsByLCID(nPPCID, nUserID);
        }
        public static List<ImportLCDetailProduct> GetsBySQL(string sSQL, int nUserID)
        {
            return ImportLCDetailProduct.Service.GetsBySQL(sSQL, nUserID);
        }



        #endregion


        #region ServiceFactory

        internal static IPurchasePaymentContractDetailService Service
        {
            get { return (IPurchasePaymentContractDetailService)Services.Factory.CreateService(typeof(IPurchasePaymentContractDetailService)); }
        }

    }
    #endregion


    #region IPurchasePaymentContractDetail interface
    
    public interface IPurchasePaymentContractDetailService
    {
        [OperationContract]
        ImportLCDetailProduct Get(int id, Int64 nUserId);
      
        [OperationContract]
        List<ImportLCDetailProduct> Gets(int nPPCID, Int64 nUserId);
        [OperationContract]
        List<ImportLCDetailProduct> GetsByLCID(int nLCID, Int64 nUserId);
        [OperationContract]
        List<ImportLCDetailProduct> GetsBySQL(string sSQL, Int64 nUserId);

      
    }
    #endregion
    #endregion
   
}
