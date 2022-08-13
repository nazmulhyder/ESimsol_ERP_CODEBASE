using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region ExportTermsAndCondition
    public class ExportTermsAndCondition : BusinessObject
    {
        public ExportTermsAndCondition()
        {
            ExportTermsAndConditionID = 0;
            Clause = "";
            ClauseType = 0;
            ExportTnCCaptionID = 0;
            Activity = true;
            BUID = 0;
            DocFor = EnumDocFor.Common;
            BUName = "";
            SLNo = 0;
            ErrorMessage = "";
            ExportTermsAndConditions = new List<ExportTermsAndCondition>();
        }

        #region Properties
        public int ExportTermsAndConditionID { get; set; }        
        public string Clause { get; set; }
        public string CaptionName { get; set; }  
        public string Note { get; set; }
        public string BUName { get; set; }    
        public int ClauseType { get; set; }
        public int ExportTnCCaptionID { get; set; }        
        public bool Activity { get; set; }
        public int BUID { get; set; }
        public int SLNo { get; set; }
        public List<ExportTermsAndCondition> ExportTermsAndConditions { get; set; }
        public EnumDocFor DocFor { get; set; }
        public int DocForInInt { get; set; }
        public string ErrorMessage { get; set; }
        private string _sClauseType = "";     
        public string ClauseTypeInString
        {
            get
            {
                
                if (ClauseType == 1)
                { 
                    _sClauseType= "Payment Terms";
                }
                if (ClauseType == 2)
                {
                    _sClauseType = "Required Terms";
                }
                if (ClauseType == 3)
                {
                    _sClauseType = "Delivery Terms";
                }
                if (ClauseType == 4)
                {
                    _sClauseType = "Other Terms";
                }
                return _sClauseType;
            }
        }
        private string _sActivity = "";
        public string DocForInString
        {
            get
            {
                return this.DocFor.ToString();

            }
        }
        public string ActivityInString
        {
            get
            {

                if (Activity == true)
                {
                    _sActivity = "Active";
                }
                if (Activity == false)
                {
                    _sActivity = "Inactive";
                }

                return _sActivity;
            }
        }        
        #endregion


        #region Functions
        public static List<ExportTermsAndCondition> GetsByTypeAndBU(string sDocFors, string BUIDs, Int64 nUserID)
        {
            return ExportTermsAndCondition.Service.GetsByTypeAndBU(sDocFors, BUIDs, nUserID);            
        }
        public static List<ExportTermsAndCondition> Gets(bool bActivity,int nBUId,Int64 nUserID)
        {
            return ExportTermsAndCondition.Service.Gets(bActivity, nBUId, nUserID);            
        }
        public static List<ExportTermsAndCondition> BUWiseGets(int nBUId, Int64 nUserID)
        {
            return ExportTermsAndCondition.Service.BUWiseGets(nBUId, nUserID);
        }
        public ExportTermsAndCondition Get(int id, Int64 nUserID)
        {
            return ExportTermsAndCondition.Service.Get(id, nUserID);            
        }
        public ExportTermsAndCondition RefreshSequence(Int64 nUserID)
        {
            return ExportTermsAndCondition.Service.RefreshSequence(this, nUserID);            
        }

        public ExportTermsAndCondition Save(Int64 nUserID)
        {
            return ExportTermsAndCondition.Service.Save(this, nUserID);
        }

        public string Delete(int id, Int64 nUserID)
        {
            return ExportTermsAndCondition.Service.Delete(id, nUserID);            
        }
        public string ActivatePITandCSetup(ExportTermsAndCondition oExportTermsAndCondition, Int64 nUserID)
        {
            return ExportTermsAndCondition.Service.ActivatePITandCSetup(oExportTermsAndCondition, nUserID);            
        }
        #endregion
        #region ServiceFactory
        internal static IExportTermsAndConditionService Service
        {
            get { return (IExportTermsAndConditionService)Services.Factory.CreateService(typeof(IExportTermsAndConditionService)); }
        }
        #endregion
    }
    #endregion

    #region IExportTermsAndCondition interface
    public interface IExportTermsAndConditionService
    {        
        ExportTermsAndCondition Get(int id, Int64 nUserID);
        ExportTermsAndCondition RefreshSequence(ExportTermsAndCondition oExportTermsAndCondition, long nUserID);
        List<ExportTermsAndCondition> GetsByTypeAndBU(string sDocFors, string BUs, Int64 nUserID);        
        List<ExportTermsAndCondition> Gets(bool bActivity, int nBUID, Int64 nUserID);
        List<ExportTermsAndCondition> BUWiseGets(int nBUID, Int64 nUserID);  
        string Delete(int id, Int64 nUserID);        
        ExportTermsAndCondition Save(ExportTermsAndCondition oExportTermsAndCondition, Int64 nUserID);        
        string ActivatePITandCSetup(ExportTermsAndCondition oExportTermsAndCondition, Int64 nUserID);
    }
    #endregion
}
