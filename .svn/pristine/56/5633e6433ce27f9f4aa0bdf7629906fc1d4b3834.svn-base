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
	#region PurchaseCost  
	public class PurchaseCost : BusinessObject
	{	
		public PurchaseCost()
		{
			PurchaseCostID = 0; 
			RefID = 0; 
			CostHeadID = 0; 
			ValueInPercent = 0; 
			ValueInAmount = 0; 
			RefType = 0;
            Sequence = 0;
			ErrorMessage = "";
		}

		#region Property
		public int PurchaseCostID { get; set; }
		public int RefID { get; set; }
		public int CostHeadID { get; set; }
		public double ValueInPercent { get; set; }
		public double ValueInAmount { get; set; }
		public int RefType { get; set; }
        public int Sequence { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public EnumCostHeadType CostHeadType { get; set; }
        //public EnumCostHeadCategorey CostHeadCategorey { get; set; }
        public string Name { get; set; }
        public string CostHeadTypeSt { get { return this.CostHeadType.ToString(); } }
        //public string CostHeadCategoreySt { get { return this.CostHeadCategorey.ToString(); } }
		#endregion 

		#region Functions 
		public static List<PurchaseCost> Gets(long nUserID)
		{
			return PurchaseCost.Service.Gets(nUserID);
		}
		public static List<PurchaseCost> Gets(string sSQL, long nUserID)
		{
			return PurchaseCost.Service.Gets(sSQL,nUserID);
		}
		public PurchaseCost Get(int id, long nUserID)
		{
			return PurchaseCost.Service.Get(id,nUserID);
		}
		public PurchaseCost Save(long nUserID)
		{
			return PurchaseCost.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return PurchaseCost.Service.Delete(id,nUserID);
		}
       
		#endregion

		#region ServiceFactory
		internal static IPurchaseCostService Service
		{
			get { return (IPurchaseCostService)Services.Factory.CreateService(typeof(IPurchaseCostService)); }
		}
		#endregion
	}
	#endregion

	#region IPurchaseCost interface
	public interface IPurchaseCostService 
	{
		PurchaseCost Get(int id, Int64 nUserID); 
		List<PurchaseCost> Gets(Int64 nUserID);
		List<PurchaseCost> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		PurchaseCost Save(PurchaseCost oPurchaseCost, Int64 nUserID);
	}
	#endregion
}
