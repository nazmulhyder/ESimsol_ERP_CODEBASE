using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;


namespace ESimSol.BusinessObjects
{

    #region ProformaInvoiceTermsAndCondition
    
    public class ProformaInvoiceTermsAndCondition : BusinessObject
    {
        public ProformaInvoiceTermsAndCondition()
        {

            ProformaInvoiceTermsAndConditionID = 0;
            ProformaInvoiceID= 0;
            TermsAndCondition ="";
            ProformaInvoiceTermsAndConditionLogID = 0;
            ProformaInvoiceLogID = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int ProformaInvoiceTermsAndConditionID { get; set; }
         
        public int ProformaInvoiceID { get; set; }
         
        public string TermsAndCondition { get; set; }
         
        public int ProformaInvoiceTermsAndConditionLogID { get; set; }
         
        public int ProformaInvoiceLogID { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        #endregion

        #region Functions

        public static List<ProformaInvoiceTermsAndCondition> Gets(int id,  long nUserID)
        {
            return ProformaInvoiceTermsAndCondition.Service.Gets(id, nUserID);
        }
        public static List<ProformaInvoiceTermsAndCondition> GetsPILog(int id, long nUserID) // id is PI Log ID
        {
            return ProformaInvoiceTermsAndCondition.Service.GetsPILog(id, nUserID);
        }

        public static List<ProformaInvoiceTermsAndCondition> Gets(string sSQL, long nUserID)
        {
            return ProformaInvoiceTermsAndCondition.Service.Gets(sSQL, nUserID);
        }

        public ProformaInvoiceTermsAndCondition Get(int id, long nUserID)
        {
            return ProformaInvoiceTermsAndCondition.Service.Get(id, nUserID);
        }

        public ProformaInvoiceTermsAndCondition Save(long nUserID)
        {
            return ProformaInvoiceTermsAndCondition.Service.Save(this, nUserID);
        }
        public string ProformaInvoiceTermsAndConditionSave(List<ProformaInvoiceTermsAndCondition> oProformaInvoiceTermsAndConditions, long nUserID)
        {
            return ProformaInvoiceTermsAndCondition.Service.ProformaInvoiceTermsAndConditionSave(oProformaInvoiceTermsAndConditions,nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return ProformaInvoiceTermsAndCondition.Service.Delete(id, nUserID);
        }



        #endregion

        #region ServiceFactory

 
        internal static IProformaInvoiceTermsAndConditionService Service
        {
            get { return (IProformaInvoiceTermsAndConditionService)Services.Factory.CreateService(typeof(IProformaInvoiceTermsAndConditionService)); }
        }

        #endregion
    }
    #endregion

    #region IProformaInvoiceTermsAndCondition interface
     
    public interface IProformaInvoiceTermsAndConditionService
    {
         
        ProformaInvoiceTermsAndCondition Get(int id, Int64 nUserID);
         
        List<ProformaInvoiceTermsAndCondition> Gets(int id, Int64 nUserID);
         
        List<ProformaInvoiceTermsAndCondition> GetsPILog(int id, Int64 nUserID);
        
         
        List<ProformaInvoiceTermsAndCondition> Gets(string sSQL, Int64 nUserID);
         
        ProformaInvoiceTermsAndCondition Save(ProformaInvoiceTermsAndCondition oProformaInvoiceTermsAndCondition, Int64 nUserID);
         
        string ProformaInvoiceTermsAndConditionSave(List<ProformaInvoiceTermsAndCondition> oProformaInvoiceTermsAndConditions, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);

    }
    #endregion 
}
