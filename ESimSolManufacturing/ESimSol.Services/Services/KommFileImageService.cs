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
    public class KommFileImageService : MarshalByRefObject, IKommFileImageService
    {
        #region Private functions and declaration
        private KommFileImage MapObject(NullHandler oReader)
        {
            KommFileImage oKommFileImage = new KommFileImage();
            oKommFileImage.KommFileImageID = oReader.GetInt32("KommFileImageID");
            oKommFileImage.KommFileID = oReader.GetInt32("KommFileID");
            oKommFileImage.ImageTitle = oReader.GetString("ImageTitle");
            oKommFileImage.LargeImage = oReader.GetBytes("LargeImage");
            oKommFileImage.ImageType =(EnumImageType)oReader.GetInt32("ImageType");
            oKommFileImage.ImageTypeInInt =oReader.GetInt32("ImageType");
            return oKommFileImage;
        }

        private KommFileImage CreateObject(NullHandler oReader)
        {
            KommFileImage oKommFileImage = new KommFileImage();
            oKommFileImage = MapObject(oReader);
            return oKommFileImage;
        }

        private List<KommFileImage> CreateObjects(IDataReader oReader)
        {
            List<KommFileImage> oKommFileImage = new List<KommFileImage>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KommFileImage oItem = CreateObject(oHandler);
                oKommFileImage.Add(oItem);
            }
            return oKommFileImage;
        }

        #endregion

        #region Interface implementation
        public KommFileImageService() { }

        #region Old System
        public KommFileImage Save(KommFileImage oKommFileImage, Int64 nUserID)
        {
            KommFileThumbnail oKommFileThumbnail = new KommFileThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oKommFileImage.KommFileImageID <= 0)
                {
                    if (oKommFileImage.ImageType == EnumImageType.FrontImage)
                    {
                        KommFileImageDA.ResetFrontPage(tc, oKommFileImage.KommFileID);
                        KommFileThumbnailDA.ResetFrontPage(tc, oKommFileImage.KommFileID);
                    }
                    if (oKommFileImage.ImageType == EnumImageType.BackImage)
                    {
                        KommFileImageDA.ResetBackPage(tc, oKommFileImage.KommFileID);
                        KommFileThumbnailDA.ResetBackPage(tc, oKommFileImage.KommFileID);
                    }

                    if (oKommFileImage.ImageType == EnumImageType.MeasurmentSpecImage)
                    {
                        KommFileImageDA.ResetMeasurementSpecPage(tc, oKommFileImage.KommFileID);
                        KommFileThumbnailDA.ResetMeasurementSpecPage(tc, oKommFileImage.KommFileID);
                    }
                    oKommFileImage.KommFileImageID = KommFileImageDA.GetNewID(tc);
                    KommFileImageDA.Insert(tc, oKommFileImage, nUserID);

                    #region Insert Thumblin Image
                    oKommFileThumbnail = new KommFileThumbnail();
                    oKommFileThumbnail.KommFileThumbnailID = KommFileThumbnailDA.GetNewID(tc);
                    oKommFileThumbnail.KommFileID = oKommFileImage.KommFileID;
                    oKommFileThumbnail.ImageTitle = oKommFileImage.ImageTitle;
                    oKommFileThumbnail.ImageType= oKommFileImage.ImageType;
                    oKommFileThumbnail.ThumbnailImage = oKommFileImage.ThumbnailImage;
                    oKommFileThumbnail.KommFileImageID = oKommFileImage.KommFileImageID;
                    KommFileThumbnailDA.Insert(tc, oKommFileThumbnail, nUserID);
                    #endregion
                }
                else
                {
                    KommFileImageDA.Update(tc, oKommFileImage, nUserID);

                    #region Update Thumblin Image
                    oKommFileThumbnail = new KommFileThumbnail();
                    oKommFileThumbnail.KommFileThumbnailID = oKommFileImage.KommFileThumbnailID;
                    oKommFileThumbnail.KommFileID = oKommFileImage.KommFileID;
                    oKommFileThumbnail.ImageTitle = oKommFileImage.ImageTitle;
                    oKommFileThumbnail.ImageType = oKommFileImage.ImageType;
                    oKommFileThumbnail.ThumbnailImage = oKommFileImage.ThumbnailImage;
                    oKommFileThumbnail.KommFileImageID = oKommFileImage.KommFileImageID;
                    KommFileThumbnailDA.Update(tc, oKommFileThumbnail, nUserID);
                    #endregion
                }

                tc.End();
                BusinessObject.Factory.SetObjectState(oKommFileImage, ObjectState.Saved);
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
            return oKommFileImage;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KommFileImageDA.Delete(tc, id);
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

        public KommFileImage Get(int id, Int64 nUserId)
        {
            KommFileImage oAccountHead = new KommFileImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = KommFileImageDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get KommFileImage", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<KommFileImage> Gets(int nKommFileID, Int64 nUserID)
        {
            List<KommFileImage> oKommFileImage = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = KommFileImageDA.Gets(tc, nKommFileID);
                oKommFileImage = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KommFileImage", e);
                #endregion
            }

            return oKommFileImage;
        }

        public KommFileImage GetFrontImage(int nKommFileID, Int64 nUserID)
        {

            KommFileImage oKommFileImage = new KommFileImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = KommFileImageDA.GetFrontImage(tc, nKommFileID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKommFileImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KommFileImage", e);
                #endregion
            }

            return oKommFileImage;

        }

        public KommFileImage GetBackImage(int nKommFileID, Int64 nUserID)
        {

            KommFileImage oKommFileImage = new KommFileImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = KommFileImageDA.GetBackImage(tc, nKommFileID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKommFileImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KommFileImage", e);
                #endregion
            }

            return oKommFileImage;

        }
        //
        public KommFileImage GetImageByType(int nKommFileID, int nType, Int64 nUserID)
        {

            KommFileImage oKommFileImage = new KommFileImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = KommFileImageDA.GetImageByType(tc, nKommFileID, nType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKommFileImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KommFileImage", e);
                #endregion
            }

            return oKommFileImage;

        }
        //
        #endregion
    }
}
