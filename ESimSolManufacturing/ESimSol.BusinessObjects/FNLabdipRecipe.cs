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
    #region FNLabdipRecipe

    public class FNLabdipRecipe : BusinessObject
    {

        #region  Constructor
        public FNLabdipRecipe()
        {
            FNLabdipRecipeID = 0;
            FNLabdipShadeID = 0;
            ProductID = 0;
            FabricOrderType = EnumFabricRequestType.None;
            FabricOrderTypeInInt = 0;
            Qty = 0;
            Note = "";
            IsDyes = false;
            PerTage = 0;
            ErrorMessage = "";
        }
        #endregion

        #region Properties

        public int FNLabdipRecipeID { get; set; }
        public int FNLabdipShadeID { get; set; }
        public int ProductID { get; set; }
        public double Qty { get; set; }
        public double PerTage { get; set; }
        public string Note { get; set; }
        public bool IsDyes { get; set; }
        public bool IsGL { get; set; }
        public EnumFabricRequestType FabricOrderType { get; set; }
        public int FabricOrderTypeInInt { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Properties
        public string ProductName { get; set; }
        public string OrderName { get; set; }
        public string ProductCode { get; set; }
        public string FabricOrderTypeSt
        {
            get
            {
                return this.FabricOrderType.ToString();
            }
        }
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

        public FNLabdipRecipe IUD(int nDBOperation, long nUserID)
        {
            return FNLabdipRecipe.Service.IUD(this, nDBOperation, nUserID);
        }
        public FNLabdipRecipe Get(int nFNLabdipRecipeID, long nUserID)
        {
            return FNLabdipRecipe.Service.Get(nFNLabdipRecipeID, nUserID);
        }
        public static List<FNLabdipRecipe> Gets(string sSQL, long nUserID)
        {
            return FNLabdipRecipe.Service.Gets(sSQL, nUserID);
        }
        public static List<FNLabdipRecipe> Gets(int nLabdipDetailID,int nShade, long nUserID)
        {
            return FNLabdipRecipe.Service.Gets(nLabdipDetailID, nShade,nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFNLabdipRecipeService Service
        {
            get { return (IFNLabdipRecipeService)Services.Factory.CreateService(typeof(IFNLabdipRecipeService)); }
        }
        #endregion
    }
    #endregion



    #region IFNLabdipRecipe interface
    public interface IFNLabdipRecipeService
    {
        FNLabdipRecipe IUD(FNLabdipRecipe oFNLabdipRecipe, int nDBOperation, long nUserID);

        FNLabdipRecipe Get(int nFNLabdipRecipeID, long nUserID);

        List<FNLabdipRecipe> Gets(string sSQL, long nUserID);
        List<FNLabdipRecipe> Gets(int nLabdipDetailID, int nShade, long nUserID);
    }
    #endregion
}