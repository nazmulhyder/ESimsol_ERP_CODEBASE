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
    #region KnitDyeingProgram
    public class KnitDyeingProgram : BusinessObject
    {
        public KnitDyeingProgram()
        {
            KnitDyeingProgramID = 0;
            KnitDyeingProgramLogID = 0;
            BUID = 0;
            ProgramType = EnumProgramType.None;
            RefType = EnumKnitDyeingProgramRefType.Open;
            RefNo = "";
            IssueDate = DateTime.Now;
            TechnicalSheetID = 0;
            FabricID = 0;
            OrderQty = 0;
            MUnitID = 0;
            MUSymbol = "";
            MerchandiserID = 0;
            DyedType = EnumDyedType.None;
            ShipmentDate = DateTime.Now;
            GSMID = 0;
            SrinkageTollarance = "";
            Remarks = "";
            TermsAndCondition = "";
            TimeOfArrivalYarn = DateTime.Now;
            TimeOfArrivalYarnNote = "";
            TimeOfCompletionKnitting = DateTime.Now;
            TimeOfCompletionKnittingNote = "";
            TimeOfCompletionDying = DateTime.Now;
            TimeOfCompletionDyingNote = "";
            ApprovedBy = 0;
            FabricName = "";
            StyleNo = "";
            BuyerName = "";
            ApprovedByName = "";
            GSMName = "";
            TotalReqFabricQty = 0.0;
            ORPackingPolicyCount = 0;
            MerchandiserName = "";
            RecapOrPAMNos = "";
            KnitDyeingProgramStatus = EnumKnitDyeingProgramStatus.None;
            ErrorMessage = "";
            KnitDyeingYarnRequisitions = new List<KnitDyeingYarnRequisition>();
            KnitDyeingProgramDetails = new List<KnitDyeingProgramDetail>();
        }

        #region Property
        public int KnitDyeingProgramID { get; set; }
        public int KnitDyeingProgramLogID { get; set; }
        public int ORPackingPolicyCount { get; set; }
        public int BUID { get; set; }
        public string RefNo { get; set; }
        public string SourcingConfigHeadName { get; set; }
        public DateTime IssueDate { get; set; }
        public int TechnicalSheetID { get; set; }
        public int FabricID { get; set; }
        public int SourcingConfigHeadID { get; set; }
        public double OrderQty { get; set; }
        public int MUnitID { get; set; }
        public string MUSymbol { get; set; }
        public int MerchandiserID { get; set; }
        public EnumDyedType DyedType { get; set; }
        public DateTime ShipmentDate { get; set; }
        public int GSMID { get; set; }
        public string SrinkageTollarance { get; set; }
        public string Remarks { get; set; }
        public string TermsAndCondition { get; set; }
        public DateTime TimeOfArrivalYarn { get; set; }
        public string TimeOfArrivalYarnNote { get; set; }
        public DateTime TimeOfCompletionKnitting { get; set; }
        public string TimeOfCompletionKnittingNote { get; set; }
        public DateTime TimeOfCompletionDying { get; set; }
        public string TimeOfCompletionDyingNote { get; set; }
        public int ApprovedBy { get; set; }
        public string GSMName { get; set; }
        public EnumProgramType ProgramType { get; set; }
        public int ProgramTypeInt { get; set; }
        public EnumKnitDyeingProgramRefType RefType { get; set; }
        public int RefTypeInt { get; set; }
        public string MerchandiserName { get; set; }
        public EnumKnitDyeingProgramStatus KnitDyeingProgramStatus { get; set; }
        public int KnitDyeingProgramStatusInt { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string RecapOrPAMNos { get; set; } 
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public int SessionID { get; set; }
        public string SessionName { get; set; }
        public System.Drawing.Image StyleImage { get; set; }
        public System.Drawing.Image CareImage { get; set; }
        public List<KnitDyeingYarnRequisition> KnitDyeingYarnRequisitions { get; set; }
        public List<KnitDyeingProgramDetail> KnitDyeingProgramDetails { get; set; }
        public string FabricName { get; set; }
        public string StyleNo { get; set; }
        public string BuyerName { get; set; }
        public string ApprovedByName { get; set; }
        public double TotalReqFabricQty { get; set; }
        public string ProgramTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ProgramType);
            }
        }
        public string RefTypeSt
        {
            get
            {
                return EnumObject.jGet(this.RefType);
            }
        }
        
        public string TotalReqFabricQtySt
        {
            get
            {
                return this.MUSymbol + " " + this.TotalReqFabricQty.ToString("##0.00");
            }
        }


        public string KnitDyeingProgramStatusSt
        {
            get
            {
                return EnumObject.jGet(this.KnitDyeingProgramStatus);
            }
        }
        public string IssueDateInString
        {
            get
            {
                return IssueDate.ToString("dd MMM yyyy");
            }
        }
        public string ApprovedDateInString
        {
            get
            {
                return this.ApprovedDate!=DateTime.MinValue?ApprovedDate.ToString("dd MMM yyyy"):"";
            }
        }
  
        public string ShipmentDateInString
        {
            get
            {
                return ShipmentDate.ToString("dd MMM yyyy");
            }
        }        
        public string TimeOfArrivalYarnInString
        {
            get
            {
                return TimeOfArrivalYarn.ToString("dd MMM yyyy");
            }
        }
        public string TimeOfCompletionKnittingInString
        {
            get
            {
                return TimeOfCompletionKnitting.ToString("dd MMM yyyy");
            }
        }
        public string TimeOfCompletionDyingInString
        {
            get
            {
                return TimeOfCompletionDying.ToString("dd MMM yyyy");
            }
        }

        public string NumberOfPackingPolicyCountInString
        {
            get
            {
                return this.KnitDyeingProgramID + "~" + this.ORPackingPolicyCount + "~'" + this.RefNo + "'";
            }
        }
        #endregion

        #region Functions

        public static List<KnitDyeingProgram> Gets(string sSQL, long nUserID)
        {
            return KnitDyeingProgram.Service.Gets(sSQL, nUserID);
        }
        public KnitDyeingProgram Get(int id, long nUserID)
        {
            return KnitDyeingProgram.Service.Get(id, nUserID);
        }
        public KnitDyeingProgram GetLog(int LogId, long nUserID)
        {
            return KnitDyeingProgram.Service.GetLog(LogId, nUserID);
        }
        public KnitDyeingProgram Save(long nUserID)
        {
            return KnitDyeingProgram.Service.Save(this, nUserID);
        }
        public string CommitGrace(long nUserID)
        {
            return KnitDyeingProgram.Service.CommitGrace(this, nUserID);
        }
        public KnitDyeingProgram AcceptRevise(long nUserID)
        {
            return KnitDyeingProgram.Service.AcceptRevise(this, nUserID);
        }
        public KnitDyeingProgram Approve(long nUserID)
        {
            return KnitDyeingProgram.Service.Approve(this, nUserID);
        }
        public KnitDyeingProgram SendToFactory(long nUserID)
        {
            return KnitDyeingProgram.Service.SendToFactory(this, nUserID);
        }
        public KnitDyeingProgram ProductionStart(long nUserID)
        {
            return KnitDyeingProgram.Service.ProductionStart(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return KnitDyeingProgram.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IKnitDyeingProgramService Service
        {
            get { return (IKnitDyeingProgramService)Services.Factory.CreateService(typeof(IKnitDyeingProgramService)); }
        }
        #endregion
    }
    #endregion

    #region IKnitDyeingProgram interface
    public interface IKnitDyeingProgramService
    {
        KnitDyeingProgram Get(int id, Int64 nUserID);
        KnitDyeingProgram GetLog(int LogId, Int64 nUserID);
        
        List<KnitDyeingProgram> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        KnitDyeingProgram Save(KnitDyeingProgram oKnitDyeingProgram, Int64 nUserID);
        string CommitGrace(KnitDyeingProgram oKnitDyeingProgram, Int64 nUserID);
        KnitDyeingProgram AcceptRevise(KnitDyeingProgram oKnitDyeingProgram, Int64 nUserID);       
       KnitDyeingProgram Approve(KnitDyeingProgram oKnitDyeingProgram, Int64 nUserID);
        KnitDyeingProgram SendToFactory(KnitDyeingProgram oKnitDyeingProgram, Int64 nUserID);
        KnitDyeingProgram ProductionStart(KnitDyeingProgram oKnitDyeingProgram, Int64 nUserID);
    }
    #endregion
}
