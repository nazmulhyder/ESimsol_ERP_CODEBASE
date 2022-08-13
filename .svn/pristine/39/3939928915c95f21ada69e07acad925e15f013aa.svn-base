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
    public class LabdipShadeService : MarshalByRefObject, ILabdipShadeService
    {
        #region Private functions and declaration
        private LabdipShade MapObject(NullHandler oReader)
        {
            LabdipShade oLabdipShade = new LabdipShade();
            oLabdipShade.LabdipShadeID = oReader.GetInt32("LabdipShadeID");
            oLabdipShade.LabdipDetailID = oReader.GetInt32("LabdipDetailID");
            oLabdipShade.ShadeID = (EnumShade)oReader.GetInt16("ShadeID");
            oLabdipShade.ShadePercentage = oReader.GetInt32("ShadePercentage");
            oLabdipShade.Qty = oReader.GetInt32("Qty");
            oLabdipShade.Note = oReader.GetString("Note");
            oLabdipShade.ApproveBy = oReader.GetInt32("ApproveBy");
            oLabdipShade.ApproveDate = oReader.GetDateTime("ApproveDate");
            oLabdipShade.ApproveByName = oReader.GetString("ApproveByName");
            return oLabdipShade;
        }

        public static LabdipShade CreateObject(NullHandler oReader)
        {
            LabdipShade oLabdipShade = new LabdipShade();
            LabdipShadeService oService = new LabdipShadeService();
            oLabdipShade = oService.MapObject(oReader);
            return oLabdipShade;
        }
        private List<LabdipShade> CreateObjects(IDataReader oReader)
        {
            List<LabdipShade> oLabdipShades = new List<LabdipShade>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LabdipShade oItem = CreateObject(oHandler);
                oLabdipShades.Add(oItem);
            }
            return oLabdipShades;
        }

        #endregion

        #region Interface implementation
        public LabdipShadeService() { }

        public LabdipShade IUD(LabdipShade oLabdipShade, int nDBOperation, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = LabdipShadeDA.IUD(tc, oLabdipShade, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabdipShade = new LabdipShade();
                    oLabdipShade = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oLabdipShade = new LabdipShade(); oLabdipShade.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oLabdipShade.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oLabdipShade;
        }

        public LabdipShade Get(int nLabdipShadeID, long nUserID)
        {
            LabdipShade oLabdipShade = new LabdipShade();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LabdipShadeDA.Get(tc, nLabdipShadeID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabdipShade = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oLabdipShade.ErrorMessage = e.Message;
                #endregion
            }

            return oLabdipShade;
        }

        public List<LabdipShade> Gets(string sSQL, long nUserID)
        {
            List<LabdipShade> oLabdipShades = new List<LabdipShade>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabdipShadeDA.Gets(tc, sSQL);
                oLabdipShades = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                LabdipShade oLabdipShade = new LabdipShade();
                oLabdipShade.ErrorMessage = e.Message;
                oLabdipShades.Add(oLabdipShade);
                #endregion
            }

            return oLabdipShades;
        }

        public LabdipShade CopyLabdipShade(LabdipShade oLabdipShade, long nUserID)
        {
            List<LabdipRecipe> oLabdipRecipes = new List<LabdipRecipe>();
            if(oLabdipShade.RecipeDyes.Count()>0){ oLabdipRecipes.AddRange(oLabdipShade.RecipeDyes); }
            if (oLabdipShade.RecipeChemicals.Count() > 0) { oLabdipRecipes.AddRange(oLabdipShade.RecipeChemicals); }

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                NullHandler oReader;

                reader = LabdipShadeDA.IUD(tc, oLabdipShade, (int)EnumDBOperation.Insert, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabdipShade = CreateObject(oReader);
                }
                reader.Close();

                if (oLabdipRecipes.Count() > 0)
                {
                    foreach (LabdipRecipe oItem in oLabdipRecipes)
                    {
                        LabdipRecipe oLabdipRecipe = new LabdipRecipe();
                        oItem.LabdipShadeID = oLabdipShade.LabdipShadeID;
                        reader = LabdipRecipeDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oLabdipRecipe =LabdipRecipeService.CreateObject(oReader);
                        }
                        reader.Close();

                        if (oLabdipRecipe.LabdipRecipeID > 0 && oLabdipRecipe.IsDyes)
                        {
                            oLabdipShade.RecipeDyes.Add(oLabdipRecipe);
                        }
                        else if (oLabdipRecipe.LabdipRecipeID > 0 && !oLabdipRecipe.IsDyes)
                        {
                            oLabdipShade.RecipeChemicals.Add(oLabdipRecipe);
                        }
                    }
                   
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oLabdipShade.ErrorMessage = e.Message;
                #endregion
            }

            return oLabdipShade;
        }
        

        #endregion
    }
}