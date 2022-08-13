using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class KnitDyeingRecipeService : MarshalByRefObject, IKnitDyeingRecipeService
    {
        private KnitDyeingRecipe MapObject(NullHandler oReader)
        {
            KnitDyeingRecipe oKnitDyeingRecipe = new KnitDyeingRecipe();
            oKnitDyeingRecipe.KnitDyeingRecipeID = oReader.GetInt32("KnitDyeingRecipeID");
            oKnitDyeingRecipe.RecipeCode = oReader.GetString("RecipeCode");
            oKnitDyeingRecipe.RecipeName = oReader.GetString("RecipeName");
            oKnitDyeingRecipe.Note = oReader.GetString("Note");
            oKnitDyeingRecipe.BUID = oReader.GetInt32("BUID");
            oKnitDyeingRecipe.IsActive = oReader.GetBoolean("IsActive");
            oKnitDyeingRecipe.UserName = oReader.GetString("UserName");

            return oKnitDyeingRecipe;
        }
        private KnitDyeingRecipe CreateObject(NullHandler oReader)
        {
            KnitDyeingRecipe oKnitDyeingRecipe = new KnitDyeingRecipe();
            oKnitDyeingRecipe = MapObject(oReader);
            return oKnitDyeingRecipe;
        }

        private List<KnitDyeingRecipe> CreateObjects(IDataReader oReader)
        {
            List<KnitDyeingRecipe> oKnitDyeingRecipe = new List<KnitDyeingRecipe>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KnitDyeingRecipe oItem = CreateObject(oHandler);
                oKnitDyeingRecipe.Add(oItem);
            }
            return oKnitDyeingRecipe;
        }
        public KnitDyeingRecipe Save(KnitDyeingRecipe oKnitDyeingRecipe, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sKnitDyeingRecipeDetailIDs = "";
            List<KnitDyeingRecipeDetail> oKnitDyeingRecipeDetails = new List<KnitDyeingRecipeDetail>();
            KnitDyeingRecipeDetail oKnitDyeingRecipeDetail = new KnitDyeingRecipeDetail();
            oKnitDyeingRecipeDetails = oKnitDyeingRecipe.KnitDyeingRecipeDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oKnitDyeingRecipe.KnitDyeingRecipeID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.KnitDyeingRecipe, EnumRoleOperationType.Add);
                    reader = KnitDyeingRecipeDA.InsertUpdate(tc, oKnitDyeingRecipe, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.KnitDyeingRecipe, EnumRoleOperationType.Edit);
                    reader = KnitDyeingRecipeDA.InsertUpdate(tc, oKnitDyeingRecipe, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnitDyeingRecipe = new KnitDyeingRecipe();
                    oKnitDyeingRecipe = CreateObject(oReader);
                }
                reader.Close();
                #region KnitDyeingRecipe Detail
                foreach (KnitDyeingRecipeDetail oItem in oKnitDyeingRecipeDetails)
                {
                    IDataReader readerdetail;
                    oItem.KnitDyeingRecipeID = oKnitDyeingRecipe.KnitDyeingRecipeID;
                    if (oItem.KnitDyeingRecipeDetailID <= 0)
                    {
                        readerdetail = KnitDyeingRecipeDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = KnitDyeingRecipeDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sKnitDyeingRecipeDetailIDs = sKnitDyeingRecipeDetailIDs + oReaderDetail.GetString("KnitDyeingRecipeDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sKnitDyeingRecipeDetailIDs.Length > 0)
                {
                    sKnitDyeingRecipeDetailIDs = sKnitDyeingRecipeDetailIDs.Remove(sKnitDyeingRecipeDetailIDs.Length - 1, 1);
                }
                oKnitDyeingRecipeDetail = new KnitDyeingRecipeDetail();
                oKnitDyeingRecipeDetail.KnitDyeingRecipeID = oKnitDyeingRecipe.KnitDyeingRecipeID;
                KnitDyeingRecipeDetailDA.Delete(tc, oKnitDyeingRecipeDetail, EnumDBOperation.Delete, nUserID, sKnitDyeingRecipeDetailIDs);
                #endregion

                #region Get KnitDyeing Recipe
                reader = KnitDyeingRecipeDA.Get(tc, oKnitDyeingRecipe.KnitDyeingRecipeID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnitDyeingRecipe = new KnitDyeingRecipe();
                    oKnitDyeingRecipe = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oKnitDyeingRecipe = new KnitDyeingRecipe();
                    oKnitDyeingRecipe.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnitDyeingRecipe;
        }
        public KnitDyeingRecipe Activity(KnitDyeingRecipe oKnitDyeingRecipe, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KnitDyeingRecipeDA.Activity(tc, oKnitDyeingRecipe, nUserID);

                IDataReader reader;
                reader = KnitDyeingRecipeDA.Get(tc, oKnitDyeingRecipe.KnitDyeingRecipeID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnitDyeingRecipe = new KnitDyeingRecipe();
                    oKnitDyeingRecipe = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oKnitDyeingRecipe = new KnitDyeingRecipe();
                    oKnitDyeingRecipe.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnitDyeingRecipe;
        }
        public KnitDyeingRecipe Get(int id, Int64 nUserId)
        {
            KnitDyeingRecipe oKnitDyeingRecipe = new KnitDyeingRecipe();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = KnitDyeingRecipeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnitDyeingRecipe = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KnitDyeingRecipe", e);
                #endregion
            }

            return oKnitDyeingRecipe;
        }
        public List<KnitDyeingRecipe> Gets(string sSQL, Int64 nUserID)
        {
            List<KnitDyeingRecipe> oKnitDyeingRecipe = new List<KnitDyeingRecipe>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnitDyeingRecipeDA.Gets(tc, sSQL);
                oKnitDyeingRecipe = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnitDyeingRecipe", e);
                #endregion
            }
            return oKnitDyeingRecipe;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KnitDyeingRecipe oKnitDyeingRecipe = new KnitDyeingRecipe();
                oKnitDyeingRecipe.KnitDyeingRecipeID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.KnitDyeingRecipe, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "KnitDyeingRecipe", id);
                KnitDyeingRecipeDA.Delete(tc, oKnitDyeingRecipe, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

    }
}
