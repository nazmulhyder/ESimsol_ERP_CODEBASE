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
    #region Fabric
    
    public class Fabric : BusinessObject
    {
        public Fabric()
        {
            FabricID = 0;
            FabricNo = "";
            BuyerReference = "";
            IssueDate = DateTime.Today;
            BuyerID = 0;
            ProductID = 0; //Composition
            MKTPersonID = 0;
            Construction = "";
            StyleNo = "";
            ColorInfo = "";
            FabricWidth = "";
            IsWash = false;
            IsFinish = false;
            IsDyeing = false;
            IsPrint = false;
            FinishType = 0;
            FinishTypeName= "";
            ApprovedBy = 0;
            ApprovedByName = "";
            Remarks = "";
            BuyerName = "";
            ProductName = "";
            MKTPersonName = "";
            //CompareOperatorValue = 0;
            //FromDate = DateTime.Today;
            //ToDate = DateTime.Today;
            ProcessType = 0;
            ProcessTypeName = "";
            FabricWeave = 0;
            FabricWeaveName = "";
            FabricDesignID = 0;
            FabricDesignName = "";
            ErrorMessage = "";

            MUID = 0;
           
            PriorityLevel = EnumPriorityLevel.None;
            PriorityLevelInInt = (int)EnumPriorityLevel.None;
            SeekingSubmissionDate = DateTime.Today;
            SubmissionDate = DateTime.MinValue;
            ReceiveDate = DateTime.MinValue;
         
            ApprovedByDate = DateTime.Today;
            StickerHeader = "";
            //nCheckReceiveOrSubmitSave = 0;
            //FabricAttachmentCount = 0;
            Params = "";
         
            PrepareByName = "";
            FabricSeekingDates = new List<FabricSeekingDate>();
            AttCount = "";
            ConstructionPI = "";
            HandLoomNo = string.Empty;
            PrimaryLightSourceID = 0;
            SecondaryLightSourceID = 0;
            EndUse = string.Empty;
            OptionNo = string.Empty;
            NoOfFrame = 0;
            WeftColor = string.Empty;
            ActualConstruction = string.Empty;
            PrimaryLightSource="";
            SecondaryLightSource="";
            WarpCount = string.Empty;
            WeftCount = string.Empty;
            EPI = string.Empty;
            PPI = string.Empty;
            FabricOrderType = EnumFabricRequestType.None;
            ProductIDWeft = 0;
            Code = "";
            CurrencyID = 0;
            CurrencyName = "";
            Symbol = "";
            Price = 0;
        }

        #region Properties
        public int FabricID { get; set; }
        public string FabricNo { get; set; }
        public string BuyerReference { get; set; }
        public DateTime IssueDate { get; set; }
        public int BuyerID { get; set; }
        public int ProductID { get; set; }
        public int ProductIDWeft { get; set; }
        public int MKTPersonID { get; set; }
        public string Construction { get; set; }
        public string ConstructionPI { get; set; }
        public string StyleNo { get; set; }
        public string ColorInfo { get; set; }
        public string FabricWidth { get; set; }
        public bool IsWash { get; set; }
        public bool IsFinish { get; set; }
        public bool IsDyeing { get; set; }
        public bool IsPrint { get; set; }
        public int FinishType { get; set; }
        public string FinishTypeName { get; set; }
        public int FinishTypeSugg { get; set; }
        public string FinishTypeNameSugg { get; set; }
        public string Remarks { get; set; }
        public string BuyerName { get; set; }
        public string ProductName { get; set; }
        public string ProductNameWeft { get; set; }
        public string MKTPersonName { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public string PrepareByName { get; set; }
        public EnumPriorityLevel PriorityLevel { get; set; }
        public int PriorityLevelInInt { get; set; }
        public string Params { get; set; }
        public int FabricDesignID { get; set; }
        public string FabricDesignName { get; set; }
        public DateTime SeekingSubmissionDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime ApprovedByDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public int ProcessType { get; set; }
        public string ProcessTypeName { get; set; }
        public int FabricWeave { get; set; }
        public string FabricWeaveName { get; set; }
        public string AttCount { get; set; }
        public string HandLoomNo { get; set; }
        public string Code { get; set; }
        public int PrimaryLightSourceID { get; set; }
        public int SecondaryLightSourceID { get; set; }
        public string EndUse { get; set; }
        public string OptionNo { get; set; }
        public int NoOfFrame { get; set; }
        public string WeftColor { get; set; }
        public string ActualConstruction { get; set; }
        public string WarpCount { get; set; }
        public string Symbol { get; set; }
        public string WeftCount { get; set; }
        public string EPI { get; set; }
        public string PPI { get; set; }
        public string FabricNum { get; set; }
        public EnumFabricRequestType FabricOrderType { get; set; }
        public int FabricOrderTypeInt { get; set; }
        public double WeightAct { get; set; }
        public double WeightCal { get; set; }
        public double WeightDec { get; set; }
        public string NoteRnD { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public double Price { get; set; }
        #endregion
        #region Derived Property  
        public int MUID { get; set; }
        public string PrimaryLightSource { get; set; }
        public string SecondaryLightSource { get; set; }
        public string ErrorMessage { get; set; }
        public string StickerHeader { get; set; }
        public List<Fabric> Fabrics { get; set; }
        public List<FabricSeekingDate> FabricSeekingDates { get; set; }
        //public int FabricAttachmentCount { get; set; }
     
        //public string FabricAttachmentCountSt
        //{
        //    get
        //    {
        //        if (this.FabricAttachmentCount == 0)
        //        {
        //            return "-";
        //        }
        //        else
        //        {
        //            return this.FabricAttachmentCount.ToString();
        //        }
        //    }
        //}
        
        public string ApprovedByNameSt
        {
            get
            {
                if (this.ApprovedBy == 0)
                {
                    return "-";
                }
                else {
                    return this.ApprovedByName;
                }
            }
        }
        public string IssueDateInString
        {
            get { return this.IssueDate.ToString("dd MMM yyyy"); }
        }
        public string PriorityLevelInString
        {
            get
            {
                if (this.PriorityLevel == EnumPriorityLevel.Low) return "Low";
                else if (this.PriorityLevel == EnumPriorityLevel.Medium) return "Medium";
                else if (this.PriorityLevel == EnumPriorityLevel.High) return "High";
                else return "";
            }
        }
        public string SeekingSubmissionDateInString
        {
            get { return this.SeekingSubmissionDate.ToString("dd MMM yyyy"); }
        }
      
        public string ApprovedByDateInString
        {
            get { return this.ApprovedByDate.ToString("dd MMM yyyy"); }
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
        public int ToDateSt
        {
            get
            {
                if (this.SubmissionDate == DateTime.MinValue && this.SeekingSubmissionDate < DateTime.Today)
                {
                    return 2;
                }
                else if (this.SubmissionDate == DateTime.MinValue && this.SeekingSubmissionDate == DateTime.Today)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        public string SubmissionDateSt
        {
            get
            {
                if (this.SubmissionDate ==DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.SubmissionDate.ToString("dd MMM yyyy");
                }
            }
        }
        private string _sStatus = "";
        public string Status
        {
            get
            {
                _sStatus = "Initialize";
                if (this.ApprovedBy!=0)
                {
                    _sStatus= "Issued";
                }
                if (this.ReceiveDate !=DateTime.MinValue)
                {
                    _sStatus = "Received";
                }
                if (this.SubmissionDate != DateTime.MinValue)
                {
                    _sStatus = "Submitted";
                }
               return _sStatus;
            }
        }

        #endregion

        #region Functions

        public static List<Fabric> Gets(long nUserID)
        {
            return Fabric.Service.Gets(nUserID);
        }
        public static List<Fabric> Gets(string sSQL, long nUserID)
        {
            return Fabric.Service.Gets(sSQL, nUserID);
        }
     
        public Fabric Get(int nId, long nUserID)
        {
            return Fabric.Service.Get(nId,nUserID);
        }
        public Fabric StatusChange(int nFabricId, long nUserID)
        {
            return Fabric.Service.StatusChange(nFabricId, nUserID);
        }
        public Fabric Save(long nUserID)
        {
            return Fabric.Service.Save(this, nUserID);
        }
        public static List<Fabric> SaveAll(List<Fabric> oFabrics, long nUserID)
        {
            return Fabric.Service.SaveAll(oFabrics, nUserID);
        }
        public Fabric SaveReceiveDate(long nUserID)
        {
            return Fabric.Service.SaveReceiveDate(this, nUserID);
        }
        public Fabric SaveSubmissionDate(long nUserID)
        {
            return Fabric.Service.SaveSubmissionDate(this, nUserID);
        }
        public Fabric ReFabricSubmission(long nUserID)
        {
            return Fabric.Service.ReFabricSubmission(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return Fabric.Service.Delete(nId, nUserID);
        }
        public static List<Fabric> Approve(List<Fabric> oFabrics, long nUserID)
        {
            return Fabric.Service.Approve(oFabrics, nUserID);
        }
        public static List<Fabric> Received(List<Fabric> oFabrics, long nUserID)
        {
            return Fabric.Service.Received(oFabrics, nUserID);
        }
        public static List<Fabric> Submission(List<Fabric> oFabrics, long nUserID)
        {
            return Fabric.Service.Submission(oFabrics, nUserID);
        }
        public Fabric SaveSubmission(long nUserID)
        {
            return Fabric.Service.SaveSubmission(this, nUserID);
        }
        public Fabric SaveReceived(long nUserID)
        {
            return Fabric.Service.SaveReceived(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricService Service
        {
            get { return (IFabricService)Services.Factory.CreateService(typeof(IFabricService)); }
        }
        #endregion

       
    }
    #endregion

    #region Report Study
    public class FabricList : List<Fabric>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IFabric interface
    public interface IFabricService
    {
        Fabric Get(int id, long nUserID);
        Fabric StatusChange(int nFabricId, long nUserID);
        List<Fabric> Approve(List<Fabric> oFabrics, long nUserID);
        List<Fabric> SaveAll(List<Fabric> oFabrics, long nUserID);
        List<Fabric> Gets(long nUserID);
        List<Fabric> Gets(string sSQL, long nUserID);
        string Delete(int id, long nUserID);
        Fabric Save(Fabric oFabric, long nUserID);
        Fabric SaveReceiveDate(Fabric oFabric, long nUserID);
        Fabric SaveSubmissionDate(Fabric oFabric, long nUserID);
        Fabric ReFabricSubmission(Fabric oFabric, long nUserID);
        List<Fabric> Received(List<Fabric> oFabrics, long nUserID);
        List<Fabric> Submission(List<Fabric> oFabrics, long nUserID);
        Fabric SaveSubmission(Fabric oFabric, long nUserID);
        Fabric SaveReceived(Fabric oFabric, long nUserID);
    }
    #endregion
}