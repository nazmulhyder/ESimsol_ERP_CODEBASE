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

    #region RMRequisition
    public class RMRequisition : BusinessObject
    {
        public RMRequisition()
        {
            RMRequisitionID = 0;
            RefNo = "";
            RequisitionDate = DateTime.Now;
            ApprovedBy = 0;
            BUID = 0;
            Remarks = "";
            ApprovedByName = "";
            PreparedByName = "";
            PINo = "";
            RMRequisitionSheets = new List<RMRequisitionSheet>();
            RMRequisitionMaterials = new List<RMRequisitionMaterial>();
          
        }

        #region Property
        public int RMRequisitionID { get; set; }
        public DateTime RequisitionDate { get; set; }
        public int ApprovedBy { get; set; }
        public int BUID { get; set; }
        public string Remarks { get; set; }
        public string PreparedByName { get; set; }
        public string ApprovedByName { get; set; }       
         public string RefNo { get; set; }
         public string PINo { get; set; }
        public string ErrorMessage { get; set; }
     
        #endregion

        #region Derived Property
        public List<RMRequisitionSheet> RMRequisitionSheets { get; set; }
        public List<RMRequisitionMaterial> RMRequisitionMaterials { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public Company Company { get; set; }
        public string RequisitionDateSt
        {
            get
            {
                return this.RequisitionDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions

        public RMRequisition Save(long nUserID)
        {
            return RMRequisition.Service.Save(this, nUserID);
        }
        public RMRequisition Approve(long nUserID)
        {
            return RMRequisition.Service.Approve(this, nUserID);
        }
        
        public static List<RMRequisition> BUWiseGets(int nBUID, long nUserID)
        {
            return RMRequisition.Service.BUWiseGets(nBUID, nUserID);
        }
        public static List<RMRequisition> Gets(string sSQL, long nUserID)
        {
            return RMRequisition.Service.Gets(sSQL, nUserID);
        }
        public RMRequisition Get(int id, long nUserID)
        {
            return RMRequisition.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return RMRequisition.Service.Delete(id, nUserID);
        }
     
        #endregion

        #region ServiceFactory
        internal static IRMRequisitionService Service
        {
            get { return (IRMRequisitionService)Services.Factory.CreateService(typeof(IRMRequisitionService)); }
        }
        #endregion
    }
    #endregion

    #region IRMRequisition interface
    public interface IRMRequisitionService
    {
        RMRequisition Get(int id, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        
        RMRequisition Save(RMRequisition oRMRequisition, Int64 nUserID);
        RMRequisition Approve(RMRequisition oRMRequisition, Int64 nUserID);
        List<RMRequisition> BUWiseGets(int nBUID, Int64 nUserID);
        List<RMRequisition> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
    
    
}
