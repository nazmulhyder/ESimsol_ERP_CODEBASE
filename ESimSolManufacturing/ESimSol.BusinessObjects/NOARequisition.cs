using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region NOARequisition
    public class NOARequisition : BusinessObject
    {
        public NOARequisition()
        {
            NOARequisitionLogID = 0;
            NOALogID = 0;
            NOARequisitionID = 0;
            NOAID = 0;
            PRID = 0;
            PrepareByName = "";
            ErrorMessage = "";
            NOARequisitions = new List<NOARequisition>();
            PRDate = DateTime.Now;
            RequirementDate = DateTime.Now;
            RequisitionByName = "";
            ApprovedByName = "";
            Note = "";
        }

        #region Properties
        public int NOARequisitionLogID { get; set; }
        public int NOALogID { get; set; }
        public int NOARequisitionID { get; set; }
        public int NOAID { get; set; }
        public int PRID { get; set; }
        public DateTime PRDate { get; set; }
        public string RequisitionByName { get; set; }
        public string Note { get; set; }
        public string ApprovedByName { get; set; }
        public DateTime RequirementDate { get; set; }
        public string PrepareByName { get; set; }
             
        public string PRNo { get; set; }
     
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived
        public List<NOARequisition> NOARequisitions { get; set; }

        public string PRDateST
        {
            get
            {
                return PRDate.ToString("dd MMM yyyy");
            }
        }
        public string RequirementDateSt
        {
            get
            {
                return this.RequirementDate.ToString("dd MMM yyyy");
            }
        }
        
     
       
        #endregion

        #region Functions
        public static List<NOARequisition> Gets(int nUserID)
        {
            return NOARequisition.Service.Gets(nUserID);
        }
        public NOARequisition Get(int id, int nUserID)
        {
            return NOARequisition.Service.Get(id, nUserID);
        }

        public static List<NOARequisition> Save(NOARequisition oNOARequisition, int nUserID)
        {
            return NOARequisition.Service.Save(oNOARequisition, nUserID);
        }
        public static List<NOARequisition> Gets(long nNOADetailID, int nUserID)
        {
            return NOARequisition.Service.Gets(nNOADetailID, nUserID);
        }
        public string Delete(int nNOARequisitionID,  int nUserID)
        {
            return NOARequisition.Service.Delete(nNOARequisitionID, nUserID);
        }
        public static List<NOARequisition> Gets(string sSQL, int nUserID)
        {
            return NOARequisition.Service.Gets(sSQL,nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static INOARequisitionService Service
        {
            get { return (INOARequisitionService)Services.Factory.CreateService(typeof(INOARequisitionService)); }
        }
        #endregion
    }
    #endregion

    #region INOARequisition interface
    public interface INOARequisitionService
    {
        NOARequisition Get(int id, int nUserID);
        List<NOARequisition> Gets(int nUserID);
        List<NOARequisition> Gets(string sSQL, int nUserID);
        List<NOARequisition> Gets(long nNOADetailID, int nUserID);
        string Delete(int nNOARequisitionID,  int nUserID);
       List<NOARequisition> Save(NOARequisition oNOARequisition, int nUserID);
    }
    #endregion
    
  
}
