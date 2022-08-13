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
	#region FNRequisitionReport  
	public class FNRequisitionReport : BusinessObject
	{	
		public FNRequisitionReport()
		{
			FNRID  = 0; 
			FNRNo = ""; 
			FNRDetailID = 0; 
			DisburseQty  = 0; 
			RequestDate  = DateTime.Now; 
			RequestByID  = 0; 
			RequestByName  = ""; 
			TreatmentID  = 0; 
			TreatmentName   = ""; 
			ProductID  = 0; 
			ProductName   = ""; 
			LotID  = 0; 
			LotNo  = ""; 
			FSCDID  = 0; 
			DispoNo  = ""; 
			Construction  = ""; 
			ColorName  = ""; 
			BuyerID  = 0; 
			BuyerName  = ""; 
			MachineID  = 0; 
			MachineName  = ""; 
			WorkingUnitID  = 0; 
			WUName  = ""; 
			Qty_Order  = 0; 
			Qty_Requisition  = 0; 
			Qty_Consume  = 0; 
			MUnitID = 0; 
			MUName = "";
            Params = "";
			ErrorMessage = "";
		}

		#region Property
		public int FNRID  { get; set; }
		public string FNRNo { get; set; }
		public int FNRDetailID { get; set; }
		public double DisburseQty  { get; set; }
		public DateTime RequestDate  { get; set; }
		public int RequestByID  { get; set; }
		public string RequestByName  { get; set; }
		public int TreatmentID  { get; set; }
		public string TreatmentName   { get; set; }
		public int ProductID  { get; set; }
		public string ProductName   { get; set; }
		public int LotID  { get; set; }
		public string LotNo  { get; set; }
		public int FSCDID  { get; set; }
		public string DispoNo  { get; set; }
		public string Construction  { get; set; }
		public string ColorName  { get; set; }
		public int BuyerID  { get; set; }
		public string BuyerName  { get; set; }
		public int MachineID  { get; set; }
		public string MachineName  { get; set; }
		public int WorkingUnitID  { get; set; }
		public string WUName  { get; set; }
		public double Qty_Order  { get; set; }
		public double Qty_Requisition  { get; set; }
		public double Qty_Consume  { get; set; }
		public int MUnitID { get; set; }
        public string MUName { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

        #region Derived Property
        public string Params { get; set; }
        public string TreatmentSt { get { return EnumObject.jGet((EnumFNTreatment)this.TreatmentID); } }
        public double Qty_Balance { get { return (this.Qty_Requisition - this.Qty_Consume); } }
		public string RequestDateInString 
		{
			get
			{
				return RequestDate .ToString("dd MMM yyyy") ; 
			}
		}
		#endregion 

		#region Functions 
	
		public static List<FNRequisitionReport> Gets(string sSQL, long nUserID)
		{
			return FNRequisitionReport.Service.Gets(sSQL,nUserID);
		}
	
		#endregion

		#region ServiceFactory
		internal static IFNRequisitionReportService Service
		{
			get { return (IFNRequisitionReportService)Services.Factory.CreateService(typeof(IFNRequisitionReportService)); }
		}
		#endregion

    }
	#endregion

	#region IFNRequisitionReport interface
	public interface IFNRequisitionReportService 
	{
		List<FNRequisitionReport> Gets( string sSQL, Int64 nUserID);
	}
	#endregion
}
