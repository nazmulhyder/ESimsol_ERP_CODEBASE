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
	#region NOASignatoryComment  
	public class NOASignatoryComment : BusinessObject
	{	
		public NOASignatoryComment()
		{
			ErrorMessage = "";
            NOASignatoryCommentID = 0;
            NOADetailID = 0;
            PQDetailID = 0;
            NOASignatoryID = 0;
            Comment = "";
            DBServerDateTime = DateTime.Now;
            ErrorMessage = "";
            SupplierName = "";
            ProductName = "";
            Name = "";
            PQNo = "";
            UnitPrice = 0;
            Note = "";
            IsAllSave = false;
            PurchaseQty = 0;
		}

		#region Property
        public int NOASignatoryCommentID { get; set; }
        public int NOADetailID { get; set; }
        public int PQDetailID { get; set; }
        public string Comment { get; set; }
        public string Note { get; set; }
        public int NOASignatoryID { get; set; }
        public DateTime DBServerDateTime { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 
		#region Derived Property
        public string SupplierName { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public double PurchaseQty { get; set; }
        public string Name { get; set; }
        public string PQNo { get; set; }
        public bool IsAllSave { get; set; }
        public List<NOAQuotation> NOAQuotations { get; set; }
        public List<NOASignatoryComment> NOASignatoryComments { get; set; }
        public string DateSt
        {
            get
            {
                return DBServerDateTime.ToString("dd MMM yyyy HH:mm");
            }
        }
		#endregion 

		#region Functions 
		public static List<NOASignatoryComment> Gets(int nNOADetailID,long nUserID)
		{
            return NOASignatoryComment.Service.Gets(nNOADetailID,nUserID);
		}
		public static List<NOASignatoryComment> Gets(string sSQL, long nUserID)
		{
			return NOASignatoryComment.Service.Gets(sSQL,nUserID);
		}
		public NOASignatoryComment Get(int id, long nUserID)
		{
			return NOASignatoryComment.Service.Get(id,nUserID);
		}
		public NOASignatoryComment Save(long nUserID)
		{
			return NOASignatoryComment.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return NOASignatoryComment.Service.Delete(id,nUserID);
		}
        public static List<NOASignatoryComment> SaveAll(List<NOASignatoryComment> oNOASignatoryComments, long nUserID)
        {
            return NOASignatoryComment.Service.SaveAll(oNOASignatoryComments, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static INOASignatoryCommentService Service
		{
			get { return (INOASignatoryCommentService)Services.Factory.CreateService(typeof(INOASignatoryCommentService)); }
		}
		#endregion
	}
	#endregion

	#region INOASignatoryComment interface
	public interface INOASignatoryCommentService 
	{
		NOASignatoryComment Get(int id, Int64 nUserID);
        List<NOASignatoryComment> Gets(int nNOADetailID,Int64 nUserID);
		List<NOASignatoryComment> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		NOASignatoryComment Save(NOASignatoryComment oNOASignatoryComment, Int64 nUserID);
        List<NOASignatoryComment> SaveAll(List<NOASignatoryComment> oNOASignatoryComments, Int64 nUserID);
	}
	#endregion
}
