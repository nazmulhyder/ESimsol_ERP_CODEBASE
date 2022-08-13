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
	#region ProductSpecHead  
	public class ProductSpecHead : BusinessObject
	{	
		public ProductSpecHead()
		{
			ProductSpecHeadID = 0; 
			ProductID = 0; 
			SpecHeadID = 0; 
			Sequence = 0; 
			ErrorMessage = "";
            Params ="";
            ProductSpecHeads = new List<ProductSpecHead>();
            ProductName = "";
            ProductCode = "";
            SpecName = "";
		}
        
		#region Property
		public int ProductSpecHeadID { get; set; }
		public int ProductID { get; set; }
		public int SpecHeadID { get; set; }
		public int Sequence { get; set; }
		public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public List<ProductSpecHead> ProductSpecHeads { get; set; }

		#endregion 

		#region Derived Property
        public string ProductCode{get;set;}
        public string ProductName{get;set;}
        public string SpecName {get;set;}
        public bool IsUp { get; set; }
		#endregion 

		#region Functions 
		public static List<ProductSpecHead> Gets(long nUserID)
		{
			return ProductSpecHead.Service.Gets(nUserID);
		}
		public static List<ProductSpecHead> Gets(string sSQL, long nUserID)
		{
			return ProductSpecHead.Service.Gets(sSQL,nUserID);
		}
		public ProductSpecHead Get(int id, long nUserID)
		{
			return ProductSpecHead.Service.Get(id,nUserID);
		}
		public ProductSpecHead Save(long nUserID)
		{
			return ProductSpecHead.Service.Save(this,nUserID);
        }
        public string Delete(int id,int productID, long nUserID)
        {
            return ProductSpecHead.Service.Delete(id,productID, nUserID);
        }

        
        public ProductSpecHead UpDown(ProductSpecHead oProductSpecHead, long nUserID)
        {
            return ProductSpecHead.Service.UpDown(oProductSpecHead, nUserID);
        }
		#endregion

		#region ServiceFactory
		internal static IProductSpecHeadService Service
		{
			get { return (IProductSpecHeadService)Services.Factory.CreateService(typeof(IProductSpecHeadService)); }
		}
		#endregion
	}
	#endregion

	#region IProductSpecHead interface
	public interface IProductSpecHeadService 
	{
		ProductSpecHead Get(int id, Int64 nUserID); 
		List<ProductSpecHead> Gets(Int64 nUserID);
		List<ProductSpecHead> Gets( string sSQL, Int64 nUserID);
        string Delete(int id,int productID, Int64 nUserID);
 		ProductSpecHead Save(ProductSpecHead oProductSpecHead, Int64 nUserID);
        ProductSpecHead UpDown(ProductSpecHead oProductSpecHead, Int64 nUserID);
        
	}
	#endregion
}
