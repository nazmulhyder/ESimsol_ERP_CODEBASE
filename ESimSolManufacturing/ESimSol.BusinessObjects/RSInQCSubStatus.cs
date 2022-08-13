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
	#region RSInQCSubStatus  
	public class RSInQCSubStatus : BusinessObject
	{	
		public RSInQCSubStatus()
		{
			RouteSheetID = 0;
            RSSubStatus = EnumRSSubStates.None;
            Note = "";
            Param = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int RouteSheetID { get; set; }
        public EnumRSSubStates RSSubStatus { get; set; }
        public int RSSubStatusInt { get; set; }
        public string Note { get; set; }
        public string UpdateByName { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string Param { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string RSSubStateStr
        {
            get
            {
                return EnumObject.jGet(this.RSSubStatus);
            }
        }
        public string LastUpdateDateTimeStr
        {
            get
            {
                return this.LastUpdateDateTime.ToString("dd MMM yyyy hh:mm tt");
            }
        }
		#endregion 

		#region Functions 
		public static List<RSInQCSubStatus> Gets(long nUserID)
		{
			return RSInQCSubStatus.Service.Gets(nUserID);
		}
		public static List<RSInQCSubStatus> Gets(string sSQL, long nUserID)
		{
			return RSInQCSubStatus.Service.Gets(sSQL,nUserID);
		}
		public RSInQCSubStatus Get(int id, long nUserID)
		{
			return RSInQCSubStatus.Service.Get(id,nUserID);
		}
		public RSInQCSubStatus Save(long nUserID)
		{
			return RSInQCSubStatus.Service.Save(this,nUserID);
		}
		public  string  Delete(long nUserID)
		{
			return RSInQCSubStatus.Service.Delete(this,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IRSInQCSubStatusService Service
		{
			get { return (IRSInQCSubStatusService)Services.Factory.CreateService(typeof(IRSInQCSubStatusService)); }
		}
		#endregion
	}
	#endregion

	#region IRSInQCSubStatus interface
	public interface IRSInQCSubStatusService 
	{
		RSInQCSubStatus Get(int id, Int64 nUserID); 
		List<RSInQCSubStatus> Gets(Int64 nUserID);
		List<RSInQCSubStatus> Gets( string sSQL, Int64 nUserID);
        string Delete(RSInQCSubStatus oRSInQCSubStatus, Int64 nUserID);
 		RSInQCSubStatus Save(RSInQCSubStatus oRSInQCSubStatus, Int64 nUserID);
	}
	#endregion
}
