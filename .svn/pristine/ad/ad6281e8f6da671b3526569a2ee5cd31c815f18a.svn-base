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
	#region LotMixingDetail  
	public class LotMixingDetail : BusinessObject
	{	
		public LotMixingDetail()
		{
			LotMixingDetailID = 0; 
			LotMixingID = 0; 
			ProductID = 0; 
			LotID = 0; 
			Qty = 0; 
			Qty_Percentage  = 0; 
			MUnitID = 0; 
			BagCount = 0; 
			UnitPrice = 0; 
			CurrencyID = 0;
            LotNo = "";
            ProductCode = "";
            ProductName = "";
            MUnit = "";
            MUSymbol = "";
            CurrenName = "";
            CurrenSymbol = "";
            Remarks = ""; 
			IsLotMendatory = true; 
			InOutType = EnumInOutType.None; 
			ErrorMessage = "";
		}

		#region Property
		public int LotMixingDetailID { get; set; }
        public int LotMixingID { get; set; }
		public int ProductID { get; set; }
		public int LotID { get; set; }
		public double Qty { get; set; }
		public double Qty_Percentage  { get; set; }
		public int MUnitID { get; set; }
		public double BagCount { get; set; }
		public double UnitPrice { get; set; }
		public int CurrencyID { get; set; }
		public string Remarks { get; set; }
		public bool IsLotMendatory { get; set; }
        public EnumInOutType InOutType { get; set; }
        public int InOutTypeInt { get; set; }
        public double LotBalance { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string LotNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string MUnit { get; set; }
        public string MUSymbol { get; set; }
        public string CurrenName { get; set; }
        public string CurrenSymbol { get; set; }
        public double Amount { get { return this.Qty * this.UnitPrice; } }
        //public string InOutTypeSt { get { return EnumObject.jGet(this.InOutType); } }
		#endregion 

		#region Functions 
		public static List<LotMixingDetail> Gets(long nUserID)
		{
			return LotMixingDetail.Service.Gets(nUserID);
		}
		public static List<LotMixingDetail> Gets(string sSQL, long nUserID)
		{
			return LotMixingDetail.Service.Gets(sSQL,nUserID);
		}
		public LotMixingDetail Get(int id, long nUserID)
		{
			return LotMixingDetail.Service.Get(id,nUserID);
		}
		public LotMixingDetail Save(long nUserID)
		{
			return LotMixingDetail.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return LotMixingDetail.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static ILotMixingDetailService Service
		{
			get { return (ILotMixingDetailService)Services.Factory.CreateService(typeof(ILotMixingDetailService)); }
		}
		#endregion

    }
	#endregion

	#region ILotMixingDetail interface
	public interface ILotMixingDetailService 
	{
		LotMixingDetail Get(int id, Int64 nUserID); 
		List<LotMixingDetail> Gets(Int64 nUserID);
		List<LotMixingDetail> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		LotMixingDetail Save(LotMixingDetail oLotMixingDetail, Int64 nUserID);
	}
	#endregion
}
