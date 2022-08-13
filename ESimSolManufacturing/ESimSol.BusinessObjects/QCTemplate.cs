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

    #region QCTemplate
    
    public class QCTemplate : BusinessObject
    {
        public QCTemplate()
        {
            QCTemplateID = 0;
            TemplateNo = "";
            TemplateName ="";
            CreateBy = 0;
            CreateByName = "";
            CreateDate = DateTime.Now;
            Note = "";
            bIsInitialSave = false;
            QCTemplateDetails = new List<QCTemplateDetail>();
            ErrorMessage = "";

        }

        #region Properties
         
        public int QCTemplateID { get; set; }
         
        public int CreateBy { get; set; }
         
        public string TemplateNo { get; set; }
         
        public string TemplateName { get; set; }
         
        public string CreateByName { get; set; }
         
        public DateTime CreateDate { get; set; }
        public bool bIsInitialSave { get; set; }
      
        public string Note { get; set; }

         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<QCTemplate> QCTemplates { get; set; }
         
        public List<QCTemplateDetail> QCTemplateDetails { get; set; }
        public Company Company { get; set; }
        public List<TechnicalSheetSize> TechnicalSheetSizes { get; set; }
        public List<ColorSizeRatio> ColorSizeRatios { get; set; }
        public TQCTemplateDetail TQCTemplateDetail { get; set; }
     
        public string CreateDateInString
        {
            get
            {
                return this.CreateDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions

        public static List<QCTemplate> Gets(long nUserID)
        {
            return QCTemplate.Service.Gets( nUserID);
        }


        public static List<QCTemplate> Gets(string sSQL, long nUserID)
        {           
            return QCTemplate.Service.Gets(sSQL, nUserID);
        }

        public QCTemplate Get(int id, long nUserID)
        {
            
            return QCTemplate.Service.Get(id, nUserID);
        }

        public QCTemplate Save(long nUserID)
        {
            return QCTemplate.Service.Save(this, nUserID);
        }
        public QCTemplate UpDown(QCTemplateDetail oQCTemplateDetail,  long nUserID)
        {
            return QCTemplate.Service.UpDown(oQCTemplateDetail, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return QCTemplate.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

  
        internal static IQCTemplateService Service
        {
            get { return (IQCTemplateService)Services.Factory.CreateService(typeof(IQCTemplateService)); }
        }

        #endregion
    }
    #endregion

    #region Report Study
    public class QCTemplateList : List<QCTemplate>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IQCTemplate interface
     
    public interface IQCTemplateService
    {
         
        QCTemplate Get(int id, Int64 nUserID);
         
        List<QCTemplate> Gets(Int64 nUserID);
   
        List<QCTemplate> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        QCTemplate Save(QCTemplate oQCTemplate, Int64 nUserID);
        QCTemplate UpDown(QCTemplateDetail oQCTemplateDetail, Int64 nUserID);
        

    }
    #endregion
    

}
