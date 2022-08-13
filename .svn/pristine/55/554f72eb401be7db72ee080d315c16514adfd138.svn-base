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
	#region TechnicalSheetShipment  
	public class TechnicalSheetShipment : BusinessObject
	{	
		public TechnicalSheetShipment()
		{
			TechnicalSheetShipmentID = 0; 
			TechnicalSheetID = 0; 
			DeliveryDate = ""; 
			StyleNo = ""; 
			Qty = 0; 
			Remarks = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int TechnicalSheetShipmentID { get; set; }
		public int TechnicalSheetID { get; set; }
		public string DeliveryDate { get; set; }
		public string StyleNo { get; set; }
		public double Qty { get; set; }
		public string Remarks { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
        public static List<TechnicalSheetShipment> Gets(int TechnicalSheetID, long nUserID)
		{
            return TechnicalSheetShipment.Service.Gets(TechnicalSheetID,nUserID);
		}
		public static List<TechnicalSheetShipment> Gets(string sSQL, long nUserID)
		{
			return TechnicalSheetShipment.Service.Gets(sSQL,nUserID);
		}

		
		#endregion

		#region ServiceFactory
		internal static ITechnicalSheetShipmentService Service
		{
			get { return (ITechnicalSheetShipmentService)Services.Factory.CreateService(typeof(ITechnicalSheetShipmentService)); }
		}
		#endregion
	}
	#endregion

	#region ITechnicalSheetShipment interface
	public interface ITechnicalSheetShipmentService 
	{
		List<TechnicalSheetShipment> Gets(int TechnicalSheetID, Int64 nUserID);
		List<TechnicalSheetShipment> Gets( string sSQL, Int64 nUserID);

	}
	#endregion
}
