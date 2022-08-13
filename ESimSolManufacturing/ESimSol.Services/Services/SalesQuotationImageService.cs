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
    public class SalesQuotationImageService : MarshalByRefObject, ISalesQuotationImageService
    {
        #region Private functions and declaration
        private SalesQuotationImage MapObject(NullHandler oReader)
        {
            SalesQuotationImage oSalesQuotationImage = new SalesQuotationImage();
            oSalesQuotationImage.SalesQuotationImageID = oReader.GetInt32("SalesQuotationImageID");
            oSalesQuotationImage.SalesQuotationID = oReader.GetInt32("SalesQuotationID");
            oSalesQuotationImage.ImageTitle = oReader.GetString("ImageTitle");
            oSalesQuotationImage.LargeImage = oReader.GetBytes("LargeImage");
            oSalesQuotationImage.ImageType =(EnumImageType)oReader.GetInt32("ImageType");
            oSalesQuotationImage.ImageTypeInInt =oReader.GetInt32("ImageType");
            return oSalesQuotationImage;
        }

        private SalesQuotationImage CreateObject(NullHandler oReader)
        {
            SalesQuotationImage oSalesQuotationImage = new SalesQuotationImage();
            oSalesQuotationImage = MapObject(oReader);
            return oSalesQuotationImage;
        }

        private List<SalesQuotationImage> CreateObjects(IDataReader oReader)
        {
            List<SalesQuotationImage> oSalesQuotationImage = new List<SalesQuotationImage>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalesQuotationImage oItem = CreateObject(oHandler);
                oSalesQuotationImage.Add(oItem);
            }
            return oSalesQuotationImage;
        }

        #endregion

        #region Interface implementation
        public SalesQuotationImageService() { }

        #region New System
        //public SalesQuotationImage Save(SalesQuotationImage oSalesQuotationImage, Int64 nUserID)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        IDataReader reader;
        //        if (oSalesQuotationImage.SalesQuotationImageID <= 0)
        //        {
        //            reader = SalesQuotationImageDA.InsertUpdate(tc, oSalesQuotationImage, EnumDBOperation.Insert, nUserID);
        //        }
        //        else
        //        {
        //            reader = SalesQuotationImageDA.InsertUpdate(tc, oSalesQuotationImage, EnumDBOperation.Update, nUserID);
        //        }
        //        NullHandler oReader = new NullHandler(reader);
        //        if (reader.Read())
        //        {
        //            oSalesQuotationImage = new SalesQuotationImage();
        //            oSalesQuotationImage = CreateObject(oReader);
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
        //        throw new ServiceException("Failed to Save SalesQuotationImage. Because of " + e.Message, e);
        //        #endregion
        //    }
        //    return oSalesQuotationImage;
        //}
        //public string Delete(int id, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        SalesQuotationImage oSalesQuotationImage = new SalesQuotationImage();
        //        oSalesQuotationImage.SalesQuotationImageID = id;
        //        SalesQuotationImageDA.Delete(tc, oSalesQuotationImage, EnumDBOperation.Delete, nUserId);
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        return e.Message;
        //        //ExceptionLog.Write(e);
        //        //throw new ServiceException("Failed to Delete SalesQuotationImage. Because of " + e.Message, e);
        //        #endregion
        //    }
        //    return "Data delete successfully";
        //}
        #endregion

        #region Old System
        public SalesQuotationImage Save(SalesQuotationImage oSalesQuotationImage, Int64 nUserID)
        {
            SalesQuotationThumbnail oSalesQuotationThumbnail = new SalesQuotationThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oSalesQuotationImage.SalesQuotationImageID <= 0)
                {
                    if (oSalesQuotationImage.ImageType == EnumImageType.FrontImage)
                    {
                        SalesQuotationImageDA.ResetFrontPage(tc, oSalesQuotationImage.SalesQuotationID);
                        SalesQuotationThumbnailDA.ResetFrontPage(tc, oSalesQuotationImage.SalesQuotationID);
                    }
                    if (oSalesQuotationImage.ImageType == EnumImageType.BackImage)
                    {
                        SalesQuotationImageDA.ResetBackPage(tc, oSalesQuotationImage.SalesQuotationID);
                        SalesQuotationThumbnailDA.ResetBackPage(tc, oSalesQuotationImage.SalesQuotationID);
                    }

                    if (oSalesQuotationImage.ImageType == EnumImageType.MeasurmentSpecImage)
                    {
                        SalesQuotationImageDA.ResetMeasurementSpecPage(tc, oSalesQuotationImage.SalesQuotationID);
                        SalesQuotationThumbnailDA.ResetMeasurementSpecPage(tc, oSalesQuotationImage.SalesQuotationID);
                    }
                    oSalesQuotationImage.SalesQuotationImageID = SalesQuotationImageDA.GetNewID(tc);
                    SalesQuotationImageDA.Insert(tc, oSalesQuotationImage, nUserID);

                    #region Insert Thumblin Image
                    oSalesQuotationThumbnail = new SalesQuotationThumbnail();
                    oSalesQuotationThumbnail.SalesQuotationThumbnailID = SalesQuotationThumbnailDA.GetNewID(tc);
                    oSalesQuotationThumbnail.SalesQuotationID = oSalesQuotationImage.SalesQuotationID;
                    oSalesQuotationThumbnail.ImageTitle = oSalesQuotationImage.ImageTitle;
                    oSalesQuotationThumbnail.ImageType= oSalesQuotationImage.ImageType;
                    oSalesQuotationThumbnail.ThumbnailImage = oSalesQuotationImage.ThumbnailImage;
                    oSalesQuotationThumbnail.SalesQuotationImageID = oSalesQuotationImage.SalesQuotationImageID;
                    SalesQuotationThumbnailDA.Insert(tc, oSalesQuotationThumbnail, nUserID);
                    #endregion
                }
                else
                {
                    SalesQuotationImageDA.Update(tc, oSalesQuotationImage, nUserID);

                    #region Update Thumblin Image
                    oSalesQuotationThumbnail = new SalesQuotationThumbnail();
                    oSalesQuotationThumbnail.SalesQuotationThumbnailID = oSalesQuotationImage.SalesQuotationThumbnailID;
                    oSalesQuotationThumbnail.SalesQuotationID = oSalesQuotationImage.SalesQuotationID;
                    oSalesQuotationThumbnail.ImageTitle = oSalesQuotationImage.ImageTitle;
                    oSalesQuotationThumbnail.ImageType = oSalesQuotationImage.ImageType;
                    oSalesQuotationThumbnail.ThumbnailImage = oSalesQuotationImage.ThumbnailImage;
                    oSalesQuotationThumbnail.SalesQuotationImageID = oSalesQuotationImage.SalesQuotationImageID;
                    SalesQuotationThumbnailDA.Update(tc, oSalesQuotationThumbnail, nUserID);
                    #endregion
                }

                tc.End();
                BusinessObject.Factory.SetObjectState(oSalesQuotationImage, ObjectState.Saved);
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
            return oSalesQuotationImage;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SalesQuotationImageDA.Delete(tc, id);
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


        public SalesQuotationImage Get(int id, Int64 nUserId)
        {
            SalesQuotationImage oAccountHead = new SalesQuotationImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalesQuotationImageDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get SalesQuotationImage", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<SalesQuotationImage> Gets(int nSalesQuotationID, Int64 nUserID)
        {
            List<SalesQuotationImage> oSalesQuotationImage = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesQuotationImageDA.Gets(tc, nSalesQuotationID);
                oSalesQuotationImage = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalesQuotationImage", e);
                #endregion
            }

            return oSalesQuotationImage;
        }
        public SalesQuotationImage GetFrontImage(int nSalesQuotationID, Int64 nUserID)
        {

            SalesQuotationImage oSalesQuotationImage = new SalesQuotationImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalesQuotationImageDA.GetFrontImage(tc, nSalesQuotationID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesQuotationImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SalesQuotationImage", e);
                #endregion
            }

            return oSalesQuotationImage;

        }
        public SalesQuotationImage GetBackImage(int nSalesQuotationID, Int64 nUserID)
        {

            SalesQuotationImage oSalesQuotationImage = new SalesQuotationImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalesQuotationImageDA.GetBackImage(tc, nSalesQuotationID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesQuotationImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SalesQuotationImage", e);
                #endregion
            }

            return oSalesQuotationImage;

        }
        public SalesQuotationImage GetImageByType(int nSalesQuotationID, int nImageType, Int64 nUserID)
        {
            SalesQuotationImage oSalesQuotationImage = new SalesQuotationImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalesQuotationImageDA.GetImageByType(tc, nSalesQuotationID, nImageType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesQuotationImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SalesQuotationImage", e);
                #endregion
            }

            return oSalesQuotationImage;

        }
        public SalesQuotationImage GetLogImageByType(int nSalesQuotationID, int nImageType, Int64 nUserID)
        {
            SalesQuotationImage oSalesQuotationImage = new SalesQuotationImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalesQuotationImageDA.GetImageByType(tc, nSalesQuotationID, nImageType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesQuotationImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SalesQuotationImage", e);
                #endregion
            }

            return oSalesQuotationImage;

        }
        #endregion
    }
}
