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
	#region LabdipChallan  
	public class LabdipChallan : BusinessObject
	{	
		public LabdipChallan()
		{
			LabdipChallanID = 0; 
			ChallanDate = DateTime.Now;
            ChallanNo = "";
            ChallanNoFull = ""; 
			ContractorID = 0; 
			DeliveryZoneID  = 0; 
			Status = EnumLabDipChallanStatus.None;
            ContractorName = "";
            Contractor_Address = "";
            PrepareBy = "";
            DeliveryZoneName = "";
            ColorCount = 0;
            Remarks = "";
            LabDipDetails = new List<LabDipDetail>();
            ErrorMessage = "";
		}

		#region Property
		public int LabdipChallanID { get; set; }
		public DateTime ChallanDate { get; set; }
		public string ChallanNo { get; set; }
		public int ContractorID { get; set; }
		public int DeliveryZoneID  { get; set; }
        public EnumLabDipChallanStatus Status { get; set; }
        public string ContractorName { get; set; }
        public string Contractor_Address { get; set; }
        public string PrepareBy { get; set; }
        public string DeliveryZoneName { get; set; }
        public string ChallanNoFull { get; set; }
        public int ColorCount { get; set; }
        public string Remarks { get; set; }
        public List<LabDipDetail> LabDipDetails { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		public string ChallanDateST 
		{
			get
			{
				return ChallanDate.ToString("dd MMM yyyy") ; 
			}
		}
        public string StatusST
        {
            get
            {
                return EnumObject.jGet(this.Status);
            }
        }
        public int StatusInt
        {
            get
            {
                return (int)Status;
            }
        }
		#endregion 

		#region Functions 
		public static List<LabdipChallan> Gets(long nUserID)
		{
			return LabdipChallan.Service.Gets(nUserID);
		}
		public static List<LabdipChallan> Gets(string sSQL, long nUserID)
		{
			return LabdipChallan.Service.Gets(sSQL,nUserID);
		}
		public LabdipChallan Get(int id, long nUserID)
		{
			return LabdipChallan.Service.Get(id,nUserID);
		}
        public LabdipChallan Save(long nUserID)
        {
            return LabdipChallan.Service.Save(this, nUserID);
        }
        public LabdipChallan UpdateStatus(long nUserID)
        {
            return LabdipChallan.Service.UpdateStatus(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return LabdipChallan.Service.Delete(id,nUserID);
		}
        public string RemoveDetail(int id, long nUserID)
        {
            return LabdipChallan.Service.RemoveDetail(id, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static ILabdipChallanService Service
		{
			get { return (ILabdipChallanService)Services.Factory.CreateService(typeof(ILabdipChallanService)); }
		}
		#endregion
    }
	#endregion

	#region ILabdipChallan interface
	public interface ILabdipChallanService 
	{
		LabdipChallan Get(int id, Int64 nUserID); 
		List<LabdipChallan> Gets(Int64 nUserID);
		List<LabdipChallan> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
        string RemoveDetail(int id, Int64 nUserID);
        LabdipChallan Save(LabdipChallan oLabdipChallan, Int64 nUserID);
        LabdipChallan UpdateStatus(LabdipChallan oLabdipChallan, Int64 nUserID);
	}
	#endregion
}
