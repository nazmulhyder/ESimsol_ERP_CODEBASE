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
    public class LabdipRecipeService : MarshalByRefObject, ILabdipRecipeService
    {
        #region Private functions and declaration
        private LabdipRecipe MapObject(NullHandler oReader)
        {
            LabdipRecipe oLabdipRecipe = new LabdipRecipe();
            oLabdipRecipe.LabdipRecipeID = oReader.GetInt32("LabdipRecipeID");
            oLabdipRecipe.LabdipShadeID = oReader.GetInt32("LabdipShadeID");
            oLabdipRecipe.ProductID = oReader.GetInt32("ProductID");
            oLabdipRecipe.Qty = oReader.GetDouble("Qty");
            oLabdipRecipe.Note = oReader.GetString("Note");
            oLabdipRecipe.IsDyes = oReader.GetBoolean("IsDyes");
            oLabdipRecipe.IsGL = oReader.GetBoolean("IsGL");
            oLabdipRecipe.ProductName = oReader.GetString("ProductName");
            oLabdipRecipe.ProductCode = oReader.GetString("ProductCode");
            oLabdipRecipe.LotNo = oReader.GetString("LotNo");
            return oLabdipRecipe;
        }

        public static LabdipRecipe CreateObject(NullHandler oReader)
        {
            LabdipRecipe oLabdipRecipe = new LabdipRecipe();
            LabdipRecipeService oService = new LabdipRecipeService();
            oLabdipRecipe = oService.MapObject(oReader);
            return oLabdipRecipe;
        }
        private List<LabdipRecipe> CreateObjects(IDataReader oReader)
        {
            List<LabdipRecipe> oLabdipRecipes = new List<LabdipRecipe>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LabdipRecipe oItem = CreateObject(oHandler);
                oLabdipRecipes.Add(oItem);
            }
            return oLabdipRecipes;
        }

        #endregion

        #region Interface implementation
        public LabdipRecipeService() { }

        public LabdipRecipe IUD(LabdipRecipe oLabdipRecipe, int nDBOperation, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = LabdipRecipeDA.IUD(tc, oLabdipRecipe, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabdipRecipe = new LabdipRecipe();
                    oLabdipRecipe = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oLabdipRecipe = new LabdipRecipe(); oLabdipRecipe.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oLabdipRecipe.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oLabdipRecipe;
        }

        public LabdipRecipe Get(int nLabdipRecipeID, long nUserID)
        {
            LabdipRecipe oLabdipRecipe = new LabdipRecipe();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LabdipRecipeDA.Get(tc, nLabdipRecipeID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabdipRecipe = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oLabdipRecipe.ErrorMessage = e.Message;
                #endregion
            }

            return oLabdipRecipe;
        }

        public List<LabdipRecipe> Gets(string sSQL, long nUserID)
        {
            List<LabdipRecipe> oLabdipRecipes = new List<LabdipRecipe>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabdipRecipeDA.Gets(tc, sSQL);
                oLabdipRecipes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                LabdipRecipe oLabdipRecipe = new LabdipRecipe();
                oLabdipRecipe.ErrorMessage = e.Message;
                oLabdipRecipes.Add(oLabdipRecipe);
                #endregion
            }

            return oLabdipRecipes;
        }
        public List<LabdipRecipe> Gets(int nLabdipDetailID, int nShade, long nUserID)
        {
            List<LabdipRecipe> oLabdipRecipes = new List<LabdipRecipe>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabdipRecipeDA.Gets(tc, nLabdipDetailID, nShade);
                oLabdipRecipes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                LabdipRecipe oLabdipRecipe = new LabdipRecipe();
                oLabdipRecipe.ErrorMessage = e.Message;
                oLabdipRecipes.Add(oLabdipRecipe);
                #endregion
            }

            return oLabdipRecipes;
        }
        public LabdipRecipe UpdateLot(LabdipRecipe oLabdipRecipe, int nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = LabdipRecipeDA.UpdateLot(tc, oLabdipRecipe, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabdipRecipe = CreateObject(oReader);
                }

                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLabdipRecipe = new LabdipRecipe();
                oLabdipRecipe.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oLabdipRecipe;

        }
        #endregion
    }
}