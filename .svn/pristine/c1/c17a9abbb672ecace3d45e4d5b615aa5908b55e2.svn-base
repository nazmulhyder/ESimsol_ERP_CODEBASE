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
    #region LHRuleDetail
    
    public class LHRuleDetail : BusinessObject
    {
        public LHRuleDetail()
        {
            LHRuleDetailID = 0;
            LHRuleID = 0;
            LHRuleValueType = EnumLHRuleValueType.None;
            LHRuleValue = "";
            Sequence = 0;
            LHRuleValueTypeInt = 0;
            ErrorMessage = "";
        }

        #region Properties

        public int LHRuleDetailID { get; set; }
        public int LHRuleID { get; set; }
        public EnumLHRuleValueType LHRuleValueType { get; set; }
        public int LHRuleValueTypeInt { get; set; }
        public string LHRuleValue { get; set; }
        public int Sequence { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string LHRuleValueTypeSt
        {
            get
            {
                return this.LHRuleValueType.ToString();
            }
        }
     
        #endregion

        #region Functions

        public static List<LHRuleDetail> Gets(long nUserID)
        {
            return LHRuleDetail.Service.Gets(nUserID);
        }
        public static List<LHRuleDetail> Gets(string sSQL, Int64 nUserID)
        {
            return LHRuleDetail.Service.Gets(sSQL, nUserID);
        }
        public LHRuleDetail Get(int nId, long nUserID)
        {
            return LHRuleDetail.Service.Get(nId,nUserID);
        }
        public LHRuleDetail Save(long nUserID)
        {
            return LHRuleDetail.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return LHRuleDetail.Service.Delete(nId, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static ILHRuleDetailService Service
        {
            get { return (ILHRuleDetailService)Services.Factory.CreateService(typeof(ILHRuleDetailService)); }
        }
        #endregion
    }
    #endregion

    #region ILHRuleDetail interface
    
    public interface ILHRuleDetailService
    {
        
        LHRuleDetail Get(int id, long nUserID);
        List<LHRuleDetail> Gets(long nUserID);
        List<LHRuleDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        LHRuleDetail Save(LHRuleDetail oLHRuleDetail, long nUserID);
        
    }
    #endregion
}



