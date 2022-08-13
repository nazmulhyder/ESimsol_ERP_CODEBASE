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
   
    
    #region MaterialType
    
    public class MaterialType:BusinessObject
    {
        public MaterialType()
        {
            MaterialTypeID = 0;             
            Name = "";
            Note = "";
            ErrorMessage = "";

        }

        #region Properties
         
        public int MaterialTypeID { get; set; }
      
         
        public string Name { get; set; }
       
         
        public string Note { get; set; }
       
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<MaterialType> MaterialTypes { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions

        public static List<MaterialType> Gets(long nUserID)
        {
            return MaterialType.Service.Gets( nUserID);
        }

        public static List<MaterialType> Gets(string sSQL, long nUserID)
        {
            return MaterialType.Service.Gets(sSQL, nUserID);
        }

        public MaterialType Get(int id, long nUserID)
        {
            return MaterialType.Service.Get(id, nUserID);
        }

        public MaterialType Save(long nUserID)
        {
            return MaterialType.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return MaterialType.Service.Delete(id, nUserID);
        }
        

        #endregion

        #region ServiceFactory
        internal static IMaterialTypeService Service
        {
            get { return (IMaterialTypeService)Services.Factory.CreateService(typeof(IMaterialTypeService)); }
        }


        #endregion
    }
    #endregion

    #region Report Study
    public class MaterialTypeList : List<MaterialType>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IMaterialType interface
     
    public interface IMaterialTypeService
    {
         
        MaterialType Get(int id, Int64 nUserID);
         
        List<MaterialType> Gets(Int64 nUserID);
         
        List<MaterialType> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        MaterialType Save(MaterialType oMaterialType, Int64 nUserID);
       
    }
    #endregion
    
  
}
