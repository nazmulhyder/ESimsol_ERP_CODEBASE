using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;



namespace ESimSol.BusinessObjects
{

    #region POTandCClause

    public class POTandCClause : BusinessObject
    {
        public POTandCClause()
        {

            POTandCClauseID = 0;
            POID = 0;
            TermsAndCondition = "";
            POTandCClauseLogID = 0;
            ProformaInvoiceLogID = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int POTandCClauseID { get; set; }
        public int POID { get; set; }
        public string TermsAndCondition { get; set; }
        public int POTandCClauseLogID { get; set; }
        public int ProformaInvoiceLogID { get; set; }
        public int ClauseType { get; set; }
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
        public static List<POTandCClause> Gets(int nPOID, int nUserID)
        {
            return POTandCClause.Service.Gets(nPOID, nUserID);
        }
        public static List<POTandCClause> GetsPOLog(int id, int nUserID)
        {
            return POTandCClause.Service.GetsPOLog( id, nUserID);
        }
        public static List<POTandCClause> Gets(string sSQL , int nUserID)
        {
            return POTandCClause.Service.Gets(sSQL, nUserID);
        }
        public POTandCClause Get(int id, int nUserID)
        {
            return POTandCClause.Service.Get(id, nUserID);
        }

        public POTandCClause Save(int nUserID)
        {
            return POTandCClause.Service.Save(this, nUserID);
        }
        public string POTandCClauseSave(List<POTandCClause> oPOTandCClauses, int nUserID)
        {
            return POTandCClause.Service.POTandCClauseSave(oPOTandCClauses, nUserID);
        }

        public string Delete(POTandCClause oPOTandCClause, int nUserID)
        {
            return POTandCClause.Service.Delete(oPOTandCClause, nUserID);
        }


        #endregion

        #region ServiceFactory

     
        internal static IPOTandCClauseService Service
        {
            get { return (IPOTandCClauseService)Services.Factory.CreateService(typeof(IPOTandCClauseService)); }
        }
        #endregion
    }
    #endregion

    #region IPOTandCClause interface
    public interface IPOTandCClauseService
    {
        POTandCClause Get(int id, Int64 nUserID);
        List<POTandCClause> Gets(int nPOID, Int64 nUserID);
        List<POTandCClause> GetsPOLog(int id, Int64 nUserID);
        List<POTandCClause> Gets(string sSQL, Int64 nUserID);
        POTandCClause Save(POTandCClause oPOTandCClause, Int64 nUserID);
        string POTandCClauseSave(List<POTandCClause> oPOTandCClauses, Int64 nUserID);
        string Delete(POTandCClause ooPOTandCClause, Int64 nUserID);

    }
    #endregion
}
