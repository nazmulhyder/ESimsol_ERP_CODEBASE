using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region FabricDeliveryOrder

    public class FabricDeliveryOrder : BusinessObject
    {
        #region  Constructor
        public FabricDeliveryOrder()
        {
            FDOID = 0;
            FDODID = 0;
            FDOType = EnumDOType.None;
            FDOTypeInInt = (int)EnumDOType.None;
            DONo = "";
            DOStatus = EnumFabricDOStatus.Initialized;
            DOStatusInInt = 0;
            DODate = DateTime.Now;
            DeliveryPoint = "";
            ExpDeliveryDate = DateTime.Now;
            CurrencyID = 0;
            ContractorID = 0;
            Note = "";
            ApproveBy = 0;
            ApproveDate = DateTime.MinValue;
            ErrorMessage = "";
            Params = "";
            Qty = 0;
            SCNo = "";
            PINo = "";
            IsFinish = false;
            BUID = 0;
            PreparedByName = "";
            ContactPersonnelName = "";
            BuyerID = 0;
            BCPID = 0;
            MKTPersonID = 0;
            FDODetails = new List<FabricDeliveryOrderDetail>();
            FDONotes = new List<FDONote>();
            FEOID = 0;
            LCAmount = 0;
            PIAmount = 0;
            BuyerAddress = "";
            FactoryAddress = "";
            BuyerCPName = "";
            BuyerCPPhone = "";
            DeliveryToCPName = "";
            DeliveryToCPPhone = "";
            ContractorName = "";
            IsRevise = false;
            Qty_DC = 0;
            ExportPIID = 0;
            LCNo = "";
            CheckedByName = "";
            AttachDocumentID = 0;
        }
        #endregion
      
        #region Properties
        public int FDOID { get; set; }
        public int FDODID { get; set; }
        public int FEOID { get; set; }
        public int DeliveryZoneID { get; set; }
        public string DONo { get; set; }
        public EnumFabricDOStatus DOStatus { get; set; }
        public DateTime DODate { get; set; }
        public int DOStatusInInt { get; set; }
      
        public string DeliveryPoint { get; set; }
        public DateTime ExpDeliveryDate { get; set; }
        public int CurrencyID { get; set; }
        public int ContractorID { get; set; }// Delivery To
        public int IssueToID { get; set; }
        public int BuyerID { get; set; }
        public string Note { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public int CheckedBy { get; set; }
        public DateTime CheckedDate { get; set; }
        public EnumDOType FDOType { get; set; }
        public int FDOTypeInInt { get; set; }
        public string ErrorMessage { get; set; }
        public string SCNo { get; set; }
        public int BCPID { get; set; }
        public int MKTPersonID { get; set; }
        public string ContactPersonnelName { get; set; }
        public double LCAmount { get; set; }
        public double PIAmount { get; set; }
        public bool IsFinish { get; set; }
        public bool IsDisPo { get; set; }
        public int DeliveryFrom_BUID { get; set; }
        public int ExportPIID { get; set; }
        public int BUID { get; set; }
        public bool IsRevise { get; set; } 
        public string PreparedByName { get; set; }
        public int AttachDocumentID { get; set; } 
        #endregion

        #region Derive Properties
        public string BuyerCPName { get; set; }
        public string BuyerCPPhone { get; set; }
        public string DeliveryToCPName { get; set; }
        public string DeliveryToCPPhone { get; set; }
        public string BuyerAddress { get; set; }
        public string FactoryAddress { get; set; }
        public string CurrencyName { get; set; }
        public string OrderRef { get; set; }
        public bool IsInHouse { get; set; }
        public string Construction { get; set; }
        public string PINo { get; set; }
        public string LCNo { get; set; }
        public int PaymentType { get; set; }
        public int BuyerIDEPI { get; set; }
        public string BuyerName { get; set; }
        public string ContractorName { get; set; }
        public string DeliveryToName { get; set; }
        public string MKTPerson { get; set; }
        public string ApproveByName { get; set; }
        public string CheckedByName { get; set; }
        public double Qty_DC { get; set; }
        public double FEOQty { get; set; }
        public double Qty { get; set; }
        public System.Drawing.Image Signature { get; set; }
      
        public string FDOTypeInSt
        {
            get
            {
                return EnumObject.jGet((EnumDOType)this.FDOType).ToString();
            }

        }
        public string DOStatusSt
        {
            get
            {
                return EnumObject.jGet((EnumFabricDOStatus)this.DOStatus).ToString();
            }

        }
     
        public string OrderNo { get { return this.FEOID > 0 ? this.IsInHouse ? "EXE-" + this.SCNo : "SWC-" + this.SCNo : ""; } }
        public string DODateSt { get { return this.DODate.ToString("dd MMM yyyy"); } }
        public string ExpDeliveryDateSt { get { return this.ExpDeliveryDate.ToString("dd MMM yyyy"); } }
        public string ApproveDateInStr { get { if (this.ApproveBy > 0 && this.ApproveDate != DateTime.MinValue) return this.ApproveDate.ToString("dd MMM yyyy"); else  return ""; } }
        public string QtySt { get { return Global.MillionFormat(this.Qty); } }
        public string Qty_DCSt { get { return Global.MillionFormat(this.Qty-this.Qty_DC); } }
        public List<FabricDeliveryOrderDetail> FDODetails { get; set; }
        public List<FDONote> FDONotes { get; set; }
        public string Params { get; set; }
        public string IsFinishSt
        {
            get
            {
                if (this.IsFinish) return "Finished";
                else return "-";
            }
        }
    
    
        #endregion
    

        #region Functions
        public  FabricDeliveryOrder Get(int nFDOID, long nUserID)
        {
            return FabricDeliveryOrder.Service.Get(nFDOID, nUserID);
        }
        public static List<FabricDeliveryOrder> Gets(string sSQL, long nUserID)
        {
            return FabricDeliveryOrder.Service.Gets(sSQL, nUserID);
        }
        public FabricDeliveryOrder IUD(int nDBOperation,long nUserID)
        {
            return FabricDeliveryOrder.Service.IUD(this,nDBOperation, nUserID);
        }
        public FabricDeliveryOrder Approved(Int64 nUserID)
        {
            return FabricDeliveryOrder.Service.Approved(this, nUserID);
        }
        public FabricDeliveryOrder Checked(Int64 nUserID)
        {
            return FabricDeliveryOrder.Service.Checked(this, nUserID);
        }
        
        public FabricDeliveryOrder Cancel(Int64 nUserID)
        {
            return FabricDeliveryOrder.Service.Cancel(this, nUserID);
        }
        public FabricDeliveryOrder SaveLog(Int64 nUserID)
        {
            return FabricDeliveryOrder.Service.SaveLog(this, nUserID);
        }
        public FabricDeliveryOrder UpdateFDOStatus(int nFDOID, int nStatus,string sDeliveryPoint, long nUserID)
        {
            return FabricDeliveryOrder.Service.UpdateFDOStatus(nFDOID, nStatus,sDeliveryPoint, nUserID);
        }
        public FabricDeliveryOrder UpdateFinish(int nFDOID, bool bIsFinish, long nUserID)
        {
            return FabricDeliveryOrder.Service.UpdateFinish(nFDOID, bIsFinish, nUserID);
        }
        public string Delete(int nUserID)
        {
            return FabricDeliveryOrder.Service.Delete(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricDeliveryOrderService Service
        {
            get { return (IFabricDeliveryOrderService)Services.Factory.CreateService(typeof(IFabricDeliveryOrderService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricDeliveryOrder interface
    public interface IFabricDeliveryOrderService
    {
        FabricDeliveryOrder Get(int nFDOID, long nUserID);
        List<FabricDeliveryOrder> Gets(string sSQL, long nUserID);
        FabricDeliveryOrder IUD(FabricDeliveryOrder oFabricDeliveryOrder, int nDBOperation, long nUserID);
        FabricDeliveryOrder SaveLog(FabricDeliveryOrder oFabricDeliveryOrder, Int64 nUserID);
        FabricDeliveryOrder Approved(FabricDeliveryOrder oFabricDeliveryOrder, Int64 nUserID);
        FabricDeliveryOrder Checked(FabricDeliveryOrder oFabricDeliveryOrder, Int64 nUserID);
        FabricDeliveryOrder Cancel(FabricDeliveryOrder oFabricDeliveryOrder, Int64 nUserID);
        FabricDeliveryOrder UpdateFDOStatus(int nFDOID,int nStatus,string sDeliveryPoint, long nUserID);
        FabricDeliveryOrder UpdateFinish(int nFDOID, bool bIsFinish, Int64 nUserID);
        string Delete(FabricDeliveryOrder oFabricDeliveryOrder, Int64 nUserID);
    }
    #endregion
}
