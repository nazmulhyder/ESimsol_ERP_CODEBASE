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
	#region PAM  
	public class PAM : BusinessObject
	{	
		public PAM()
		{
			PAMID = 0; 
			PAMNo = ""; 
			StyleID = 0; 
			ForwardWeek = ""; 
			Remarks = ""; 
			StyleNo = ""; 
			BuyerName = "";
            SessionName = ""; 
			ProductName = ""; 
			FabricName = "";
            ApprovedBy = 0;
            ApprovedByName = "";
            YetToRecapQty = 0;
            TotalQuantity = 0;
            UnitSymbol = "";
            ApprovedDate = DateTime.MinValue;
            PAMDetailLst = new List<PAMDetail>();
			ErrorMessage = "";
            MeasurementUnitID = 0;
		}

		#region Property
		public int PAMID { get; set; }
		public string PAMNo { get; set; }
		public int StyleID { get; set; }
		public string ForwardWeek { get; set; }
		public string Remarks { get; set; }
		public string StyleNo { get; set; }
		public string BuyerName { get; set; }
        public string SessionName { get; set; }
		public string ProductName { get; set; }
		public string FabricName { get; set; }
        public double YetToRecapQty { get; set; }
        public int  ApprovedBy { get; set; }
        public string     ApprovedByName { get; set; }
         public DateTime   ApprovedDate { get; set; }
         public double TotalQuantity { get; set; }
         public string UnitSymbol { get; set; }
         public int MeasurementUnitID { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public List<PAMDetail> PAMDetailLst { get; set; }
        public string ApprovedDateSt
        {
            get
            {
                return this.ApprovedDate.ToString("dd MMM yyyy");
            }
        }
        public string TotalQuantitySt
        {
            get
            {
                return Global.MillionFormat(this.TotalQuantity);
            }
        }
		#endregion 

		#region Functions 
		public static List<PAM> Gets(int nStyleID, long nUserID)
		{
			return PAM.Service.Gets(nStyleID, nUserID);
		}
		public static List<PAM> Gets(string sSQL, long nUserID)
		{
			return PAM.Service.Gets(sSQL,nUserID);
		}
		public PAM Get(int id, long nUserID)
		{
			return PAM.Service.Get(id,nUserID);
		}
		public PAM Save(long nUserID)
		{
			return PAM.Service.Save(this,nUserID);
		}
        public string SaveMultiPAM(long nUserID)
        {
            return PAM.Service.SaveMultiPAM(this, nUserID);
        }
        public PAM Revise(long nUserID)
        {
            return PAM.Service.Revise(this, nUserID);
        }
        public PAM Approve(long nUserID)
        {
            return PAM.Service.Approve(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return PAM.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IPAMService Service
		{
			get { return (IPAMService)Services.Factory.CreateService(typeof(IPAMService)); }
		}
		#endregion
	}
	#endregion

	#region IPAM interface
	public interface IPAMService 
	{
		PAM Get(int id, Int64 nUserID); 
		List<PAM> Gets(int nStyleID, Int64 nUserID);
		List<PAM> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		PAM Save(PAM oPAM, Int64 nUserID);
        string SaveMultiPAM(PAM oPAM, Int64 nUserID);
        PAM Approve(PAM oPAM, Int64 nUserID);
        PAM Revise(PAM oPAM, Int64 nUserID);
        
	}
	#endregion
}
