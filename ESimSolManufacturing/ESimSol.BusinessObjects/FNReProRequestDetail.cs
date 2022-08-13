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
    #region FNReProRequestDetail
    public class FNReProRequestDetail : BusinessObject
    {
        public FNReProRequestDetail()
        {
            FNReProRequestDetailID = 0;
            FNReProRequestID = 0;
            FNBatchCardID = 0;
            Qty = 0;
            Note = "";
            FNBatchID = 0;
            BatchNo = "";
            FNTreatmentProcessID = 0;
            FNProcess = "";
            Qty_Prod = 0;
            Qty_YetTo = 0;
            FNExeNo = "";
            ErrorMessage = "";
        }

        #region Property
        public int FNReProRequestDetailID { get; set; }
        public int FNReProRequestID { get; set; }
        public int FNBatchCardID { get; set; }
        public double Qty { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public int FNBatchID { get; set; }
        public string BatchNo { get; set; }
        public string FNExeNo { get; set; }
        public double Qty_Prod { get; set; }
        public double Qty_YetTo { get; set; }
        public double Qty_M
        {
            get
            {
                return (this.Qty * 0.9144);
            }
        }
        public int FNTreatmentProcessID { get; set; }
        public string FNProcess { get; set; }
        #endregion

        #region Functions
        public static List<FNReProRequestDetail> Gets(int nFNReProRequestID, long nUserID)
        {
            return FNReProRequestDetail.Service.Gets(nFNReProRequestID, nUserID);
        }
        public static List<FNReProRequestDetail> Gets(string sSQL, long nUserID)
        {
            return FNReProRequestDetail.Service.Gets(sSQL, nUserID);
        }
        public FNReProRequestDetail Get(int id, long nUserID)
        {
            return FNReProRequestDetail.Service.Get(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFNReProRequestDetailService Service
        {
            get { return (IFNReProRequestDetailService)Services.Factory.CreateService(typeof(IFNReProRequestDetailService)); }
        }
        #endregion

        public List<FNReProRequestDetail> FNReProRequestDetails { get; set; }
    }
    #endregion

    #region IFNReProRequestDetail interface
    public interface IFNReProRequestDetailService
    {
        FNReProRequestDetail Get(int id, Int64 nUserID);
        List<FNReProRequestDetail> Gets(int nFNReProRequestID, Int64 nUserID);
        List<FNReProRequestDetail> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
