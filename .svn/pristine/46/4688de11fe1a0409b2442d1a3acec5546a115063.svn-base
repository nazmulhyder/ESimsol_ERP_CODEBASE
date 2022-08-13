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
    public class HangerStickerService : MarshalByRefObject, IHangerStickerService
    {
        #region Private functions and declaration
        private HangerSticker MapObject(NullHandler oReader)
        {
            HangerSticker oHangerSticker = new HangerSticker();
            oHangerSticker.HangerStickerID = oReader.GetInt32("HangerStickerID");
            oHangerSticker.FSCDID = oReader.GetInt32("FSCDID");
            oHangerSticker.ART = oReader.GetString("ART");
            oHangerSticker.Supplier = oReader.GetString("Supplier");
            oHangerSticker.Finishing = oReader.GetString("Finishing");
            oHangerSticker.Composition = oReader.GetString("Composition");
            oHangerSticker.Construction = oReader.GetString("Construction");
            oHangerSticker.Width = oReader.GetString("Width");
            oHangerSticker.MOQ = oReader.GetString("MOQ");
            oHangerSticker.Remarks = oReader.GetString("Remarks");
            oHangerSticker.Price = oReader.GetDouble("Price");
            oHangerSticker.Date = oReader.GetString("Date");
            return oHangerSticker;
        }

        private HangerSticker CreateObject(NullHandler oReader)
        {
            HangerSticker oHangerSticker = new HangerSticker();
            oHangerSticker = MapObject(oReader);
            return oHangerSticker;
        }

        private List<HangerSticker> CreateObjects(IDataReader oReader)
        {
            List<HangerSticker> oHangerStickers = new List<HangerSticker>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                HangerSticker oItem = CreateObject(oHandler);
                oHangerStickers.Add(oItem);
            }
            return oHangerStickers;
        }
        #endregion

        #region Interface implementation
        public HangerSticker Save(HangerSticker oHangerSticker, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oHangerSticker.HangerStickerID <= 0)
                {
                    reader = HangerStickerDA.InsertUpdate(tc, oHangerSticker, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = HangerStickerDA.InsertUpdate(tc, oHangerSticker, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHangerSticker = new HangerSticker();
                    oHangerSticker = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save HangerSticker. Because of " + e.Message, e);
                #endregion
            }
            return oHangerSticker;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                HangerSticker oHangerSticker = new HangerSticker();
                oHangerSticker.HangerStickerID = id;
               // DBTableReferenceDA.HasReference(tc, "HangerSticker", id);
                HangerStickerDA.Delete(tc, oHangerSticker, EnumDBOperation.Delete, nUserId);
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

        public HangerSticker Get(int id, Int64 nUserId)
        {
            HangerSticker oHangerSticker = new HangerSticker();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = HangerStickerDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHangerSticker = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get HangerSticker", e);
                #endregion
            }
            return oHangerSticker;
        }
        public List<HangerSticker> Gets(Int64 nUserID)
        {
            List<HangerSticker> oFabricStickers = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HangerStickerDA.Gets(tc);
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
        public List<HangerSticker> Gets(string sSQL, Int64 nUserID)
        {
            List<HangerSticker> oFabricStickers = new List<HangerSticker>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = HangerStickerDA.Gets(tc, sSQL);
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
                throw new ServiceException("Failed to Get HangerSticker", e);
                #endregion
            }
            return oFabricStickers;
        }
        #endregion
    }
}
