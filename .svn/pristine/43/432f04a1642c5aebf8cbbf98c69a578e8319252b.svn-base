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
    #region KnittingOrderTermsAndCondition
    public class KnittingOrderTermsAndCondition : BusinessObject
    {
        public KnittingOrderTermsAndCondition()
        {
            KnittingOrderTermsAndConditionID = 0;
            KnittingOrderID = 0;
            ClauseType = 0;
            TermsAndCondition = "";
            ErrorMessage = "";
        }

        #region Property
        public int KnittingOrderTermsAndConditionID { get; set; }
        public int KnittingOrderID { get; set; }
        public int ClauseType { get; set; }
        public string TermsAndCondition { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        private string _sClauseType = "";
        public string ClauseTypeSt
        {
            get
            {
                _sClauseType = ((EnumPOTerms)this.ClauseType).ToString();

                return _sClauseType;
            }
        }
        #endregion

        #region Functions
        public static List<KnittingOrderTermsAndCondition> Gets(long nUserID)
        {
            return KnittingOrderTermsAndCondition.Service.Gets(nUserID);
        }
        public static List<KnittingOrderTermsAndCondition> Gets(int id, long nUserID)
        {
            return KnittingOrderTermsAndCondition.Service.Gets(id, nUserID);
        }
        public static List<KnittingOrderTermsAndCondition> Gets(string sSQL, long nUserID)
        {
            return KnittingOrderTermsAndCondition.Service.Gets(sSQL, nUserID);
        }
        public KnittingOrderTermsAndCondition Get(int id, long nUserID)
        {
            return KnittingOrderTermsAndCondition.Service.Get(id, nUserID);
        }
        public KnittingOrderTermsAndCondition Save(long nUserID)
        {
            return KnittingOrderTermsAndCondition.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return KnittingOrderTermsAndCondition.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IKnittingOrderTermsAndConditionService Service
        {
            get { return (IKnittingOrderTermsAndConditionService)Services.Factory.CreateService(typeof(IKnittingOrderTermsAndConditionService)); }
        }
        #endregion
    }
    #endregion

    #region IKnittingOrderTermsAndCondition interface
    public interface IKnittingOrderTermsAndConditionService
    {
        KnittingOrderTermsAndCondition Get(int id, Int64 nUserID);
        List<KnittingOrderTermsAndCondition> Gets(Int64 nUserID);
        List<KnittingOrderTermsAndCondition> Gets(int id, Int64 nUserID);
        List<KnittingOrderTermsAndCondition> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        KnittingOrderTermsAndCondition Save(KnittingOrderTermsAndCondition oKnittingOrderTermsAndCondition, Int64 nUserID);
    }
    #endregion
}
