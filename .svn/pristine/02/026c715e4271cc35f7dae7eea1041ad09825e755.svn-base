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
	#region PackingList  
	public class PackingList : BusinessObject
	{	
		public PackingList()
		{
			PackingListID = 0; 
			CIDID = 0; 
			UnitInPack = 0; 
			PackInCarton = 0; 
			QtyInCarton = 0; 
			CartonQty = 0; 
			CartonNo = ""; 
			GrossWeight = 0; 
			NetWeight = 0; 
			CTNMeasurement = ""; 
			TotalVolume = 0; 
			Note = ""; 
			TotalGrossWeight = 0; 
			TotalNetWeight = 0; 
			StyleNo = ""; 
			RecapNo = ""; 
			InvoiceNo = ""; 
			BuyerName = ""; 
			FactoryName = ""; 
			ProductName = ""; 
			Fabrication = ""; 
			TotalQty = 0;
            TechnicalSheetID = 0;
            OrderRecapID = 0;
            StyleWithRecapNo = "";
            InvoiceDate = DateTime.Now;
            PackingListDetails = new List<PackingListDetail>();
            PackingLists = new List<PackingList>();
            ColorSizeRatios = new List<ColorSizeRatio>();
            TechnicalSheetSizes = new List<TechnicalSheetSize>();
            ErrorMessage = "";
		}

		#region Property
		public int PackingListID { get; set; }
		public int CIDID { get; set; }
		public double UnitInPack { get; set; }
		public double PackInCarton { get; set; }
		public double QtyInCarton { get; set; }
		public double CartonQty { get; set; }
		public string CartonNo { get; set; }
		public double GrossWeight { get; set; }
		public double NetWeight { get; set; }
		public string CTNMeasurement { get; set; }
		public double TotalVolume { get; set; }
		public string Note { get; set; }
		public double TotalGrossWeight { get; set; }
		public double TotalNetWeight { get; set; }
		public string StyleNo { get; set; }
		public string RecapNo { get; set; }
		public string InvoiceNo { get; set; }
		public string BuyerName { get; set; }
		public string FactoryName { get; set; }
		public string ProductName { get; set; }
		public string Fabrication { get; set; }
		public double TotalQty { get; set; }
        public string StyleWithRecapNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int OrderRecapID { get; set; }
        public int ProductionFactoryID { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public int TechnicalSheetID { get; set; }
        public List<TechnicalSheetSize> TechnicalSheetSizes { get; set; }
        public List<PackingListDetail> PackingListDetails { get; set; }
        public List<PackingList> PackingLists { get; set; }
        public List<ColorSizeRatio> ColorSizeRatios { get; set; }
        public List<SizeCategory> SizeCategories { get; set; }
        public List<ColorCategory> ColorCategories { get; set; }
        public Company Company { get; set; }
        public string InvoiceDateInString
        {
            get
            {
                return this.InvoiceDate.ToString("dd MMM yyyy");
            }
        }
         
		#endregion 

		#region Functions 
		public static List<PackingList> Gets(long nUserID)
		{
			return PackingList.Service.Gets(nUserID);
		}
		public static List<PackingList> Gets(string sSQL, long nUserID)
		{
			return PackingList.Service.Gets(sSQL,nUserID);
		}
		public PackingList Get(int id, long nUserID)
		{
			return PackingList.Service.Get(id,nUserID);
		}
		public PackingList Save(long nUserID)
		{
			return PackingList.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return PackingList.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IPackingListService Service
		{
			get { return (IPackingListService)Services.Factory.CreateService(typeof(IPackingListService)); }
		}
		#endregion
	}
	#endregion

	#region IPackingList interface
	public interface IPackingListService 
	{
		PackingList Get(int id, Int64 nUserID); 
		List<PackingList> Gets(Int64 nUserID);
		List<PackingList> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		PackingList Save(PackingList oPackingList, Int64 nUserID);
	}
	#endregion
}
