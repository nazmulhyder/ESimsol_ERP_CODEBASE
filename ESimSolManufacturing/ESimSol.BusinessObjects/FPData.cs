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
	#region FPData  
	public class FPData : BusinessObject
	{	
		public FPData()
		{
			FPDataID = 0; 
			OperationalCost = 0; 
			BTBCost = 0; 
			ExportHMonth = ""; 
			ExportHQty = 0; 
			EHValue = 0; 
			ExportQty = 0; 
			ExportValue = 0;
            FPDate = DateTime.Today;
			ErrorMessage = "";
		}

		#region Property
		public int FPDataID { get; set; }
		public double OperationalCost { get; set; }
		public double BTBCost { get; set; }
		public string ExportHMonth { get; set; }
		public double ExportHQty { get; set; }
		public double EHValue { get; set; }
		public double ExportQty { get; set; }
		public double ExportValue { get; set; }
        public DateTime FPDate { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string FPDateSt
        {
            get
            {
                return this.FPDate.ToString("dd MMM yyyy");
            }
        }
		#endregion 

		#region Functions 
	
		public static List<FPData> Gets(string sSQL, long nUserID)
		{
			return FPData.Service.Gets(sSQL,nUserID);
		}
		public FPData Get(int id, long nUserID)
		{
			return FPData.Service.Get(id,nUserID);
		}
		public FPData Save(long nUserID)
		{
			return FPData.Service.Save(this,nUserID);
		}

		#endregion

		#region ServiceFactory
		internal static IFPDataService Service
		{
			get { return (IFPDataService)Services.Factory.CreateService(typeof(IFPDataService)); }
		}
		#endregion
	}
	#endregion

	#region IFPData interface
	public interface IFPDataService 
	{
		FPData Get(int id, Int64 nUserID); 
		List<FPData> Gets( string sSQL, Int64 nUserID);
 		FPData Save(FPData oFPData, Int64 nUserID);
	}
	#endregion
}
