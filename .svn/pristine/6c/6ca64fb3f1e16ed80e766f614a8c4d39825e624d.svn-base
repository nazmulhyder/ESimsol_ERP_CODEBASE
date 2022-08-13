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
   public class FabricReceiveRegister:BusinessObject
    {
        public FabricReceiveRegister()
        {
            KnittingFabricReceiveDetailID = 0;
            KnittingFabricReceiveID = 0;
            KnittingOrderDetailID = 0;
            FabricID = 0;
            ReceiveStoreID = 0;
            LotID = 0;
            NewLotNo = "";
            MUnitID = 0;
            Qty = 0;
            FabricReceiveDetailsRemarks = "";
            ProcessLossQty = 0;
            ErrorMessage = "";
            LotMUSymbol = "";
            PAM = 0;
            GSM = "";
            MICDia = "";
            FinishDia = "";
            MUnitName = "";
            OperationUnitName = "";
            FabricName = "";
            FabricCode = "";
            LotNo = "";
            LotBalance = 0;
            LotMUSymbol = "";

            KnittingOrderID = 0;
            ReceiveNo = "";
            ReceiveDate = DateTime.Now;
            PartyChallanNo = "";
            FabricReceiveRemarks = "";
            ApprovedBy = 0;
            ApprovedByName = "";
            BUID = 0;
            
            KnittingOrderDate = DateTime.Now;
            KnittingOrderNo = "";
            BuyerName = "";
            StyleNo = "";
            OrderQty = 0;
            OrderType = EnumKnittingOrderType.None;
            StartDate = DateTime.Now;
            BusinessSessionName = "";
            FactoryName = "";
            BuyerID=0;
            StyleID=0;
            FactoryID = 0;
        }
        #region Property
        public int BuyerID { get; set; }
        public int StyleID { get; set; }
        public int FactoryID { get; set; }
        public int KnittingFabricReceiveDetailID { get; set; }
        public int KnittingFabricReceiveID { get; set; }
        public int KnittingOrderDetailID { get; set; }
        public int FabricID { get; set; }
        public int ReceiveStoreID { get; set; }
        public int LotID { get; set; }
        public string NewLotNo { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public double ProcessLossQty { get; set; }
        public string FabricReceiveDetailsRemarks { get; set; }
        public string GSM { get; set; }
        public string MICDia { get; set; }
        public string FinishDia { get; set; }
        public string MUnitName { get; set; }
        public string OperationUnitName { set; get; }
        public string FabricName { get; set; }
        public string FabricCode { get; set; }

        public string LotNo { get; set; }
        public double LotBalance { get; set; }
        public string LotMUSymbol { get; set; }
        public int PAM { get; set; }
        public int KnittingOrderID { get; set; }
        public string ReceiveNo { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string PartyChallanNo { get; set; }
        public string FabricReceiveRemarks { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public int BUID { get; set; }
        public string KnittingOrderNo { get; set; }
        public DateTime KnittingOrderDate { get; set; }
        public string BuyerName { get; set; }
        public string StyleNo { get; set; }
        public double OrderQty { get; set; }
        public string BusinessSessionName { get; set; }
        public EnumKnittingOrderType OrderType { get; set; }
        public string FactoryName { get; set; }
        public DateTime StartDate { get; set; }
        public string ErrorMessage { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        
       public string OrderTypeInString
        {
            get
            {
                return EnumObject.jGet(this.OrderType);
            }
        }
       public string StartDateInString
       {
           get
           {
               return StartDate.ToString("dd MMM yyyy");
           }
       }
       public string ReceiveDateInString
       {
           get
           {
               return ReceiveDate.ToString("dd MMM yyyy");
           }
       }
       public string KnittingOrderDateInString
       {
           get
           {
               return KnittingOrderDate.ToString("dd MMM yyyy");
           }
       }
        #endregion
        #region Function
        public static List<FabricReceiveRegister> Gets(string sSQL, long nUserID)
        {
            return FabricReceiveRegister.Service.Gets(sSQL, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IFabricReceiveRegisterService Service
        {
            get { return (IFabricReceiveRegisterService)Services.Factory.CreateService(typeof(IFabricReceiveRegisterService)); }
        }
        #endregion
    }
    #region IYarnChallanRegister interface
    public interface IFabricReceiveRegisterService
    {
        List<FabricReceiveRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
