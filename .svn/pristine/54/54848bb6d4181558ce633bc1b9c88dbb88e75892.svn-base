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
	#region OrderRecapComment 
	public class OrderRecapComment : BusinessObject
	{	
		public OrderRecapComment()
		{
			OrderRecapCommentID = 0; 
			OrderRecapID = 0; 
			CommentsBy = ""; 
			CommentsText = ""; 
			ErrorMessage = "";
		}

		#region Property
		public int OrderRecapCommentID { get; set; }
		public int OrderRecapID { get; set; }
		public string CommentsBy { get; set; }
		public string CommentsText { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public List<OrderRecapComment> OrderRecapComments { get; set; }
        public Company Company { get; set; }
		#endregion 

		#region Functions 

        public static List<OrderRecapComment> Gets(int id,long nUserID)
        {
            return OrderRecapComment.Service.Gets(id, nUserID);
        }
		public static List<OrderRecapComment> Gets(long nUserID)
		{
			return OrderRecapComment.Service.Gets(nUserID);
		}
		public static List<OrderRecapComment> Gets(string sSQL, long nUserID)
		{
			return OrderRecapComment.Service.Gets(sSQL,nUserID);
		}
		public OrderRecapComment Get(int id, long nUserID)
		{
			return OrderRecapComment.Service.Get(id,nUserID);
		}
		public OrderRecapComment Save(long nUserID)
		{
			return OrderRecapComment.Service.Save(this,nUserID);
		}
		public  string  Delete(int id, long nUserID)
		{
			return OrderRecapComment.Service.Delete(id,nUserID);
		}
		#endregion

		#region ServiceFactory
		internal static IOrderRecapCommentService Service
		{
			get { return (IOrderRecapCommentService)Services.Factory.CreateService(typeof(IOrderRecapCommentService)); }
		}
		#endregion
	}
	#endregion

	#region IOrderRecapComment interface
	public interface IOrderRecapCommentService 
	{
		OrderRecapComment Get(int id, Int64 nUserID);
        List<OrderRecapComment> Gets(int id, Int64 nUserID);
		List<OrderRecapComment> Gets(Int64 nUserID);
		List<OrderRecapComment> Gets( string sSQL, Int64 nUserID);
		string Delete(int id, Int64 nUserID);
 		OrderRecapComment Save(OrderRecapComment oOrderRecapComment, Int64 nUserID);
	}
	#endregion
}
