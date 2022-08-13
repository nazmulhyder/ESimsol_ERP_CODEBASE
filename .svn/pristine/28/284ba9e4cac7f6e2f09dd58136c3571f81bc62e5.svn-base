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
    public class FNRecipeService : MarshalByRefObject, IFNRecipeService
    {
        #region Private functions and declaration
        private FNRecipe MapObject(NullHandler oReader)
        {
            FNRecipe oFNRecipe = new FNRecipe();
            oFNRecipe.FNRecipeID = oReader.GetInt32("FNRecipeID");
            oFNRecipe.FSCDID = oReader.GetInt32("FSCDID");
            oFNRecipe.FNTPID = oReader.GetInt32("FNTPID");
            oFNRecipe.ProductType = (EnumProductType)oReader.GetInt16("ProductType");
            oFNRecipe.ProductID = oReader.GetInt32("ProductID");
            oFNRecipe.GL = oReader.GetDouble("GL");
            oFNRecipe.QtyColor = oReader.GetDouble("QtyColor");
            oFNRecipe.Qty = oReader.GetDouble("Qty");
            oFNRecipe.BathSize = oReader.GetDouble("BathSize");
            oFNRecipe.Note = oReader.GetString("Note");
            oFNRecipe.ProductTypeInInt = oReader.GetInt32("ProductType");
            oFNRecipe.ProductName = oReader.GetString("ProductName");
            oFNRecipe.ProductCode = oReader.GetString("ProductCode");
            oFNRecipe.FNTreatment = oReader.GetInt32("FNTreatment");
            oFNRecipe.FNProcess = oReader.GetString("FNProcess");
            oFNRecipe.PadderPressure = oReader.GetString("PadderPressure");
            oFNRecipe.Temp = oReader.GetString("Temp");
            oFNRecipe.Speed = oReader.GetString("Speed");
            oFNRecipe.PH = oReader.GetString("PH");
            oFNRecipe.CausticStrength = oReader.GetString("CausticStrength");
            oFNRecipe.Flem = oReader.GetString("Flem");
            oFNRecipe.IsProcess = oReader.GetBoolean("IsProcess");
            oFNRecipe.Code = oReader.GetString("Code");
            oFNRecipe.PrepareByName = oReader.GetString("PrepareByName");
            oFNRecipe.ShadeID = (EnumShade)oReader.GetInt16("ShadeID");

            return oFNRecipe;
        }

        private FNRecipe CreateObject(NullHandler oReader)
        {
            FNRecipe oFNRecipe = new FNRecipe();
            oFNRecipe = MapObject(oReader);
            return oFNRecipe;
        }

        private List<FNRecipe> CreateObjects(IDataReader oReader)
        {
            List<FNRecipe> oFNRecipe = new List<FNRecipe>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNRecipe oItem = CreateObject(oHandler);
                oFNRecipe.Add(oItem);
            }
            return oFNRecipe;
        }

        #endregion

        #region Interface implementation
        public FNRecipeService() { }

        public FNRecipe Save(FNRecipe oFNRecipe, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFNRecipe.FNRecipeID <= 0)
                {
                    reader = FNRecipeDA.InsertUpdate(tc, oFNRecipe, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FNRecipeDA.InsertUpdate(tc, oFNRecipe, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNRecipe = new FNRecipe();
                    oFNRecipe = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save FNRecipe. Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oFNRecipe;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FNRecipe oFNRecipe = new FNRecipe();
                oFNRecipe.FNRecipeID = id;
                FNRecipeDA.Delete(tc, oFNRecipe, EnumDBOperation.Delete, nUserId);
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
        public string DeleteProcess(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FNRecipe oFNRecipe = new FNRecipe();
                oFNRecipe.FNRecipeID = id;
                FNRecipeDA.Delete(tc, oFNRecipe, EnumDBOperation.Cancel, nUserId);
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
        public FNRecipe Get(int id, Int64 nUserId)
        {
            FNRecipe oFNRecipe = new FNRecipe();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FNRecipeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNRecipe = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FNRecipe", e);
                #endregion
            }
            return oFNRecipe;
        }
        public List<FNRecipe> Gets(Int64 nUserID)
        {
            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNRecipeDA.Gets(tc);
                oFNRecipes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNRecipe", e);
                #endregion
            }
            return oFNRecipes;
        }
        public List<FNRecipe> CopyOrder(int nFNLabDipDetailID, bool IsFromLabDip, int nFSCDID, int nFromFSCDID, int nShadeID, long nUserID)
        {
            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNRecipeDA.CopyOrder(tc, nFNLabDipDetailID, IsFromLabDip, nFSCDID, nFromFSCDID, nShadeID, nUserID, EnumDBOperation.Update);
                oFNRecipes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNRecipe", e);
                #endregion
            }
            return oFNRecipes;
        }
        
        public List<FNRecipe> Gets(string sSQL,Int64 nUserID)
        {
            List<FNRecipe> oFNRecipes = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNRecipeDA.Gets(tc,sSQL);
                oFNRecipes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNRecipe", e);
                #endregion
            }
            return oFNRecipes;
        }
        #endregion
    }   
}
