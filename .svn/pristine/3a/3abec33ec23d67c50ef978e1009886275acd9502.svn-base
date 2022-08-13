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
	#region FADepreciationDetail  
	public class FADepreciationDetail : BusinessObject
	{	
		public FADepreciationDetail()
		{
			FADepreciationDetailID = 0; 
			FADepreciationID = 0; 
			FAScheduleID = 0; 
			FARegisterID = 0; 
			DepreciationAmount = 0; 
			StartDate = DateTime.Now; 
			EndDate = DateTime.Now; 
			DepreciationRate = 0; 
			FAMethod = EnumFAMethod.None; 
			FACodeFull = ""; 
			ProductCategoryName = ""; 
			ProductCode = ""; 
			ProductName = ""; 
			CurrencySymbol = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int FADepreciationDetailID { get; set; }
		public int FADepreciationID { get; set; }
		public int FAScheduleID { get; set; }
		public int FARegisterID { get; set; }
		public double DepreciationAmount { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public double DepreciationRate { get; set; }
        public EnumFAMethod FAMethod { get; set; }
		public string FACodeFull { get; set; }
		public string ProductCategoryName { get; set; }
		public string ProductCode { get; set; }
		public string ProductName { get; set; }
		public string CurrencySymbol { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		public string StartDateInString 
		{
			get
			{
				return StartDate.ToString("dd MMM yyyy") ; 
			}
		}
		public string EndDateInString 
		{
			get
			{
				return EndDate.ToString("dd MMM yyyy") ; 
			}
		}
        public string DepreciationRateSt
        {
            get
            {
                return Global.MillionFormat(this.DepreciationRate);
            }
        }
        public string DepreciationAmountSt
        {
            get
            {
                return this.CurrencySymbol+" "+Global.MillionFormat(this.DepreciationAmount);
            }
        }
        public string FAMethodSt
        {
            get
            {
                return EnumObject.jGet(this.FAMethod);
            }
        }
		#endregion 

		#region Functions 
		public static List<FADepreciationDetail> Gets(int id, long nUserID)
		{
			return FADepreciationDetail.Service.Gets(id, nUserID);
		}
		public static List<FADepreciationDetail> Gets(string sSQL, long nUserID)
		{
			return FADepreciationDetail.Service.Gets(sSQL,nUserID);
		}
		public FADepreciationDetail Get(int id, long nUserID)
		{
			return FADepreciationDetail.Service.Get(id,nUserID);
		}
	
		#endregion

		#region ServiceFactory
		internal static IFADepreciationDetailService Service
		{
			get { return (IFADepreciationDetailService)Services.Factory.CreateService(typeof(IFADepreciationDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IFADepreciationDetail interface
	public interface IFADepreciationDetailService 
	{
		FADepreciationDetail Get(int id, Int64 nUserID); 
		List<FADepreciationDetail> Gets(int id, Int64 nUserID);
		List<FADepreciationDetail> Gets( string sSQL, Int64 nUserID);
		
	}
	#endregion
}
