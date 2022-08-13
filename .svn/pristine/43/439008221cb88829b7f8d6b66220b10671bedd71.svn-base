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
	#region FARegisterSummery  
	public class FARegisterSummery : BusinessObject
	{	
		public FARegisterSummery()
		{
			ProductID = 0; 
			ProductCategoryID = 0; 
			ProductName = ""; 
			CategoryName = ""; 
			DeprRate = 0; 
			SubGroupHeadID = 0; 
			AssetOpeningAmount = 0; 
			AssetAdditionAmount = 0; 
			TotalAssetAmount = 0; 
			DeprOpeningAmount = 0; 
			DeprAdditionAmount = 0; 
			TotalDeprAmount = 0; 
			EndingAssetAmount = 0;
            Param = "";
            ReportViewLayout = 0;
			ErrorMessage = "";
		}

		#region Property
		public int ProductID { get; set; }
		public int ProductCategoryID { get; set; }
		public string ProductName { get; set; }
		public string CategoryName { get; set; }
		public double DeprRate { get; set; }
		public int SubGroupHeadID { get; set; }
		public double AssetOpeningAmount { get; set; }
		public double AssetAdditionAmount { get; set; }
		public double TotalAssetAmount { get; set; }
		public double DeprOpeningAmount { get; set; }
		public double DeprAdditionAmount { get; set; }
		public double TotalDeprAmount { get; set; }
		public double EndingAssetAmount { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public int ReportViewLayout { get; set; }
        public string Param { get; set; }
        public string PartiCularNameInString
        {
            get
            {
                if (this.ReportViewLayout == 1)//category wise
                {
                    return this.ProductCategoryID + "~" + this.CategoryName;
                }
                else
                {
                    return  "0~" + this.ProductName;
                }
            }
        }
		#endregion 

		#region Functions 
		public static List<FARegisterSummery> Gets(string BUIDs, DateTime StartDate, DateTime EndDate, int ProductCategoryID, int ReportLayout, long nUserID)
		{
			return FARegisterSummery.Service.Gets(BUIDs,StartDate, EndDate, ProductCategoryID, ReportLayout, nUserID);
		}
		public static List<FARegisterSummery> Gets(string sSQL, long nUserID)
		{
			return FARegisterSummery.Service.Gets(sSQL,nUserID);
		}
	
		#endregion

		#region ServiceFactory
		internal static IFARegisterSummeryService Service
		{
			get { return (IFARegisterSummeryService)Services.Factory.CreateService(typeof(IFARegisterSummeryService)); }
		}
		#endregion
	}
	#endregion

	#region IFARegisterSummery interface
	public interface IFARegisterSummeryService 
	{

        List<FARegisterSummery> Gets(string BUIDs, DateTime StartDate, DateTime EndDate, int ProductCategoryID, int ReportLayout, Int64 nUserID);
		List<FARegisterSummery> Gets( string sSQL, Int64 nUserID);

	}
	#endregion
}
