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
	#region FabricLotAssign  
	public class FabricLotAssign : BusinessObject
	{	
		public FabricLotAssign()
		{
			FabricLotAssignID = 0; 
			LotID = 0; 
			FEOSDID = 0; 
			Qty = 0; 
			Balance = 0; 
			ErrorMessage = "";
            FabricLotAssigns = new List<FabricLotAssign>();
            WorkingUnitID = 0;
            MUName = "";
            MUSymbol = "";
            WarpWeftType = EnumWarpWeft.None;
            FEOSID = 0;
            ParentLotID = 0;
            FSCDetailID = 0;
            DyeingOrderID = 0;
            LocationName = "";
            ProductNameLot = "";
            DyeingOrderDetailID = 0;
		}

		#region Property
		public int FabricLotAssignID { get; set; }
		public int LotID { get; set; }
		public int FEOSDID { get; set; }
        public int FEOSID { get; set; }
        public double Qty { get; set; }
        public double Qty_Order { get; set; }
        public double Qty_RS { get; set; }
        public double Qty_Req { get; set; }
        public double Balance { get; set; }
        public string ProductName { get; set; }
        public string ProductNameLot { get; set; }
        public string ProductCode { get; set; }
        public string LotNo { get; set; }
        public double BalanceLot { get; set; }
        public string OperationUnitName { get; set; }
        public string LocationName { get; set; }
        public string ExeNo { get; set; }
        public string Params { get; set; }
        public int ProductID { get; set; }
        public int ParentLotID { get; set; }
        public int WorkingUnitID { get; set; }
        public DateTime FabricLotAssignDate { get; set; }
        public string BuyerName { get; set; }
        public string CustomerName { get; set; }
        public string ColorName { get; set; }
        public string SearchByOrderDate { get; set; }
        public string SearchByAssingDate { get; set; }
        public EnumWarpWeft WarpWeftType { get; set; }
        public string WarpWeftTypeSt { get { return EnumObject.jGet(this.WarpWeftType); } }
        public double Qty_Assign { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public int FSCDetailID { get; set; }
        public int DyeingOrderID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public double Qty_Consume { get { return this.Qty - this.Qty_RS; } }
        public double Qty_YetToAssign{ get { return this.Qty_Order - this.Qty_Assign; } }
        public string FabricLotAssignDateSt { get { return this.FabricLotAssignDate.ToString("dd MMM yyyy hh:mm tt"); } }
        public List<FabricLotAssign> FabricLotAssigns { get; set; }
		#endregion 

		#region Functions 
		public static List<FabricLotAssign> Gets(long nUserID)
		{
			return FabricLotAssign.Service.Gets(nUserID);
		}
		public static List<FabricLotAssign> Gets(string sSQL, long nUserID)
		{
			return FabricLotAssign.Service.Gets(sSQL,nUserID);
		}
		public FabricLotAssign Get(int id, long nUserID)
		{
			return FabricLotAssign.Service.Get(id,nUserID);
		}
        public FabricLotAssign Save(long nUserID)
        {
            return FabricLotAssign.Service.Save(this, nUserID);
        }
		public FabricLotAssign Save(List<FabricLotAssign> oFabricLotAssigns, long nUserID)
		{
            return FabricLotAssign.Service.Save(oFabricLotAssigns, nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return FabricLotAssign.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFabricLotAssignService Service
		{
			get { return (IFabricLotAssignService)Services.Factory.CreateService(typeof(IFabricLotAssignService)); }
		}
		#endregion

    }
	#endregion

	#region IFabricLotAssign interface
	public interface IFabricLotAssignService 
	{
		FabricLotAssign Get(int id, Int64 nUserID); 
		List<FabricLotAssign> Gets(Int64 nUserID);
		List<FabricLotAssign> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		FabricLotAssign Save(FabricLotAssign oFabricLotAssign, Int64 nUserID);
        FabricLotAssign Save(List<FabricLotAssign> oFabricLotAssigns, Int64 nUserID);
	}
	#endregion
}
