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
	#region RecycleProcessDetail  
	public class RecycleProcessDetail : BusinessObject
	{	
		public RecycleProcessDetail()
		{
			RecycleProcessDetailID = 0; 
			RecycleProcessID = 0; 
			ProductID = 0; 
			UnitID = 0; 
			Qty = 0;
            ProcessProductType = 0;
            LotID = 0;
            WorkingUnitID = 0;
			ProductCode = ""; 
			ProductName = ""; 
			UnitName = ""; 
			ErrorMessage = "";
            WorkingUnitName = "";
            WorkingUName="";
            LotNo = "";
            Symbol = "";
		}

		#region Property
		public int RecycleProcessDetailID { get; set; }
		public int RecycleProcessID { get; set; }
		public int ProductID { get; set; }
		public int UnitID { get; set; }
		public double Qty { get; set; }
        public EnumProcessProductType ProcessProductType { get; set; }
        public int LotID { get; set; }
        public int WorkingUnitID { get; set; }
        public string LotNo { get; set; }
		public string ProductCode { get; set; }
		public string ProductName { get; set; }
		public string Symbol { get; set; }
		public string UnitName { get; set; }
        public string WorkingUnitName { get; set; }
        public string WorkingUName { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		public static List<RecycleProcessDetail> Gets(int id, long nUserID)
		{
			return RecycleProcessDetail.Service.Gets(id, nUserID);
		}
		public static List<RecycleProcessDetail> Gets(string sSQL, long nUserID)
		{
			return RecycleProcessDetail.Service.Gets(sSQL,nUserID);
		}
		public RecycleProcessDetail Get(int id, long nUserID)
		{
			return RecycleProcessDetail.Service.Get(id,nUserID);
		}
		
		#endregion

		#region ServiceFactory
		internal static IRecycleProcessDetailService Service
		{
			get { return (IRecycleProcessDetailService)Services.Factory.CreateService(typeof(IRecycleProcessDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IRecycleProcessDetail interface
	public interface IRecycleProcessDetailService 
	{
		RecycleProcessDetail Get(int id, Int64 nUserID); 
		List<RecycleProcessDetail> Gets(int id, Int64 nUserID);
		List<RecycleProcessDetail> Gets( string sSQL, Int64 nUserID);
		
	}
	#endregion
}
