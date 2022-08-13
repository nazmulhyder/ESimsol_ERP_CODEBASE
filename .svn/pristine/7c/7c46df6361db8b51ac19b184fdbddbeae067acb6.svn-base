using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DUProductionYetTo
    public class DUProductionYetTo : BusinessObject
    {
        public DUProductionYetTo()
        {
            OrderNo = "";
            OrderDate = DateTime.Now;
            PINo = "";
            ContractorID = 0;
            CategoryName = "";
            ContractorName = "";
            ProductCode = 0;
            ProductName = "";
            ProductID = 0;
            DyeingOrderType = 0;
            DyeingOrderID = 0;
            OrderType = "";
            Qty = 0.0;
            Qty_Prod = 0.0;
            Qty_DC = 0.0;
            StockInHand = 0;
            Qty_Unit = 0.0;
            Qty_Req = 0.0;
            ErrorMessage = "";
            MKTPName = "";
            MKTPNickName = "";
            ColorCount = 0;
            MUName = "";
            Params = "";
            HankorCone = 0;
            BuyerConcern = "";
            Qty_DCToDay=0.0;
            Qty_QCToDay=0.0;
            IsInHouse = false;
            EndBuyer = "";
        }

        #region Properties
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string PINo { get; set; }
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public string BuyerConcern { get; set; }
        public string CategoryName { get; set; }
        public int ProductCode { get; set; }
        public string ProductName { get; set; }
        public int DyeingOrderType { get; set; }
        public int DyeingOrderID { get; set; }
        public int ProductID { get; set; }
        public string OrderType { get; set; }
        public double Qty { get; set; }
        public double Qty_Prod { get; set; } 
        public double Qty_Req { get; set; }
        public double Qty_DC { get; set; }
        public double StockInHand { get; set; }
        public EumDyeingType HankorCone { get; set; }
        public double Qty_Unit { get; set; }
        public string MKTPName { get; set; }
        public int ColorCount { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set;}
        public string MUName { get; set; }
        public string EndBuyer { get; set; }
        public string MKTPNickName { get; set; }
        public double Qty_DCToDay { get; set; }
        public double Qty_QCToDay { get; set; }
        public List<DUDyeingTypeMapping> DUDyeingTypeMapping { get; set; }
        public bool IsInHouse { get; set; }
        #endregion

        #region Derive Property
        public string HankorConeInStr
        {
            get
            {
                return this.HankorCone.ToString();
            }
        }
        public string OrderDateInStr
        {
            get
            {
                return this.OrderDate.ToString("dd MMM yyyy");
            }
        }
        public string YetToProduction
        {
            get
            {
                return ((this.Qty - this.Qty_Prod) < 0) ? "0": (this.Qty - this.Qty_Prod).ToString();
            }
        }
        public string YetToDelivery
        {
            get
            {
                return (this.Qty - this.Qty_DC).ToString();
            }
        }
        public double ReqTime
        {
            get
            {
                return (((this.Qty - this.Qty_Prod) * this.Qty_Unit) < 0 ? 0 : (this.Qty - this.Qty_Prod) * this.Qty_Unit);
            }
        }
        public string ReqTimeInStr
        {
            get
            {
                return ((this.Qty - this.Qty_Prod) * this.Qty_Unit).ToString();
            }
        }
        #endregion
        
      
        #region Functions
        public DUProductionYetTo Get(int id, Int64 nUserID)
        {
            return DUProductionYetTo.Service.Get(id, nUserID);
        }
        public static List<DUProductionYetTo> Gets(string sSQL, Int64 nUserID)
        {
            return DUProductionYetTo.Service.Gets(sSQL, nUserID);
        }
        #endregion
        #region Non DB Functions
        #endregion

        #region ServiceFactory
        internal static IDUProductionYetToService Service
        {
            get { return (IDUProductionYetToService)Services.Factory.CreateService(typeof(IDUProductionYetToService)); }
        }
        #endregion

        
    }
    #endregion

    #region IDUProductionYetTo interface
    public interface IDUProductionYetToService
    {
        DUProductionYetTo Get(int id, Int64 nUserID);
        List<DUProductionYetTo> Gets(string sSQL, Int64 nUserID);
       
    }
    #endregion
}

