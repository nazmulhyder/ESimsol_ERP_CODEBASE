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


    #region MasterLCTermsAndCondition
    
    public class MasterLCTermsAndCondition : BusinessObject
    {
        public MasterLCTermsAndCondition()
        {

            MasterLCTermsAndConditionID = 0;
            MasterLCID = 0;
            TermsAndCondition = "";
            MasterLCTermsAndConditionLogID = 0;
            MasterLCLogID = 0;
            ErrorMessage = "";
        }

        #region Properties
         
        public int MasterLCTermsAndConditionID { get; set; }
         
        public int MasterLCID { get; set; }
         
        public string TermsAndCondition { get; set; }
         
        public int MasterLCTermsAndConditionLogID { get; set; }
         
        public int MasterLCLogID { get; set; }
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property

        #endregion

        #region Functions

        public static List<MasterLCTermsAndCondition> Gets(int id, long nUserID)
        {
            return MasterLCTermsAndCondition.Service.Gets(id, nUserID);
        }
        public static List<MasterLCTermsAndCondition> GetsMasterLCLog(int id, long nUserID) // id is Master LC Log ID
        {
            return MasterLCTermsAndCondition.Service.GetsMasterLCLog(id, nUserID);
        }

        public static List<MasterLCTermsAndCondition> Gets(string sSQL, long nUserID)
        {
            return MasterLCTermsAndCondition.Service.Gets(sSQL, nUserID);
        }

        public MasterLCTermsAndCondition Get(int id, long nUserID)
        {
            return MasterLCTermsAndCondition.Service.Get(id, nUserID);
        }

        public MasterLCTermsAndCondition Save(long nUserID)
        {           
            return MasterLCTermsAndCondition.Service.Save(this, nUserID);
        }
        public string MasterLCTermsAndConditionSave(List<MasterLCTermsAndCondition> oMasterLCTermsAndConditions, long nUserID)
        {
            return MasterLCTermsAndCondition.Service.MasterLCTermsAndConditionSave(oMasterLCTermsAndConditions, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return MasterLCTermsAndCondition.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IMasterLCTermsAndConditionService Service
        {
            get { return (IMasterLCTermsAndConditionService)Services.Factory.CreateService(typeof(IMasterLCTermsAndConditionService)); }
        }

        #endregion
    }
    #endregion

    #region IMasterLCTermsAndCondition interface
     
    public interface IMasterLCTermsAndConditionService
    {
         
        MasterLCTermsAndCondition Get(int id, Int64 nUserID);
         
        List<MasterLCTermsAndCondition> Gets(int id, Int64 nUserID);
         
        List<MasterLCTermsAndCondition> GetsMasterLCLog(int id, Int64 nUserID);

         
        List<MasterLCTermsAndCondition> Gets(string sSQL, Int64 nUserID);
         
        MasterLCTermsAndCondition Save(MasterLCTermsAndCondition oMasterLCTermsAndCondition, Int64 nUserID);
         
        string MasterLCTermsAndConditionSave(List<MasterLCTermsAndCondition> oMasterLCTermsAndConditions, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);

    }
    #endregion 
    
    

}
