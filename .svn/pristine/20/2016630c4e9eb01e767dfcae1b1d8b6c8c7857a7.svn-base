using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region FabricRnD
    
    public class FabricRnD : BusinessObject
    {
        public FabricRnD()
        {
            FabricRnDID = 0;
            FabricNo = "";
            ProductIDWarp = 0; //Composition
            FabricWidth = "";
            FinishType = 0;
            FinishTypeName= "";
            ProcessType = 0;
            ProcessTypeName = "";
            FabricWeave = 0;
            FabricWeaveName = "";
            FabricDesignID = 0;
            FabricDesignName = "";
            ErrorMessage = "";
            ForwardDate = DateTime.MinValue;
            ReceiveDate = DateTime.MinValue;
            ForwardToBuyerDate = DateTime.MinValue;
            YarnQuality = "";
            ProductNameWarp = "";
            ProductNameWeft = "";
            WarpCount = "";
            WeftCount = "";
            EPI = "";
            PPI = "";
            FabricNum = "";
            WeightAct = 0;
            WeightCal = 0;
            YarnQuality = "";
            YarnFly = "";
            ConstructionSuggest = "";

            ProductIDWarp = 0;
            ProductIDWeft = 0;
            FabricWeaveSuggest = 0;
            //ProductNameWarpSuggest = "";
            //ProductNameWeftSuggest = "";
            FabricWeaveNameSuggest = "";
            Note = "";

            WashType = 0;
            ShadeType = EnumFabricRndShade.None;
            CrimpWP = "";
            CrimpWF = "";
            Growth = "";
            Recovy = "";
            Elongation = "";
            SlubLengthWP = "";
            PauseLengthWP = "";
            SlubDiaWP = "";
            SlubLengthWF = "";
            PauseLengthWF = "";
            SlubDiaWF = "";
            WidthGrey = "";
            ProductWarpRnd_Suggest = "";
        }

        #region Properties
        public int FabricRnDID { get; set; }
        public int FabricID { get; set; }
        public int FSCDID { get; set; }
        public string FabricNo { get; set; }
        public DateTime IssueDate { get; set; }
        public int ProductIDWarp { get; set; }
        public int ProductIDWeft { get; set; }
        public string FabricWidth { get; set; }
        public int FinishType { get; set; }
        public string FinishTypeName { get; set; }
        public string Note { get; set; }
        public string ProductNameWarp { get; set; }
        public string ProductNameWeft { get; set; }
        public int ReceivedBy { get; set; }
        public int ForwardBy { get; set; }
        public int ReceivedMKTBy { get; set; }
        public int ForwardToBuyerBy { get; set; }
        public string ReceivedName { get; set; }
        public string PrepareByName { get; set; }
        public string ForwardByName { get; set; }
        public string ForwardToBuyerName { get; set; }
        public int FabricDesignID { get; set; }
        public string FabricDesignName { get; set; }
        public DateTime ReceiveDate { get; set; }
        public DateTime ForwardDate { get; set; }
        public DateTime ReceiveMKTDate { get; set; }
        public DateTime ForwardToBuyerDate { get; set; }
        public int ProcessType { get; set; }
        public string ProcessTypeName { get; set; }
        public int FabricWeave { get; set; }
        public string FabricWeaveName { get; set; }
        public string WeftColor { get; set; }
        public string ConstructionSuggest { get; set; }
        public string WarpCount { get; set; }
        public string WeftCount { get; set; }
        public string EPI { get; set; }
        public string PPI { get; set; }
        public string FabricNum { get; set; }
        public double WeightAct { get; set; }
        public double WeightCal { get; set; }
        public double WeightDec { get; set; }
        public string YarnQuality { get; set; }
        public string YarnFly { get; set; }
        //public int ProductIDWarpSuggest { get; set; }
        //public int ProductIDWeftSuggest { get; set; }
        public string ProductWarpRnd_Suggest { get; set; }
        //public string ProductNameWarpSuggest { get; set; }
        //public string ProductNameWeftSuggest { get; set; }
        public int FabricWeaveSuggest { get; set; }
        public string FabricWeaveNameSuggest { get; set; }

        public int WashType { get; set; }
        public EnumFabricRndShade ShadeType { get; set; }
        public string CrimpWP { get; set; }
        public string CrimpWF { get; set; }
        public string Growth { get; set; }
        public string Recovy { get; set; }
        public string Elongation { get; set; }
        public string SlubLengthWP { get; set; }
        public string PauseLengthWP { get; set; }
        public string SlubDiaWP { get; set; }
        public string SlubLengthWF { get; set; }
        public string PauseLengthWF { get; set; }
        public string SlubDiaWF { get; set; }
        public string WidthGrey { get; set; }

        #endregion
        #region Derived Property  
        public string ErrorMessage { get; set; }
        public string IssueDateInString
        {
            get { return this.IssueDate.ToString("dd MMM yyyy"); }
        }
        public string ShadeTypeInString
        {
            get 
            {
                return EnumObject.jGet(this.ShadeType);
            }
        }
        public string WashTypeInString
        {
            get
            {
                if (this.WashType == 1) return "B/W";
                else if (this.WashType == 2) return "A/W";
                else return "";
            }
        }
        public string ReceiveDateSt
        {
            get 
            {
                if (this.ReceiveDate== DateTime.MinValue)
                {
                    return "-";
                }
                else {
                    return this.ReceiveDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion

        #region Functions

        public static List<FabricRnD> Gets(long nUserID)
        {
            return FabricRnD.Service.Gets(nUserID);
        }
        public static List<FabricRnD> Gets(string sSQL, long nUserID)
        {
            return FabricRnD.Service.Gets(sSQL, nUserID);
        }
        public FabricRnD Get(int nId, long nUserID)
        {
            return FabricRnD.Service.Get(nId,nUserID);
        }
        public FabricRnD GetBy(int nFSCDID, int nFabricID, long nUserID)
        {
            return FabricRnD.Service.GetBy(nFSCDID, nFabricID, nUserID);
        }
        public FabricRnD StatusChange(int nFabricRnDId, long nUserID)
        {
            return FabricRnD.Service.StatusChange(nFabricRnDId, nUserID);
        }
        public FabricRnD Save(long nUserID)
        {
            return FabricRnD.Service.Save(this, nUserID);
        }
   
        public string Delete(int nId, long nUserID)
        {
            return FabricRnD.Service.Delete(nId, nUserID);
        }
        public static List<FabricRnD> Approve(List<FabricRnD> oFabricRnDs, long nUserID)
        {
            return FabricRnD.Service.Approve(oFabricRnDs, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricRnDService Service
        {
            get { return (IFabricRnDService)Services.Factory.CreateService(typeof(IFabricRnDService)); }
        }
        #endregion

    }
    #endregion

    #region Report Study
    public class FabricRnDList : List<FabricRnD>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IFabricRnD interface
    public interface IFabricRnDService
    {
        FabricRnD Get(int id, long nUserID);
        FabricRnD GetBy(int nFSCDID, int nFabricID, long nUserID);
        FabricRnD StatusChange(int nFabricRnDId, long nUserID);
        List<FabricRnD> Approve(List<FabricRnD> oFabricRnDs, long nUserID);
        List<FabricRnD> Gets(long nUserID);
        List<FabricRnD> Gets(string sSQL, long nUserID);
        string Delete(int id, long nUserID);
        FabricRnD Save(FabricRnD oFabricRnD, long nUserID);
       
    }
    #endregion
}