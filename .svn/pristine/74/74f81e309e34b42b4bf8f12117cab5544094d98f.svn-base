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
	#region AttachDocument  
	public class AttachDocument : BusinessObject
	{	
		public AttachDocument()
		{
			AttachDocumentID = 0; 
			RefID = 0; 
			FileName = ""; 
			AttachFile = null; 
			FileType = ""; 
			Remarks = ""; 
			ArticleNo = "";
            RefType = EnumAttachRefType.None;
            RefTypeInInt = 0;
            AttachDocuments = new List<AttachDocument>();
			ErrorMessage = "";
		}

		#region Property
		public int AttachDocumentID { get; set; }
		public int RefID { get; set; }
        public EnumAttachRefType RefType { get; set; }
		public string FileName { get; set; }
		public byte[] AttachFile { get; set; }
		public string FileType { get; set; }
		public string Remarks { get; set; }
		public string ArticleNo { get; set; }
        public int RefTypeInInt { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public List<AttachDocument> AttachDocuments { get; set; }
        public string AttatchFileinString
        {
            get
            {
                return AttachDocumentID.ToString();
            }
        }
		#endregion 

		#region Functions 
		public static List<AttachDocument> Gets(long nUserID)
		{
			return AttachDocument.Service.Gets(nUserID);
		}
        public static List<AttachDocument> Gets(int id, int nRefType, long nUserID)
        {
            return AttachDocument.Service.Gets(id,nRefType, nUserID);
        }
        public static List<AttachDocument> Gets_WithAttachFile(int nRefID, int nRefType, long nUserID)
        {
            return AttachDocument.Service.Gets_WithAttachFile(nRefID, nRefType, nUserID);
        }
		public static List<AttachDocument> Gets(string sSQL, long nUserID)
		{
			return AttachDocument.Service.Gets(sSQL,nUserID);
		}
        public static AttachDocument GetWithAttachFile(int id, long nUserID)
		{
            return AttachDocument.Service.GetWithAttachFile(id, nUserID);
		}
        public AttachDocument GetUserSignature(int nSignatureUserID, long nUserID)
        {
            return AttachDocument.Service.GetUserSignature(nSignatureUserID, nUserID);
        }
		public AttachDocument Save(long nUserID)
		{
			return AttachDocument.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return AttachDocument.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IAttachDocumentService Service
		{
			get { return (IAttachDocumentService)Services.Factory.CreateService(typeof(IAttachDocumentService)); }
		}
		#endregion
	}
	#endregion

	#region IAttachDocument interface
	public interface IAttachDocumentService 
	{
        AttachDocument GetUserSignature(int nSignatureUserID, Int64 nUserID);
        AttachDocument GetWithAttachFile(int id, Int64 nUserID); 
		List<AttachDocument> Gets(Int64 nUserID);
        List<AttachDocument> Gets(int id,int nRefType, Int64 nUserID);
        List<AttachDocument> Gets_WithAttachFile(int nRefID, int nRefType, Int64 nUserID);
		List<AttachDocument> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		AttachDocument Save(AttachDocument oAttachDocument, Int64 nUserID);
	}
	#endregion
}
