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
    #region FabricPattern

    public class FabricPattern : BusinessObject
    {
        #region  Constructor
        public FabricPattern()
        {
            FPID=0;
            PatternNo="";
            FabricID=0;
            Weave=0;
            WeaveName = "";
            Reed=0;
            Pick=0;
            GSM=0;
            Warp=0;
            Weft=0;
            Dent = 0;
            Note = "";
            Ratio="";
            RepeatSize = "";
            ApproveBy=0;
            ApproveDate = DateTime.MinValue;
            IsActive = true;
            ErrorMessage = "";
            FPDetails = new List<FabricPatternDetail>();
            WarpPlans = new List<dynamic>();
            WeftPlans = new List<dynamic>();
            FabricPatternDetails = new List<FabricPatternDetail>();
            FabricWeave = "";// EnumFabricWeave.None;
            ProductID = 0;
            StyleNo = "";
            FabricDesignName = "";
            CopyFabricID = 0;
            FSCDetailID = 0;
        }
        #endregion

        #region Properties
        public int FPID { get; set; }
        public string PatternNo { get; set; }
        public int FabricID { get; set; }
        public int Weave { get; set; }
        public int Reed { get; set; }
        public int Pick { get; set; }
        public double GSM { get; set; }
        public int Warp { get; set; }
        public int Weft { get; set; }
        public double Dent { get; set; }
        public string Note { get; set; }
        public string Ratio { get; set; }
        public string RepeatSize { get; set; }
        public int ApproveBy { get; set; }
        public DateTime ApproveDate { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        public string WeaveName { get; set; }
        public string StyleNo { get; set; }
        public string FabricDesignName { get; set; }
    
        #endregion

        #region Derive Properties
        public int CopyFabricID { get; set; }
        public string FabricWeave { get; set; }
        public int FabricWeaveInInt { get; set; }
        public List<FabricPatternDetail> FabricPatternDetails { get; set; }
        public string ApproveByName { get; set; }
        public string FabricNo { get; set; }
        public string Construction { get; set; }
        public string BuyerName { get; set; }
        public string Status { get { if (this.IsActive)return "Active"; else return "Inactive"; } }
        public string ApproveDateInStr { get { return (this.ApproveDate != DateTime.MinValue) ? this.ApproveDate.ToString("dd MMM yyyy") : ""; } }
        public List<FabricPatternDetail> FPDetails { get; set; } //
        public List<dynamic> WarpPlans { get; set; }
        public List<dynamic> WeftPlans { get; set; }
        public int ProductID { get; set; }
        public int FSCDetailID { get; set; }
        
        #endregion

        #region Functions
        public static FabricPattern Get(int nFPID, long nUserID)
        {
            return FabricPattern.Service.Get(nFPID, nUserID);
        }
        public static FabricPattern GetActiveFP(int nFabricId, long nUserID)
        {
            return FabricPattern.Service.GetActiveFP(nFabricId, nUserID);
        }
        public static List<FabricPattern> Gets(string sSQL, long nUserID)
        {
            return FabricPattern.Service.Gets(sSQL, nUserID);
        }
        public FabricPattern IUD(int nDBOperation,long nUserID)
        {
            return FabricPattern.Service.IUD(this, nDBOperation, nUserID);
        }
        public FabricPattern Save( long nUserID)
        {
            return FabricPattern.Service.Save(this, nUserID);
        }
        public FabricPattern Copy(long nUserID)
        {
            return FabricPattern.Service.Copy(this, nUserID);
        }
        public FabricPattern SaveSequence(long nUserID)
        {
            return FabricPattern.Service.SaveSequence(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricPatternService Service
        {
            get { return (IFabricPatternService)Services.Factory.CreateService(typeof(IFabricPatternService)); }
        }
        #endregion

        
    }
    #endregion


    #region IFabricPattern interface
    public interface IFabricPatternService
    {
        FabricPattern Get(int nFPID, long nUserID);
        FabricPattern GetActiveFP(int nFabricId, long nUserID);
        List<FabricPattern> Gets(string sSQL, long nUserID);
        FabricPattern IUD(FabricPattern oFabricPattern, int nDBOperation, long nUserID);
        FabricPattern Copy(FabricPattern oFabricPattern, long nUserID);
        FabricPattern Save(FabricPattern oFabricPattern, long nUserID);
        FabricPattern SaveSequence(FabricPattern oFabricPattern, long nUserID);
    }
    #endregion
}