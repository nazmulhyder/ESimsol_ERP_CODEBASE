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
	#region VehicleRegistration  
	public class VehicleRegistration : BusinessObject
	{	
		public VehicleRegistration()
		{
			VehicleRegistrationID=0;
            FileNo = "";
            VehicleRegNo = "";
            VehicleRegDate=DateTime.Now;
            VehicleTypeID=0;
            CustomerID=0;
            ContactPersonID=0;
            VehicleChassisID=0;
            VehicleEngineID=0;
            VehicleModelNo="";
            Remarks="";
            EngineNo="";
            ChassisNo="";
            VehicleTypeName="";
            CustomerName="";
            ContactPerson="";
            NoShowStatus = "1";
            ServicePlan = "1 Free Service";
            VehicleRegistrationType = EnumVehicleRegistrationType.Inhouse_Client;
			ErrorMessage = "";
            RemainingFreeService = "";
		}

		#region Property
		public int VehicleRegistrationID { get; set; }
        public int VehicleModelID { get; set; }
        public int VehicleTypeID { get; set; }
        public int CustomerID { get; set; }
        public int ContactPersonID { get; set; }
		public string FileNo { get; set; }
        public string VehicleRegNo { get; set; }
		public DateTime VehicleRegDate { get; set; }
        public int VehicleColorID { get; set; }
        public int VehicleEngineID { get; set; }
        public int VehicleChassisID { get; set; }
        public string ChassisNo { get; set; }
        public EnumVehicleRegistrationType VehicleRegistrationType { get; set; }
        public string EngineNo { get; set; }
		public string VehicleTypeName { get; set; }
		public string CustomerName { get; set; }
		public string ContactPerson { get; set; }
		public string VehicleModelNo { get; set; }
        public string ColorName { get; set; }
		public string Remarks { get; set; }
        public string DeliveryDate { get; set; }
        public string NoShowStatus { get; set; }
        public string ServicePlan { get; set; }
        public string RemainingFreeService { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string VehicleRegistrationTypeSt
        {
            get
            {
                return EnumObject.jGet(this.VehicleRegistrationType);
            }
        }
        public int VehicleRegistrationTypeInt
        {
            get
            {
                return (int)VehicleRegistrationType;
            }
        }
        public string VehicleRegDateSt
        {
            get
            {
                return this.VehicleRegDate.ToString("dd MMM yyyy");
            }
        }
    
		#endregion 

		#region Functions 
        public static List<VehicleRegistration> Gets(long nUserID)
        {
            return VehicleRegistration.Service.Gets(nUserID);
        }
		public static List<VehicleRegistration> Gets(string sSQL, long nUserID)
		{
			return VehicleRegistration.Service.Gets(sSQL,nUserID);
		}
		public VehicleRegistration Get(int id, long nUserID)
		{
			return VehicleRegistration.Service.Get(id,nUserID);
		}
		public VehicleRegistration Save(long nUserID)
		{
			return VehicleRegistration.Service.Save(this,nUserID);
		}

        public VehicleRegistration Approve(long nUserID)
        {
            return VehicleRegistration.Service.Approve(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return VehicleRegistration.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IVehicleRegistrationService Service
		{
			get { return (IVehicleRegistrationService)Services.Factory.CreateService(typeof(IVehicleRegistrationService)); }
		}
		#endregion
    }
	#endregion

	#region IVehicleRegistration interface
	public interface IVehicleRegistrationService 
	{
		VehicleRegistration Get(int id, Int64 nUserID);
		List<VehicleRegistration> Gets( string sSQL, Int64 nUserID);
        List<VehicleRegistration> Gets(Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		VehicleRegistration Save(VehicleRegistration oVehicleRegistration, Int64 nUserID);
        VehicleRegistration Approve(VehicleRegistration oVehicleRegistration, Int64 nUserID);
	}
	#endregion
}
