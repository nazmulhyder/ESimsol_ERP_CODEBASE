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
	#region DocPrintEngine  
	public class DocPrintEngine : BusinessObject
	{	
		public DocPrintEngine()
		{
			DocPrintEngineID = 0;
            LetterType = EnumDocumentPrintType.None; 
			PageSize = ""; 
			Margin = "";
            BUID = 0;
            LetterName = "";
            ModuleID = 0;
            ModuleType = EnumModuleName.None; 
			FontName = "";
            Activity = true;
			ErrorMessage = "";
            BusinessUnitName = "";
            DocPrintEngineDetails=new List<DocPrintEngineDetail>(); 
		}

		#region Property
		public int DocPrintEngineID { get; set; }
        public EnumDocumentPrintType LetterType { get; set; }
		public string PageSize { get; set; }
		public string Margin { get; set; }
		public string FontName { get; set; }
        public string BusinessUnitName { get; set; }
        public bool Activity { get; set; }
        public string LetterName { get; set; }
        public int ModuleID { get; set; }
        public int BUID { get; set; }
        public EnumModuleName ModuleType { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public List<DocPrintEngineDetail> DocPrintEngineDetails { get; set; }
        public string LetterTypeST { get { return EnumObject.jGet(this.LetterType); } }
        public string ModuleTypeST { get { return EnumObject.jGet(this.ModuleType); } }
        public int LetterTypeInt { get { return (int)this.LetterType; } }
        public string ActivityST { get { return (this.Activity ? "Active" : "InActive"); } }
		#endregion 

		#region Functions 
		public static List<DocPrintEngine> Gets(long nUserID)
		{
			return DocPrintEngine.Service.Gets(nUserID);
		}
		public static List<DocPrintEngine> Gets(string sSQL, long nUserID)
		{
			return DocPrintEngine.Service.Gets(sSQL,nUserID);
		}
		public DocPrintEngine Get(int id, long nUserID)
		{
			return DocPrintEngine.Service.Get(id,nUserID);
		}
		public DocPrintEngine Save(long nUserID)
		{
			return DocPrintEngine.Service.Save(this,nUserID);
		}
        public DocPrintEngine Copy(long nUserID)
        {
            return DocPrintEngine.Service.Copy(this, nUserID);
        }
		public  string  Delete(int id, long nUserID)
		{
			return DocPrintEngine.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IDocPrintEngineService Service
		{
			get { return (IDocPrintEngineService)Services.Factory.CreateService(typeof(IDocPrintEngineService)); }
		}
		#endregion

        public DocPrintEngine Update(int nDocPrintEngineID, long nUserID)
        {
            return DocPrintEngine.Service.Update(nDocPrintEngineID, nUserID);
        }
        public DocPrintEngine GetActiveByType(int type, long userID)
        {
            return DocPrintEngine.Service.GetActiveByType(type, userID);
        }
        public DocPrintEngine GetActiveByTypenModule(int type, int moduleID, long userID)
        {
            return DocPrintEngine.Service.GetActiveByTypenModule(type, moduleID,userID);
        }
    }
	#endregion

	#region IDocPrintEngine interface
	public interface IDocPrintEngineService 
	{
        DocPrintEngine Get(int id, Int64 nUserID);
        DocPrintEngine GetActiveByType(int type, Int64 nUserID);
        DocPrintEngine GetActiveByTypenModule(int type, int moduleID, Int64 nUserID); 
		List<DocPrintEngine> Gets(Int64 nUserID);
		List<DocPrintEngine> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		DocPrintEngine Save(DocPrintEngine oDocPrintEngine, Int64 nUserID);
        DocPrintEngine Copy(DocPrintEngine oDocPrintEngine, Int64 nUserID);
        DocPrintEngine Update(int nDocPrintEngineID, long nUserID);
	}
	#endregion
}
