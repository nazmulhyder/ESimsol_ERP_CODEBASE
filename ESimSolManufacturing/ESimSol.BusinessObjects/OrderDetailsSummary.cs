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

    #region OrderDetailsSummary
    
    public class OrderDetailsSummary : BusinessObject
    {
        public OrderDetailsSummary()
        {
            OrderRecapID =0;	
			OrderRecapDate =DateTime.Now;						
			StyleNo = "";	
			BuyerName = "";
			RecapNo = "";
			RecapQty =0;
			RecapValue =0;
			ShipmentDate =DateTime.Now;
			ODSNo = "";
			ONSNo = "";
			ExportPINo = "";
			MasterLCNo = "";
			LCTransferNo = "";
			ProductionOrderQty =0;
			ProductionQty =0;
			QCStatus  = "";
			ShipmentQty =0;
			ComercialInvoiceQty =0;
			YarnValue =0;
			AccessoriesValue =0;
			CMValue =0;
			EndosmentCommission =0;
			B2BCommission =0;
			TotalCommission =0;
            CommissionRealise = 0;
            ErrorMessage = "";

        }

        #region Properties
         
        public int OrderRecapID  { get; set; }
         
        public DateTime OrderRecapDate  { get; set; }
         
        public string StyleNo  { get; set; }
         
        public string BuyerName { get; set; }
         
        public string RecapNo { get; set; }
         
        public double RecapQty  { get; set; }
         
        public double RecapValue  { get; set; }
         
        public DateTime ShipmentDate { get; set; }
         
        public string ODSNo { get; set; }
         
        public string ONSNo { get; set; }
         
        public string ExportPINo { get; set; }
         
        public string MasterLCNo { get; set; }
         
        public string LCTransferNo { get; set; }
         
        public double ProductionOrderQty  { get; set; }
         
        public double ProductionQty  { get; set; }
         
        public string  QCStatus   { get; set; }
         
        public double ShipmentQty  { get; set; }
         
        public double ComercialInvoiceQty { get; set; }
         
        public double YarnValue { get; set; }
         
        public double AccessoriesValue { get; set; }
      
         
        public double CMValue { get; set; }
         
        public double EndosmentCommission { get; set; }
         
        public double B2BCommission { get; set; }
         
        public double TotalCommission { get; set; }
         
        public double CommissionRealise { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        public List<OrderDetailsSummary> OrderDetailsSummarys { get; set; }
        public Company Company { get; set; }


        public string OrderDateInString
        {
            get
            {
                return this.OrderRecapDate.ToString("dd MMM yyyy");
            }
        }
        
        public string ShipmentDateInString
        {
            get
            {
                return this.ShipmentDate.ToString("dd MMM yyyy");
            }
        }

        #endregion

        #region Functions
        public static List<OrderDetailsSummary> Gets(string sIDs, long nUserID)
        {
            return OrderDetailsSummary.Service.Gets(sIDs, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IOrderDetailsSummaryService Service
        {
            get { return (IOrderDetailsSummaryService)Services.Factory.CreateService(typeof(IOrderDetailsSummaryService)); }
        }
        #endregion
    }
    #endregion

    #region Report Study
    public class OrderDetailsSummaryList : List<OrderDetailsSummary>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IOrderDetailsSummary interface
     
    public interface IOrderDetailsSummaryService
    {
         
        List<OrderDetailsSummary> Gets(string sIDs, Int64 nUserID);

    }
    #endregion
    
    
  
}
