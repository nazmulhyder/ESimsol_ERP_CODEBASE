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
	#region OrderSheetDetail  
	public class OrderSheetDetail : BusinessObject
	{	
		public OrderSheetDetail()
		{
			OrderSheetDetailID = 0; 
			OrderSheetID = 0; 
			ProductID = 0; 
			ColorID = 0;
            ProductDescription = ""; 
			StyleDescription = ""; 
			Measurement = ""; 
			Qty = 0; 
			UnitPrice = 0; 
			DeliveryDate = DateTime.Now; 
			BuyerRef = ""; 
			Note = ""; 
			ModelReferenceID = 0;
            ProductCode = "";
			ProductName = ""; 
			ModelReferenceName = ""; 
			ColorName = ""; 
			SizeName = "";
            UnitName = "";
            UnitID = 0;
            YetToPIQty = 0;
            Amount = 0;
            UnitSymbol = "";
            IsApplySizer = false;
            OrderSheetLogID = 0;
            OrderSheetDetailLogID = 0;
            ColorQty = 0;
            RecipeID = 0;
            PolyMUnitID = 0;
            RecipeName = "";
			ErrorMessage = "";
		}

		#region Property
		public int OrderSheetDetailID { get; set; }
		public int OrderSheetID { get; set; }
		public int ProductID { get; set; }
		public int ColorID { get; set; }
        public int PolyMUnitID { get; set; }
        public string ProductDescription { get; set; }
		public string StyleDescription { get; set; }
		public string Measurement { get; set; }
		public double Qty { get; set; }
		public double UnitPrice { get; set; }
		public DateTime DeliveryDate { get; set; }
		public string BuyerRef { get; set; }
		public string Note { get; set; }
		public int ModelReferenceID { get; set; }
        public string ProductCode { get; set; }
		public string ProductName { get; set; }
		public string ModelReferenceName { get; set; }
		public string ColorName { get; set; }
		public string SizeName { get; set; }
        public string UnitName { get; set; }
        public int UnitID { get; set; }
        public string UnitSymbol { get; set; }
        public double YetToPIQty { get; set; }
        public double Amount { get; set; }
        public bool IsApplySizer { get; set; }
        public int OrderSheetLogID { get; set; }
        public int OrderSheetDetailLogID { get; set; }
        public double ColorQty { get; set; }
        public int RecipeID { get; set; }
        public string RecipeName { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        
		public string DeliveryDateInString 
		{
			get
			{
				return DeliveryDate.ToString("dd MMM yyyy") ; 
			}
		}
        public string QtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.Qty);
            }
        }
        public string UnitPriceSt
        {
            get
            {
                return Global.MillionFormat(this.UnitPrice);
            }
        }
        public string AmountSt
        {
            get
            {
                return Global.MillionFormat(this.Amount);
            }
        }
	
		#endregion 

		#region Functions 
		public static List<OrderSheetDetail> Gets(int nORSID, long nUserID)
		{
            return OrderSheetDetail.Service.Gets(nORSID,nUserID);
		}

        public static List<OrderSheetDetail> GetsByLog(int nORSLogID, long nUserID)
        {
            return OrderSheetDetail.Service.GetsByLog(nORSLogID, nUserID);
        }
		public static List<OrderSheetDetail> Gets(string sSQL, long nUserID)
		{
			return OrderSheetDetail.Service.Gets(sSQL,nUserID);
		}
		public OrderSheetDetail Get(int id, long nUserID)
		{
			return OrderSheetDetail.Service.Get(id,nUserID);
		}
		
	
		#endregion

		#region ServiceFactory
		internal static IOrderSheetDetailService Service
		{
			get { return (IOrderSheetDetailService)Services.Factory.CreateService(typeof(IOrderSheetDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IOrderSheetDetail interface
	public interface IOrderSheetDetailService 
	{
		OrderSheetDetail Get(int id, Int64 nUserID); 
		List<OrderSheetDetail> Gets(int nORSID, Int64 nUserID);
        List<OrderSheetDetail> GetsByLog(int nORSID, Int64 nUserID);
		List<OrderSheetDetail> Gets( string sSQL, Int64 nUserID);
	
	}
	#endregion
}
