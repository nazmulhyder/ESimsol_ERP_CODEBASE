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
	#region DocPrintEngineDetail  
	public class DocPrintEngineDetail : BusinessObject
	{	
		public DocPrintEngineDetail()
		{
			DocPrintEngineDetailID = 0; 
			DocPrintEngineID = 0;
            SLNo = "";
			SetWidths = ""; 
			SetAligns = ""; 
			SetFields = ""; 
			FontSize = ""; 
			RowHeight = 0; 
			TableName = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int DocPrintEngineDetailID { get; set; }
		public int DocPrintEngineID { get; set; }
		public string SLNo { get; set; }
		public string SetWidths { get; set; }
		public string SetAligns { get; set; }
		public string SetFields { get; set; }
		public string FontSize { get; set; }
		public double RowHeight { get; set; }
		public string TableName { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Functions 
		public static List<DocPrintEngineDetail> Gets(long nUserID)
		{
			return DocPrintEngineDetail.Service.Gets(nUserID);
		}
		public static List<DocPrintEngineDetail> Gets(string sSQL, long nUserID)
		{
			return DocPrintEngineDetail.Service.Gets(sSQL,nUserID);
		}
		public DocPrintEngineDetail Get(int id, long nUserID)
		{
			return DocPrintEngineDetail.Service.Get(id,nUserID);
		}
		public DocPrintEngineDetail Save(long nUserID)
		{
			return DocPrintEngineDetail.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return DocPrintEngineDetail.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IDocPrintEngineDetailService Service
		{
			get { return (IDocPrintEngineDetailService)Services.Factory.CreateService(typeof(IDocPrintEngineDetailService)); }
		}
		#endregion

    }
	#endregion

	#region IDocPrintEngineDetail interface
	public interface IDocPrintEngineDetailService 
	{
		DocPrintEngineDetail Get(int id, Int64 nUserID); 
		List<DocPrintEngineDetail> Gets(Int64 nUserID);
		List<DocPrintEngineDetail> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		DocPrintEngineDetail Save(DocPrintEngineDetail oDocPrintEngineDetail, Int64 nUserID);
	}
	#endregion
}
