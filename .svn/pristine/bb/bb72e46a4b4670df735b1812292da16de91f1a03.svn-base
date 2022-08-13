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
    public class FabricStickerService : MarshalByRefObject, IFabricStickerService
    {
        #region Private functions and declaration
        private FabricSticker MapObject(NullHandler oReader)
        {
            FabricSticker oFabricSticker = new FabricSticker();
            oFabricSticker.FabricStickerID = oReader.GetInt32("FabricStickerID");
            oFabricSticker.Title = oReader.GetString("Title");
            oFabricSticker.FabricMillName = oReader.GetString("FabricMillName");
            oFabricSticker.FabricArticleNo = oReader.GetString("FabricArticleNo");
            oFabricSticker.Composition = oReader.GetInt32("Composition");
            oFabricSticker.Construction = oReader.GetString("Construction");
            oFabricSticker.FabricDesignID = oReader.GetInt32("FabricDesignID");
            oFabricSticker.FabricWeave = oReader.GetInt32("FabricWeave");
            oFabricSticker.Width = oReader.GetString("Width");
            oFabricSticker.Weight = oReader.GetString("Weight");
            oFabricSticker.FinishType = oReader.GetInt32("FinishType");
            oFabricSticker.StickerDate = oReader.GetDateTime("StickerDate");
            oFabricSticker.Price = oReader.GetDouble("Price");
            oFabricSticker.PrintCount = oReader.GetInt32("PrintCount");
            oFabricSticker.ProductName = oReader.GetString("ProductName");
            oFabricSticker.FinishTypeName = oReader.GetString("FinishTypeName");
            oFabricSticker.FabricDesignName = oReader.GetString("FabricDesignName");
            oFabricSticker.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oFabricSticker.Email = oReader.GetString("Email");
            oFabricSticker.Phone = oReader.GetString("Phone");
            return oFabricSticker;
        }

        private FabricSticker CreateObject(NullHandler oReader)
        {
            FabricSticker oFabricSticker = new FabricSticker();
            oFabricSticker = MapObject(oReader);
            return oFabricSticker;
        }

        private List<FabricSticker> CreateObjects(IDataReader oReader)
        {
            List<FabricSticker> oFabricSticker = new List<FabricSticker>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricSticker oItem = CreateObject(oHandler);
                oFabricSticker.Add(oItem);
            }
            return oFabricSticker;
        }
        #endregion

        #region Interface implementation
        public FabricSticker Save(FabricSticker oFabricSticker, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricSticker.FabricStickerID <= 0)
                {
                    reader = FabricStickerDA.InsertUpdate(tc, oFabricSticker, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricStickerDA.InsertUpdate(tc, oFabricSticker, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSticker = new FabricSticker();
                    oFabricSticker = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save FabricSticker. Because of " + e.Message, e);
                #endregion
            }
            return oFabricSticker;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricSticker oFabricSticker = new FabricSticker();
                oFabricSticker.FabricStickerID = id;
                DBTableReferenceDA.HasReference(tc, "FabricSticker", id);
                FabricStickerDA.Delete(tc, oFabricSticker, EnumDBOperation.Delete, nUserId);
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

        public FabricSticker Get(int id, Int64 nUserId)
        {
            FabricSticker oFabricSticker = new FabricSticker();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricStickerDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSticker = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricSticker", e);
                #endregion
            }
            return oFabricSticker;
        }
        public List<FabricSticker> Gets(Int64 nUserID)
        {
            List<FabricSticker> oFabricStickers = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricStickerDA.Gets(tc);
                oFabricStickers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                #endregion
            }
            return oFabricStickers;
        }
        public List<FabricSticker> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricSticker> oFabricStickers = new List<FabricSticker>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricStickerDA.Gets(tc, sSQL);
                oFabricStickers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricSticker", e);
                #endregion
            }
            return oFabricStickers;
        }
        #endregion
    }
}
