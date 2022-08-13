using ICS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    public class WUSubContractFabricReceive : BusinessObject
    {
        public WUSubContractFabricReceive()
        {
            WUSubContractFabricReceiveID = 0;
            WUSubContractID = 0;
            ReceiveNo = "";
            ReceiveDate = DateTime.Today;
            PartyChallanNo = "";
            ReceiveStoreID = 0;
            CompositionID = 0;
            Construction = "";
            LotID = 0;
            NewLotNo = "";
            MunitID = 0;
            ReceivedQty = 0;
            RollNo = 0;
            ProcessLossQty = 0;
            Remarks = "";
            ApprovedBy = 0;
            DBUserID = 0;
            DBServerDateTime = DateTime.Today;
            ErrorMessage = "";
            ApprovedByName = "";
            EntyUserName = "";
            JobNo = "";
            ContractDate = DateTime.Today;
            SupplierName = "";
            SupplierCPName = "";           
            BuyerName = "";
            StyleNo = "";
            CompositionName = "";
            SubContractConstruction = "";
            YetToRcvQty = 0;
            MUSymbol = "";
            StoreName = "";
            BUID = 0;
            LotNo = "";
        }

        #region Properties

        public int WUSubContractFabricReceiveID { get; set; }
        public int WUSubContractID { get; set; }
        public string ReceiveNo { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string PartyChallanNo { get; set; }
        public int ReceiveStoreID { get; set; }
        public int CompositionID { get; set; }
        public string Construction { get; set; }
        public int LotID { get; set; }
        public string NewLotNo { get; set; }
        public int MunitID { get; set; }
        public double ReceivedQty { get; set; }
        public int RollNo { get; set; }
        public double ProcessLossQty { get; set; }
        public string Remarks { get; set; }
        public int ApprovedBy { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string ApprovedByName { get; set; }
        public string EntyUserName { get; set; }
        public string JobNo { get; set; }
        public DateTime ContractDate { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCPName { get; set; }
        public string BuyerName { get; set; }
        public string StyleNo { get; set; }
        public string CompositionName { get; set; }
        public string SubContractConstruction { get; set; }
        public double YetToRcvQty { get; set; }
        public string MUSymbol { get; set; }
        public string StoreName { get; set; }
        public int BUID { get; set; }
        public string LotNo { get; set; }

        #endregion

        #region Derived Property

        public string ReceivedQtySt
        {
            get
            {
                return this.ReceivedQty.ToString("#,##0.00");
            }
        }
        public string ProcessLossQtySt
        {
            get
            {
                return this.ProcessLossQty.ToString("#,##0.00");
            }
        }
        public string YetToRcvQtySt
        {
            get
            {
                return this.YetToRcvQty.ToString("#,##0.00");
            }
        }
        public string ReceiveDateSt
        {
            get
            {
                return this.ReceiveDate.ToString("dd MMM yyyy");
            }
        }

        #endregion

        #region Functions

        public WUSubContractFabricReceive Get(int nId, long nUserID)
        {
            return WUSubContractFabricReceive.Service.Get(nId, nUserID);
        }        
        public static List<WUSubContractFabricReceive> Gets(string sSQL, int nCurrentUserID)
        {
            return WUSubContractFabricReceive.Service.Gets(sSQL, nCurrentUserID);
        }
        public WUSubContractFabricReceive Save(long nUserID)
        {
            return WUSubContractFabricReceive.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return WUSubContractFabricReceive.Service.Delete(nId, nUserID);
        }
        public WUSubContractFabricReceive Approve(long nUserID)
        {
            return WUSubContractFabricReceive.Service.Approve(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IWUSubContractFabricReceiveService Service
        {
            get { return (IWUSubContractFabricReceiveService)Services.Factory.CreateService(typeof(IWUSubContractFabricReceiveService)); }
        }
        #endregion
    }


    #region IWUSubContractFabricReceive interface

    public interface IWUSubContractFabricReceiveService
    {
        WUSubContractFabricReceive Get(int id, long nUserID);        
        List<WUSubContractFabricReceive> Gets(string sSQL, int nCurrentUserID);
        string Delete(int id, long nUserID);
        WUSubContractFabricReceive Save(WUSubContractFabricReceive oWUSubContractFabricReceive, long nUserID);
        WUSubContractFabricReceive Approve(WUSubContractFabricReceive oWUSubContractFabricReceive, long nUserID);      
    }

    #endregion
}

