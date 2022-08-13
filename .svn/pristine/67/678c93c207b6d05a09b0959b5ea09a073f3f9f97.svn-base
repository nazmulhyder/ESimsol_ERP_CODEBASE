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

    #region TAPTemplate
    
    public class TAPTemplate : BusinessObject
    {
        public TAPTemplate()
        {
            TAPTemplateID = 0;
            TemplateNo = "";
            TemplateName ="";
            CreateBy = 0;
            CreateByName = "";
            CreateDate = DateTime.Now;
            Remarks = "";
            TampleteType = EnumTSType.Sweater;
            CalculationType = EnumCalculationType.Days;
            bIsInitialSave = false;
            TAPTemplateDetails = new List<TAPTemplateDetail>();
            ErrorMessage = "";

        }

        #region Properties
         
        public int TAPTemplateID { get; set; }
         
        public int CreateBy { get; set; }
         
        public string TemplateNo { get; set; }
         
        public string TemplateName { get; set; }
         
        public string CreateByName { get; set; }
         
        public DateTime CreateDate { get; set; }
        public bool bIsInitialSave { get; set; }
        public EnumTSType TampleteType { get; set; }
        public EnumCalculationType CalculationType { get; set; }
        public string Remarks { get; set; }

        public int TampleteTypeInInt { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<TAPTemplate> TAPTemplates { get; set; }
         
        public List<TAPTemplateDetail> TAPTemplateDetails { get; set; }
        public Company Company { get; set; }
        public List<TechnicalSheetSize> TechnicalSheetSizes { get; set; }
        public List<ColorSizeRatio> ColorSizeRatios { get; set; }
        public TTAPTemplateDetail TTAPTemplateDetail { get; set; }
        public string TampleteTypeInString 
        { 
            get
            {
                return this.TampleteType.ToString();
            }
        }
        public string CalculationTypeInString
        {
            get
            {
                return this.CalculationType.ToString();
            }
        }
        public string CreateDateInString
        {
            get
            {
                return this.CreateDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions

        public static List<TAPTemplate> Gets(long nUserID)
        {
            return TAPTemplate.Service.Gets( nUserID);
        }

        public static List<TAPTemplate> GetsByTemplateType(int nTamplateType, long nUserID)
        {
            return TAPTemplate.Service.GetsByTemplateType(nTamplateType, nUserID);
        }

        public static List<TAPTemplate> Gets(string sSQL, long nUserID)
        {           
            return TAPTemplate.Service.Gets(sSQL, nUserID);
        }

        public TAPTemplate Get(int id, long nUserID)
        {
            
            return TAPTemplate.Service.Get(id, nUserID);
        }

        public TAPTemplate Save(long nUserID)
        {
            return TAPTemplate.Service.Save(this, nUserID);
        }
        public TAPTemplate UpDown(TAPTemplateDetail oTAPTemplateDetail,  long nUserID)
        {
            return TAPTemplate.Service.UpDown(oTAPTemplateDetail, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return TAPTemplate.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

  
        internal static ITAPTemplateService Service
        {
            get { return (ITAPTemplateService)Services.Factory.CreateService(typeof(ITAPTemplateService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class TAPTemplateList : List<TAPTemplate>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region ITAPTemplate interface
     
    public interface ITAPTemplateService
    {
         
        TAPTemplate Get(int id, Int64 nUserID);
         
        List<TAPTemplate> Gets(Int64 nUserID);
        List<TAPTemplate> GetsByTemplateType(int nTempateType, Int64 nUserID);
         
        List<TAPTemplate> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        TAPTemplate Save(TAPTemplate oTAPTemplate, Int64 nUserID);
        TAPTemplate UpDown(TAPTemplateDetail oTAPTemplateDetail, Int64 nUserID);
        

    }
    #endregion
    

}
