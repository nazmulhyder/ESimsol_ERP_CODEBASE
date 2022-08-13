using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DUDeliveryLot
    public class DUDeliveryLot : BusinessObject
    {
        #region  Constructor
        public DUDeliveryLot()
        {
            LotNo = "";
            LotID = 0;
            Qty_DC = 0;
            Product = "";
            LotNo = "";
            Balance = 0;
            Contractor = "";
            PINo = "";
            LCNo = "";
            OrderNo = "";
            OrderType = 0;
            ReportType = 0;
            Qty_Order = 0;
            LocationName = "";
            BagNo = 0;

            #region  DUHardWinding Stock
            Qty_Batch = 0;
            Qty_Warp = 0;
            Qty_Weft = 0;
            ProductID = 0;
            ContractorID = 0;
            PID = 0;
            RouteSheetID = 0;
            WorkingUnitID = 0;
            DODID = 0;
            DOID = 0;
            BuyerID = 0;
            IsInHouse = false;
            #endregion

        }
        #endregion

        #region Properties
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public string Product { get; set; }
        public double Qty_DC { get; set; }
        public double Qty_Order { get; set; }
        public double Balance { get; set; }
        public string RSDate { get; set; }
        public string Contractor { get; set; }
        public string PINo { get; set; }
        public string LCNo { get; set; }
        public string OrderNo { get; set; }
        public string ColorName { get; set; }
        public string OrderTypeSt { get; set; }//EnumOrderType
        public int OrderType { get; set; }//EnumOrderType
        public int ReportType { get; set; }
        private string _sOrderNoFull = "";

        #region DUHardWinding Stock
        public double Qty_Batch { get; set; }
        public double Qty_Warp { get; set; }
        public double Qty_Weft { get; set; }
        public int ProductID { get; set; }
        public int ContractorID { get; set; }
        public int PID { get; set; }
        public int RouteSheetID { get; set; }
        public int WorkingUnitID { get; set; }
        public int DODID { get; set; }
        public int DOID { get; set; }
        public int BuyerID { get; set; }
        public bool IsInHouse { get; set; }
        #endregion
        public string OrderNoFull
        {
            get
            {
                _sOrderNoFull = this.OrderNo;
                return _sOrderNoFull;
            }
        }

        public int LotLocationID { get; set; }
        public string LocationName { get; set; }
        public int BagNo { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Functions
        public static List<DUDeliveryLot> Gets(int nOrderType, int nWorkingUnitID, int nReportType, long nUserID)
        {
            return DUDeliveryLot.Service.Gets(nOrderType, nWorkingUnitID, nReportType, nUserID);
        }
        public static List<DUDeliveryLot> GetsFromAdv(string sSQL, int nReportType, long nUserID)
        {
            return DUDeliveryLot.Service.GetsFromAdv(sSQL, nReportType, nUserID);
        }
        public static List<DUDeliveryLot> GetsDUHardWindingStock(string sSQL, int nReportType, long nUserID)
        {
            return DUDeliveryLot.Service.GetsDUHardWindingStock(sSQL, nReportType, nUserID);
        }
        public static List<DUDeliveryLot> Gets(string sSQL, int nUserID)
        {
            return DUDeliveryLot.Service.Gets(sSQL, nUserID);
        }
        
        #endregion

        #region Non DB Function

        #endregion

        #region ServiceFactory
        internal static IDUDeliveryLotService Service
        {
            get { return (IDUDeliveryLotService)Services.Factory.CreateService(typeof(IDUDeliveryLotService)); }
        }

        #endregion

    }
    #endregion


    #region IDUDeliveryLot interface
    public interface IDUDeliveryLotService
    {
        List<DUDeliveryLot> Gets(int nOrderType, int nWorkingUnitID, int nReportType, Int64 nUserID);
        List<DUDeliveryLot> GetsFromAdv(string sSQL, int nReportType, Int64 nUserID);
        List<DUDeliveryLot> GetsDUHardWindingStock(string sSQL, int nReportType, Int64 nUserID);
        List<DUDeliveryLot> Gets(string sSQL, int nUserID);
    }

    #endregion

}