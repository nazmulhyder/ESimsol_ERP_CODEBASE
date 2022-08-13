using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class RecipeService : MarshalByRefObject, IRecipeService
    {
        #region Private functions and declaration
        private Recipe MapObject(NullHandler oReader)
        {
            Recipe oRecipe = new Recipe();
            oRecipe.RecipeID = oReader.GetInt32("RecipeID");
            oRecipe.RecipeCode = oReader.GetString("RecipeCode");
            oRecipe.RecipeName = oReader.GetString("RecipeName");
            oRecipe.RecipeType = oReader.GetInt16("RecipeType");
            oRecipe.ProductNature =  (EnumProductNature) oReader.GetInt32("ProductNature");
            oRecipe.ProductNatureInInt = oReader.GetInt32("ProductNature");
            oRecipe.ColorName = oReader.GetString("ColorName");
            oRecipe.BUID = oReader.GetInt32("BUID");
            oRecipe.IsActive = oReader.GetBoolean("IsActive");
            oRecipe.Note = oReader.GetString("Note");
            return oRecipe;
        }

        private Recipe CreateObject(NullHandler oReader)
        {
            Recipe oRecipe = new Recipe();
            oRecipe = MapObject(oReader);
            return oRecipe;
        }

        private List<Recipe> CreateObjects(IDataReader oReader)
        {
            List<Recipe> oRecipe = new List<Recipe>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Recipe oItem = CreateObject(oHandler);
                oRecipe.Add(oItem);
            }
            return oRecipe;
        }

        #endregion

        #region Interface implementation
        public RecipeService() { }

        public Recipe Save(Recipe oRecipe, Int64 nUserId)
        {
            TransactionContext tc = null;
            List<RecipeDetail> oRecipeDetails = new List<RecipeDetail>();
            oRecipeDetails = oRecipe.RecipeDetails;
            string sRecipeDetailIDs = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oRecipe.RecipeID <= 0)
                {
                    reader = RecipeDA.InsertUpdate(tc, oRecipe, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = RecipeDA.InsertUpdate(tc, oRecipe, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRecipe = new Recipe();
                    oRecipe = CreateObject(oReader);
                }
                reader.Close();
                #region Recipe Detail Part
                if (oRecipeDetails != null)
                {
                    foreach (RecipeDetail oItem in oRecipeDetails)
                    {
                        IDataReader readerdetail;
                        oItem.RecipeID = oRecipe.RecipeID;
                        if (oItem.RecipeDetailID <= 0)
                        {
                            readerdetail = RecipeDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                        }
                        else
                        {
                            readerdetail = RecipeDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sRecipeDetailIDs = sRecipeDetailIDs + oReaderDetail.GetString("RecipeDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sRecipeDetailIDs.Length > 0)
                    {
                        sRecipeDetailIDs = sRecipeDetailIDs.Remove(sRecipeDetailIDs.Length - 1, 1);
                    }
                }
                RecipeDetail oRecipeDetail = new RecipeDetail();
                oRecipeDetail.RecipeID = oRecipe.RecipeID;
                RecipeDetailDA.Delete(tc, oRecipeDetail, EnumDBOperation.Delete, nUserId, sRecipeDetailIDs);
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRecipe.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Recipe. Because of " + e.Message, e);
                #endregion
            }
            return oRecipe;
        }

        public Recipe ActiveInActive(Recipe oRecipe, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                
                RecipeDA.ActiveInActive(tc, oRecipe, nUserId);
                reader = RecipeDA.Get(tc, oRecipe.RecipeID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRecipe = new Recipe();
                    oRecipe = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRecipe.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Recipe. Because of " + e.Message, e);
                #endregion
            }
            return oRecipe;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Recipe oRecipe = new Recipe();
                oRecipe.RecipeID = id;
                RecipeDA.Delete(tc, oRecipe, EnumDBOperation.Delete, nUserId);
                tc.End();                
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                Recipe oRecipe = new Recipe();
                oRecipe.ErrorMessage = e.Message;
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Recipe. Because of " + e.Message, e);
                #endregion
            }
            return "Delete sucessfully";
        }

        public Recipe Get(int id, Int64 nUserId)
        {
            Recipe oAccountHead = new Recipe();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RecipeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Recipe", e);
                #endregion
            }

            return oAccountHead;
        }

        public string GetRecipeNo(Int64 nUserId)
        {
            string sRecipeNo = "";
            Recipe oRecipe = new Recipe();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RecipeDA.GetMaxRecipe(tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRecipe = CreateObject(oReader);
                    sRecipeNo = (oRecipe.RecipeID + 1001).ToString();
                }
                else
                {
                    sRecipeNo = "1001";
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Recipe", e);
                #endregion
            }

            return sRecipeNo;
        }

        public List<Recipe> Gets(Int64 nUserId)
        {
            List<Recipe> oRecipes =  new List<Recipe>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RecipeDA.Gets(tc);
                oRecipes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Recipe", e);
                #endregion
            }

            return oRecipes;
        }


        public List<Recipe> GetsByBUWithProductNature(int nBUID, int nProductNature, Int64 nUserId)
        {
            List<Recipe> oRecipes = new List<Recipe>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RecipeDA.GetsByBUWithProductNature(nBUID, nProductNature, tc);
                oRecipes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Recipe", e);
                #endregion
            }

            return oRecipes;
        }
        public List<Recipe> GetsByTypeWithBUAndNature(int nRecipeType, int nBUID, int nProductNature, Int64 nUserId)
        {
            List<Recipe> oRecipes = new List<Recipe>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RecipeDA.GetsByTypeWithBUAndNature(nRecipeType, nBUID, nProductNature, tc);
                oRecipes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Recipe", e);
                #endregion
            }

            return oRecipes;
        }
        public List<Recipe> Gets(string sSQL, Int64 nUserId)
        {
            List<Recipe> oRecipe = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RecipeDA.Gets(tc, sSQL);
                oRecipe = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Recipe", e);
                #endregion
            }

            return oRecipe;
        }
        #endregion
    }
}
