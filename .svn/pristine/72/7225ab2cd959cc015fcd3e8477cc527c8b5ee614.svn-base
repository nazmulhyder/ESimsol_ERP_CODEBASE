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
    #region FabricTransferRequisitionSlipDetail
    public class FabricTransferRequisitionSlipDetail : BusinessObject
    {
        public FabricTransferRequisitionSlipDetail()
        {
            FabricTRSDetailID = 0;
            FabricTRSID = 0;
            ProductID = 0;
            LotID = 0;
            Qty = 0;
            RollQty = 0;
            MUnitID = 0;
            BagBales = 0;
            UnitPrice = 0;
            CurrencyID = 0;
            Remarks = "";
            ReceiveDate = DateTime.Now;
            DestinationLotID = 0;
            LotNo = "";
            FSCDID = 0;
            ProductName = "";
            DispoNo = "";
            FabricID = 0;
            FabricNo = "";
            Construction = "";
            FinishType = 0;
            FinishTypeName = "";
            FabricWeave = 0;
            FabricWeaveName = "";
            ErrorMessage = "";
        }

        #region Property
        public int FabricTRSDetailID { get; set; }
        public int FabricTRSID { get; set; }
        public int ProductID { get; set; }
        public int LotID { get; set; }
        public double Qty { get; set; }
        public int RollQty { get; set; }
        public int MUnitID { get; set; }
        public double BagBales { get; set; }
        public double UnitPrice { get; set; }
        public int CurrencyID { get; set; }
        public string Remarks { get; set; }
        public DateTime ReceiveDate { get; set; }
        public int DestinationLotID { get; set; }
        public string LotNo { get; set; }
        public int FSCDID { get; set; }
        public string ProductName { get; set; }
        public string DispoNo { get; set; }
        public int FabricID { get; set; }
        public string FabricNo { get; set; }
        public string Construction { get; set; }
        public int FinishType { get; set; }
        public string FinishTypeName { get; set; }
        public int FabricWeave { get; set; }
        public string FabricWeaveName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ReceiveDateInString
        {
            get
            {
                return ReceiveDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public static List<FabricTransferRequisitionSlipDetail> Gets(long nUserID)
        {
            return FabricTransferRequisitionSlipDetail.Service.Gets(nUserID);
        }
        public static List<FabricTransferRequisitionSlipDetail> Gets(string sSQL, long nUserID)
        {
            return FabricTransferRequisitionSlipDetail.Service.Gets(sSQL, nUserID);
        }
        public FabricTransferRequisitionSlipDetail Get(int id, long nUserID)
        {
            return FabricTransferRequisitionSlipDetail.Service.Get(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricTransferRequisitionSlipDetailService Service
        {
            get { return (IFabricTransferRequisitionSlipDetailService)Services.Factory.CreateService(typeof(IFabricTransferRequisitionSlipDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricTransferRequisitionSlipDetail interface
    public interface IFabricTransferRequisitionSlipDetailService
    {
        FabricTransferRequisitionSlipDetail Get(int id, Int64 nUserID);
        List<FabricTransferRequisitionSlipDetail> Gets(Int64 nUserID);
        List<FabricTransferRequisitionSlipDetail> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
