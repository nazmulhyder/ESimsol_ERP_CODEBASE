using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class FNLabdipShadeService : MarshalByRefObject, IFNLabdipShadeService
    {
        #region Private functions and declaration
        private FNLabdipShade MapObject(NullHandler oReader)
        {
            FNLabdipShade oFNLabdipShade = new FNLabdipShade();
            oFNLabdipShade.FNLabdipShadeID = oReader.GetInt32("FNLabdipShadeID");
            oFNLabdipShade.FNLabDipDetailID = oReader.GetInt32("FNLabDipDetailID");
            oFNLabdipShade.ShadeID = (EnumShade)oReader.GetInt16("ShadeID");
            oFNLabdipShade.ShadePercentage = oReader.GetInt32("ShadePercentage");
            oFNLabdipShade.Qty = oReader.GetInt32("Qty");
            oFNLabdipShade.Note = oReader.GetString("Note");
            oFNLabdipShade.ApproveBy = oReader.GetInt32("ApproveBy");
            oFNLabdipShade.ApproveDate = oReader.GetDateTime("ApproveDate");
            oFNLabdipShade.ApproveByName = oReader.GetString("ApproveByName");
            return oFNLabdipShade;
        }

        public static FNLabdipShade CreateObject(NullHandler oReader)
        {
            FNLabdipShade oFNLabdipShade = new FNLabdipShade();
            FNLabdipShadeService oService = new FNLabdipShadeService();
            oFNLabdipShade = oService.MapObject(oReader);
            return oFNLabdipShade;
        }
        private List<FNLabdipShade> CreateObjects(IDataReader oReader)
        {
            List<FNLabdipShade> oFNLabdipShades = new List<FNLabdipShade>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNLabdipShade oItem = CreateObject(oHandler);
                oFNLabdipShades.Add(oItem);
            }
            return oFNLabdipShades;
        }

        #endregion

        #region Interface implementation
        public FNLabdipShadeService() { }

        public FNLabdipShade IUD(FNLabdipShade oFNLabdipShade, int nDBOperation, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FNLabdipShadeDA.IUD(tc, oFNLabdipShade, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNLabdipShade = new FNLabdipShade();
                    oFNLabdipShade = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oFNLabdipShade = new FNLabdipShade(); oFNLabdipShade.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFNLabdipShade.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oFNLabdipShade;
        }

        public FNLabdipShade Get(int nFNLabdipShadeID, long nUserID)
        {
            FNLabdipShade oFNLabdipShade = new FNLabdipShade();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FNLabdipShadeDA.Get(tc, nFNLabdipShadeID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNLabdipShade = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oFNLabdipShade.ErrorMessage = e.Message;
                #endregion
            }

            return oFNLabdipShade;
        }

        public List<FNLabdipShade> Gets(string sSQL, long nUserID)
        {
            List<FNLabdipShade> oFNLabdipShades = new List<FNLabdipShade>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNLabdipShadeDA.Gets(tc, sSQL);
                oFNLabdipShades = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                FNLabdipShade oFNLabdipShade = new FNLabdipShade();
                oFNLabdipShade.ErrorMessage = e.Message;
                oFNLabdipShades.Add(oFNLabdipShade);
                #endregion
            }

            return oFNLabdipShades;
        }

        public FNLabdipShade CopyFNLabdipShade(FNLabdipShade oFNLabdipShade, long nUserID)
        {
            List<FNLabdipRecipe> oFNLabdipRecipes = new List<FNLabdipRecipe>();
            if(oFNLabdipShade.RecipeDyes.Count()>0){ oFNLabdipRecipes.AddRange(oFNLabdipShade.RecipeDyes); }
            if (oFNLabdipShade.RecipeChemicals.Count() > 0) { oFNLabdipRecipes.AddRange(oFNLabdipShade.RecipeChemicals); }

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                NullHandler oReader;

                reader = FNLabdipShadeDA.IUD(tc, oFNLabdipShade, (int)EnumDBOperation.Insert, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNLabdipShade = CreateObject(oReader);
                }
                reader.Close();

                if (oFNLabdipRecipes.Count() > 0)
                {
                    foreach (FNLabdipRecipe oItem in oFNLabdipRecipes)
                    {
                        FNLabdipRecipe oFNLabdipRecipe = new FNLabdipRecipe();
                        oItem.FNLabdipShadeID = oFNLabdipShade.FNLabdipShadeID;
                        reader = FNLabdipRecipeDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFNLabdipRecipe =FNLabdipRecipeService.CreateObject(oReader);
                        }
                        reader.Close();

                        if (oFNLabdipRecipe.FNLabdipRecipeID > 0 && oFNLabdipRecipe.IsDyes)
                        {
                            oFNLabdipShade.RecipeDyes.Add(oFNLabdipRecipe);
                        }
                        else if (oFNLabdipRecipe.FNLabdipRecipeID > 0 && !oFNLabdipRecipe.IsDyes)
                        {
                            oFNLabdipShade.RecipeChemicals.Add(oFNLabdipRecipe);
                        }
                    }
                   
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oFNLabdipShade.ErrorMessage = e.Message;
                #endregion
            }

            return oFNLabdipShade;
        }
        

        #endregion
    }
}