using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region QC

    public class QC : BusinessObject
    {
        public QC()
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
            IsExist = false;
            ErrorMessage = "";
            MUName = "";
            sParam = "";
            MeasurementUnits = new List<MeasurementUnit>();
            PETransactions = new List<PETransaction>();
            BusinessUnit = new BusinessUnit();
            FGCosts = new List<FGCost>();

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
        public string sParam { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived QC
        
        public string OperationTimeInString
        {
            get
            {
                return this.OperationTime.ToString("dd MMM yyyy hh:mm:ss tt");
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
        public List<FGCost> FGCosts { get; set; }
        public List<MeasurementUnit> MeasurementUnits { get; set; }
        public List<PETransaction> PETransactions { get; set; }
        public List<QC> QCs { get; set; }
        public Company Company { get; set; }
        public BusinessUnit BusinessUnit { get; set; }

        #endregion

        #region Functions

        public static List<QC> Gets(long nUserID)
        {
            return QC.Service.Gets(nUserID);
        }
        public static List<QC> Gets(int nPSID, long nUserID)
        {
            return QC.Service.Gets(nPSID, nUserID);
        }
        public static List<QC> Gets(string sSQL, long nUserID)
        {
            return QC.Service.Gets(sSQL, nUserID);
        }

        public QC Get(int id, long nUserID)
        {
            return QC.Service.Get(id, nUserID);
        }

        public QC Save(long nUserID)
        {
            return QC.Service.Save(this, nUserID);
        }
        public QC FGCostProcess(long nUserID)
        {
            return QC.Service.FGCostProcess(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return QC.Service.Delete(id, nUserID);
        }


        #endregion

        #region ServiceFactory
        internal static IQCService Service
        {
            get { return (IQCService)Services.Factory.CreateService(typeof(IQCService)); }
        }
        #endregion
    }
    #endregion

    #region IQC interface

    public interface IQCService
    {
        QC Get(int id, Int64 nUserID);

        List<QC> Gets(Int64 nUserID);
        List<QC> Gets(int nPSID, Int64 nUserID);
        List<QC> Gets(string sSQL, Int64 nUserID);

        string Delete(int id, Int64 nUserID);

        QC Save(QC oQC, Int64 nUserID);
        QC FGCostProcess(QC oQC, Int64 nUserID);
        

    }
    #endregion

    
}
