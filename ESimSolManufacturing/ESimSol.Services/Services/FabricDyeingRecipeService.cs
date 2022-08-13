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
    public class FabricDyeingRecipeService : MarshalByRefObject, IFabricDyeingRecipeService
    {
        #region Private functions and declaration
        private FabricDyeingRecipe MapObject(NullHandler oReader)
        {
            FabricDyeingRecipe oFabricDyeingRecipe = new FabricDyeingRecipe();
            oFabricDyeingRecipe.FabricDyeingRecipeID = oReader.GetInt32("FabricDyeingRecipeID");
            oFabricDyeingRecipe.FEOSID = oReader.GetInt32("FEOSID");
            oFabricDyeingRecipe.DyeingSolutionID = oReader.GetInt32("DyeingSolutionID");
            oFabricDyeingRecipe.SCNoFull = oReader.GetString("SCNoFull");
            oFabricDyeingRecipe.Name = oReader.GetString("Name");
            return oFabricDyeingRecipe;
        }

        private FabricDyeingRecipe CreateObject(NullHandler oReader)
        {
            FabricDyeingRecipe oFabricDyeingRecipe = new FabricDyeingRecipe();
            oFabricDyeingRecipe = MapObject(oReader);
            return oFabricDyeingRecipe;
        }

        private List<FabricDyeingRecipe> CreateObjects(IDataReader oReader)
        {
            List<FabricDyeingRecipe> oFabricDyeingRecipe = new List<FabricDyeingRecipe>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricDyeingRecipe oItem = CreateObject(oHandler);
                oFabricDyeingRecipe.Add(oItem);
            }
            return oFabricDyeingRecipe;
        }

        #endregion

        #region Interface implementation
        public FabricDyeingRecipeService() { }

        public FabricDyeingRecipe Save(FabricDyeingRecipe oFabricDyeingRecipe, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricDyeingRecipe.FabricDyeingRecipeID <= 0)
                {
                    reader = FabricDyeingRecipeDA.InsertUpdate(tc, oFabricDyeingRecipe, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricDyeingRecipeDA.InsertUpdate(tc, oFabricDyeingRecipe, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricDyeingRecipe = new FabricDyeingRecipe();
                    oFabricDyeingRecipe = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save FabricDyeingRecipe. Because of " + e.Message, e);
                #endregion
            }
            return oFabricDyeingRecipe;
        }
        public string Delete(string sSQL, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricDyeingRecipe oFabricDyeingRecipe = new FabricDyeingRecipe();
                //oFabricDyeingRecipe.FabricDyeingRecipeID = id;
                FabricDyeingRecipeDA.Delete(tc, sSQL);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public FabricDyeingRecipe Get(int id, Int64 nUserId)
        {
            FabricDyeingRecipe oFabricDyeingRecipe = new FabricDyeingRecipe();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricDyeingRecipeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricDyeingRecipe = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricDyeingRecipe", e);
                #endregion
            }
            return oFabricDyeingRecipe;
        }

        public List<FabricDyeingRecipe> Gets(Int64 nUserID)
        {
            List<FabricDyeingRecipe> oFabricDyeingRecipes = new List<FabricDyeingRecipe>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricDyeingRecipeDA.Gets(tc);
                oFabricDyeingRecipes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricDyeingRecipe", e);
                #endregion
            }
            return oFabricDyeingRecipes;
        }
        public List<FabricDyeingRecipe> Gets(string sSQL,Int64 nUserID)
        {
            List<FabricDyeingRecipe> oFabricDyeingRecipes = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricDyeingRecipeDA.Gets(tc,sSQL);
                oFabricDyeingRecipes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricDyeingRecipe", e);
                #endregion
            }
            return oFabricDyeingRecipes;
        }

        #endregion
    }   
}
