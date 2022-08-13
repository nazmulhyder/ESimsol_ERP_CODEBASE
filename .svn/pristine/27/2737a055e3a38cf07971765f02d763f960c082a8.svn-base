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
	#region DyeingOrderFabricDetail  
	public class DyeingOrderFabricDetail : BusinessObject
	{	
		public DyeingOrderFabricDetail()
		{
			DyeingOrderFabricDetailID = 0; 
			DyeingOrderID = 0; 
			DyeingOrderDetailID = 0; 
			FSCDetailID = 0; 
			FEOSID = 0; 
			FEOSDID = 0; 
			SLNo = "";
            Qty = 0;
            Qty_RS = 0;
            Qty_Req = 0;
            WarpWeftType = EnumWarpWeft.None;
            WarpWeftTypeInt = 0; 
			ProductID = 0; 
			BuyerReference = ""; 
			ColorInfo = ""; 
			StyleNo = ""; 
			FinishType = 0; 
			ProcessType = 0; 
			Construction = ""; 
			FabricWidth = ""; 
			ExeNo = ""; 
			IsWarp = true; 
			ColorName = "";
            EndsCount = 0;
            BUID = 0; 
			BatchNo = ""; 
			ErrorMessage = "";
            OrderNo="";
			OrderDate=DateTime.MinValue;
			CustomerName="";
            BuyerName = "";
            QtyDyed = 0;
            LengthReq = 0;
            ConeReq = 0;
            Length = 0;
		}

		#region Property
		public int DyeingOrderFabricDetailID { get; set; }
		public int DyeingOrderID { get; set; }
		public int DyeingOrderDetailID { get; set; }
		public int FSCDetailID { get; set; }
		public int FEOSID { get; set; }
		public int FEOSDID { get; set; }
		public string SLNo { get; set; }
		public double Qty { get; set; }
        public double QtyDyed { get; set; }
        
        public int WarpWeftTypeInt { get; set; }
        public EnumWarpWeft WarpWeftType { get; set; }
		public int ProductID { get; set; }
		public string BuyerReference { get; set; }
		public string ColorInfo { get; set; }
		public string StyleNo { get; set; }
		public int FinishType { get; set; }
		public int ProcessType { get; set; }
		public string Construction { get; set; }
		public string FabricWidth { get; set; }
		public string ExeNo { get; set; }
		public bool IsWarp { get; set; }
		public string ColorName { get; set; }
		public int EndsCount { get; set; }
        public string BatchNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string BuyerName { get; set; }
        public string SearchStringDate { get; set; }
        public string ProductName { get; set; }
        public string LotNo { get; set; }
        public bool YetToLotAssign { get; set; }
        public double Qty_RS { get; set; }
        public double Qty_Req { get; set; }
        public double Qty_Assign { get; set; }
        public double LengthReq { get; set; }
        public double Length { get; set; }
        public double ConeReq { get; set; }
        public double Cone { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
		#endregion 

        #region Derived Property
        public double Qty_YetToAssign { get { return this.Qty - this.Qty_Assign; } }
        public string WarpWeftTypeSt { get { return EnumObject.jGet(this.WarpWeftType); } }
        public string OrderDateSt { get { return (this.OrderDate== DateTime.MinValue)? "" : this.OrderDate.ToString("dd MMM yyyy"); } }
        
		#endregion 

		#region Functions 
		public static List<DyeingOrderFabricDetail> Gets(long nUserID)
		{
			return DyeingOrderFabricDetail.Service.Gets(nUserID);
		}
		public static List<DyeingOrderFabricDetail> Gets(string sSQL, long nUserID)
		{
			return DyeingOrderFabricDetail.Service.Gets(sSQL,nUserID);
		}
		public DyeingOrderFabricDetail Get(int id, long nUserID)
		{
			return DyeingOrderFabricDetail.Service.Get(id,nUserID);
		}
		public DyeingOrderFabricDetail Save(long nUserID)
		{
			return DyeingOrderFabricDetail.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return DyeingOrderFabricDetail.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IDyeingOrderFabricDetailService Service
		{
			get { return (IDyeingOrderFabricDetailService)Services.Factory.CreateService(typeof(IDyeingOrderFabricDetailService)); }
		}
		#endregion




        public int BUID { get; set; }
    }
	#endregion

	#region IDyeingOrderFabricDetail interface
	public interface IDyeingOrderFabricDetailService 
	{
		DyeingOrderFabricDetail Get(int id, Int64 nUserID); 
		List<DyeingOrderFabricDetail> Gets(Int64 nUserID);
		List<DyeingOrderFabricDetail> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		DyeingOrderFabricDetail Save(DyeingOrderFabricDetail oDyeingOrderFabricDetail, Int64 nUserID);
	}
	#endregion
}
