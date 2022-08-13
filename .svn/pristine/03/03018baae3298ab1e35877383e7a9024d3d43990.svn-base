using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{

    #region ConsumptionRequisitionDetail

    public class ConsumptionRequisitionDetail : BusinessObject
    {
        public ConsumptionRequisitionDetail()
        {
            ConsumptionRequisitionDetailID = 0;
            ConsumptionRequisitionID = 0;
            ProductID = 0;
            LotID = 0;
            UnitID = 0;
            Quantity = 0;
            UnitPrice = 0;
            Amount = 0;
            ProductCode = "";
            ProductName = "";
            LotNo = "";
            Balance = 0;
            LotUnitPrice = 0;
            LotUnitID = 0;
            StyleID = 0;
            ColorID = 0;
            SizeID = 0;
            StyleNo = "";
            BuyerName = "";
            ColorName = "";
            SizeName = "";
            UnitName = "";
            Symbol = "";
            ProductGroupName = "";
            YetToReturnQty = 0;
            ConsumptionRequisitionDetailLogID = 0;
            ConsumptionRequisitionLogID = 0;
            ErrorMessage = "";
        }

        #region Properties
        public int ConsumptionRequisitionDetailID { get; set; }
        public int ConsumptionRequisitionID { get; set; }
        public int ProductID { get; set; }
        public int LotID { get; set; }
        public int UnitID { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string LotNo { get; set; }
        public double Balance { get; set; }
        public double LotUnitPrice { get; set; }
        public int LotUnitID { get; set; }
        public int StyleID { get; set; }
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public string StyleNo { get; set; }
        public string BuyerName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string UnitName { get; set; }
        public string Symbol { get; set; }
        public double YetToReturnQty { get; set; }
        public int ConsumptionRequisitionDetailLogID { get; set; }
        public int ConsumptionRequisitionLogID { get; set; }
        public string ProductGroupName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property        
        public string UnitPriceInString
        {
            get
            {
                return Global.MillionFormat(this.UnitPrice) + " " + this.LotNo;

            }
        }
        #endregion

        #region Functions

        public static List<ConsumptionRequisitionDetail> Gets(int ProductID, long nUserID)
        {
            return ConsumptionRequisitionDetail.Service.Gets(ProductID, nUserID);
        }
        public static List<ConsumptionRequisitionDetail> GetsLog(int id, long nUserID)
        {
            return ConsumptionRequisitionDetail.Service.GetsLog(id, nUserID);
        }
        public static List<ConsumptionRequisitionDetail> Gets(string sSQL, long nUserID)
        {

            return ConsumptionRequisitionDetail.Service.Gets(sSQL, nUserID);
        }
        public ConsumptionRequisitionDetail Get(int ConsumptionRequisitionDetailID, long nUserID)
        {
            return ConsumptionRequisitionDetail.Service.Get(ConsumptionRequisitionDetailID, nUserID);
        }

        public ConsumptionRequisitionDetail Save(long nUserID)
        {
            return ConsumptionRequisitionDetail.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IConsumptionRequisitionDetailService Service
        {
            get { return (IConsumptionRequisitionDetailService)Services.Factory.CreateService(typeof(IConsumptionRequisitionDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IConsumptionRequisitionDetail interface

    public interface IConsumptionRequisitionDetailService
    {
        ConsumptionRequisitionDetail Get(int ConsumptionRequisitionDetailID, Int64 nUserID);
        List<ConsumptionRequisitionDetail> Gets(int ProductID, Int64 nUserID);
        List<ConsumptionRequisitionDetail> GetsLog(int id, Int64 nUserID);
        List<ConsumptionRequisitionDetail> Gets(string sSQL, Int64 nUserID);
        ConsumptionRequisitionDetail Save(ConsumptionRequisitionDetail oConsumptionRequisitionDetail, Int64 nUserID);
    }
    #endregion

}
