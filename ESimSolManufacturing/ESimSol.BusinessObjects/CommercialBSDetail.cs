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
	#region CommercialBSDetail  
	public class CommercialBSDetail : BusinessObject
	{	
		public CommercialBSDetail()
		{
			CommercialBSDetailID = 0; 
			CommercialBSID = 0; 
			CommercialInvoiceID = 0; 
			InvoiceAmount = 0; 
			Remarks = ""; 
			InvoiceNo = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int CommercialBSDetailID { get; set; }
		public int CommercialBSID { get; set; }
		public int CommercialInvoiceID { get; set; }
		public double InvoiceAmount { get; set; }
		public string Remarks { get; set; }
		public string InvoiceNo { get; set; }

        public double InvoiceQty { get; set; }
        public EnumTransportType ShipmentMode { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string InvoiceQtyST
        {
            get
            {
                return Global.MillionFormat(this.InvoiceQty, 0);
            }
        }
        public string ShipmentModeST
        {
            get
            {
                return EnumObject.jGet(this.ShipmentMode);
            }
        }
		#endregion 

		#region Functions 
		public static List<CommercialBSDetail> Gets(int CommercialBSID, long nUserID)
		{
            return CommercialBSDetail.Service.Gets(CommercialBSID, nUserID);
		}
		public static List<CommercialBSDetail> Gets(string sSQL, long nUserID)
		{
			return CommercialBSDetail.Service.Gets(sSQL,nUserID);
		}
		public CommercialBSDetail Get(int id, long nUserID)
		{
			return CommercialBSDetail.Service.Get(id,nUserID);
		}
		
		#endregion

		#region ServiceFactory
		internal static ICommercialBSDetailService Service
		{
			get { return (ICommercialBSDetailService)Services.Factory.CreateService(typeof(ICommercialBSDetailService)); }
		}
		#endregion
	}
	#endregion

	#region ICommercialBSDetail interface
	public interface ICommercialBSDetailService 
	{
		CommercialBSDetail Get(int id, Int64 nUserID); 
		List<CommercialBSDetail> Gets(int CommercialBSID,  Int64 nUserID);
		List<CommercialBSDetail> Gets( string sSQL, Int64 nUserID);
		
	}
	#endregion
}
