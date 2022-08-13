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
	#region DUPSLot  
	public class DUPSLot : BusinessObject
	{	
		public DUPSLot()
		{
            DUPScheduleID = 0;
            DUPScheduleDetailID = 0;
            LotID = 0;
            DODID = 0; 
			Qty = 0;
            Balance = 0;
            LotNo = "";
            ErrorMessage = "";
		}

		#region Property
        public int DUPSLotID { get; set; }
        public int DUPScheduleID { get; set; }
        public int DUPScheduleDetailID { get; set; }
        public int LotID { get; set; }
        public double Qty { get; set; }
        public double Balance { get; set; }
        public string LotNo { get; set; }
        public int DODID { get; set; }
        public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
		#endregion 

		#region Functions 
		public static List<DUPSLot> Gets(long nUserID)
		{
			return DUPSLot.Service.Gets(nUserID);
		}

        public static List<DUPSLot> GetsBy(int nDUPScheduleID, long nUserID)
		{
            return DUPSLot.Service.GetsBy(nDUPScheduleID, nUserID);
		}
        public static List<DUPSLot> Gets(string sSQL, long nUserID)
		{
			return DUPSLot.Service.Gets(sSQL,nUserID);
		}
		public DUPSLot Get(int id, long nUserID)
		{
			return DUPSLot.Service.Get(id,nUserID);
		}
		public DUPSLot Save(long nUserID)
		{
			return DUPSLot.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return DUPSLot.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IDUPSLotService Service
		{
			get { return (IDUPSLotService)Services.Factory.CreateService(typeof(IDUPSLotService)); }
		}
		#endregion


    }
	#endregion

	#region IDUPSLot interface
	public interface IDUPSLotService 
	{
		DUPSLot Get(int id, Int64 nUserID); 
		List<DUPSLot> Gets(Int64 nUserID);
		List<DUPSLot> Gets( string sSQL, Int64 nUserID);
        List<DUPSLot> GetsBy(int nDUPScheduleID, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		DUPSLot Save(DUPSLot oDUPSLot, Int64 nUserID);
	}
	#endregion
}
