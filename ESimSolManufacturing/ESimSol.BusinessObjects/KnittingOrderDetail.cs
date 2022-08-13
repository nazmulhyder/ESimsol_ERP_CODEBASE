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
    #region KnittingOrderDetail
    public class KnittingOrderDetail : BusinessObject
    {
        public KnittingOrderDetail()
        {
            KnittingOrderDetailID = 0; 
            KnittingOrderID = 0;
            StyleID = 0;
            OrderQty = 0;
            OrderUnitID = 0;
            PAM = 0;
            FabricID = 0;
            GSMID = 0;
            MICDiaID = 0;
            FinishDiaID = 0;
            GSM = "";
            MICDia = "";
            FinishDia = "";
            ColorID = 0;
            StratchLength = "";
            MUnitID = 0;
            Qty = 0;
            UnitPrice = 0;
            Amount = 0;
            MUSymbol = "";
            Remarks = "";
            BrandName = "";
            KnitDyeingProgramDetailID = 0;
            YetReqFabricQty = 0;
            ErrorMessage = "";
            StylePcsQty = 0;
            CompositionID = 0;
        }

        #region Property
        public int KnittingOrderDetailID { get; set; }
        public int KnittingOrderID { get; set; }
        public int KnitDyeingProgramDetailID { get; set; }
        public int StyleID { get; set; }
        public int OrderQty { get; set; }
        public int OrderUnitID { get; set; }
        public int PAM { get; set; }
        public int FabricID { get; set; }
        public int GSMID { get; set; }
        public int MICDiaID { get; set; }
        public int FinishDiaID { get; set; }
        public int ColorID { get; set; }
        public string StratchLength { get; set; }
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public string BrandName { get; set; }
        public string LotNo { get; set; }
        public string ErrorMessage { get; set; }
        public double StylePcsQty { get; set; }
        public int CompositionID { get; set; }
        #endregion

        #region Derived Property 
        public string GSM { get; set; }
        public string MICDia { get; set; }
        public string FinishDia { get; set; }
        public string StyleNo { get; set; }
        public double YetToChallanQty { get; set; }
        public string BuyerName { get; set; }
        public string FabricName { get; set; }
        public string FabricCode { get; set; }
        public string ColorName { get; set; }
        public string MUnitName { get; set; }
        public string OrderUnitName { get; set; }
        public string MUSymbol { get; set; }
        public double YetReqFabricQty { get; set; }

        public string FabricWithCodeColor
        {
            get
            {
                return
                    ((!string.IsNullOrEmpty(this.StyleNo) ? "[Style No: " + this.StyleNo + "]" : ""))                    
                    + ((!string.IsNullOrEmpty(this.FabricName) ? " - [Fabric: " + this.FabricName + "]" : ""))
                    + (((!string.IsNullOrEmpty(this.FabricCode) ? " - [Code: " + this.FabricCode + "]" : "")))
                    + (((!string.IsNullOrEmpty(this.ColorName) ? " - [Color: " + this.ColorName + "]" : "")))
                    + (((!string.IsNullOrEmpty(this.FinishDia) ? " - [F/D: " + this.FinishDia + "]" : "")));
            }
        }
         public string FabricWithColor
        {
            get
            {
                return "[Style: " + this.StyleNo + "] - " + "[Fabric: " + this.FabricName + "] - " + "[Color: " + this.ColorName + "] - " + (" [Order Qty : " + this.Qty.ToString("#,##0.00") + "] - ") + "[PAM: " + this.PAM + "] - " + "[GSM: " + this.GSM + "] - " + "[MIC Dia: " + this.MICDia + "] - " + "[Finish Dia: " + this.FinishDia + "]";
            }
        }

         public string StyleWithColor
         {
             get
             {
                 return this.StyleNo + "(" + this.ColorName + ")";
             }
         }

        #endregion

        #region Functions
        public static List<KnittingOrderDetail> Gets(long nUserID)
        {
            return KnittingOrderDetail.Service.Gets(nUserID);
        }
        public static List<KnittingOrderDetail> Gets(int id,long nUserID)
        {
            return KnittingOrderDetail.Service.Gets(id,nUserID);
        }
        public static List<KnittingOrderDetail> Gets(string sSQL, long nUserID)
        {
            return KnittingOrderDetail.Service.Gets(sSQL, nUserID);
        }
        public KnittingOrderDetail Get(int id, long nUserID)
        {
            return KnittingOrderDetail.Service.Get(id, nUserID);
        }
        public KnittingOrderDetail Save(long nUserID)
        {
            return KnittingOrderDetail.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return KnittingOrderDetail.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IKnittingOrderDetailService Service
        {
            get { return (IKnittingOrderDetailService)Services.Factory.CreateService(typeof(IKnittingOrderDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IKnittingOrderDetail interface
    public interface IKnittingOrderDetailService
    {
        KnittingOrderDetail Get(int id, Int64 nUserID);
        List<KnittingOrderDetail> Gets(Int64 nUserID);
        List<KnittingOrderDetail> Gets(Int64 id, Int64 nUserID);
        List<KnittingOrderDetail> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        KnittingOrderDetail Save(KnittingOrderDetail oKnittingOrderDetail, Int64 nUserID);
    }
    #endregion
}
