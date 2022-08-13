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
    #region KnittingFabricReceiveDetail
    public class KnittingFabricReceiveDetail : BusinessObject
    {
        public KnittingFabricReceiveDetail()
        {
            KnittingFabricReceiveDetailID = 0;
            KnittingFabricReceiveID = 0;
            KnittingOrderDetailID = 0;
            FabricID = 0;
            ReceiveStoreID = 0;
            LotID = 0;
            MUnitID = 0;
            Qty = 0;
            Remarks = "";
            ProcessLossQty = 0;
            ErrorMessage = "";
            LotMUSymbol = "";
            PAM = 0;
            GSM = "";
            MICDia = "";
            FinishDia = "";
        }

        #region Property 
        public int KnittingFabricReceiveDetailID { get; set; }
        public int KnittingFabricReceiveID { get; set; }
        public int KnittingOrderDetailID { get; set; }
        public int FabricID { get; set; }
        public int ReceiveStoreID { get; set; }
        public int LotID { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public double ProcessLossQty { get; set; }
        public string Remarks { get; set; }
        public string NewLotNo { get; set; }
        public int PAM { get; set; }
        public string GSM { get; set; }
        public string MICDia { get; set; }
        public string FinishDia { get; set; }
        public string ErrorMessage { get; set; }
        #endregion 

        #region Derived Property
        public string MUnitName { get; set; }
        public string OperationUnitName { get; set; }
        public string FabricName { get; set; }
        public string FabricCode { get; set; }
        public string LotNo { get; set; }
        public double LotBalance { get; set; }
        public string LotMUSymbol { get; set; }
       
        #endregion

        #region Functions
        public static List<KnittingFabricReceiveDetail> Gets(long nUserID)
        {
            return KnittingFabricReceiveDetail.Service.Gets(nUserID);
        }
        public static List<KnittingFabricReceiveDetail> Gets(string sSQL, long nUserID)
        {
            return KnittingFabricReceiveDetail.Service.Gets(sSQL, nUserID);
        }
        public KnittingFabricReceiveDetail Get(int id, long nUserID)
        {
            return KnittingFabricReceiveDetail.Service.Get(id, nUserID);
        }
        public KnittingFabricReceiveDetail Save(long nUserID)
        {
            return KnittingFabricReceiveDetail.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return KnittingFabricReceiveDetail.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IKnittingFabricReceiveDetailService Service
        {
            get { return (IKnittingFabricReceiveDetailService)Services.Factory.CreateService(typeof(IKnittingFabricReceiveDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IKnittingFabricReceiveDetail interface
    public interface IKnittingFabricReceiveDetailService
    {
        KnittingFabricReceiveDetail Get(int id, Int64 nUserID);
        List<KnittingFabricReceiveDetail> Gets(Int64 nUserID);
        List<KnittingFabricReceiveDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        KnittingFabricReceiveDetail Save(KnittingFabricReceiveDetail oKnittingFabricReceiveDetail, Int64 nUserID);
    }
    #endregion
}
