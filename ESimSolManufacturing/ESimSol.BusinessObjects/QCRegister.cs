using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region QCRegister
    public class QCRegister : BusinessObject
    {
        public QCRegister()
        {
            QCID = 0;
            ProductionSheetID = 0;
            PassQuantity = 0;
            RejectQuantity = 0;
            ProductID = 0;
            WorkingUnitID = 0;
            OperationTime = DateTime.Now;
            QCPerson = 0;
            SheetNo = "";
            QCPersonName = "";
            StoreName = "";
            ProductCode = "";
            ProductName = "";
            CartonQty = 0;
            PerCartonFGQty = 0;
            LotID = 0;
            LotNo = "";
            BUID = 0;
            BuyerID = 0;
            BuyerName = "";
            ExportPINo = "";
            IsExist = false;
            ErrorMessage = "";
            MUName = "";
            sParam = "";
            UnitPrice = 0.0;
            MeasurementUnits = new List<MeasurementUnit>();
            PETransactions = new List<PETransaction>();
            BusinessUnit = new BusinessUnit();
            FGCosts = new List<FGCost>();

            PSIssueDate = DateTime.Now;
            ColorName = "";
            SheetQty = 0.0;
            ConsumptionQty = 0.0;
            QCPassQty = 0.0;
            RejectQty = 0.0;
            QCQty = 0.0;
            YetToQCQty = 0.0;
        }

        #region Properties
        public int QCID { get; set; }
        public double PassQuantity { get; set; }
        public double RejectQuantity { get; set; }
        public int ProductID { get; set; }
        public DateTime OperationTime { get; set; }
        public string SheetNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int WorkingUnitID { get; set; }
        public int ProductionSheetID { get; set; }
        public int QCPerson { get; set; }
        public string QCPersonName { get; set; }
        public string StoreName { get; set; }
        public int CartonQty { get; set; }
        public double PerCartonFGQty { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public bool IsExist { get; set; }
        public int BUID { get; set; }
        public string MUName { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public string ExportPINo { get; set; }
        public int QCStatus { get; set; }
        public string sParam { get; set; }
        public string SearchingData { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime PSIssueDate { get; set; }
        public string ColorName { get; set; }
        public double SheetQty { get; set; }
        public double ConsumptionQty { get; set; }
        public double QCPassQty { get; set; }
        public double RejectQty { get; set; }
        public double QCQty { get; set; }
        public double YetToQCQty { get; set; }

        #endregion

        #region Derived QC
        public string QCStatusSt
        {
            get
            {
                return ((EnumQCStatus)this.QCStatus).ToString();
            }
        }
        public string OperationTimeInString
        {
            get
            {
                return this.OperationTime.ToString("dd MMM yyyy");
            }
        }
        public string PassQuantityInString
        {
            get
            {
                return Global.MillionFormatRound(this.PassQuantity,0)+" "+this.MUName;
            }
        }
        public double TotalQty
        {
            get
            {
                return (this.PassQuantity + this.RejectQuantity);
            }
        }
        public double Amount
        {
            get
            {
                return (this.PassQuantity * UnitPrice);
            }
        }
        public string PSIssueDateInString
        {
            get
            {
                return this.PSIssueDate.ToString("dd MMM yyyy");
            }
        }
        public List<FGCost> FGCosts { get; set; }
        public List<MeasurementUnit> MeasurementUnits { get; set; }
        public List<PETransaction> PETransactions { get; set; }
        public List<QC> QCs { get; set; }
        public Company Company { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public DateTime ConsumptionStartDate { get; set; }
        public DateTime ConsumptionEndDate { get; set; }
        public string ConsumptionStartDateInStr
        {
            get
            {
                return this.ConsumptionStartDate.ToString("dd MMM yyyy");
            }
        }
        public string ConsumptionEndDateInStr
        {
            get
            {
                return this.ConsumptionEndDate.ToString("dd MMM yyyy");
            }
        }

        #endregion

        #region Functions
        public static List<QCRegister> Gets(string sSQL, long nUserID)
        {
            return QCRegister.Service.Gets(sSQL, nUserID);
        }
        public List<QCRegister> GetsByQCFollowUp(long nUserID)
        {
            return QCRegister.Service.GetsByQCFollowUp(this, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static IQCRegisterService Service
        {
            get { return (IQCRegisterService)Services.Factory.CreateService(typeof(IQCRegisterService)); }
        }
        #endregion

        public string CurrencySymbol { get; set; }

        public double UnitPrice { get; set; }
    }
    #endregion

    #region IQC interface
    public interface IQCRegisterService
    {
        List<QCRegister> Gets(string sSQL, Int64 nUserID);
        List<QCRegister> GetsByQCFollowUp(QCRegister oQCRegister, Int64 nUserID);
    }
    #endregion

}