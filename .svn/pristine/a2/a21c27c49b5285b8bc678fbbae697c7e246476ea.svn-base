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
    #region LabdipRecipe

    public class LabdipRecipe : BusinessObject
    {

        #region  Constructor
        public LabdipRecipe()
        {
            LabdipRecipeID = 0;
            LabdipShadeID = 0;
            ProductID = 0;
            Qty = 0;
            Note = "";
            IsDyes = false;
            LotID = 0;
            PerTage = 0;
            ErrorMessage = "";
            LotNo = "";
        }
        #endregion

        #region Properties

        public int LabdipRecipeID { get; set; }
        public int LabdipShadeID { get; set; }
        public int ProductID { get; set; }
        public int LotID { get; set; } // For Dyes-Chemical Lot , not mendatory
        public double Qty { get; set; }
        public double PerTage { get; set; }
        public string Note { get; set; }
        public bool IsDyes { get; set; }
        public bool IsGL { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Properties
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string LotNo { get; set; }
        public string ProductNameCode
        {
            get
            {
                return this.ProductName + " [" + this.ProductCode + "]";
            }
        }
         
        public string IsGLSt
        {
            get
            {
                if (this.IsGL)
                { return "GL"; }
                else {return "%";}
            }
        }
        #endregion


        #region Functions

        public LabdipRecipe IUD(int nDBOperation, long nUserID)
        {
            return LabdipRecipe.Service.IUD(this, nDBOperation, nUserID);
        }
        public LabdipRecipe Get(int nLabdipRecipeID, long nUserID)
        {
            return LabdipRecipe.Service.Get(nLabdipRecipeID, nUserID);
        }
        public static List<LabdipRecipe> Gets(string sSQL, long nUserID)
        {
            return LabdipRecipe.Service.Gets(sSQL, nUserID);
        }
        public static List<LabdipRecipe> Gets(int nLabdipDetailID,int nShade, long nUserID)
        {
            return LabdipRecipe.Service.Gets(nLabdipDetailID, nShade,nUserID);
        }
        public LabdipRecipe UpdateLot(int nUserID)
        {
            return LabdipRecipe.Service.UpdateLot(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILabdipRecipeService Service
        {
            get { return (ILabdipRecipeService)Services.Factory.CreateService(typeof(ILabdipRecipeService)); }
        }
        #endregion
    }
    #endregion



    #region ILabdipRecipe interface
    public interface ILabdipRecipeService
    {
        LabdipRecipe IUD(LabdipRecipe oLabdipRecipe, int nDBOperation, long nUserID);
        LabdipRecipe Get(int nLabdipRecipeID, long nUserID);
        List<LabdipRecipe> Gets(string sSQL, long nUserID);
        List<LabdipRecipe> Gets(int nLabdipDetailID, int nShade, long nUserID);
        LabdipRecipe UpdateLot(LabdipRecipe oLabdipRecipe, int nUserID);
    }
    #endregion
}