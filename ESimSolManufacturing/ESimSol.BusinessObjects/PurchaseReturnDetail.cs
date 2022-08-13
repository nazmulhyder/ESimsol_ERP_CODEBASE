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
	#region PurchaseReturnDetail  
	public class PurchaseReturnDetail : BusinessObject
	{	
		public PurchaseReturnDetail()
		{
			PurchaseReturnDetailID = 0; 
			PurchaseReturnID = 0; 
			ProductID = 0; 
			MUnitID = 0; 
			LotID = 0; 
			StyleID = 0; 
			RefObjectDetailID = 0; 
			ColorID = 0; 
			SizeID = 0; 
			ReturnQty = 0; 
			ProductCode = ""; 
			ProductName = ""; 
			MUName = ""; 
			LotNo = ""; 
			StyleNo = ""; 
			ColorName = ""; 
			SizeName = ""; 
			RefDetailQty = 0;
            MCDia = "";
            FinishDia = "";
            GSM = "";
			ErrorMessage = "";
		}
		#region Property
		public int PurchaseReturnDetailID { get; set; }
		public int PurchaseReturnID { get; set; }
		public int ProductID { get; set; }
		public int MUnitID { get; set; }
		public int LotID { get; set; }
		public int StyleID { get; set; }
		public int RefObjectDetailID { get; set; }
		public int ColorID { get; set; }
		public int SizeID { get; set; }
		public double ReturnQty { get; set; }
		public string ProductCode { get; set; }
		public string ProductName { get; set; }
        public string BuyerName { get; set; }
		public string MUName { get; set; }
		public string LotNo { get; set; }
		public string StyleNo { get; set; }
		public string ColorName { get; set; }
		public string SizeName { get; set; }
        public string MUSymbol { get; set; }
		public double RefDetailQty { get; set; }
        public EnumPurchaseReturnType RefType { get; set; }
        public int RefTypeInt { get; set; }
        public double LotBalance { get; set; }
        public double YetToPurchaseReturnQty { get; set; }
        public string MCDia { get; set; }
        public string FinishDia { get; set; }
        public string GSM { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 
		#region Derived Property
		#endregion 
		#region Functions 
		public static List<PurchaseReturnDetail> Gets(int id, long nUserID)
		{
			return PurchaseReturnDetail.Service.Gets(id, nUserID);
		}
		public static List<PurchaseReturnDetail> Gets(string sSQL, long nUserID)
		{
			return PurchaseReturnDetail.Service.Gets(sSQL,nUserID);
		}
		public PurchaseReturnDetail Get(int id, long nUserID)
		{
			return PurchaseReturnDetail.Service.Get(id,nUserID);
		}
	
		#endregion

		#region ServiceFactory
		internal static IPurchaseReturnDetailService Service
		{
			get { return (IPurchaseReturnDetailService)Services.Factory.CreateService(typeof(IPurchaseReturnDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IPurchaseReturnDetail interface
	public interface IPurchaseReturnDetailService 
	{
		PurchaseReturnDetail Get(int id, Int64 nUserID); 
		List<PurchaseReturnDetail> Gets(int id, Int64 nUserID);
        List<PurchaseReturnDetail> Gets(string sSQL, Int64 nUserID);
	}
	#endregion
}
