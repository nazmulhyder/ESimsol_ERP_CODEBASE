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
    #region ModelCategory
    
    public class ModelCategory : BusinessObject
    {
        public ModelCategory()
        {
            ModelCategoryID = 0;
            CategoryName="";
            Remarks = "";
            ErrorMessage = "";
        }

        #region Properties
         
        public int ModelCategoryID { get; set; }
         
        public string CategoryName { get; set; }
         
        public string Remarks { get; set; }
        public string Param { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
       public List<ModelCategory> ColorCategories { get; set; }
        #endregion

        #region Functions
        
        public static List<ModelCategory> Gets(long nUserID)
        {
            return ModelCategory.Service.Gets( nUserID);
        }

        public static List<ModelCategory> GetsbyCategoryName(string sCategoryName, long nUserID)
        {
            return ModelCategory.Service.GetsbyCategoryName(sCategoryName, nUserID);
        }

        public static List<ModelCategory> GetsTSNotAssignColor(int nTechnicalSheetID, long nUserID)
        {
            return ModelCategory.Service.GetsTSNotAssignColor(nTechnicalSheetID, nUserID);
        }

        public ModelCategory Get(int id, long nUserID)
        {
            return ModelCategory.Service.Get(id, nUserID);
        }
        public static List<ModelCategory> Gets(string sSQL, long nUserID)
        {           
            return ModelCategory.Service.Gets(sSQL, nUserID);
        }
        public ModelCategory Save(long nUserID)
        {
            
            return ModelCategory.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ModelCategory.Service.Delete(id, nUserID);
        }
        #region Piker FunctionQC

        public static List<ModelCategory> GetsColorPikerForQC(int nTechnicalSheetID, long nUserID)
        {
            return ModelCategory.Service.GetsColorPikerForQC(nTechnicalSheetID,nUserID);
        }
        #endregion Piker Function QC
       
        #endregion

        #region ServiceFactory

        internal static IModelCategoryService Service
        {
            get { return (IModelCategoryService)Services.Factory.CreateService(typeof(IModelCategoryService)); }
        }


        #endregion
    }
    #endregion

    #region IModelCategory interface
     
    public interface IModelCategoryService
    {
         
        ModelCategory Get(int id, Int64 nUserID);
         
        List<ModelCategory> GetsTSNotAssignColor(int nTechnicalSheetID, Int64 nUserID);
         
        List<ModelCategory> Gets(Int64 nUserID);
         
        List<ModelCategory> Gets(string sSQL, Int64 nUserID);
         
        List<ModelCategory> GetsbyCategoryName(string sCategoryName, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        ModelCategory Save(ModelCategory oModelCategory, Int64 nUserID);

        #region Piker Function QC
         
        List<ModelCategory> GetsColorPikerForQC(int nTechnicalSheetID, Int64 nUserID);
        #endregion Piker Function QC
    }
    #endregion
}
