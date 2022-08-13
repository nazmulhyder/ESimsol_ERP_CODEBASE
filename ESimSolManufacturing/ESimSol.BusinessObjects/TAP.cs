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
    #region TAP
    public class TAP : BusinessObject
    {
        public TAP()
        {
            TAPID = 0;
            BUID = 0;
            OrderRecapID = 0;
            OrderRecapNo = "";
            PlanNo = "";
            PlanDate = DateTime.Now;
            PlanBy = 0;
            ApprovedBy = 0;
            ApprovedByName = "";
            Remarks = "";
            StyleNo = "";
            TechnicalSheetID = 0;
            BuyerName = "";
            BuyerID = 0;
            OrderDate = DateTime.Now;
            ShipmentDate = DateTime.Now;
            RecapShipmentDate = DateTime.Now;
            FactoryShipmentDate = DateTime.Now;
            Quantity = 0;
            FOB = 0;
            Amount = 0;
            PlanByName = "";
            BrandName = "";
            TAPExecutionID = 0;
            ProductionFactoryID = 0;
            ProductionFactoryName = "";
            TAPStatus = EnumTAPStatus.Initialize;
            TAPStatusInInt = 0;
            nPriviousTAPID = 0;
            TAPActionType = EnumTAPActionType.None;
            ActionTypeExtra = "";
            TAPDetails = new List<TAPDetail>();
            TSType = EnumTSType.Sweater;
            TampleteName = "";
            UnitName = "";
            MerchandiserName = "";
            TotalTask = 0;
            CompleteTask = 0;
            PendingTask = 0;
            EstimatedDay = 0;
            JobID = 0;
            BusinessUnit = new BusinessObjects.BusinessUnit();
            PODate = DateTime.Now;
            OrderQty = 0;
            LeadTime = 0;
            ApprovalDuration = 0;
            ProductionLeadTime = 0;	
            FabricLeadTime = 0;
            ProductionTime = 0;
            BufferingDays = 0;
            ProductName = "";
            YarnCategoryName = "";
            TAPApprovalProcesList = new List<TAPDetail>();
            TAPProductionProcesList = new List<TAPDetail>();
            ErrorMessage = "";


        }

        #region Properties
        public int TAPID { get; set; }
        public int OrderRecapID { get; set; }
        public string OrderRecapNo { get; set; }
        public DateTime PlanDate { get; set; }
        public int PlanBy { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string Remarks { get; set; }
        public string BrandName { get; set; }
        public string PlanNo { get; set; }
        public int TAPExecutionID { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public EnumTAPActionType TAPActionType { get; set; }
        public string StyleNo { get; set; }
        public EnumTAPStatus TAPStatus { get; set; }
        public int TAPStatusInInt { get; set; }
        public int TechnicalSheetID { get; set; }
        public string ActionTypeExtra { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public DateTime OrderDate { get; set; }
        public double Quantity { get; set; }
        public double FOB { get; set; }
        public double Amount { get; set; }
        public string UnitName { get; set; }
        public DateTime RecapShipmentDate { get; set; }
        public DateTime FactoryShipmentDate { get; set; }
        public string PlanByName { get; set; }
        public string TampleteName { get; set; }
        public int ProductionFactoryID { get; set; }
        public string ProductionFactoryName { get; set; }
        public string MerchandiserName { get; set; }
        public int BUID { get; set; }
        public double TotalTask { get; set; }
        public double CompleteTask { get; set; }
        public double PendingTask { get; set; }
        public int EstimatedDay { get; set; }
        public DateTime   PODate{ get; set; }
        public double OrderQty { get; set; }	
        public int     LeadTime { get; set; }	
        public int ApprovalDuration { get; set; }	
        public int ProductionLeadTime { get; set; }	
        public int FabricLeadTime { get; set; }	
        public int ProductionTime { get; set; }
        public int BufferingDays { get; set; }
        public string ProductName { get; set; }
        public string YarnCategoryName { get; set; }
        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public BusinessUnit BusinessUnit { get; set; }
        
        public TAPExecution TAPExecution { get; set; }
        public EnumTSType TSType { get; set; }
        public int nPriviousTAPID { get; set; }
        public int JobID { get; set; }
        public List<TAP> TAPs { get; set; }
        public TechnicalSheetImage TechnicalSheetImage { get; set; }
        public List<TAPDetail> TAPDetails { get; set; }
        public List<TAPDetail> TAPApprovalProcesList { get; set; }
        public List<TAPDetail> TAPProductionProcesList { get; set; }
        public ApprovalRequest ApprovalRequest { get; set; }
        public Company Company { get; set; }
        public List<TechnicalSheetColor> TechnicalSheetColors { get; set; }
        public List<TechnicalSheetSize> TechnicalSheetSizes { get; set; }
        public List<ColorSizeRatio> ColorSizeRatios { set; get; }
        public List<OrderRecapDetail> OrderRecapDetails { get; set; }
        public List<SizeCategory> SizeCategories { get; set; }
        public List<ColorCategory> ColorCategories { get; set; }
        public List<User> Users { get; set; }

        public string PlanTypeInString
        {
            get
            {
               
                    return "Order Recap";
         

            }
        }

        public string ShipmentDateInString
        {
            get
            {
                return this.ShipmentDate.ToString("dd MMM yyyy");
            }
        }
        public string PlanDateInString
        {
            get
            {
                return this.PlanDate.ToString("dd MMM yyyy");
            }
        }
        public string PODateInString
        {
            get
            {
                return this.PODate.ToString("dd MMM yyyy");
            }
        }
        
        public string OrderDateInString
        {
            get
            {
                return this.OrderDate.ToString("dd MMM yyyy");

            }
        }
        public string FactoryShipmentDateInString
        {
            get
            {
                return this.FactoryShipmentDate.ToString("dd MMM yyyy");
            }
        }
        public string TAPStatusInString
        {
            get
            {
                return this.TAPStatus.ToString();
            }
        }
        public string PendingTaskInSt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.PendingTask);
            }
        }
        public string CompleteTaskInSt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.CompleteTask);
            }
        }
        public string TotalTaskInSt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.TotalTask);
            }
        }

        public string PlanNoWithIDInString
        {
            get
            {
                //REfID ~RefNo~RefType //don't delete use in Merchanidsing deshboard
                return this.TAPID + "~" + this.PlanNo+"~4";
            }
        }
        #endregion

        #region Functions
        public static List<TAP> Gets(long nUserID)
        {
            return TAP.Service.Gets(nUserID);
        }
        public static List<TAP> Gets(string sSQL, long nUserID)
        {
            return TAP.Service.Gets(sSQL, nUserID);
        }
        public TAP ChangeStatus(ApprovalRequest oApprovalRequest, long nUserID)
        {
            return TAP.Service.ChangeStatus(this, oApprovalRequest, nUserID);
        }
        public TAP Get(int id, long nUserID)
        {
            return TAP.Service.Get(id, nUserID);
        }
        public TAP GetByRecap(int id, long nUserID)
        {
            return TAP.Service.GetByRecap(id, nUserID);
        }

        public TAP GetByHIA(int nHIAID, long nUserID)
        {
            return TAP.Service.GetByHIA(nHIAID, nUserID);
        }
        public TAP GetFactoryTAP(int id, long nUserID)
        {
            return TAP.Service.GetFactoryTAP(id, nUserID);
        }
        public TAP Save(long nUserID)
        {
            return TAP.Service.Save(this, nUserID);
        }
        public TAP AcceptRevise(long nUserID)
        {
            return TAP.Service.AcceptRevise(this, nUserID);
        }        
        public TAP UpDown(TAPDetail oTAPDetail, long nUserID)
        {
            return TAP.Service.UpDown(oTAPDetail, nUserID);
        }
        public TAP UpdateApprovePlanDate(TAPDetail oTAPDetail, long nUserID)
        {
            return TAP.Service.UpdateApprovePlanDate(oTAPDetail, nUserID);
        }
        public TAP SaveFactoryTAP(long nUserID)
        {
            return TAP.Service.SaveFactoryTAP(this, nUserID);

        }
        public string Delete(int id, long nUserID)
        {
            return TAP.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ITAPService Service
        {
            get { return (ITAPService)Services.Factory.CreateService(typeof(ITAPService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class TAPList : List<TAP>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region ITAP interface

    public interface ITAPService
    {
        TAP Get(int nTAPID, Int64 nUserID);
        TAP GetByRecap(int nORID, Int64 nUserID);
        
        TAP GetByHIA(int id, Int64 nUserID);
        TAP GetFactoryTAP(int id, Int64 nUserID);
        TAP ChangeStatus(TAP oTAP, ApprovalRequest oApprovalRequest, Int64 nUserID);
        List<TAP> Gets(Int64 nUserID);
        List<TAP> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        TAP Save(TAP oTAP, Int64 nUserID);
        TAP AcceptRevise(TAP oTAP, Int64 nUserID);
        TAP UpDown(TAPDetail oTAPDetail, Int64 nUserID);
        TAP UpdateApprovePlanDate(TAPDetail oTAPDetail, Int64 nUserID);
        TAP SaveFactoryTAP(TAP oTAP, Int64 nUserID);
    }
    #endregion
}
