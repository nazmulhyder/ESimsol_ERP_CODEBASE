using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class FNLabdipRecipeService : MarshalByRefObject, IFNLabdipRecipeService
    {
        #region Private functions and declaration
        private FNLabdipRecipe MapObject(NullHandler oReader)
        {
            FNLabdipRecipe oFNLabdipRecipe = new FNLabdipRecipe();
            oFNLabdipRecipe.FNLabdipRecipeID = oReader.GetInt32("FNLabdipRecipeID");
            oFNLabdipRecipe.FNLabdipShadeID = oReader.GetInt32("FNLabdipShadeID");
            oFNLabdipRecipe.ProductID = oReader.GetInt32("ProductID");
            oFNLabdipRecipe.Qty = oReader.GetDouble("Qty");
            oFNLabdipRecipe.Note = oReader.GetString("Note");
            oFNLabdipRecipe.IsDyes = oReader.GetBoolean("IsDyes");
            oFNLabdipRecipe.IsGL = oReader.GetBoolean("IsGL");
            oFNLabdipRecipe.ProductName = oReader.GetString("ProductName");
            oFNLabdipRecipe.OrderName = oReader.GetString("OrderName");
            oFNLabdipRecipe.ProductCode = oReader.GetString("ProductCode");
            oFNLabdipRecipe.FabricOrderType = (EnumFabricRequestType)oReader.GetInt16("FabricOrderType");
            oFNLabdipRecipe.FabricOrderTypeInInt = (int)oReader.GetInt32("FabricOrderType");
            return oFNLabdipRecipe;
        }

        public static FNLabdipRecipe CreateObject(NullHandler oReader)
        {
            FNLabdipRecipe oFNLabdipRecipe = new FNLabdipRecipe();
            FNLabdipRecipeService oService = new FNLabdipRecipeService();
            oFNLabdipRecipe = oService.MapObject(oReader);
            return oFNLabdipRecipe;
        }
        private List<FNLabdipRecipe> CreateObjects(IDataReader oReader)
        {
            List<FNLabdipRecipe> oFNLabdipRecipes = new List<FNLabdipRecipe>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNLabdipRecipe oItem = CreateObject(oHandler);
                oFNLabdipRecipes.Add(oItem);
            }
            return oFNLabdipRecipes;
        }

        #endregion

        #region Interface implementation
        public FNLabdipRecipeService() { }

        public FNLabdipRecipe IUD(FNLabdipRecipe oFNLabdipRecipe, int nDBOperation, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FNLabdipRecipeDA.IUD(tc, oFNLabdipRecipe, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNLabdipRecipe = new FNLabdipRecipe();
                    oFNLabdipRecipe = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oFNLabdipRecipe = new FNLabdipRecipe(); oFNLabdipRecipe.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFNLabdipRecipe.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oFNLabdipRecipe;
        }

        public FNLabdipRecipe Get(int nFNLabdipRecipeID, long nUserID)
        {
            FNLabdipRecipe oFNLabdipRecipe = new FNLabdipRecipe();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FNLabdipRecipeDA.Get(tc, nFNLabdipRecipeID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNLabdipRecipe = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oFNLabdipRecipe.ErrorMessage = e.Message;
                #endregion
            }

            return oFNLabdipRecipe;
        }

        public List<FNLabdipRecipe> Gets(string sSQL, long nUserID)
        {
            List<FNLabdipRecipe> oFNLabdipRecipes = new List<FNLabdipRecipe>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNLabdipRecipeDA.Gets(tc, sSQL);
                oFNLabdipRecipes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                FNLabdipRecipe oFNLabdipRecipe = new FNLabdipRecipe();
                oFNLabdipRecipe.ErrorMessage = e.Message;
                oFNLabdipRecipes.Add(oFNLabdipRecipe);
                #endregion
            }

            return oFNLabdipRecipes;
        }
        public List<FNLabdipRecipe> Gets(int nLabdipDetailID, int nShade, long nUserID)
        {
            List<FNLabdipRecipe> oFNLabdipRecipes = new List<FNLabdipRecipe>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNLabdipRecipeDA.Gets(tc, nLabdipDetailID, nShade);
                oFNLabdipRecipes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                FNLabdipRecipe oFNLabdipRecipe = new FNLabdipRecipe();
                oFNLabdipRecipe.ErrorMessage = e.Message;
                oFNLabdipRecipes.Add(oFNLabdipRecipe);
                #endregion
            }

            return oFNLabdipRecipes;
        }

        #endregion
    }
}