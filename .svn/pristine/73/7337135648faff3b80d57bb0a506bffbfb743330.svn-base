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
	#region UploadConfigure  
	public class UploadConfigure : BusinessObject
	{	
		public UploadConfigure()
		{
			UploadConfigureID = 0; 
			UserID = 0; 
			FieldNames = ""; 
			CaptionNames = "";
            UploadType = EnumUploadType.EmployeeBasicUpload;
            UploadConfigures = new List<UploadConfigure>();
			ErrorMessage = "";
		}

		#region Property
		public int UploadConfigureID { get; set; }
		public int UserID { get; set; }
		public string FieldNames { get; set; }
		public string CaptionNames { get; set; }
        public EnumUploadType UploadType { get; set; }
        public int UploadTypeInInt { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public string TableCaption { get; set; }
        public string FieldName { get; set; }
        
        public List<UploadConfigure> UploadConfigures { get; set; }
		#endregion 

		#region Functions 
		public static List<UploadConfigure> Gets(long nUserID)
		{
			return UploadConfigure.Service.Gets(nUserID);
		}
		public static List<UploadConfigure> Gets(string sSQL, long nUserID)
		{
			return UploadConfigure.Service.Gets(sSQL,nUserID);
		}
		public UploadConfigure Get(int nUploadType, long nUserID)
		{
			return UploadConfigure.Service.Get(nUploadType,nUserID);
		}
		public UploadConfigure Save(long nUserID)
		{
			return UploadConfigure.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return UploadConfigure.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IUploadConfigureService Service
		{
			get { return (IUploadConfigureService)Services.Factory.CreateService(typeof(IUploadConfigureService)); }
		}
		#endregion
	}
	#endregion

	#region IUploadConfigure interface
	public interface IUploadConfigureService 
	{
        UploadConfigure Get(int nUploadType, Int64 nUserID); 
		List<UploadConfigure> Gets(Int64 nUserID);
		List<UploadConfigure> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		UploadConfigure Save(UploadConfigure oUploadConfigure, Int64 nUserID);
	}
	#endregion
}
