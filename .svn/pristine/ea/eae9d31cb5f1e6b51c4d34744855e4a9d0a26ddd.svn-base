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
    #region sp_Rpt_ShadeWiseColorRecipe
    [DataContract]
    public class sp_Rpt_ShadeWiseColorRecipe : BusinessObject
    {
        #region  Constructor
        public sp_Rpt_ShadeWiseColorRecipe()
        {
            Shade = "";
            LabdipResultID = 0;
            LabdipRecipeChemicalID = 0;
            ChemicalID = 0;
            ChemicalName = "";
            ChemiQuantity = 0.0;
            LabdipRecipeDyeID = 0;
            DyeID = 0;
            DyeName = "";
            DyeQuantity = 0.0;
        }
        #endregion

        #region Properties
        
        public string Shade { get; set; }
        
        public int LabdipResultID { get; set; }
        
        public int LabdipRecipeChemicalID { get; set; }
        
        public int ChemicalID { get; set; }
        
        public string ChemicalName { get; set; }
        
        public double ChemiQuantity { get; set; }
        
        public int LabdipRecipeDyeID { get; set; }
        
        public int DyeID { get; set; }
        
        public string DyeName { get; set; }
        
        public double DyeQuantity { get; set; }
       
        #endregion

        #region Functions
        public static List<sp_Rpt_ShadeWiseColorRecipe> GetsFromJFSP(string sWYLDIDs, long nUserID)
        {
            return sp_Rpt_ShadeWiseColorRecipe.Service.GetsFromJFSP(sWYLDIDs, nUserID);
        } 
        #endregion
        #region ServiceFactory
        internal static Isp_Rpt_ShadeWiseColorRecipeService Service
        {
            get { return (Isp_Rpt_ShadeWiseColorRecipeService)Services.Factory.CreateService(typeof(Isp_Rpt_ShadeWiseColorRecipeService)); }
        }
      
        #endregion
    }
    #endregion
    
    #region Isp_Rpt_ShadeWiseColorRecipe interface
    
    public interface Isp_Rpt_ShadeWiseColorRecipeService
    {
        
        List<sp_Rpt_ShadeWiseColorRecipe> GetsFromJFSP(string sWYLDIDs,Int64 nUserId);
     
    }
   
    #endregion

}