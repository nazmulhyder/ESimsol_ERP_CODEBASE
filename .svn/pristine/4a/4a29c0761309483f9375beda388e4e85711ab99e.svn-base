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
	#region ServiceInvoiceTerms  
	public class ServiceInvoiceTerms : BusinessObject
	{	
		public ServiceInvoiceTerms()
		{
            ServiceInvoiceTermsID = 0; 
            ServiceInvoiceID = 0; 
      
            Terms = "";
			ErrorMessage = "";
		}

		#region Property
        public int ServiceInvoiceTermsID { get; set; }
		public int ServiceInvoiceID { get; set; }
        public int ServiceInvoiceTermsLogID { get; set; }
        public int ServiceInvoiceLogID { get; set; }

        public string Terms { get; set; }
    
		public string ErrorMessage { get; set; }
  
		#endregion 
   
		#region Functions 
		public static List<ServiceInvoiceTerms> Gets(int id, long nUserID)
		{
			return ServiceInvoiceTerms.Service.Gets(id, nUserID);
		}
        public static List<ServiceInvoiceTerms> GetsLog(int id, long nUserID)
        {
            return ServiceInvoiceTerms.Service.GetsLog(id, nUserID);
        }
		public static List<ServiceInvoiceTerms> Gets(string sSQL, long nUserID)
		{
			return ServiceInvoiceTerms.Service.Gets(sSQL,nUserID);
		}
		public ServiceInvoiceTerms Get(int id, long nUserID)
		{
			return ServiceInvoiceTerms.Service.Get(id,nUserID);
		}
		
		#endregion

		#region ServiceFactory
        internal static IServiceInvoiceTermsService Service
		{
            get { return (IServiceInvoiceTermsService)Services.Factory.CreateService(typeof(IServiceInvoiceTermsService)); }
		}
		#endregion

    }
	#endregion

	#region IServiceInvoiceDetail interface
    public interface IServiceInvoiceTermsService 
	{
		ServiceInvoiceTerms Get(int id, Int64 nUserID); 
		List<ServiceInvoiceTerms> Gets(int id, Int64 nUserID);
        List<ServiceInvoiceTerms> GetsLog(int id, Int64 nUserID);
		List<ServiceInvoiceTerms> Gets( string sSQL, Int64 nUserID);
	}
	#endregion
}
