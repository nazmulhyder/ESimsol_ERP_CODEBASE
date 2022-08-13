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
    #region SizeCategory
    
    public class SizeCategory : BusinessObject
    {
        public SizeCategory()
        {
            SizeCategoryID=0;            
            SizeCategoryName = "";
            Sequence = 0;
            Note = "";
            Param = "";
            ErrorMessage = "";
            SizeCategories = new List<SizeCategory>();
        }

        #region Properties
         
        public int SizeCategoryID{ get; set; }        
         
        public string SizeCategoryName{ get; set; }
         
        public int Sequence { get; set; }
         
        public string Note { get; set; }
        public string Param { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property        
        public bool Selected { get; set; }
         
        public List<SizeCategory> SizeCategories { get; set; }
        #endregion

        #region Functions

        public static List<SizeCategory> Gets(long nUserID)
        {
            
            return SizeCategory.Service.Gets( nUserID);
        }

        public static List<SizeCategory> ResetSequence(SizeCategory oSizeCategory, long nUserID)
        {
            
            return SizeCategory.Service.ResetSequence(oSizeCategory, nUserID);
        }

        public static List<SizeCategory> GetsBySizeCategory(string sSizeCategory, long nUserID)
        {
            return SizeCategory.Service.GetsBySizeCategory(sSizeCategory, nUserID);
        }
        public SizeCategory Get(int id, long nUserID)
        {
            return SizeCategory.Service.Get(id, nUserID);
        }

        public SizeCategory Save(long nUserID)
        {
            return SizeCategory.Service.Save(this, nUserID);
        }

        public static List<SizeCategory> Gets(string sSQL, long nUserID)
        {
            return SizeCategory.Service.Gets(sSQL, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return SizeCategory.Service.Delete(id, nUserID);
        }
        public static List<SizeCategory> GetsSizeForQC(int nSaleorderID, long nUserID)
        {
            return SizeCategory.Service.GetsSizeForQC(nSaleorderID, nUserID);
        }
        #endregion

        #region ServiceFactory

 
        internal static ISizeCategoryService Service
        {
            get { return (ISizeCategoryService)Services.Factory.CreateService(typeof(ISizeCategoryService)); }
        }

        #endregion
    }
    #endregion

    #region ISizeCategory interface
     
    public interface ISizeCategoryService
    {
         
        SizeCategory Get(int id, Int64 nUserID);
         
        List<SizeCategory> Gets(Int64 nUserID);
         
        List<SizeCategory> Gets(string sSQL, Int64 nUserID);
         
        List<SizeCategory> ResetSequence(SizeCategory oSizeCategory, Int64 nUserID);
         
        List<SizeCategory> GetsBySizeCategory(string sSizeCategory, Int64 nUserID);        
         
        string Delete(int id, Int64 nUserID);
         
        SizeCategory Save(SizeCategory oSizeCategory, Int64 nUserID);
         
        List<SizeCategory> GetsSizeForQC(int nSaleOrderID, Int64 nUserID);
    }
    #endregion
}
