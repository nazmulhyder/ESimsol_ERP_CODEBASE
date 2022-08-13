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
	#region FAAccountHead  
	public class FAAccountHead : BusinessObject
	{	
		public FAAccountHead()
		{
			FAAccountHeadID = 0; 
			FAAccountHeadName = "";
            FAAccountHeadType = EnumFAAccountHeadType.None;
            FAAccountHeadTypeInt = 0;
            ChartsOfAccountID = 0;
			ErrorMessage = "";
		}

		#region Property
		public int FAAccountHeadID { get; set; }
        public string FAAccountHeadName { get; set; }
        public EnumFAAccountHeadType FAAccountHeadType { get; set; }
        public int FAAccountHeadTypeInt { get; set; }
        public int ChartsOfAccountID { get; set; }
        public string CAHeadName { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string FAAccountTypeSt { get { return EnumObject.jGet(this.FAAccountHeadType);} }
		#endregion 

		#region Functions 
		public static List<FAAccountHead> Gets(long nUserID)
		{
			return FAAccountHead.Service.Gets(nUserID);
		}
		public static List<FAAccountHead> Gets(string sSQL, long nUserID)
		{
			return FAAccountHead.Service.Gets(sSQL,nUserID);
		}
		public FAAccountHead Get(int id, long nUserID)
		{
			return FAAccountHead.Service.Get(id,nUserID);
		}
		public FAAccountHead Save(long nUserID)
		{
			return FAAccountHead.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return FAAccountHead.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IFAAccountHeadService Service
		{
			get { return (IFAAccountHeadService)Services.Factory.CreateService(typeof(IFAAccountHeadService)); }
		}
		#endregion

    }
	#endregion

	#region IFAAccountHead interface
	public interface IFAAccountHeadService 
	{
		FAAccountHead Get(int id, Int64 nUserID); 
		List<FAAccountHead> Gets(Int64 nUserID);
		List<FAAccountHead> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		FAAccountHead Save(FAAccountHead oFAAccountHead, Int64 nUserID);
	}
	#endregion
}
