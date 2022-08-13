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
   
    
    #region DevelopmentType
    
    public class DevelopmentType:BusinessObject
    {
        public DevelopmentType()
        {
            DevelopmentTypeID = 0;             
            Name = "";
            Note = "";
            ErrorMessage = "";

        }

        #region Properties
         
        public int DevelopmentTypeID { get; set; }
      
         
        public string Name { get; set; }
       
         
        public string Note { get; set; }
       
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<DevelopmentType> DevelopmentTypes { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions

        public static List<DevelopmentType> Gets(long nUserID)
        {
            return DevelopmentType.Service.Gets( nUserID);
        }

        public static List<DevelopmentType> Gets(string sSQL, long nUserID)
        {
            return DevelopmentType.Service.Gets(sSQL, nUserID);
        }

        public DevelopmentType Get(int id, long nUserID)
        {
            return DevelopmentType.Service.Get(id, nUserID);
        }

        public DevelopmentType Save(long nUserID)
        {
            return DevelopmentType.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return DevelopmentType.Service.Delete(id, nUserID);
        }
        

        #endregion

        #region ServiceFactory
        internal static IDevelopmentTypeService Service
        {
            get { return (IDevelopmentTypeService)Services.Factory.CreateService(typeof(IDevelopmentTypeService)); }
        }


        #endregion
    }
    #endregion

    #region Report Study
    public class DevelopmentTypeList : List<DevelopmentType>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IDevelopmentType interface
     
    public interface IDevelopmentTypeService
    {
         
        DevelopmentType Get(int id, Int64 nUserID);
         
        List<DevelopmentType> Gets(Int64 nUserID);
         
        List<DevelopmentType> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        DevelopmentType Save(DevelopmentType oDevelopmentType, Int64 nUserID);
       
    }
    #endregion
    
  
}
