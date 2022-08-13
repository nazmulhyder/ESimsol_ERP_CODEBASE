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
	#region SalesQuotationDetail  
	public class SalesQuotationDetail : BusinessObject
	{	
		public SalesQuotationDetail()
		{
			SalesQuotationDetailID = 0; 
			SalesQuotationID = 0;             
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
		public int SalesQuotationDetailID { get; set; }
		public int SalesQuotationID { get; set; }
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
		public static List<SalesQuotationDetail> Gets(int id, long nUserID)
		{
			return SalesQuotationDetail.Service.Gets(id, nUserID);
		}
		public static List<SalesQuotationDetail> Gets(string sSQL, long nUserID)
		{
			return SalesQuotationDetail.Service.Gets(sSQL,nUserID);
		}
		public SalesQuotationDetail Get(int id, long nUserID)
		{
			return SalesQuotationDetail.Service.Get(id,nUserID);
		}
		
		#endregion

		#region ServiceFactory
		internal static ISalesQuotationDetailService Service
		{
			get { return (ISalesQuotationDetailService)Services.Factory.CreateService(typeof(ISalesQuotationDetailService)); }
		}
		#endregion
	}
	#endregion

	#region ISalesQuotationDetail interface
	public interface ISalesQuotationDetailService 
	{
		SalesQuotationDetail Get(int id, Int64 nUserID); 
		List<SalesQuotationDetail> Gets(int id, Int64 nUserID);
		List<SalesQuotationDetail> Gets( string sSQL, Int64 nUserID);
		
	}
	#endregion
}
