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
	#region FNProductionBatchQuality  
	public class FNProductionBatchQuality : BusinessObject
	{	
		public FNProductionBatchQuality()
		{
			FNPBQualityID = 0; 
			FNPBatchID = 0; 
			IsOk = true; 
			Remark = ""; 
			BatchNo = ""; 
			FNExONo = "";
            StartDateTime = DateTime.Now;
            EndDateTime = DateTime.Now; 
			StartBatcherName = ""; 
			EndBatcherName = ""; 
			StartQty = 0; 
			EndQty = 0; 
			StartWidth = 0; 
			EndWidth = 0;
            QCStatus = EnumQCStatus.Yet_To_QC;
            FNProductionBatch = new FNProductionBatch();
			ErrorMessage = "";
		}

		#region Property
		public int FNPBQualityID { get; set; }
		public int FNPBatchID { get; set; }
		public bool IsOk { get; set; }
		public string Remark { get; set; }
		public string BatchNo { get; set; }
		public string FNExONo { get; set; }
		public string StartBatcherName { get; set; }
		public string EndBatcherName { get; set; }
		public double StartQty { get; set; }
		public double EndQty { get; set; }
		public double StartWidth { get; set; }
		public double EndWidth { get; set; }
        public FNProductionBatch FNProductionBatch { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public EnumQCStatus QCStatus { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string StartDateTimeSt
        {
            get
            {
                return this.StartDateTime.ToString("dd MMM yyyy hh:mm:ss tt");
            }
        }
        public string EndDateTimeSt
        {
            get
            {
                    return this.EndDateTime.ToString("dd MMM yyyy hh:mm:ss tt");
            }
        }
        public string QCStatusStr { get { return EnumObject.jGet(this.QCStatus); } }
		#endregion 

		#region Functions 
		public static List<FNProductionBatchQuality> Gets(long nUserID)
		{
			return FNProductionBatchQuality.Service.Gets(nUserID);
		}
		public static List<FNProductionBatchQuality> Gets(string sSQL, long nUserID)
		{
			return FNProductionBatchQuality.Service.Gets(sSQL,nUserID);
		}
		public FNProductionBatchQuality Get(int id, long nUserID)
		{
			return FNProductionBatchQuality.Service.Get(id,nUserID);
		}
		public FNProductionBatchQuality Save(long nUserID)
		{
			return FNProductionBatchQuality.Service.Save(this,nUserID);
		}
        public FNProductionBatchQuality Reprocess(int nUserID)
        {
            return FNProductionBatchQuality.Service.Reprocess(this, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IFNProductionBatchQualityService Service
		{
			get { return (IFNProductionBatchQualityService)Services.Factory.CreateService(typeof(IFNProductionBatchQualityService)); }
		}
		#endregion

    }
	#endregion

	#region IFNProductionBatchQuality interface
	public interface IFNProductionBatchQualityService 
	{
		FNProductionBatchQuality Get(int id, Int64 nUserID); 
		List<FNProductionBatchQuality> Gets(Int64 nUserID);
		List<FNProductionBatchQuality> Gets( string sSQL, Int64 nUserID);
        FNProductionBatchQuality Save(FNProductionBatchQuality oFNProductionBatchQuality, Int64 nUserID);
        FNProductionBatchQuality Reprocess(FNProductionBatchQuality oFNProductionBatchQuality, Int64 nUserID);
	}
	#endregion
}
