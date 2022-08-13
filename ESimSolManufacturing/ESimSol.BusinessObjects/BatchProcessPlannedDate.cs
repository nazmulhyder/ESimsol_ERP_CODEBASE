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
    #region BatchProcessPlannedDate
    
    public class BatchProcessPlannedDate : BusinessObject
    {
        public BatchProcessPlannedDate()
        {
            BatchProcessPlannedDateID = 0;
            FNBatchCardID = 0;
            FNTreatmentProcessID = 0;
            FNBatchID = 0;
            PlannedDate = DateTime.MinValue;
            Params = "";
            ErrorMessage = "";
        }

        #region Properties
        
        public int BatchProcessPlannedDateID { get; set; }
        public int FNBatchCardID { get; set; }
        public int FNTreatmentProcessID { get; set; }
        public int FNBatchID { get; set; }
        public DateTime PlannedDate { get; set; }
        public string Params { get; set; }
        public string FNProcess { get; set; }
        public string Code { get; set; }
        public string ErrorMessage { get; set; }
        
        #endregion

        #region Derived Property
        public string PlannedDateSt
        {
            get
            {
                if (this.PlannedDate == DateTime.MinValue) return "";
                else return this.PlannedDate.ToString("dd MMM yyyy");
            }
        }
     
        #endregion

        #region Functions

        public static List<BatchProcessPlannedDate> Gets(long nUserID)
        {
            return BatchProcessPlannedDate.Service.Gets(nUserID);
        }
        public static List<BatchProcessPlannedDate> Gets(string sSQL, Int64 nUserID)
        {
            return BatchProcessPlannedDate.Service.Gets(sSQL, nUserID);
        }
        public BatchProcessPlannedDate Get(int nId, long nUserID)
        {
            return BatchProcessPlannedDate.Service.Get(nId,nUserID);
        }
        public BatchProcessPlannedDate Save(long nUserID)
        {
            return BatchProcessPlannedDate.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return BatchProcessPlannedDate.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IBatchProcessPlannedDateService Service
        {
            get { return (IBatchProcessPlannedDateService)Services.Factory.CreateService(typeof(IBatchProcessPlannedDateService)); }
        }
        #endregion
    }
    #endregion

    #region IBatchProcessPlannedDate interface
    
    public interface IBatchProcessPlannedDateService
    {
        BatchProcessPlannedDate Get(int id, long nUserID);
        List<BatchProcessPlannedDate> Gets(long nUserID);
        List<BatchProcessPlannedDate> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, long nUserID);
        BatchProcessPlannedDate Save(BatchProcessPlannedDate oBatchProcessPlannedDate, long nUserID);
    }
    #endregion
}