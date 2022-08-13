using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region LHRule
    
    public class LHRule : BusinessObject
    {
        public LHRule()
        {
            LHRuleID = 0;
            LeaveHeadID = 0;
            LHRuleType = EnumLHRuleType.None;
            LHRuleTypeInt = 0;
            Remarks = "";
            LHRuleDetails = new List<LHRuleDetail>();
            LHRuleTypeDescription = "";
            ErrorMessage = "";
        }

        #region Properties
        
        public int LHRuleID { get; set; }
        public int LeaveHeadID { get; set; }
        public EnumLHRuleType LHRuleType { get; set; }
        public int LHRuleTypeInt { get; set; }
        public string Remarks { get; set; }
        public List<LHRuleDetail> LHRuleDetails { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string LHRuleTypeSt
        {
            get
            {
                return this.LHRuleType.ToString();
            }
        }
        public string LHRuleTypeDescription { get; set; }     
        #endregion

        #region Functions

        public static List<LHRule> Gets(long nUserID)
        {
            return LHRule.Service.Gets(nUserID);
        }
        public static List<LHRule> Gets(string sSQL, Int64 nUserID)
        {
            return LHRule.Service.Gets(sSQL, nUserID);
        }
        public LHRule Get(int nId, long nUserID)
        {
            return LHRule.Service.Get(nId,nUserID);
        }
        public LHRule Save(long nUserID)
        {
            return LHRule.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return LHRule.Service.Delete(nId, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ILHRuleService Service
        {
            get { return (ILHRuleService)Services.Factory.CreateService(typeof(ILHRuleService)); }
        }
        #endregion
    }
    #endregion

    #region ILHRule interface
    
    public interface ILHRuleService
    {
        
        LHRule Get(int id, long nUserID);
        List<LHRule> Gets(long nUserID);
        List<LHRule> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        LHRule Save(LHRule oLHRule, long nUserID);
        
    }
    #endregion
}


