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
    #region PartsRequisitionRegister
    public class PartsRequisitionRegister : BusinessObject
    {
        public PartsRequisitionRegister()
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

            ServiceOrderID = 0;
            VehicleRegID = 0;
            RequisitionNo = "";
            BUID = 0;
            PRTypeInt = 0;
            RequisitionBy = 0;
            PRStatus = EnumCRStatus.Initiallize;
            PRStatusInt = 0;
            IssueDate = DateTime.Today;
            StoreID = 0;
            Remarks = "";
            DeliveryBy = 0;
            ApprovedBy = 0;
            StoreCode = "";
            StoreName = "";
            RequisitionByName = "";
            ApprovedByName = "";
            DeliveryByName = "";
            Company = new Company();
            BusinessUnit = new BusinessUnit();

            ReportLayout = EnumReportLayout.None;
            ErrorMessage = "";
        }

        #region Property
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
        public string ProductGroupName { get; set; }

        public int ServiceOrderID { get; set; }
        public int VehicleRegID { get; set; }
        public string RequisitionNo { get; set; }
        public int BUID { get; set; }
        public EnumPRequisutionType PRType { get; set; }
        public int PRTypeInt { get; set; }
        public int RequisitionBy { get; set; }
        public EnumCRStatus PRStatus { get; set; }
        public int PRStatusInt { get; set; }
        public DateTime IssueDate { get; set; }
        public int StoreID { get; set; }
        public string Remarks { get; set; }
        public int DeliveryBy { get; set; }
        public int ApprovedBy { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string ServiceOrderNo { get; set; }
        public string ModelNo { get; set; }
        public string ChassisNo { get; set; }
        public string EngineNo { get; set; }
        public string VehicleRegNo { get; set; }
        public string RequisitionByName { get; set; }
        public string ApprovedByName { get; set; }
        public string DeliveryByName { get; set; }
        public string ActionTypeExtra { get; set; }
        public EnumCRActionType PRActionType { get; set; }
        public int PRActionTypeInt { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int RackID { get; set; }
        public string RackNo { get; set; }
        public string ShelfName { get; set; }
        public string ShelfNo { get; set; }
        public string ErrorMessage { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property
        public string OperationUnitName { get; set; }
        public string LocationName { get; set; }
        public string OUShortName { get; set; }
        public string LocationShortName { get; set; }
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

        public string PRStatusSt
        {
            get
            {
                return EnumObject.jGet(this.PRStatus);
            }
        }
        public string PRTypeSt
        {
            get
            {
                if (this.PRType == 0)
                {
                    return "None";
                }
                return EnumObject.jGet(this.PRType);
            }
        }
        public string IssueDateSt
        {
            get
            {
                return this.IssueDate.ToString("dd MMM yyyy");
            }
        }
        public Company Company { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        #endregion

        #region Functions
        public static List<PartsRequisitionRegister> Gets(int nPartsRequisitionID, long nUserID)
        {
            return PartsRequisitionRegister.Service.Gets(nPartsRequisitionID, nUserID);
        }
        public static List<PartsRequisitionRegister> Gets(string sSQL, long nUserID)
        {
            return PartsRequisitionRegister.Service.Gets(sSQL, nUserID);
        }
        public PartsRequisitionRegister Get(int id, long nUserID)
        {
            return PartsRequisitionRegister.Service.Get(id, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IPartsRequisitionRegisterService Service
        {
            get { return (IPartsRequisitionRegisterService)Services.Factory.CreateService(typeof(IPartsRequisitionRegisterService)); }
        }
        #endregion

        public List<PartsRequisitionRegister> PartsRequisitionRegisters { get; set; }
    }
    #endregion

    #region IPartsRequisitionRegister interface
    public interface IPartsRequisitionRegisterService
    {
        PartsRequisitionRegister Get(int id, Int64 nUserID);
        List<PartsRequisitionRegister> Gets(int nPartsRequisitionID, Int64 nUserID);
        List<PartsRequisitionRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
