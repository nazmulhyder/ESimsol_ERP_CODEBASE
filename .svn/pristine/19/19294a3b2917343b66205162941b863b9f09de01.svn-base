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
	#region DUProductionStatusReport  
	public class DUProductionStatusReport : BusinessObject
	{	
		public DUProductionStatusReport()
		{
			DUProGuideLineID = 0; 
			DyingOrderID = 0; 
			OrderNo = "";
            OrderDate = DateTime.MinValue;
			OrderType = 0;
            StyleNo = "";
            MUnit = "";
            BuyerName = ""; 
			BuyerRef = ""; 
			ProductName = ""; 
			Qty_Order = 0; 
			Qty_Rcv = 0; 
			Qty_YetToRcv = 0; 
			Qty_SW = 0; 
			Qty_YetToSW = 0;
            Qty_Return = 0;
            OrderName = "";
			ErrorMessage = "";
		}

		#region Property
		public int DUProGuideLineID { get; set; }
		public int DyingOrderID { get; set; }
		public string OrderNo { get; set; }
		public int OrderType { get; set; }
		public string BuyerName { get; set; }
		public string BuyerRef { get; set; }
		public string ProductName { get; set; }
		public double Qty_Order { get; set; }
		public double Qty_Rcv { get; set; }
		public double Qty_YetToRcv { get; set; }
		public double Qty_SW { get; set; }
		public double Qty_YetToSW { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductID { get; set; }
        public string StyleNo { get; set; }
        public string MUnit { get; set; }
        public string Remarks { get; set; }
        public string YarnLotNo { get; set; }
        public double Qty_Transfer_In { get; set; }
        public double Qty_Transfer_Out { get; set; }
        public string OrderName { get; set; }
        public double Qty_Return { get; set; }
        public string Params { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string OrderTypeST { get { return EnumObject.jGet((EnumOrderType)this.OrderType); } }
        public string OrderDateST { get { if (this.OrderDate == DateTime.MinValue) return "-"; else return this.OrderDate.ToString("dd MMM yyyy"); } }
		#endregion 

        #region Functions
        public static List<DUProductionStatusReport> Gets(string sSQL, EnumReportLayout eEnumReportLayout, long nUserID)
        {
            return DUProductionStatusReport.Service.Gets(sSQL, eEnumReportLayout, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDUProductionStatusReportService Service
        {
            get { return (IDUProductionStatusReportService)Services.Factory.CreateService(typeof(IDUProductionStatusReportService)); }
        }
        #endregion

    }
	#endregion

	#region IDUProductionStatusReport interface
    public interface IDUProductionStatusReportService
    {
        List<DUProductionStatusReport> Gets(string sSQL, EnumReportLayout eEnumReportLayout, long nUserID);
    }
	#endregion
}
