using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
	#region GRNDetailBreakDown  
	public class GRNDetailBreakDown : BusinessObject
	{	
		public GRNDetailBreakDown()
		{
			GRNDetailBreakDownID = 0; 
			GRNDetailID = 0; 
			PurchaseInvoiceDetailID = 0; 
			RefType = 0; 
			GRNID = 0; 
			ReceivedQty = 0; 
			Amount = 0; 
			ImportInvoiceDetailID = 0; 
			LandingCostValue = 0; 
			CostHeadID = 0; 
			ProductName = ""; 
			MUSymbol = ""; 
			InvoiceNo = ""; 
			LCID = 0; 
			InvoiceID = 0; 
			InvoiceDetailID = 0; 
			CostHeadCode = ""; 
			CostHeadName = ""; 
			ConvertionRate = 0; 
			CurrencySymbol = "";
            LCNo = "";
			ErrorMessage = "";
		}

		#region Property
		public int GRNDetailBreakDownID { get; set; }
		public int GRNDetailID { get; set; }
		public int PurchaseInvoiceDetailID { get; set; }
		public int RefType { get; set; }
		public int GRNID { get; set; }
		public double ReceivedQty { get; set; }
		public double Amount { get; set; }
		public int ImportInvoiceDetailID { get; set; }
		public double LandingCostValue { get; set; }
		public int CostHeadID { get; set; }
		public string ProductName { get; set; }
		public string MUSymbol { get; set; }
		public string InvoiceNo { get; set; }
		public int LCID { get; set; }
		public int InvoiceID { get; set; }
		public int InvoiceDetailID { get; set; }
		public string CostHeadCode { get; set; }
		public string CostHeadName { get; set; }
		public double ConvertionRate { get; set; }
		public string CurrencySymbol { get; set; }
        public string LCNo { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
	
		public static List<GRNDetailBreakDown> Gets(string sSQL, long nUserID)
		{
			return GRNDetailBreakDown.Service.Gets(sSQL,nUserID);
		}
	
		#endregion

		#region ServiceFactory
		internal static IGRNDetailBreakDownService Service
		{
			get { return (IGRNDetailBreakDownService)Services.Factory.CreateService(typeof(IGRNDetailBreakDownService)); }
		}
		#endregion
	}
	#endregion

	#region IGRNDetailBreakDown interface
	public interface IGRNDetailBreakDownService 
	{
		
		List<GRNDetailBreakDown> Gets( string sSQL, Int64 nUserID);
		
	}
	#endregion
}
