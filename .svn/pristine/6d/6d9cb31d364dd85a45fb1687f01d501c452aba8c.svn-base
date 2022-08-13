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
    #region FabricRequisitionDetail
    public class FabricRequisitionDetail : BusinessObject
    {
        public FabricRequisitionDetail()
        {
            FabricRequisitionDetailID = 0;
            FabricRequisitionID = 0;
            FEOSID = 0;
            FSCDID = 0;
            ReqQty = 0;
            ExeNo = "";
            RollType = 0;
            ErrorMessage = "";
        }

        #region Property
        public int FabricRequisitionDetailID { get; set; }
        public int FabricRequisitionID { get; set; }
        public int FEOSID { get; set; }
        public int FSCDID { get; set; }
        public double ReqQty { get; set; }
        public string ErrorMessage { get; set; }
        public int RollType { get; set; }
        #endregion

        #region Derived Property
        public string ExeNo { get; set; }
        public double ReqQtyM
        {
            get
            {
                return this.ReqQty * 0.9144;
            }
        }
        #endregion

        #region Functions
        public static List<FabricRequisitionDetail> Gets(long nUserID)
        {
            return FabricRequisitionDetail.Service.Gets(nUserID);
        }
        public static List<FabricRequisitionDetail> Gets(string sSQL, long nUserID)
        {
            return FabricRequisitionDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<FabricRequisitionDetail> Gets(int id, long nUserID)
        {
            return FabricRequisitionDetail.Service.Gets(id,nUserID);
        }
        public FabricRequisitionDetail Get(int id, long nUserID)
        {
            return FabricRequisitionDetail.Service.Get(id, nUserID);
        }
        public FabricRequisitionDetail Save(long nUserID)
        {
            return FabricRequisitionDetail.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return FabricRequisitionDetail.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricRequisitionDetailService Service
        {
            get { return (IFabricRequisitionDetailService)Services.Factory.CreateService(typeof(IFabricRequisitionDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricRequisitionDetail interface
    public interface IFabricRequisitionDetailService
    {
        FabricRequisitionDetail Get(int id, Int64 nUserID);
        List<FabricRequisitionDetail> Gets(Int64 nUserID);
        List<FabricRequisitionDetail> Gets(int id, Int64 nUserID);
        List<FabricRequisitionDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        FabricRequisitionDetail Save(FabricRequisitionDetail oFabricRequisitionDetail, Int64 nUserID);
    }
    #endregion
}
