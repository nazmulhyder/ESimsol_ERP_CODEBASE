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
	#region ServiceInvoiceDetail  
	public class ServiceInvoiceDetail : BusinessObject
	{	
		public ServiceInvoiceDetail()
		{
			ServiceInvoiceDetailID = 0; 
            ServiceInvoiceID = 0; 
            VehiclePartsID = 0;
            MUnitID = 0;
            Qty = 0;
            UnitPrice = 0;
            Amount = 0;
            WorkChargeType = EnumServiceILaborChargeType.None;
            ServiceType = EnumServiceType.None;
            WorkDate = DateTime.Now;
            Remarks = "";
			ErrorMessage = "";
		}

		#region Property
		public int ServiceInvoiceDetailID { get; set; }
		public int ServiceInvoiceID { get; set; }
        public int ServiceInvoiceDetailLogID { get; set; }
        public int ServiceInvoiceLogID { get; set; }
        public int VehiclePartsID { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string PartsNo { get; set; }
        public string PartsName { get; set; }
        public string PartsCode { get; set; }
        public string MUName { get; set; }
        public DateTime WorkDate { get; set; }
        public string Remarks { get; set; }
        public EnumServiceILaborChargeType WorkChargeType { get; set; }
        public EnumServiceType ServiceType { get; set; }
		public string ErrorMessage { get; set; }
  
		#endregion 
        public string WorkChargeTypeSt
        {
            get
            {
                return EnumObject.jGet(this.WorkChargeType);
            }
        }
        public int WorkChargeTypeInt
        {
            get
            {
                return (int)this.WorkChargeType;
            }
        }
        public string WorkDateSt
        {
            get
            {
                return this.WorkDate.ToString("dd MMM yyyy");
            }
        }
		#region Functions 
		public static List<ServiceInvoiceDetail> Gets(int id, long nUserID)
		{
			return ServiceInvoiceDetail.Service.Gets(id, nUserID);
		}
        public static List<ServiceInvoiceDetail> GetsLog(int id, long nUserID)
        {
            return ServiceInvoiceDetail.Service.GetsLog(id, nUserID);
        }
		public static List<ServiceInvoiceDetail> Gets(string sSQL, long nUserID)
		{
			return ServiceInvoiceDetail.Service.Gets(sSQL,nUserID);
		}
		public ServiceInvoiceDetail Get(int id, long nUserID)
		{
			return ServiceInvoiceDetail.Service.Get(id,nUserID);
		}
		
		#endregion

		#region ServiceFactory
		internal static IServiceInvoiceDetailService Service
		{
			get { return (IServiceInvoiceDetailService)Services.Factory.CreateService(typeof(IServiceInvoiceDetailService)); }
		}
		#endregion

    }
	#endregion

	#region IServiceInvoiceDetail interface
	public interface IServiceInvoiceDetailService 
	{
		ServiceInvoiceDetail Get(int id, Int64 nUserID); 
		List<ServiceInvoiceDetail> Gets(int id, Int64 nUserID);
        List<ServiceInvoiceDetail> GetsLog(int id, Int64 nUserID);
		List<ServiceInvoiceDetail> Gets( string sSQL, Int64 nUserID);
	}
	#endregion
}
