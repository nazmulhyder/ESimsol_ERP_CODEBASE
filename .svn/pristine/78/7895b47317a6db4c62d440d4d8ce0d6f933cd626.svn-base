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
    #region TechnicalSheet
    
    public class TechnicalSheet : BusinessObject
    {
        public TechnicalSheet()
        {
            TechnicalSheetID = 0;
            StyleNo = "";
            BusinessSessionID = 0;
            DevelopmentStatus = EnumDevelopmentStatus.Initialize;
            BuyerID = 0;
            ProductID = 0;
            Dept = 0;
            BuyerConcernID = 0;
            YarnCategoryID = 0;

            GG = "";
            Count = "";
            SpecialFinish = "";
            Weight = "";
            Line = "";
            Designer = "";
            Story = "";
            GarmentsClassID = 0;
            GarmentsSubClassID = 0;
            Intake = "";
            Note = "";
            KnittingPattern = 0;
            StyleDescription = "";
            BuyerName = "";
            BuyerAddress = "";
            BuyerPhone = "";
            BuyerShortName = "";
            ProductCode = "";
            ProductName = "";
          
            ConcernName = "";
            ConcernEmail = "";
            ClassName = "";
            SubClassName = "";
            SessionName = "";
            ErrorMessage = "";
            ColorRangeIDs = "";
            SizeRangeIDs = "";
            MeasurementSpecID = 0;
            SampleSizeID = 0;
            SizeClassID = 0;
            GarmentsTypeID = 0;
            MeasurementUnitID = 0;
            SizeRange = "";
            SizeRangeIDs = "";
            ColorRange = "";
            ColorRangeIDs = "";
            ShownAs = "";
            MSNote = "";
            KnittingPatternName = "";
            TSType = EnumTSType.Sweater;
            TSTypeInInt = 0;

            GSMID = 0;
            GSMName = "";
            FabConstruction= "";
            Wash = "";
            FabWidth = "";
            FabCode = "";
            BrandName = "";
          
            OldTSID = 0;
            BUID = 0;
            ApproxQty = 0;
            UserName = "";
            TSCount = 0;
            MerchandiserID = 0;
            MerchandiserName = "";
            FabricDescription = "";
            DBServerDateTime = DateTime.Now;
            SubGender = EnumSubGender.None;
            IsExistDevelopmentRecap = false;
            IsExistOrderRecap = false;
            IsFronImage = true;
            BusinessSessions = new List<BusinessSession>();
            ImageComments = new List<ImageComment>();
            Buyers = new List<Contractor>();
            BuyerConcerns = new List<BuyerConcern>();
            GarmentsClasss = new List<GarmentsClass>();
            GarmentsSubClasss = new List<GarmentsClass>();
            BillOfMaterials = new List<BillOfMaterial>();
            MeasurementSpecDetails = new List<MeasurementSpecDetail>();
            TempMeasurementSpecDetails = new List<TempMeasurementSpecDetail>();
            GarmentsTypes = new List<GarmentsType>();
            SampleSizes = new List<SizeCategory>();
            SizeClasss = new List<GarmentsClass>();
            TechnicalSheetSizes = new List<TechnicalSheetSize>();
            TechnicalSheetColors = new List<TechnicalSheetColor>();
            TechnicalSheetImages = new List<TechnicalSheetImage>();
            TechnicalSheetThumbnails = new List<TechnicalSheetThumbnail>();            
            TechnicalSheetList = new List<TechnicalSheet>();
            TechnicalSheetPrint = new List<TechnicalSheet>();
            MaterialTypes = new List<MaterialType>();
            MeasurementSpec = new BusinessObjects.MeasurementSpec();
            MeasurementSpecAttachments = new List<MeasurementSpecAttachment>();
            StyleDepartments = new List<StyleDepartment>();
            KnittingPatternList = new List<Knitting>();
            BuyerWiseBrands = new List<BuyerWiseBrand>();
            TechnicalSheetShipments = new List<TechnicalSheetShipment>();
            OrderRecapYarns = new List<OrderRecapYarn>();
            MUnitSymbol = "";
        }

        #region Properties
         
        public int TechnicalSheetID { get; set; }
        public int OldTSID { get; set; }
        public string StyleNo { get; set; }
        public EnumSubGender SubGender { get; set; }
        public int BusinessSessionID { get; set; }
        public EnumDevelopmentStatus DevelopmentStatus { get; set; }
        public int BuyerID { get; set; }
        public int BUID { get; set; }
        public int ProductID { get; set; }
        public string UserName { get; set; }
        public int Dept { get; set; }
        public int TSCount { get; set; }
        public string FabricDescription { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public int BuyerConcernID { get; set; }
        public int YarnCategoryID { get; set; }

        public string Line { get; set; }
        public double ApproxQty { get; set; }
        public string Designer { get; set; }
         
        public string Story { get; set; }
         
        public string GG { get; set; }
         
        public string Count { get; set; }
         
        public string SpecialFinish{ get; set; }
        public int GSMID { get; set; }
        public string GSMName { get; set; }
        public string Weight{ get; set; }
        public string BrandName { get; set; }
        public int BrandID { get; set; }
        public int GarmentsClassID { get; set; }
         
        public int GarmentsSubClassID { get; set; }
         
        public string Intake { get; set; }
         
        public string Note { get; set; }
         
        public int KnittingPattern { get; set; }
         
        public string StyleDescription { get; set; }  
         
        public string BuyerName { get; set; }  
         
        public string BuyerAddress { get; set; }  
         
        public string BuyerPhone { get; set; }  
         
        public string BuyerShortName { get; set; }  
         
        public string ProductCode { get; set; }  
         
        public string ProductName { get; set; }  
         
        public string ConcernName { get; set; }  
         
        public string ConcernEmail { get; set; }  
         
        public string ClassName { get; set; }  
         
        public string SubClassName { get; set; }
         
        public string SessionName { get; set; }
         
        public string ErrorMessage { get; set; }
         
        public bool IsExistDevelopmentRecap { get; set; }  
         
        public bool IsExistOrderRecap { get; set; }
         
        public EnumTSType TSType { get; set; }
         
        public int TSTypeInInt { get; set; }
         
        public string FabConstruction { get; set; }
         
        public string Wash { get; set; }
         
        public string FabWidth { get; set; }
        public string KnittingPatternName { get; set; }
        public string FabCode { get; set; }
        public string DeptName { get; set; }
     
        public int MerchandiserID { get; set; }
        public int WorkingUnitID { get; set; }
        public string MerchandiserName { get; set; }

        #endregion

        #region Derived Property
        public BusinessUnit BusinessUnit { get; set; }
        public bool IsFronImage { get; set; }
        public List<BuyerWiseBrand> BuyerWiseBrands { get; set; }
        public List<StyleDepartment> StyleDepartments { get; set; }
        public List<Knitting> KnittingPatternList { get; set; }
        public List<ImageComment> ImageComments { get; set; }
        public List<BusinessSession> BusinessSessions { get; set; }
        public List<TechnicalSheet> TechnicalSheetPrint { get; set; }
        public List<MaterialType> MaterialTypes { get; set; }
        public List<OrderRecapYarn> OrderRecapYarns { get; set; }
        public List<TechnicalSheetShipment> TechnicalSheetShipments { get; set; }        
        public List<MeasurementSpecAttachment> MeasurementSpecAttachments { get; set; }
        public List<Employee> Employees { get; set; }

        public string TSCountInString
        {
            get
            {
                return this.TechnicalSheetID + "~" + this.TSCount + "~'" + this.StyleNo + "'";
            }
        }
        public string DBServerDateTimeInSt
        {
            get
            {
                return this.DBServerDateTime.ToString("dd MMM yyyy");
            }
        }
        public string TSTypeInString
        {
            get
            {
                return this.TSType.ToString();
            }
        }
        public string StyeleWithIDInString
        {
            get
            {
                //REfID ~RefNo~RefType //don't delete use in Merchanidsing Report 
                return this.TechnicalSheetID+ "~" + this.StyleNo+"~1";
            }
        }
        public string ApproxQtyInSt
        {
            get
            {
                return Global.MillionFormat(this.ApproxQty);
            }
        }


        #region String Conversion
        public string DevelopmentStatusInString
        {
            get
            {
                return this.DevelopmentStatus.ToString();
            }
        }
        public int DevelopmentStatusInInt { get; set; }
        #endregion

        #region Another Object
         
        public string ColorRangeIDs { get; set; }
         
        public string SizeRangeIDs { get; set; }
         
        public string SizeRange { get; set; }
                 
        public string ColorRange { get; set; }
         
        public int MeasurementSpecID { get; set; }
         
        public int SampleSizeID { get; set; }
         
        public int SizeClassID { get; set; }
         
        public int GarmentsTypeID { get; set; }
         
        public int MeasurementUnitID { get; set; }
        public string MUnitSymbol { get; set; }
        public string ShownAs { get; set; }
         
        public string MSNote { get; set; }
        public string ImageUrl { get; set; }
        public Company Company { get; set; }
        public string BusinessSession { get; set; }
        public List<Contractor> Buyers { get; set; }
        public List<BuyerConcern> BuyerConcerns { get; set; }
        public List<GarmentsClass> GarmentsClasss { get; set; }
        public List<GarmentsClass> GarmentsSubClasss { get; set; }
        public List<BillOfMaterial> BillOfMaterials { get; set; }
        public MeasurementSpec MeasurementSpec { get; set; }
        public List<MeasurementSpecDetail> MeasurementSpecDetails { get; set; }
        public List<TempMeasurementSpecDetail> TempMeasurementSpecDetails { get; set; }
        public List<GarmentsType> GarmentsTypes { get; set; }
        public List<SizeCategory> SampleSizes { get; set; }
        public List<GarmentsClass> SizeClasss { get; set; }
        public List<MeasurementUnit> MeasurementUnits { get; set; }
         
        public List<TechnicalSheetSize> TechnicalSheetSizes { get; set; }
         
        public List<TechnicalSheetColor> TechnicalSheetColors { get; set; }
        public TechnicalSheetImage TechnicalSheetImage { get; set; }
        public List<TechnicalSheetImage> TechnicalSheetImages { get; set; }
        public List<TechnicalSheetThumbnail> TechnicalSheetThumbnails { get; set; }
        public TechnicalSheetThumbnail TechnicalSheetThumbnail { get; set; }
        public TechnicalSheetThumbnail TechnicalSheetThumbnailForMeasurmentSpec { get; set; }        
        public List<TechnicalSheet> TechnicalSheetList { get; set; }
        
        #endregion
        #endregion

        #region Functions
        public static List<TechnicalSheet> Gets_Report(int id,long nUserID)
        {
            return TechnicalSheet.Service.Gets_Report(id, nUserID);
        }

        public static List<TechnicalSheet> BUWiseGets(int BUID, string DevelopmentStatus, long nUserID)
        {
            return TechnicalSheet.Service.BUWiseGets(BUID, DevelopmentStatus, nUserID);
        }

        public static List<TechnicalSheet> Gets(string sSQL,long nUserID)
        {
            return TechnicalSheet.Service.Gets(sSQL, nUserID);
        }

        public static List<TechnicalSheet> WaitForApproval(long nUserID)
        {
            return TechnicalSheet.Service.WaitForApproval( nUserID);
        }

        public  TechnicalSheet Get(int id, long nUserID)
        {
            
            return TechnicalSheet.Service.Get(id, nUserID);
        }        
        public TechnicalSheet GetByStyleNo(String StyleNo, long nUserID)
        {           
            return TechnicalSheet.Service.GetByStyleNo(StyleNo, nUserID);
        }

        
        public TechnicalSheet Save(long nUserID)
        {           
            return TechnicalSheet.Service.Save(this, nUserID);
        }

        public TechnicalSheet UpdateStatus(int nTechnicalSheetID, int nDevelopmentStatus, ApprovalRequest _oApprovalRequest, long nUserID)
        {
            return TechnicalSheet.Service.UpdateStatus(nTechnicalSheetID,nDevelopmentStatus,_oApprovalRequest, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return TechnicalSheet.Service.Delete(id, nUserID);
        }

        #endregion

        #region ServiceFactory

      
        internal static ITechnicalSheetService Service
        {
            get { return (ITechnicalSheetService)Services.Factory.CreateService(typeof(ITechnicalSheetService)); }
        }

        #endregion
    }
    #endregion

    #region ITechnicalSheet interface
     
    public interface ITechnicalSheetService
    {
         
        TechnicalSheet Get(int id, Int64 nUserID);
         
        TechnicalSheet GetByStyleNo(string StyleNo, Int64 nUserID);  
         
        List<TechnicalSheet> WaitForApproval(Int64 nUserID);

        List<TechnicalSheet> BUWiseGets(int BUID, string DevelopmentStatus, Int64 nUserID);
         
        List<TechnicalSheet> Gets(string sSQL,Int64 nUserID);
         
        List<TechnicalSheet> Gets_Report(int id, Int64 nUserID);       
         
        string Delete(int id, Int64 nUserID);
         
        TechnicalSheet Save(TechnicalSheet oTechnicalSheet, Int64 nUserID);
         
        TechnicalSheet UpdateStatus(int nTechnicalSheetID, int nDevelopmentStatus, ApprovalRequest _oApprovalRequest, Int64 nUserID);
    }
    #endregion
}
