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
    #region DUReturnChallanDetail
    public class DUReturnChallanDetail : BusinessObject
    {
        public DUReturnChallanDetail()
        {
            DUReturnChallanDetailID = 0;
            DUReturnChallanID = 0;
            DUDeliveryChallanDetailID = 0;
            LotID = 0;
            ProductID = 0;
            MUnitID = 0;
            Qty = 0;
            Qty_Order = 0;
            PTUID = 0;
            Note = string.Empty;
            RCNo = "";
            RCDate = "";
            ErrorMessage = "";
            ColorName = "";
            UnitPrice = 0;
            ColorNo = "";
            Shade = 0;
            BagQty = 0;
            HanksCone = 0;
            DyeingOrderID = 0;
            DUDeliveryChallanID = 0;
            ContractorName = "";
            ChallanDate = DateTime.Now;
            ChallanNo = "";


        }

        #region Property
        public int DUReturnChallanDetailID { get; set; }
        public int DUReturnChallanID { get; set; }
        public int DUDeliveryChallanDetailID { get; set; }
        public int LotID { get; set; }
        public int ProductID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public int DyeingOrderID { get; set; }
        public int DUDeliveryChallanID { get; set; }
       
        
        public int MUnitID { get; set; }
        public double Qty { get; set; }
        public string Note { get; set; }
        public double Qty_Order { get; set; }

        public DateTime ChallanDate { get; set; }
        public string ChallanNo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string PINo { get; set; }
        public string OrderNo { get; set; }
        public string MUnit { get; set; }
        public string ColorName { get; set; }
        public int Shade { get; set; }
        public string ColorNo { get; set; }
        public string LotNo { get; set; }
        public string ContractorName { get; set; }
        public string RCNo { get; set; }
        public string RCDate { get; set; }
        public int PTUID { get; set; }
        public double UnitPrice { get; set; }
        public double BagQty { get; set; }
        public int HanksCone { get; set; }
    
        #endregion

        #region Functions

        public DUReturnChallanDetail Get(int id, long nUserID)
        {
            return DUReturnChallanDetail.Service.Get(id, nUserID);
        }
        public static List<DUReturnChallanDetail> Gets(int nID, long nUserID)
        {
            return DUReturnChallanDetail.Service.Gets(nID, nUserID);
        }

        public static List<DUReturnChallanDetail> Gets(string sSQL, long nUserID)
        {
            return DUReturnChallanDetail.Service.Gets(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDUReturnChallanDetailService Service
        {
            get { return (IDUReturnChallanDetailService)Services.Factory.CreateService(typeof(IDUReturnChallanDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IDUReturnChallanDetail interface
    public interface IDUReturnChallanDetailService
    {
        DUReturnChallanDetail Get(int id, Int64 nUserID);
        List<DUReturnChallanDetail> Gets(int nID, Int64 nUserID);
        List<DUReturnChallanDetail> Gets(string sSQL, Int64 nUserID);

    }
    #endregion
}
