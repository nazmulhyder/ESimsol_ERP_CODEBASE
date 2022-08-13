using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region FabricExecutionOrderSpecification

    public class FabricExecutionOrderSpecification : BusinessObject
    {
        #region  Constructor
        public FabricExecutionOrderSpecification()
        {
            FEOSID = 0;
            FSCDID = 0;
            ReferenceFSCDID = 0;
            FESONo = "";
            Reed = 0;
            ReedWidth = 0;
            FinishPick = 0;
            GreigeFabricWidth = 0;
            Ends = 0;
            Picks = 0;
            GreigeDemand = 0;
            RequiredWarpLength = 0;
            GroundEnds = 0;
            WarpSet = 0;
            SetLength = 0;
            EndsRepeat = 0;
            RepeatSection = 0;
            SectionEnds = 0;
            PrepareByName = "";
            NoOfSection = 0;
            PerConeLength = 0;
            PerConeDia = 0;
            ApproveBy = 0;
            ApproveDate = DateTime.MinValue;
            ErrorMessage = "";
            Qty = 0;
            FinishWidth = 0;
            FEOSDetails = new List<FabricExecutionOrderSpecificationDetail>();
            FabricSpecifications = new List<FabricSpecification>();
            FabricSpecificationNotes = new List<FabricSpecificationNote>();
            WarpPlans = new List<dynamic>();
            WeftPlans = new List<dynamic>();
            YarnDyeingBreakDowns = new List<dynamic>();
            //Params = "";
            FabricNo = "";
            FabricID = 0;
            LengthSpecification = "";
            GrayTargetInPercent = 0;
            GrayWarpInPercent = 0;
            Crimp = 3.5;
            //FabricPattern = new FabricPattern();
            RefNote = "";
            WeftColor = string.Empty;
            WarpColor = string.Empty;
            FinishedDate = DateTime.MinValue;
            SelvedgeEnds = 0;
            ReqLoomProduction = 0;
            FinishEnd = 0;
            NoOfFrame = "";
            FinishWidthFS = "";
            SelvedgeEndTwo = 0;
            HLReference = "";
            Dent = 2;
            TotalEnds = 0;
            WarpLenAdd = 0;
            LoomPPAdd = 0;
            TotalEndsAdd = 0;
            IsTEndsAdd = false;
            QtyExtraMet = 0;
            WarpCount = "";
            WeftCount = "";
            ExeNo = "";
            ReviseNo = 0;
            IsRevise = false;
            ProdtionType = EnumDispoProType.General;
            IssueDate = DateTime.Now;
            ReviseDate = DateTime.Now;
            IsYD = false;
            IsSepBeam = false;
            SEBeamType =EnumBeamType.None;
            FSpcType = EnumFabricSpeType.General;
            TotalEndsUB = 0;
            TotalEndsLB = 0;
            ReqWarpLenLB = 0;
            IsOutSide = false;
            ContractorID = 0;
            RefNo = "";
            FabricSource = "";
            QtyOrder = 0;
            FabricSpecificationNotes = new List<FabricSpecificationNote>();
            ForwardToDOby = 0;
            ProdtionTypeInt = (int)EnumDispoProType.General;
            FabricQtyAllows = new List<FabricQtyAllow>();

        }
        #endregion

        #region Properties
        public int FEOSID { get; set; }
        public int FSCDID { get; set; }
        public int ReferenceFSCDID { get; set; }
        public string FESONo { get; set; }
        public string FEONo { get; set; }
        public double Reed { get; set; }
        public string FabricSource { get; set; }
        public string PrepareByName { get; set; }
        public double ReedWidth { get; set; }
        public double Crimp { get; set; }
        public double FinishPick { get; set; }
        public double GreigeFabricWidth { get; set; }
        public double Ends { get; set; }
        public double Picks { get; set; }
        public double GreigeDemand { get; set; }
        public double RequiredWarpLength { get; set; }
        public double GroundEnds { get; set; }
        public int WarpSet { get; set; }
        public double SetLength { get; set; }
        public double EndsRepeat { get; set; }
        public double RepeatSection { get; set; }
        public double SectionEnds { get; set; }
        public double NoOfSection { get; set; }
        public double PerConeLength { get; set; }
        public double PerConeDia { get; set; }
        public string LengthSpecification { get; set; }
        public int ApproveBy { get; set; }
        public string RefNote { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ErrorMessage { get; set; }
        public double GrayTargetInPercent { get; set; }
        public double GrayWarpInPercent { get; set; }
        //public double AllowancePercent { get; set; }
        public string WeftColor { get; set; }
        public string WarpColor { get; set; }
        public double SelvedgeEnds { get; set; }
        public double ReqLoomProduction { get; set; }
        public string FinishWidthFS { get; set; }
        public double FinishWidth { get; set; }
        public double FinishEnd { get; set; }
        public string NoOfFrame { get; set; }
        public double SelvedgeEndTwo { get; set; }
        public double Dent { get; set; }
        public double TotalEnds { get; set; }
        public double TotalEndsAdd { get; set; }
        public bool IsTEndsAdd { get; set; }
        public double WarpLenAdd { get; set; }
        public double LoomPPAdd { get; set; }
        public int ReviseNo { get; set; }
        public string WarpCount { get; set; }
        public string WeftCount { get; set; }
        public EnumDispoProType ProdtionType { get; set; }
        public int ProdtionTypeInt { get; set; }
        public bool IsYD { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ForwardDODate { get; set; }
        public DateTime ReviseDate { get; set; }
        public bool IsSepBeam { get; set; } ///
        public EnumBeamType SEBeamType { get; set; }///Selvedge Ends Deduct from Upper Beam, or Lower
        public EnumFabricSpeType FSpcType { get; set; }///Selvedge End Beam
        public double TotalEndsUB { get; set; }
        public double TotalEndsLB { get; set; }
        public double ReqWarpLenLB { get; set; }/// for Seer sucker Lower Beam, Another for Upper beam
        public bool IsOutSide { get; set; }
        public int ContractorID { get; set; }/// for Out Side Production
        public int ForwardToDOby { get; set; }
        public string RefNo { get; set; }
        public EnumWarpWeft WarpWeftType { get; set; }
        #endregion

        #region Derive Properties
        public string FabricNo { get; set; }
        public string ApproveByName { get; set; }
        public string SCNoFull { get; set; } /// PO no from MKT
        public string ExeNo { get; set; } /// Dispo no no from MKT
        public bool IsInHouse { get; set; }
        public string PINo { get; set; }
        public string Construction { get; set; }
        public string Composition { get; set; }
        public int FabricID { get; set; }
        public string BuyerName { get; set; }
        public string Weave { get; set; }
        public string HLReference { get; set; }
        public string FinishType { get; set; }
        public double Qty { get; set; }
        public double QtyOrder { get; set; }
        public double QtyExtraMet { get; set; }
       
        //public string RefSCNo { get; set; }
        public bool IsRevise { get; set; }
        public DateTime FinishedDate { get; set; }

     
        public string ApproveDateInStr { get { if (this.ApproveBy > 0 && this.ApproveDate != DateTime.MinValue) return this.ApproveDate.ToString("dd MMM yyyy"); else  return ""; } }
        public string ForwardDODateInStr 
        { 
            get 
            {
                if (ForwardDODate == DateTime.MinValue) return "";
                return ForwardDODate.ToString("dd MMM yyyy hh:mm tt");
            } 
        }

        public double OrderQtyInYard
        {
            get
            {
                if (this.Qty > 0) { return Math.Round(this.Qty, 2); }
                else { return 0; }
            }
        }
        public double OrderQtyInMeter
        {
            get
            {
                if (this.Qty > 0) { return Math.Round((this.Qty / 1.09361), 2); }
                else { return 0; }
            }
        }
        private string sProdtionTypeSt = "";
        public string ProdtionTypeSt
        {
            get
            {
                sProdtionTypeSt = ((EnumDispoProType)this.ProdtionType).ToString(); 
                if (this.ReviseNo > 0) { sProdtionTypeSt = sProdtionTypeSt + " R-" + this.ReviseNo; };
                return sProdtionTypeSt;
            }
        }
        private string sFortoDO = "";
        public string FortoDO
        {
            get
            {
                if (this.ForwardToDOby == 0) { sFortoDO = "No"; }
                else { sFortoDO = "Yes"; }
                return sFortoDO;
            }
        }

        public List<FabricExecutionOrderSpecificationDetail> FEOSDetails { get; set; }
        public List<FabricSpecification> FabricSpecifications { get; set; }
        public List<FabricSpecificationNote> FabricSpecificationNotes { get; set; }
        public List<FabricQtyAllow> FabricQtyAllows { get; set; }
        public List<dynamic> WarpPlans { get; set; }
        public List<dynamic> WeftPlans { get; set; }
        public List<dynamic> YarnDyeingBreakDowns { get; set; }
    

        public string IssueDateSt
        {
            get
            {
                if (this.IssueDate == DateTime.MinValue)
                    return string.Empty;
                else
                    return this.IssueDate.ToString("dd MMM yyyy");

            }
        }
        public string FinishedDateStr
        {
            get
            {
                if (this.FinishedDate == DateTime.MinValue)
                    return string.Empty;
                else
                    return FinishedDate.ToString("dd MMM yyyy");

            }
        }
        #endregion

        #region Functions
        public static FabricExecutionOrderSpecification Get(int nFEOSID, long nUserID)
        {
            return FabricExecutionOrderSpecification.Service.Get(nFEOSID, nUserID);
        }
        public static List<FabricExecutionOrderSpecification> Gets(string sSQL, long nUserID)
        {
            return FabricExecutionOrderSpecification.Service.Gets(sSQL, nUserID);
        }
        public FabricExecutionOrderSpecification IUD(int nDBOperation, long nUserID)
        {
            return FabricExecutionOrderSpecification.Service.IUD(this, nDBOperation, nUserID);
        }
        public FabricExecutionOrderSpecification IUD_DO(long nUserID)
        {
            return FabricExecutionOrderSpecification.Service.IUD_DO(this, nUserID);
        }
        public FabricExecutionOrderSpecification UpdateOutSide(Int64 nUserID)
        {
            return FabricExecutionOrderSpecification.Service.UpdateOutSide(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricExecutionOrderSpecificationService Service
        {
            get { return (IFabricExecutionOrderSpecificationService)Services.Factory.CreateService(typeof(IFabricExecutionOrderSpecificationService)); }
        }
        #endregion


    }
    #endregion


    #region IFabricExecutionOrderSpecification interface
    public interface IFabricExecutionOrderSpecificationService
    {
        FabricExecutionOrderSpecification Get(int nFEOSID, long nUserID);
        List<FabricExecutionOrderSpecification> Gets(string sSQL, long nUserID);
        FabricExecutionOrderSpecification IUD(FabricExecutionOrderSpecification oFabricExecutionOrderSpecification, int nDBOperation, long nUserID);
        FabricExecutionOrderSpecification IUD_DO(FabricExecutionOrderSpecification oFabricExecutionOrderSpecification,  long nUserID);
        FabricExecutionOrderSpecification UpdateOutSide(FabricExecutionOrderSpecification oFabricExecutionOrderSpecification, Int64 nUserID);
    }
    #endregion

    public class PlanGroup
    {
        public PlanGroup()
        {
            ColorName = "";
            ProductCode = "";
            ProductName = "";
            ProductShortName = "";
            RowCount = 0;
            Value = 0;
            EndsCount = 0;
            ColorNo = "";
            LabdipDetailID = 0;
            ProductID = 0;
            Allowance = 0;
            FEOSID = 0;
            ValueMin = 0;
            BatchNo = "";
            LDNo = "";
        }
        public string ColorName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductShortName { get; set; }
        public int EndsCount { get; set; }
        public string ColorNo { get; set; }
        public string BatchNo { get; set; }
        public string LDNo { get; set; }
        public int RowCount { get; set; }
        public int LabdipDetailID { get; set; }
        public int ProductID { get; set; }
        public double Value { get; set; }
        public double ValueMin { get; set; }
        public double Allowance { get; set; }
        public int FEOSID { get; set; }
    }

    public class FabricSpecification
    {
        public FabricSpecification()
        {
            FEOSID = 0;
            SpecificationNo = "";
            ApproveBy = 0;
        }
        public int FEOSID { get; set; }
        public string SpecificationNo { get; set; }
        public int ApproveBy { get; set; }
    }
}