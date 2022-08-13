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
    #region KnittingOrderRegister
    public class KnittingOrderRegister : BusinessObject
    {
        public KnittingOrderRegister()
        {
            KnittingOrderID  = 0;
            KnittingOrderNo = "";
            OrderDate = DateTime.Now;
            StartDate = DateTime.Now;
            ApproxCompleteDate = DateTime.Now;
            BusinessSessionID = 0;
            BusinessSessionName = "";
            FactoryID = 0;
            FactoryName = "";

            KnittingYarnID = 0;
            YarnID = 0;
            YarnName = "";
            YarnCode = "";
            YarnChallanQty = 0;
            YarnMUnitID = 0;
            YarnMUnitSymbol = "";
            YarnConsumptionQty = 0;
            YarnReturnQty = 0;
            YarnProcessLossQty = 0;
            YarnBalanceQty = 0;

            KnittingOrderDetailID = 0;
            FabricID = 0;
            FabricName = "";
            StyleID = 0;
            StyleNo = "";
            PAM = "";
            BuyerName = "";
            GSM = "";
            MICDia = "";
            FinishDia = "";
            ColorID = 0;
            ColorName = "";
            FabricQty = 0;
            FabricUnitPrice = 0;
            FabricMUnitID = 0;
            FabricMUnitSymbol = "";
            FabricAmount = 0;
            BrandName = "";
            FabricRecvQty = 0;
            FabricStyleQty = 0;
            FabricYetRecvQty = 0;
            OrderType = EnumKnittingOrderType.None;
            ErrorMessage = "";
        }



        #region Property
        public int KnittingOrderID { get; set; }        
        public string KnittingOrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ApproxCompleteDate { get; set; }
        public int BusinessSessionID { get; set; }
        public string BusinessSessionName { get; set; }
        public int FactoryID { get; set; }
        public string FactoryName { get; set; }
        public EnumKnittingOrderType OrderType { get; set; }
        public int BUID { get; set; }

        public int KnittingYarnID { get; set; }        
        public int YarnID { get; set; }
        public string YarnName { get; set; }
        public string YarnCode { get; set; }
        public double YarnChallanQty { get; set; }
        public int YarnMUnitID { get; set; }
        public string YarnMUnitSymbol { get; set; }
        public double YarnConsumptionQty { get; set; }
        public double YarnReturnQty { get; set; }
        public double YarnProcessLossQty { get; set; }
        public double YarnBalanceQty { get; set; }
        

        public int KnittingOrderDetailID { get; set; }
        public int FabricID { get; set; }
        public string FabricName { get; set; }
        public int StyleID { get; set; }
        public string StyleNo { get; set; }
        public string PAM { get; set; }
        public string BuyerName { get; set; }
        public string GSM { get; set; }
        public string MICDia { get; set; }
        public string FinishDia { get; set; }
        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public double FabricQty { get; set; }
        public double FabricUnitPrice { get; set; }
        public int FabricMUnitID { get; set; }
        public string FabricMUnitSymbol { get; set; }
        public double FabricAmount { get; set; }
        public string BrandName { get; set; }
        public double FabricRecvQty { get; set; }
        public double FabricStyleQty { get; set; }
        public double FabricYetRecvQty { get; set; }

        public string ErrorMessage { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property
        public string OrderTypeInString
        {
            get
            {
                return EnumObject.jGet(this.OrderType);
            }
        }
        public string OrderDateInString
        {
            get
            {
                if (OrderDate == DateTime.MinValue) return "";
                return OrderDate.ToString("dd MMM yyyy");
            }
        }

        public string StartDateInString
        {
            get
            {
                if (StartDate == DateTime.MinValue) return "";
                return StartDate.ToString("dd MMM yyyy");
            }
        }

        public string ApproxCompleteDateInString
        {
            get
            {
                if (ApproxCompleteDate == DateTime.MinValue) return "";
                return ApproxCompleteDate.ToString("dd MMM yyyy");
            }
        }

        public string FabricStyleQtyInString
        {
            get
            {
                if (StyleID == 0) return "-";
                return FabricStyleQty.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string FabricQtyInString
        {
            get
            {
                if (FabricID == 0) return "-";
                return FabricQty.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string FabricUnitPriceInString
        {
            get
            {
                if (FabricID == 0) return "-";
                return FabricUnitPrice.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string FabricAmountInString
        {
            get
            {
                if (FabricID == 0) return "-";
                return FabricAmount.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string FabricRecvQtyInString
        {
            get
            {
                if (FabricID == 0) return "-";
                return FabricRecvQty.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string FabricYetRecvQtyInString
        {
            get
            {
                if (FabricID == 0) return "-";
                return FabricYetRecvQty.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string YarnChallanQtyInString
        {
            get
            {
                if (YarnID == 0) return "-";
                return YarnChallanQty.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string YarnConsumptionQtyInString
        {
            get
            {
                if (YarnID == 0) return "-";
                return YarnConsumptionQty.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string YarnReturnQtyInString
        {
            get
            {
                if (YarnID == 0) return "-";
                return YarnReturnQty.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string YarnProcessLossQtyInString
        {
            get
            {
                if (YarnID == 0) return "-";
                return YarnProcessLossQty.ToString("#,##0.00;(#,##0.00)");
            }
        }
        public string YarnBalanceQtyInString
        {
            get
            {
                if (YarnID == 0) return "-";
                return YarnBalanceQty.ToString("#,##0.00;(#,##0.00)");
            }
        }
        #endregion

        #region Functions
        public static List<KnittingOrderRegister> Gets(int nKnittingOrderID, long nUserID)
        {
            return KnittingOrderRegister.Service.Gets(nKnittingOrderID, nUserID);
        }
        public static List<KnittingOrderRegister> Gets(string sSQL, long nUserID)
        {
            return KnittingOrderRegister.Service.Gets(sSQL, nUserID);
        }
        public static List<KnittingOrderRegister> GetsForOrderStatusWise(string sSQL, long nUserID)
        {
            return KnittingOrderRegister.Service.GetsForOrderStatusWise(sSQL, nUserID);
        }
        public KnittingOrderRegister Get(int id, long nUserID)
        {
            return KnittingOrderRegister.Service.Get(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IKnittingOrderRegisterService Service
        {
            get { return (IKnittingOrderRegisterService)Services.Factory.CreateService(typeof(IKnittingOrderRegisterService)); }
        }
        #endregion

        public List<KnittingOrderRegister> KnittingOrderRegisters { get; set; }
    }
    #endregion

    #region IKnittingOrderRegister interface
    public interface IKnittingOrderRegisterService
    {
        KnittingOrderRegister Get(int id, Int64 nUserID);
        List<KnittingOrderRegister> Gets(int nKnittingOrderID, Int64 nUserID);
        List<KnittingOrderRegister> Gets(string sSQL, Int64 nUserID);
        List<KnittingOrderRegister> GetsForOrderStatusWise(string sSQL, Int64 nUserID); 
    }
    #endregion
}
