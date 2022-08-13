using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Data;


namespace ESimSol.BusinessObjects
{
    #region RequisitionSlipDetail
    public class TransferRequisitionSlipDetail : BusinessObject
    {
        #region  Constructor
        public TransferRequisitionSlipDetail()
        {
            TRSDetailID = 0;
            TRSID = 0;
            StyleID = 0;
            ProductID = 0;
            LotID = 0;
            QTY = 0;
            MUnitID = 0;
            BagBales = 0;
            UnitPrice = 0;
            CurrencyID = 0;
            SuggestLotNo = "";
            Remark = "";
            ReceiveDate = DateTime.MinValue;
            DestinationLotID = 0;
            MUName = "";
            MUSymbol = "";
            CSymbol = "";
            LotNo = "";
            ColorName = "";
            SizeName = "";
            DestinationLotNo = "";
            LotCurrentBalance = 0;
            ProductName = "";
            ProductCode = "";
            StyleNo = "";
            BuyerName = "";
            SupplierSName = "";
            ErrorMessage = "";
        }
        #endregion

        #region Properties
        public int TRSDetailID { get; set; }
        public int TRSID { get; set; }
        public int StyleID { get; set; }
        public int ProductID { get; set; }
        public int LotID { get; set; }
        public double QTY { get; set; }
        public int MUnitID { get; set; }
        public double BagBales { get; set; }
        public double UnitPrice { get; set; }
        public int CurrencyID { get; set; }
        public string SuggestLotNo { get; set; }
        public string Remark { get; set; }
        public DateTime ReceiveDate { get; set; }
        public int DestinationLotID { get; set; }
        public string MUName { get; set; }
        public string MUSymbol { get; set; }
        public string CSymbol { get; set; }
        public string LotNo { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string DestinationLotNo { get; set; }
        public double LotCurrentBalance { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string StyleNo { get; set; }
        public string BuyerName { get; set; }
        public string SupplierSName { get; set; }
        public string ErrorMessage { get; set; }

        #region Derived Properties
        public string ReceiveDateSt
        {
            get
            {
                if (this.ReceiveDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.ReceiveDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion
        #endregion

        #region Functions
        public TransferRequisitionSlipDetail Get(int nRequisitionSlipDetailID, long nUserID)
        {
            return TransferRequisitionSlipDetail.Service.Get(nRequisitionSlipDetailID, nUserID);
        }
        public string Delete(Int64 nUserID)
        {
            return TransferRequisitionSlipDetail.Service.Delete(this, nUserID);
        }
        public static List<TransferRequisitionSlipDetail> Gets(string sSQL, long nUserID)
        {
            return TransferRequisitionSlipDetail.Service.Gets(sSQL, nUserID);
        }
        public static List<TransferRequisitionSlipDetail> Gets(int nTRSID, long nUserID)
        {
            return TransferRequisitionSlipDetail.Service.Gets(nTRSID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ITransferRequisitionSlipDetailService Service
        {
            get { return (ITransferRequisitionSlipDetailService)Services.Factory.CreateService(typeof(ITransferRequisitionSlipDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IRequisitionSlipDetail interface
    public interface ITransferRequisitionSlipDetailService
    {
        TransferRequisitionSlipDetail Get(int id, Int64 nUserID);
        List<TransferRequisitionSlipDetail> Gets(string sSQL, Int64 nUserID);
        List<TransferRequisitionSlipDetail> Gets(int nTRSID, Int64 nUserID);
        string Delete(TransferRequisitionSlipDetail oExportSCDetail, Int64 nUserID);
    }
    #endregion
}