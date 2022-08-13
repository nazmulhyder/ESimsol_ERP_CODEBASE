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
	#region VehicleOrderDetail  
	public class VehicleOrderDetail : BusinessObject
	{	
		public VehicleOrderDetail()
		{
			VehicleOrderDetailID = 0; 
			VehicleOrderID = 0;             
            FeatureID = 0;
            Price = 0;
            CurrencyID = 0;
            CurrencyName = "";
            CurrencySymbol = "";
            FeatureCode = "";
            FeatureName = "";
            Remarks = "";
            FeatureType = EnumFeatureType.None; 
			ErrorMessage = "";
		}

		#region Property
		public int VehicleOrderDetailID { get; set; }
		public int VehicleOrderID { get; set; }
		public int FeatureID { get; set; }
		public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
		public int FeatureTypeInInt { get; set; }
        public double Price { get; set; }
        public EnumFeatureType FeatureType { get; set; }
		public string FeatureCode { get; set; }
		public string FeatureName { get; set; }
        public string Remarks { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string FeatureTypeST
        {
            get
            {
                return this.FeatureType.ToString();
            }
        }
		#endregion 

		#region Functions 
		public static List<VehicleOrderDetail> Gets(int id, long nUserID)
		{
			return VehicleOrderDetail.Service.Gets(id, nUserID);
		}
		public static List<VehicleOrderDetail> Gets(string sSQL, long nUserID)
		{
			return VehicleOrderDetail.Service.Gets(sSQL,nUserID);
		}
		public VehicleOrderDetail Get(int id, long nUserID)
		{
			return VehicleOrderDetail.Service.Get(id,nUserID);
		}
		
		#endregion

		#region ServiceFactory
		internal static IVehicleOrderDetailService Service
		{
			get { return (IVehicleOrderDetailService)Services.Factory.CreateService(typeof(IVehicleOrderDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IVehicleOrderDetail interface
	public interface IVehicleOrderDetailService 
	{
		VehicleOrderDetail Get(int id, Int64 nUserID); 
		List<VehicleOrderDetail> Gets(int id, Int64 nUserID);
		List<VehicleOrderDetail> Gets( string sSQL, Int64 nUserID);
		
	}
	#endregion
}
