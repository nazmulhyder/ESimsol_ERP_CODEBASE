using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region Recipe

    public class Recipe : BusinessObject
    {
        public Recipe()
        {
            RecipeID = 0;
            RecipeCode = "";
            RecipeName = "";
            RecipeType = 0;
            IsActive = false;
            Note = "";
            BUID = 0;
            ColorName = "";
            ProductNature = EnumProductNature.Hanger;
            ProductNatureInInt = 0;
            ErrorMessage = "";
            MeasurementUnits = new List<MeasurementUnit>();
            RecipeDetails = new List<RecipeDetail>();
            UnitConversions = new List<UnitConversion>();
            ProductionRecipes = new List<ProductionRecipe>();

        }

        #region Properties

        public int RecipeID { get; set; }

        public string RecipeCode { get; set; }

        public string RecipeName { get; set; }
        public int RecipeType { get; set; }
        public bool IsActive { get; set; }
     
        public string Note { get; set; }
        public int BUID { get; set; }
       
        public string  ColorName { get; set; }
        public EnumProductNature ProductNature { get; set; }
        public int ProductNatureInInt { get; set; }
        public string ErrorMessage { get; set; }

         #endregion

        #region Derived Recipe
        public List<ProductionRecipe> ProductionRecipes { get; set; }
        public List<UnitConversion> UnitConversions { get; set; }
        public string IsActiveInString
        {
            get
            {
                if(this.IsActive)
                {
                    return "Active";
                }
                else
                {
                    return "In Active";
                }
            }
        }

        public string RecipeTypeInString 
        {
            get { return EnumObject.jGet((EnumRecipeType)this.RecipeType); }
        }
        public List<MeasurementUnit> MeasurementUnits { get; set; }
        public List<RecipeDetail> RecipeDetails { get; set; }
        public List<Recipe> Recipes { get; set; }
        public Company Company { get; set; }
        public CapitalResource CapitalResource { get; set; }
        #endregion

        #region Functions

        public static List<Recipe> Gets(long nUserID)
        {
            return Recipe.Service.Gets(nUserID);
        }

        public static List<Recipe> GetsByBUWithProductNature(int nBUID, int nProductNature, long nUserID)
        {
            return Recipe.Service.GetsByBUWithProductNature(nBUID, nProductNature, nUserID);
        }

        public static List<Recipe> GetsByTypeWithBUAndNature(int nRecipeType, int nBUID, int nProductNature, long nUserID)
        {
            return Recipe.Service.GetsByTypeWithBUAndNature(nRecipeType, nBUID, nProductNature, nUserID);
        }
        public static List<Recipe> Gets(string sSQL, long nUserID)
        {
            return Recipe.Service.Gets(sSQL, nUserID);
        }

        public Recipe Get(int id, long nUserID)
        {
            return Recipe.Service.Get(id, nUserID);
        }

        public Recipe Save(long nUserID)
        {
            return Recipe.Service.Save(this, nUserID);
        }

        public Recipe ActiveInActive(long nUserID)
        {
            return Recipe.Service.ActiveInActive(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return Recipe.Service.Delete(id, nUserID);
        }

        public static string GetRecipeNo(long nUserID)
        {
            return Recipe.Service.GetRecipeNo(nUserID);
        }

        #endregion

        #region Non DB Function
        public static string IDInString(List<Recipe> oRecipes)
        {
            string sIDs = "";
            foreach (Recipe oItem in oRecipes)
            {
                sIDs = sIDs + oItem.RecipeID + ",";
            }
            sIDs = sIDs.Remove(sIDs.Length - 1, 1);
            return sIDs;
        }
        #endregion

        #region ServiceFactory
        internal static IRecipeService Service
        {
            get { return (IRecipeService)Services.Factory.CreateService(typeof(IRecipeService)); }
        }
        #endregion
    }
    #endregion

    #region IRecipe interface

    public interface IRecipeService
    {

        Recipe Get(int id, Int64 nUserID);

        List<Recipe> Gets(Int64 nUserID);
        List<Recipe> GetsByBUWithProductNature(int BUID, int nProductNature, Int64 nUserID);
        List<Recipe> GetsByTypeWithBUAndNature(int nRecipeType, int BUID, int nProductNature, Int64 nUserID);
        List<Recipe> Gets(string sSQL, Int64 nUserID);

        string Delete(int id, Int64 nUserID);

        Recipe Save(Recipe oRecipe, Int64 nUserID);

        Recipe ActiveInActive(Recipe oRecipe, Int64 nUserID);

        string GetRecipeNo(Int64 nUserID);
    }
    #endregion
}
