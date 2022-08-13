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
	#region SampleAdjustmentDetail  
	public class SampleAdjustmentDetail : BusinessObject
	{	
		public SampleAdjustmentDetail()
		{
			SampleAdjustmentID = 0; 
			SampleAdjustmentDetailID = 0; 
			SampleInvoiceID = 0; 
			AdjustAmount = 0;
			Remarks = ""; 
			InvoiceNo = "";
            SampleInvoiceDate = DateTime.Now;
            RefAmount = 0;
			ErrorMessage = "";
		}

		#region Property
		public int SampleAdjustmentID { get; set; }
		public int SampleAdjustmentDetailID { get; set; }
		public int SampleInvoiceID { get; set; }
		public double AdjustAmount { get; set; }
		public string Remarks { get; set; }
		public string InvoiceNo { get; set; }
        public DateTime SampleInvoiceDate { get; set; }
        public double RefAmount { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string SampleInvoiceDateSt
        {
            get
            {
                return this.SampleInvoiceDate.ToString("dd MMM yyyy");
            }
        }
		#endregion 

		#region Functions 
		public static List<SampleAdjustmentDetail> Gets(int nID, long nUserID)
		{
			return SampleAdjustmentDetail.Service.Gets(nID, nUserID);
		}
		public static List<SampleAdjustmentDetail> Gets(string sSQL, long nUserID)
		{
			return SampleAdjustmentDetail.Service.Gets(sSQL,nUserID);
		}
		public SampleAdjustmentDetail Get(int id, long nUserID)
		{
			return SampleAdjustmentDetail.Service.Get(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static ISampleAdjustmentDetailService Service
		{
			get { return (ISampleAdjustmentDetailService)Services.Factory.CreateService(typeof(ISampleAdjustmentDetailService)); }
		}
		#endregion
	}
	#endregion

	#region ISampleAdjustmentDetail interface
	public interface ISampleAdjustmentDetailService 
	{
		SampleAdjustmentDetail Get(int id, Int64 nUserID); 
		List<SampleAdjustmentDetail> Gets(int nID, Int64 nUserID);
		List<SampleAdjustmentDetail> Gets( string sSQL, Int64 nUserID);
	
	}
	#endregion
}
