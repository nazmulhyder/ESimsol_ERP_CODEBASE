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

    #region PartsRequisitionDetail

    public class PartsRequisitionDetail : BusinessObject
    {
        public PartsRequisitionDetail()
        {
            PartsRequisitionDetailID = 0;
            PartsRequisitionID = 0;
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
            WorkingUnitID = 0;
            UnitName = "";
            Symbol = "";
            ProductGroupName = "";
            YetToReturnQty = 0;
            PartsRequisitionDetailLogID = 0;
            PartsRequisitionLogID = 0;
            ShelfName = "";
            RackNo = "";
            ErrorMessage = "";
            ChargeType = EnumServiceILaborChargeType.None;
        }

        #region Properties
        public int PartsRequisitionDetailID { get; set; }
        public int PartsRequisitionID { get; set; }
        public int ProductID { get; set; }
        public int LotID { get; set; }
        public int UnitID { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public EnumServiceILaborChargeType ChargeType { get; set; } 
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
        public int WorkingUnitID { get; set; }
        public string SizeName { get; set; }
        public string UnitName { get; set; }
        public string Symbol { get; set; }
        public double YetToReturnQty { get; set; }
        public int PartsRequisitionDetailLogID { get; set; }
        public int PartsRequisitionLogID { get; set; }
        public string ProductGroupName { get; set; }
        public string ShelfName { get; set; }
        public string RackNo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property       
        public string OperationUnitName { get; set; }
        public string LocationName { get; set; }
        public string OUShortName { get; set; }
        public string LocationShortName { get; set; }
        public string ChargeTypeSt
        {
            get
            {
                return this.ChargeType.ToString();
            }
        }
        public string WorkingUnitName
        {
            get { return this.LocationName + "[" + this.OperationUnitName + "]"; }

        }
        public string UnitPriceInString
        {
            get
            {
                return Global.MillionFormat(this.UnitPrice) + " " + this.LotNo;

            }
        }
        public string ShelfWithRackNo
        {
            get
            {
                if (string.IsNullOrEmpty(ShelfName) && string.IsNullOrEmpty(RackNo))
                {
                    return "";
                }
                return this.ShelfName + "[" + this.RackNo + "]";
            }
        }
        #endregion

        #region Functions

        public static List<PartsRequisitionDetail> Gets(int ProductID, long nUserID)
        {
            return PartsRequisitionDetail.Service.Gets(ProductID, nUserID);
        }
        public static List<PartsRequisitionDetail> GetsLog(int id, long nUserID)
        {
            return PartsRequisitionDetail.Service.GetsLog(id, nUserID);
        }
        public static List<PartsRequisitionDetail> Gets(string sSQL, long nUserID)
        {

            return PartsRequisitionDetail.Service.Gets(sSQL, nUserID);
        }
        public PartsRequisitionDetail Get(int PartsRequisitionDetailID, long nUserID)
        {
            return PartsRequisitionDetail.Service.Get(PartsRequisitionDetailID, nUserID);
        }

        public PartsRequisitionDetail Save(long nUserID)
        {
            return PartsRequisitionDetail.Service.Save(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IPartsRequisitionDetailService Service
        {
            get { return (IPartsRequisitionDetailService)Services.Factory.CreateService(typeof(IPartsRequisitionDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IPartsRequisitionDetail interface

    public interface IPartsRequisitionDetailService
    {
        PartsRequisitionDetail Get(int PartsRequisitionDetailID, Int64 nUserID);
        List<PartsRequisitionDetail> Gets(int ProductID, Int64 nUserID);
        List<PartsRequisitionDetail> GetsLog(int id, Int64 nUserID);
        List<PartsRequisitionDetail> Gets(string sSQL, Int64 nUserID);
        PartsRequisitionDetail Save(PartsRequisitionDetail oPartsRequisitionDetail, Int64 nUserID);
    }
    #endregion

}
