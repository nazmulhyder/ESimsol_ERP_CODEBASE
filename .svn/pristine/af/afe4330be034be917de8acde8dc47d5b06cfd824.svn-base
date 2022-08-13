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
    public class TechnicalSheetImageService : MarshalByRefObject, ITechnicalSheetImageService
    {
        #region Private functions and declaration
        private TechnicalSheetImage MapObject(NullHandler oReader)
        {
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            oTechnicalSheetImage.TechnicalSheetImageID = oReader.GetInt32("TechnicalSheetImageID");
            oTechnicalSheetImage.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oTechnicalSheetImage.ImageTitle = oReader.GetString("ImageTitle");
            oTechnicalSheetImage.LargeImage = oReader.GetBytes("LargeImage");
            oTechnicalSheetImage.ImageType =(EnumImageType)oReader.GetInt32("ImageType");
            oTechnicalSheetImage.ImageTypeInInt =oReader.GetInt32("ImageType");
            return oTechnicalSheetImage;
        }

        private TechnicalSheetImage CreateObject(NullHandler oReader)
        {
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            oTechnicalSheetImage = MapObject(oReader);
            return oTechnicalSheetImage;
        }

        private List<TechnicalSheetImage> CreateObjects(IDataReader oReader)
        {
            List<TechnicalSheetImage> oTechnicalSheetImage = new List<TechnicalSheetImage>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TechnicalSheetImage oItem = CreateObject(oHandler);
                oTechnicalSheetImage.Add(oItem);
            }
            return oTechnicalSheetImage;
        }

        #endregion

        #region Interface implementation
        public TechnicalSheetImageService() { }

        #region New System
        //public TechnicalSheetImage Save(TechnicalSheetImage oTechnicalSheetImage, Int64 nUserID)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        IDataReader reader;
        //        if (oTechnicalSheetImage.TechnicalSheetImageID <= 0)
        //        {
        //            reader = TechnicalSheetImageDA.InsertUpdate(tc, oTechnicalSheetImage, EnumDBOperation.Insert, nUserID);
        //        }
        //        else
        //        {
        //            reader = TechnicalSheetImageDA.InsertUpdate(tc, oTechnicalSheetImage, EnumDBOperation.Update, nUserID);
        //        }
        //        NullHandler oReader = new NullHandler(reader);
        //        if (reader.Read())
        //        {
        //            oTechnicalSheetImage = new TechnicalSheetImage();
        //            oTechnicalSheetImage = CreateObject(oReader);
        //        }
        //        reader.Close();
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();

        //        ExceptionLog.Write(e);
        //        throw new ServiceException("Failed to Save TechnicalSheetImage. Because of " + e.Message, e);
        //        #endregion
        //    }
        //    return oTechnicalSheetImage;
        //}
        //public string Delete(int id, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
        //        oTechnicalSheetImage.TechnicalSheetImageID = id;
        //        TechnicalSheetImageDA.Delete(tc, oTechnicalSheetImage, EnumDBOperation.Delete, nUserId);
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        return e.Message;
        //        //ExceptionLog.Write(e);
        //        //throw new ServiceException("Failed to Delete TechnicalSheetImage. Because of " + e.Message, e);
        //        #endregion
        //    }
        //    return "Data delete successfully";
        //}
        #endregion

        #region Old System
        public TechnicalSheetImage Save(TechnicalSheetImage oTechnicalSheetImage, Int64 nUserID)
        {
            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oTechnicalSheetImage.TechnicalSheetImageID <= 0)
                {
                    if (oTechnicalSheetImage.ImageType == EnumImageType.FrontImage)
                    {
                        TechnicalSheetImageDA.ResetFrontPage(tc, oTechnicalSheetImage.TechnicalSheetID);
                        TechnicalSheetThumbnailDA.ResetFrontPage(tc, oTechnicalSheetImage.TechnicalSheetID);
                    }
                    if (oTechnicalSheetImage.ImageType == EnumImageType.BackImage)
                    {
                        TechnicalSheetImageDA.ResetBackPage(tc, oTechnicalSheetImage.TechnicalSheetID);
                        TechnicalSheetThumbnailDA.ResetBackPage(tc, oTechnicalSheetImage.TechnicalSheetID);
                    }

                    if (oTechnicalSheetImage.ImageType == EnumImageType.MeasurmentSpecImage)
                    {
                        TechnicalSheetImageDA.ResetMeasurementSpecPage(tc, oTechnicalSheetImage.TechnicalSheetID);
                        TechnicalSheetThumbnailDA.ResetMeasurementSpecPage(tc, oTechnicalSheetImage.TechnicalSheetID);
                    }
                    oTechnicalSheetImage.TechnicalSheetImageID = TechnicalSheetImageDA.GetNewID(tc);
                    TechnicalSheetImageDA.Insert(tc, oTechnicalSheetImage, nUserID);

                    #region Insert Thumblin Image
                    oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
                    oTechnicalSheetThumbnail.TechnicalSheetThumbnailID = TechnicalSheetThumbnailDA.GetNewID(tc);
                    oTechnicalSheetThumbnail.TechnicalSheetID = oTechnicalSheetImage.TechnicalSheetID;
                    oTechnicalSheetThumbnail.ImageTitle = oTechnicalSheetImage.ImageTitle;
                    oTechnicalSheetThumbnail.ImageType= oTechnicalSheetImage.ImageType;
                    oTechnicalSheetThumbnail.ThumbnailImage = oTechnicalSheetImage.ThumbnailImage;
                    oTechnicalSheetThumbnail.TechnicalSheetImageID = oTechnicalSheetImage.TechnicalSheetImageID;
                    TechnicalSheetThumbnailDA.Insert(tc, oTechnicalSheetThumbnail, nUserID);
                    #endregion
                }
                else
                {
                    TechnicalSheetImageDA.Update(tc, oTechnicalSheetImage, nUserID);

                    #region Update Thumblin Image
                    oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
                    oTechnicalSheetThumbnail.TechnicalSheetThumbnailID = oTechnicalSheetImage.TechnicalSheetThumbnailID;
                    oTechnicalSheetThumbnail.TechnicalSheetID = oTechnicalSheetImage.TechnicalSheetID;
                    oTechnicalSheetThumbnail.ImageTitle = oTechnicalSheetImage.ImageTitle;
                    oTechnicalSheetThumbnail.ImageType = oTechnicalSheetImage.ImageType;
                    oTechnicalSheetThumbnail.ThumbnailImage = oTechnicalSheetImage.ThumbnailImage;
                    oTechnicalSheetThumbnail.TechnicalSheetImageID = oTechnicalSheetImage.TechnicalSheetImageID;
                    TechnicalSheetThumbnailDA.Update(tc, oTechnicalSheetThumbnail, nUserID);
                    #endregion
                }

                tc.End();
                BusinessObject.Factory.SetObjectState(oTechnicalSheetImage, ObjectState.Saved);
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save ImageCollection", e);
                #endregion
            }
            return oTechnicalSheetImage;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TechnicalSheetImageDA.Delete(tc, id);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //throw new ServiceException(e.Message, e);
                #endregion
            }
            return "Data Delete Successfully";
        }
        #endregion


        public TechnicalSheetImage Get(int id, Int64 nUserId)
        {
            TechnicalSheetImage oAccountHead = new TechnicalSheetImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TechnicalSheetImageDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get TechnicalSheetImage", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<TechnicalSheetImage> Gets(int nTechnicalSheetID, Int64 nUserID)
        {
            List<TechnicalSheetImage> oTechnicalSheetImage = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TechnicalSheetImageDA.Gets(tc, nTechnicalSheetID);
                oTechnicalSheetImage = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TechnicalSheetImage", e);
                #endregion
            }

            return oTechnicalSheetImage;
        }

        public TechnicalSheetImage GetFrontImage(int nTechnicalSheetID, Int64 nUserID)
        {

            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TechnicalSheetImageDA.GetFrontImage(tc, nTechnicalSheetID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTechnicalSheetImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get TechnicalSheetImage", e);
                #endregion
            }

            return oTechnicalSheetImage;

        }

        public TechnicalSheetImage GetBackImage(int nTechnicalSheetID, Int64 nUserID)
        {

            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TechnicalSheetImageDA.GetBackImage(tc, nTechnicalSheetID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTechnicalSheetImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get TechnicalSheetImage", e);
                #endregion
            }

            return oTechnicalSheetImage;

        }
        //
        #endregion
    }
}
