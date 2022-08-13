using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;



namespace ESimSol.BusinessObjects
{
    #region ColorCategory
    
    public class ColorCategory : BusinessObject
    {
        public ColorCategory()
        {
            ColorCategoryID = 0;
            ColorName="";
            Note = "";
            Param = "";
            ObjectID = 0;
            ObjectName = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int ColorCategoryID { get; set; }
         
        public string ColorName { get; set; }
         
        public string Note { get; set; }
        public string Param { get; set; } 

        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public bool Selected { get; set; }
        public List<ColorCategory> ColorCategories { get; set; }
        public int ObjectID { get; set; }
        public string ObjectName { get; set; }
        #endregion

        #region Functions
        
        public static List<ColorCategory> Gets(long nUserID)
        {
            return ColorCategory.Service.Gets( nUserID);
        }

        public static List<ColorCategory> GetsbyColorName(string sColorName, long nUserID)
        {
            return ColorCategory.Service.GetsbyColorName(sColorName, nUserID);
        }

        public static List<ColorCategory> GetsTSNotAssignColor(int nTechnicalSheetID, long nUserID)
        {
            return ColorCategory.Service.GetsTSNotAssignColor(nTechnicalSheetID, nUserID);
        }

        public ColorCategory Get(int id, long nUserID)
        {
            return ColorCategory.Service.Get(id, nUserID);
        }
        public static List<ColorCategory> Gets(string sSQL, long nUserID)
        {           
            return ColorCategory.Service.Gets(sSQL, nUserID);
        }
        public ColorCategory Save(long nUserID)
        {
            
            return ColorCategory.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ColorCategory.Service.Delete(id, nUserID);
        }
        #region Piker FunctionQC

        public static List<ColorCategory> GetsColorPikerForQC(int nTechnicalSheetID, long nUserID)
        {
            return ColorCategory.Service.GetsColorPikerForQC(nTechnicalSheetID,nUserID);
        }
        #endregion Piker Function QC
       
        #endregion

        #region ServiceFactory

        internal static IColorCategoryService Service
        {
            get { return (IColorCategoryService)Services.Factory.CreateService(typeof(IColorCategoryService)); }
        }


        #endregion
    }
    #endregion

    #region IColorCategory interface
     
    public interface IColorCategoryService
    {
         
        ColorCategory Get(int id, Int64 nUserID);
         
        List<ColorCategory> GetsTSNotAssignColor(int nTechnicalSheetID, Int64 nUserID);
         
        List<ColorCategory> Gets(Int64 nUserID);
         
        List<ColorCategory> Gets(string sSQL, Int64 nUserID);
         
        List<ColorCategory> GetsbyColorName(string sColorName, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        ColorCategory Save(ColorCategory oColorCategory, Int64 nUserID);

        #region Piker Function QC
         
        List<ColorCategory> GetsColorPikerForQC(int nTechnicalSheetID, Int64 nUserID);
        #endregion Piker Function QC
    }
    #endregion
}
