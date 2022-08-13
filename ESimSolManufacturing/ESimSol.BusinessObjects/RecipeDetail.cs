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
    #region RecipeDetail

    public class RecipeDetail : BusinessObject
    {
        public RecipeDetail()
        {
            RecipeDetailID = 0;
            RecipeID = 0;
            ProductID = 0;
            ProductBaseID = 0;
            QtyType = EnumQtyType.Percent;
            MeasurementUnitID = 0;
            MUnit = "";
            QtyInPercent = 0;
            Note = "";

        }

        #region Properties

        public int RecipeDetailID { get; set; }

        public int RecipeID { get; set; }
        public int ProductBaseID { get; set; }
        public int ProductID { get; set; }
        public EnumQtyType QtyType { get; set; }
        public int QtyTypeInt { get; set; }
        public double QtyInPercent { get; set; }
        public string Note { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public EnumProductType ProductType { get; set; }
        public int ProductCategoryID { get; set; }
        public string RecipeName { get; set; }
        public int MeasurementUnitID { get; set; }
        public string MUnit { get; set; }
        public string ErrorMessage { get; set; }
        #endregion


        #region Derived RecipeDetail
        public string QtyTypeSt
        {
            get
            {
                return this.QtyType.ToString();
            }
        }
        #endregion


        #region Functions

        public static List<RecipeDetail> Gets(long nUserID)
        {
            return RecipeDetail.Service.Gets(nUserID);
        }

        public static List<RecipeDetail> Gets(int nRecipeID, long nUserID)
        {
            return RecipeDetail.Service.Gets(nRecipeID, nUserID);
        }

        public RecipeDetail Get(int id, long nUserID)
        {
            return RecipeDetail.Service.Get(id, nUserID);
        }
        public RecipeDetail Save(long nUserID)
        {
            return RecipeDetail.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return RecipeDetail.Service.Delete(id, nUserID);
        }
        public static List<RecipeDetail> Gets(string sSQL, long nUserID)
        {
            return RecipeDetail.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region Non DB Function
        public static string IDInString(List<Recipe> oRecipes)
        {
            string sTempIDs = "";
            foreach (Recipe oItem in oRecipes)
            {
                sTempIDs = sTempIDs + oItem.RecipeID.ToString() + ",";
            }
            if (sTempIDs.Length > 0)
            {
                sTempIDs = sTempIDs.Remove(sTempIDs.Length - 1, 1);
            }
            return sTempIDs;
        }
        #endregion

        #region ServiceFactory
        internal static IRecipeDetailService Service
        {
            get { return (IRecipeDetailService)Services.Factory.CreateService(typeof(IRecipeDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IRecipeDetail interface

    public interface IRecipeDetailService
    {

        RecipeDetail Get(int id, Int64 nUserID);

        List<RecipeDetail> Gets(Int64 nUserID);

        List<RecipeDetail> Gets(string sSQL, Int64 nUserID);

        List<RecipeDetail> Gets(int nRecipeID, Int64 nUserID);

        string Delete(int id, Int64 nUserID);

        RecipeDetail Save(RecipeDetail oRecipeDetail, Int64 nUserID);
    }
    #endregion
}
