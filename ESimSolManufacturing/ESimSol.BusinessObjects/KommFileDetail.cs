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
	#region KommFileDetail  
	public class KommFileDetail : BusinessObject
	{	
		public KommFileDetail()
		{
			KommFileDetailID = 0; 
			KommFileID = 0;             
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
		public int KommFileDetailID { get; set; }
		public int KommFileID { get; set; }
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
		public static List<KommFileDetail> Gets(int id, long nUserID)
		{
			return KommFileDetail.Service.Gets(id, nUserID);
		}
		public static List<KommFileDetail> Gets(string sSQL, long nUserID)
		{
			return KommFileDetail.Service.Gets(sSQL,nUserID);
		}
		public KommFileDetail Get(int id, long nUserID)
		{
			return KommFileDetail.Service.Get(id,nUserID);
		}
		
		#endregion

		#region ServiceFactory
		internal static IKommFileDetailService Service
		{
			get { return (IKommFileDetailService)Services.Factory.CreateService(typeof(IKommFileDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IKommFileDetail interface
	public interface IKommFileDetailService 
	{
		KommFileDetail Get(int id, Int64 nUserID); 
		List<KommFileDetail> Gets(int id, Int64 nUserID);
		List<KommFileDetail> Gets( string sSQL, Int64 nUserID);
		
	}
	#endregion
}
