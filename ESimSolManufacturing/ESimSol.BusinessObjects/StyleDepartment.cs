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
   
    
    #region StyleDepartment
    
    public class StyleDepartment:BusinessObject
    {
        public StyleDepartment()
        {
            StyleDepartmentID = 0;             
            Name = "";
            Note = "";
            ErrorMessage = "";

        }

        #region Properties
         
        public int StyleDepartmentID { get; set; }
      
         
        public string Name { get; set; }
       
         
        public string Note { get; set; }
       
         
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<StyleDepartment> StyleDepartments { get; set; }
        public Company Company { get; set; }
        #endregion

        #region Functions

        public static List<StyleDepartment> Gets(long nUserID)
        {
            return StyleDepartment.Service.Gets( nUserID);
        }

        public static List<StyleDepartment> Gets(string sSQL, long nUserID)
        {
            return StyleDepartment.Service.Gets(sSQL, nUserID);
        }

        public StyleDepartment Get(int id, long nUserID)
        {
            return StyleDepartment.Service.Get(id, nUserID);
        }

        public StyleDepartment Save(long nUserID)
        {
            return StyleDepartment.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return StyleDepartment.Service.Delete(id, nUserID);
        }
        

        #endregion

        #region ServiceFactory
        internal static IStyleDepartmentService Service
        {
            get { return (IStyleDepartmentService)Services.Factory.CreateService(typeof(IStyleDepartmentService)); }
        }


        #endregion
    }
    #endregion

    #region Report Study
    public class StyleDepartmentList : List<StyleDepartment>
    {
        public string ImageUrl { get; set; }
    }
    #endregion

    #region IStyleDepartment interface
     
    public interface IStyleDepartmentService
    {
         
        StyleDepartment Get(int id, Int64 nUserID);
         
        List<StyleDepartment> Gets(Int64 nUserID);
         
        List<StyleDepartment> Gets(string sSQL, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID);
         
        StyleDepartment Save(StyleDepartment oStyleDepartment, Int64 nUserID);
       
    }
    #endregion
    
  
}
