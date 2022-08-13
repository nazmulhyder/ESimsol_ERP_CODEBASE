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
	#region CapacityAllocation  
	public class CapacityAllocation : BusinessObject
	{	
		public CapacityAllocation()
		{
			CapacityAllocationID = 0; 
			Code = ""; 
			BuyerID = 0; 
			Quantity = 0; 
			MUnitID = 0; 
			Remarks = ""; 
			BuyerName = ""; 
			MUName = "";
            ShipmentDate = DateTime.Today;
            OrderQty = 0;
            OrderValue = 0;
            MonthWiseCapacity = 0;
            MonthWiseBooking = 0;
            MonthWiseFreeCapacity = 0;
            MonthWiseValue = 0;
			ErrorMessage = "";
            Param = "";
            MonthWiseDetailsAllocations = new List<CapacityAllocation>();
		}

		#region Property
		public int CapacityAllocationID { get; set; }
		public string Code { get; set; }
		public int BuyerID { get; set; }
		public double Quantity { get; set; }
		public int MUnitID { get; set; }
		public string Remarks { get; set; }
		public string BuyerName { get; set; }
		public string MUName { get; set; }
        public DateTime  ShipmentDate { get; set; }
        public double  OrderQty { get; set; }
        public double OrderValue { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public double MonthWiseCapacity { get; set; }
        public double MonthWiseBooking { get; set; }
        public double MonthWiseFreeCapacity { get; set; }
        public double MonthWiseValue { get; set; }
        public string Param { get; set; }
        public List<CapacityAllocation> CapacityAllocations { get; set; }
        public List<CapacityAllocation> MonthWiseDetailsAllocations { get; set; }
        public string ShipmentDateSt
        {
            get
            {
                return this.ShipmentDate.ToString("MMM-yy");
            }
        }

        public string OrderValueSt
        {
            get
            {
                return "$ "+Global.MillionFormat(this.OrderValue);
            }
        }
        public string QuantitySt
        {
            get
            {
                return Global.MillionFormat(this.Quantity) + " " + this.MUName;
            }
        }
        public string BuyerIDWithDateInString
        {
            get
            {
                return this.BuyerID + "~" + this.ShipmentDate.ToString("dd MMM yyyy");
            }
        }
		#endregion 

		#region Functions 
		public static List<CapacityAllocation> Gets(long nUserID)
		{
			return CapacityAllocation.Service.Gets(nUserID);
		}
		public static List<CapacityAllocation> Gets(string sSQL, long nUserID)
		{
			return CapacityAllocation.Service.Gets(sSQL,nUserID);
		}
		public CapacityAllocation Get(int id, long nUserID)
		{
			return CapacityAllocation.Service.Get(id,nUserID);
		}
		public CapacityAllocation Save(long nUserID)
		{
			return CapacityAllocation.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return CapacityAllocation.Service.Delete(id,nUserID);
		}

        public static List<CapacityAllocation> GetsBookingStatus(DateTime dStartDate,  DateTime dEndDate, long nUserID)
        {
            return CapacityAllocation.Service.GetsBookingStatus(dStartDate, dEndDate, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static ICapacityAllocationService Service
		{
			get { return (ICapacityAllocationService)Services.Factory.CreateService(typeof(ICapacityAllocationService)); }
		}
		#endregion
	}
	#endregion

	#region ICapacityAllocation interface
	public interface ICapacityAllocationService 
	{
		CapacityAllocation Get(int id, Int64 nUserID); 
		List<CapacityAllocation> Gets(Int64 nUserID);
        List<CapacityAllocation> GetsBookingStatus(DateTime dStartDate, DateTime dEndDate, Int64 nUserID);
		List<CapacityAllocation> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		CapacityAllocation Save(CapacityAllocation oCapacityAllocation, Int64 nUserID);
	}
	#endregion
}
