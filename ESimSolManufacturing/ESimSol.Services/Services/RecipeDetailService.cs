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
    public class RecipeDetailService : MarshalByRefObject, IRecipeDetailService
    {
        #region Private functions and declaration
        private RecipeDetail MapObject(NullHandler oReader)
        {
            RecipeDetail oRecipeDetail = new RecipeDetail();
            oRecipeDetail.RecipeDetailID = oReader.GetInt32("RecipeDetailID");
            oRecipeDetail.RecipeID = oReader.GetInt32("RecipeID");
            oRecipeDetail.ProductID = oReader.GetInt32("ProductID");
            oRecipeDetail.QtyType = (EnumQtyType)oReader.GetInt32("QtyType");
            oRecipeDetail.QtyTypeInt = oReader.GetInt32("QtyType");
            oRecipeDetail.ProductBaseID = oReader.GetInt32("ProductBaseID");
            oRecipeDetail.QtyInPercent = oReader.GetDouble("QtyInPercent");
            oRecipeDetail.Note = oReader.GetString("Note");
            oRecipeDetail.ProductCode = oReader.GetString("ProductCode");
            oRecipeDetail.ProductName = oReader.GetString("ProductName");
            oRecipeDetail.ProductCategoryID = oReader.GetInt16("ProductCategoryID");
            oRecipeDetail.RecipeName = oReader.GetString("RecipeName");
            oRecipeDetail.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oRecipeDetail.MUnit = oReader.GetString("MUnit");
            return oRecipeDetail;
        }

        private RecipeDetail CreateObject(NullHandler oReader)
        {
            RecipeDetail oRecipeDetail = new RecipeDetail();
            oRecipeDetail = MapObject(oReader);
            return oRecipeDetail;
        }

        private List<RecipeDetail> CreateObjects(IDataReader oReader)
        {
            List<RecipeDetail> oRecipeDetail = new List<RecipeDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RecipeDetail oItem = CreateObject(oHandler);
                oRecipeDetail.Add(oItem);
            }
            return oRecipeDetail;
        }

        #endregion

        #region Interface implementation
        public RecipeDetailService() { }

        public RecipeDetail Save(RecipeDetail oRecipeDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oRecipeDetail.RecipeDetailID <= 0)
                {
                    reader = RecipeDetailDA.InsertUpdate(tc, oRecipeDetail, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = RecipeDetailDA.InsertUpdate(tc, oRecipeDetail, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRecipeDetail = new RecipeDetail();
                    oRecipeDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save RecipeDetail. Because of " + e.Message, e);
                #endregion
            }
            return oRecipeDetail;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                RecipeDetail oRecipeDetail = new RecipeDetail();
                oRecipeDetail.RecipeDetailID = id;
                RecipeDetailDA.Delete(tc, oRecipeDetail, EnumDBOperation.Delete, nUserId,"");
                tc.End();
                return "Delete sucessfully";
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                RecipeDetail oRecipeDetail = new RecipeDetail();
                oRecipeDetail.ErrorMessage = e.Message;
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete RecipeDetail. Because of " + e.Message, e);
                #endregion
            }

        }

        public RecipeDetail Get(int id, Int64 nUserId)
        {
            RecipeDetail oAccountHead = new RecipeDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RecipeDetailDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get RecipeDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<RecipeDetail> Gets(Int64 nUserId)
        {
            List<RecipeDetail> oRecipeDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RecipeDetailDA.Gets(tc);
                oRecipeDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RecipeDetail", e);
                #endregion
            }

            return oRecipeDetail;
        }

        public List<RecipeDetail> Gets(string sSQL, Int64 nUserId)
        {
            List<RecipeDetail> oRecipeDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RecipeDetailDA.Gets(tc, sSQL);
                oRecipeDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RecipeDetail", e);
                #endregion
            }

            return oRecipeDetail;
        }
        public List<RecipeDetail> Gets(int nRecipeID, Int64 nUserId)
        {
            List<RecipeDetail> oRecipeDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RecipeDetailDA.Gets(tc, nRecipeID);
                oRecipeDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RecipeDetail", e);
                #endregion
            }

            return oRecipeDetail;
        }
        #endregion
    }
}
