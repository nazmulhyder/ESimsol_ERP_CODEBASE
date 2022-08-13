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
	#region KnittingYarnReturnDetail  
	public class KnittingYarnReturnDetail : BusinessObject
	{	
		public KnittingYarnReturnDetail()
		{
			KnittingYarnReturnDetailID = 0; 
			KnittingYarnReturnID = 0; 
			KnittingYarnChallanDetailID = 0; 
			YarnID = 0; 
			ReceiveStoreID = 0; 
			LotID = 0; 
			NewLotNo = ""; 
			MUnitID = 0; 
			Qty = 0; 
			Remarks = ""; 
			ErrorMessage = "";
            KnittingOrderDetailID = 0;
            StyleNo = "";
            BuyerName = "";
            PAM = 0;
            ColorName = "";
            BrandShortName = "";
            DetailQty = 0;
            DetailMUShortName = "";
		}

		#region Property
		public int KnittingYarnReturnDetailID { get; set; }
		public int KnittingYarnReturnID { get; set; }
		public int KnittingYarnChallanDetailID { get; set; }
		public int YarnID { get; set; }
		public int ReceiveStoreID { get; set; }
		public int LotID { get; set; }
		public string NewLotNo { get; set; }
		public int MUnitID { get; set; }
		public double Qty { get; set; }
		public string Remarks { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string ChallanNo { get; set; }
		public string  MUnitName{get; set;}
		public string   OperationUnitName{get; set;}
		public string  YarnName{get; set;}
		public string  YarnCode{get; set;}
		public string  LotNo {get; set;}
        public double LotBalance { get; set; }
        public double ChallanQty { get; set; }
        public double ReturnBalance { get; set; }
        public double ChallanBalance { get; set; }
        public int KnittingOrderDetailID { get; set; }
        public string StyleNo { get; set; }
        public string BuyerName { get; set; }
        public int PAM { get; set; }
        public string ColorName { get; set; }
        public string BrandShortName { get; set; }
        public double DetailQty { get; set; }
        public string DetailMUShortName { get; set; }
        
		#endregion 

		#region Functions 
		public static List<KnittingYarnReturnDetail> Gets(long nUserID)
		{
			return KnittingYarnReturnDetail.Service.Gets(nUserID);
		}
		public static List<KnittingYarnReturnDetail> Gets(string sSQL, long nUserID)
		{
			return KnittingYarnReturnDetail.Service.Gets(sSQL,nUserID);
		}
		public KnittingYarnReturnDetail Get(int id, long nUserID)
		{
			return KnittingYarnReturnDetail.Service.Get(id,nUserID);
		}
		public KnittingYarnReturnDetail Save(long nUserID)
		{
			return KnittingYarnReturnDetail.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return KnittingYarnReturnDetail.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IKnittingYarnReturnDetailService Service
		{
			get { return (IKnittingYarnReturnDetailService)Services.Factory.CreateService(typeof(IKnittingYarnReturnDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IKnittingYarnReturnDetail interface
	public interface IKnittingYarnReturnDetailService 
	{
		KnittingYarnReturnDetail Get(int id, Int64 nUserID); 
		List<KnittingYarnReturnDetail> Gets(Int64 nUserID);
		List<KnittingYarnReturnDetail> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		KnittingYarnReturnDetail Save(KnittingYarnReturnDetail oKnittingYarnReturnDetail, Int64 nUserID);
	}
	#endregion
}
