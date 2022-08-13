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
	#region ServiceILaborChargeDetail  
	public class ServiceILaborChargeDetail : BusinessObject
	{	
		public ServiceILaborChargeDetail()
		{
			ServiceILaborChargeDetailID = 0; 
            ServiceInvoiceID = 0;
            ServiceWorkID = 0;
            ServiceName = "";
            ServiceCode = "";
            ChargeAmount = 0;
            WorkingCost = 0;
            WorkingHour = 0;
            LaborChargeType = EnumServiceILaborChargeType.None;
            Remarks = "";
			ErrorMessage = "";
		}

		#region Property
		public int ServiceILaborChargeDetailID { get; set; }
		public int ServiceInvoiceID { get; set; }
        public int ServiceILaborChargeDetailLogID { get; set; }
        public int ServiceInvoiceLogID { get; set; }
		public int ServiceWorkID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public double WorkingHour { get; set; }
        public double WorkingCost { get; set; }
        public double ChargeAmount { get; set; }
        public string ChargeDescription { get; set; }
        public string Remarks { get; set; }
        public EnumServiceILaborChargeType LaborChargeType { get; set; }
		public string ErrorMessage { get; set; }
  
		#endregion 

        #region Dereived
        public string LaborChargeTypeSt
        {
            get
            {
                return EnumObject.jGet(this.LaborChargeType);
            }
        }
        public int ServiceILaborChargeTypeInt
        {
            get
            {
                return (int)LaborChargeType;
            }
        }
        #endregion

        #region Functions
        public static List<ServiceILaborChargeDetail> Gets(int id, long nUserID)
		{
			return ServiceILaborChargeDetail.Service.Gets(id, nUserID);
		}
        public static List<ServiceILaborChargeDetail> GetsLog(int id, long nUserID)
        {
            return ServiceILaborChargeDetail.Service.GetsLog(id, nUserID);
        }
		public static List<ServiceILaborChargeDetail> Gets(string sSQL, long nUserID)
		{
			return ServiceILaborChargeDetail.Service.Gets(sSQL,nUserID);
		}
		public ServiceILaborChargeDetail Get(int id, long nUserID)
		{
			return ServiceILaborChargeDetail.Service.Get(id,nUserID);
		}
		
		#endregion

		#region ServiceFactory
		internal static IServiceILaborChargeDetailService Service
		{
			get { return (IServiceILaborChargeDetailService)Services.Factory.CreateService(typeof(IServiceILaborChargeDetailService)); }
		}
		#endregion
	}
	#endregion

	#region IServiceILaborChargeDetail interface
	public interface IServiceILaborChargeDetailService 
	{
		ServiceILaborChargeDetail Get(int id, Int64 nUserID); 
		List<ServiceILaborChargeDetail> Gets(int id, Int64 nUserID);
        List<ServiceILaborChargeDetail> GetsLog(int id, Int64 nUserID);
		List<ServiceILaborChargeDetail> Gets( string sSQL, Int64 nUserID);
	}
	#endregion
}
