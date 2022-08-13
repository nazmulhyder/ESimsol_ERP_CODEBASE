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
	#region ModelFeature  
	public class ModelFeature : BusinessObject
	{	
		public ModelFeature()
		{
			ModelFeatureID = 0; 
			VehicleModelID = 0;             
            FeatureID = 0; 
            Sequence = 0;
            FeatureCode = "";
            Price = 0;
            FeatureName = "";
            FeatureType = EnumFeatureType.None; 
			ErrorMessage = "";
		}

		#region Property
		public int ModelFeatureID { get; set; }
		public int VehicleModelID { get; set; }
		public int FeatureID { get; set; }
		public int Sequence { get; set; }
        public int FeatureTypeInInt { get; set; }
        public double Price { get; set; }
        public EnumFeatureType FeatureType { get; set; }
		public string FeatureCode { get; set; }
		public string FeatureName { get; set; }
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
		public static List<ModelFeature> Gets(int id, long nUserID)
		{
			return ModelFeature.Service.Gets(id, nUserID);
		}
		public static List<ModelFeature> Gets(string sSQL, long nUserID)
		{
			return ModelFeature.Service.Gets(sSQL,nUserID);
		}
		public ModelFeature Get(int id, long nUserID)
		{
			return ModelFeature.Service.Get(id,nUserID);
		}
		
		#endregion

		#region ServiceFactory
		internal static IModelFeatureService Service
		{
			get { return (IModelFeatureService)Services.Factory.CreateService(typeof(IModelFeatureService)); }
		}
		#endregion
	}
	#endregion

	#region IModelFeature interface
	public interface IModelFeatureService 
	{
		ModelFeature Get(int id, Int64 nUserID); 
		List<ModelFeature> Gets(int id, Int64 nUserID);
		List<ModelFeature> Gets( string sSQL, Int64 nUserID);
		
	}
	#endregion
}
