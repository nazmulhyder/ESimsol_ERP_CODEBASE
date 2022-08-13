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
	#region FNRequisitionDetail  
	public class FNRequisitionDetail : BusinessObject
	{	
		public FNRequisitionDetail()
		{
			FNRDetailID = 0; 
			FNRID = 0; 
			ProductID = 0;
            LotID = 0;
            DestinationLotID = 0; 
			RequiredQty = 0; 
			DisburseQty = 0;
            Rate = 0;
			Remarks = ""; 
			ProductName = ""; 
			ProductCode = ""; 
			LotNo = "";
            LotBalance = 0;
            DestinationLotBalance = 0;
            MUName = "";
            FNRNo = "";
            FNRequisition = new BusinessObjects.FNRequisition();
            RequestDate = DateTime.Now;
			ErrorMessage = "";
            MeasurementUnitID = 0;
		}

		#region Property
		public int FNRDetailID { get; set; }
		public int FNRID { get; set; }
		public int ProductID { get; set; }
		public int LotID { get; set; }
        public int DestinationLotID { get; set; }
		public double RequiredQty { get; set; }
        public double DisburseQty { get; set; }
        public double Qty { get; set; }
		public string Remarks { get; set; }
		public string ProductName { get; set; }
		public string ProductCode { get; set; }
		public string LotNo { get; set; }
        public string DestinationLotNo { get; set; }
        public double LotBalance { get; set; }
        public double DestinationLotBalance { get; set; }
        public double Rate { get; set; }
        public string FNRNo { get; set; }
        public string MUName { get; set; }
        public DateTime RequestDate { get; set; }
		public string ErrorMessage { get; set; }
        public int MeasurementUnitID { get; set; }
		#endregion 

		#region Derived Property
        public string RequestDateInString
        {
            get
            {
                if (RequestDate == DateTime.MinValue) return "";
                return RequestDate.ToString("dd MMM yy");
            }
        }

        public double Amount
        {
            get
            {
                return this.DisburseQty * this.Rate;
            }
        }
        public FNRequisition FNRequisition { get; set; }
		#endregion 

		#region Functions 
		public static List<FNRequisitionDetail> Gets(int id, long nUserID)
		{
			return FNRequisitionDetail.Service.Gets(id, nUserID);
		}
		public static List<FNRequisitionDetail> Gets(string sSQL, long nUserID)
		{
			return FNRequisitionDetail.Service.Gets(sSQL,nUserID);
		}
		public FNRequisitionDetail Get(int id, long nUserID)
		{
			return FNRequisitionDetail.Service.Get(id,nUserID);
		}
		public FNRequisitionDetail Save(long nUserID)
		{
			return FNRequisitionDetail.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return FNRequisitionDetail.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFNRequisitionDetailService Service
		{
			get { return (IFNRequisitionDetailService)Services.Factory.CreateService(typeof(IFNRequisitionDetailService)); }
		}
		#endregion


    }
	#endregion

	#region IFNRequisitionDetail interface
	public interface IFNRequisitionDetailService 
	{
		FNRequisitionDetail Get(int id, Int64 nUserID); 
		List<FNRequisitionDetail> Gets(int id, Int64 nUserID);
		List<FNRequisitionDetail> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		FNRequisitionDetail Save(FNRequisitionDetail oFNRequisitionDetail, Int64 nUserID);
	}
	#endregion
}
