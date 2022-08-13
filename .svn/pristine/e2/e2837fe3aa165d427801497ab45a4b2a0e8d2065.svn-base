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


    #region PQTermsAndCondition
    
    public class PQTermsAndCondition : BusinessObject
    {
        public PQTermsAndCondition()
        {
            PQTermsAndConditionLogID = 0;
            PQTermsAndConditionID = 0;
            PurchaseQuotationID = 0;
            TermsAndCondition = "";
            ErrorMessage = "";
        }

        #region Properties

        public int PQTermsAndConditionLogID { get; set; }
        public int PQTermsAndConditionID { get; set; }

        public int PurchaseQuotationID { get; set; }
         
        public string TermsAndCondition { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        #endregion

        #region Functions

        public static List<PQTermsAndCondition> Gets(int id, long nUserID)
        {
            return PQTermsAndCondition.Service.Gets(id, nUserID);
        }

        public static List<PQTermsAndCondition> GetsByLog(int id, long nUserID)
        {
            return PQTermsAndCondition.Service.GetsByLog(id, nUserID);
        }

        public static List<PQTermsAndCondition> Gets(string sSQL, long nUserID)
        {
            return PQTermsAndCondition.Service.Gets(sSQL, nUserID);
        }

        public PQTermsAndCondition Get(int id, long nUserID)
        {
            return PQTermsAndCondition.Service.Get(id, nUserID);
        }

        public PQTermsAndCondition Save(long nUserID)
        {
            return PQTermsAndCondition.Service.Save(this, nUserID);
        }
        public string PQTermsAndConditionSave(List<PQTermsAndCondition> oPQTermsAndCondition, long nUserID)
        {
            return PQTermsAndCondition.Service.PQTermsAndConditionSave(oPQTermsAndCondition, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return PQTermsAndCondition.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IPQTermsAndConditionService Service
        {
            get { return (IPQTermsAndConditionService)Services.Factory.CreateService(typeof(IPQTermsAndConditionService)); }
        }

        #endregion
    }
    #endregion

    #region IPQTermsAndCondition interface

    public interface IPQTermsAndConditionService
    {

        PQTermsAndCondition Get(int id, Int64 nUserID);

        List<PQTermsAndCondition> Gets(int id, Int64 nUserID);
        List<PQTermsAndCondition> GetsByLog(int id, Int64 nUserID);


        List<PQTermsAndCondition> Gets(string sSQL, Int64 nUserID);

        PQTermsAndCondition Save(PQTermsAndCondition oPQTermsAndCondition, Int64 nUserID);

        string PQTermsAndConditionSave(List<PQTermsAndCondition> oPQTermsAndConditions, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);

    }
    #endregion 
    
    

}

