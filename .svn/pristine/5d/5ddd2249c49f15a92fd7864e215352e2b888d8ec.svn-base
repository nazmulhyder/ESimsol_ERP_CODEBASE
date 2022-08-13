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
	#region VehicleModel  
	public class VehicleModel : BusinessObject
	{	
		public VehicleModel()
		{
			VehicleModelID = 0;
            ModelNo = "";
            ModelCode = "";
            ModelCategoryID = 0;
            ModelSessionID = 0;
            ModelShortName = "";
            SeatingCapacity = "";
            DriveType = 0;
            MinPrice = 0;
            MaxPrice = 0;
            Remarks = "";
            CategoryName = "";
            FileNo = "";
            VehicleModelImageID = 0;
            ExShowroomPriceBC= 0;
            OfferPrice = 0;
            VATPercentage = 0;
            ModelFeatures = new List<ModelFeature>();
            VehicleModelList = new List<VehicleModel>();
			ErrorMessage = "";
		}

		#region Property
        public int VehicleModelID { get; set; }
        public int ModelCategoryID { get; set; }
        public int ModelSessionID { get; set; }
        public int DriveType { get; set; }
		public string ModelNo { get; set; }
        public string ModelShortName { get; set; }
		public string Remarks { get; set; }
        public double MaxPrice { get; set; }
        public string CategoryName { get; set; }
        public string FileNo { get; set; }
        public double MinPrice { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string SeatingCapacity { get; set; }
        public string ModelSessionName { get; set; }
        public string CurrencySymbol { get; set; }
        public int VehicleModelImageID { get; set; }
        public double ExShowroomPriceBC { get; set; }
        public double OfferPrice { get; set; }
        public double VATPercentage { get; set; }
        public string EngineType { get; set; }
        public string MaxPowerOutput { get; set; }
        public string MaximumTorque { get; set; }
        public string Transmission { get; set; }
        public string DisplacementCC { get; set; }
        public string TopSpeed { get; set; }
        public string Acceleration { get; set; }
        public string ModelCode { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public int ProductNatureInInt { get; set; }
        public byte[] LargeImage { get; set; }
        public VehicleModelImage VehicleModelImage { get; set; }
        public string PriceRangeInString
        {
            get
            {
                return this.CurrencySymbol + " " + Global.MillionFormat(this.MinPrice) + " TO " + this.CurrencySymbol + " " + Global.MillionFormat(this.MaxPrice);
            }
        }
        public string DriveTypeInString
        {
            get
            {
                return Enum.GetName(typeof(EnumDriveType), this.DriveType).ToString();
            }
        }
        public List<ModelFeature> ModelFeatures { get; set; }
        public List<VehicleModel> VehicleModelList { get; set; }
		#endregion 

		#region Functions 
        public static List<VehicleModel> GetsByModelNo(string ModelNo, long nUserID)
		{
            return VehicleModel.Service.GetsByModelNo(ModelNo, nUserID);
		}
		public static List<VehicleModel> Gets(string sSQL, long nUserID)
		{
			return VehicleModel.Service.Gets(sSQL,nUserID);
		}
		public VehicleModel Get(int id, long nUserID)
		{
			return VehicleModel.Service.Get(id,nUserID);
		}
		public VehicleModel Save(long nUserID)
		{
			return VehicleModel.Service.Save(this,nUserID);
		}

        public VehicleModel Approve(long nUserID)
        {
            return VehicleModel.Service.Approve(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return VehicleModel.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IVehicleModelService Service
		{
			get { return (IVehicleModelService)Services.Factory.CreateService(typeof(IVehicleModelService)); }
		}
		#endregion


    }
	#endregion

	#region IVehicleModel interface
	public interface IVehicleModelService 
	{
		VehicleModel Get(int id, Int64 nUserID);
        List<VehicleModel> GetsByModelNo(string ModelNo, Int64 nUserID);
		List<VehicleModel> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		VehicleModel Save(VehicleModel oVehicleModel, Int64 nUserID);
        VehicleModel Approve(VehicleModel oVehicleModel, Int64 nUserID);
        
	}
	#endregion
}
