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
    public class VehicleModelImageService : MarshalByRefObject, IVehicleModelImageService
    {
        #region Private functions and declaration
        private VehicleModelImage MapObject(NullHandler oReader)
        {
            VehicleModelImage oVehicleModelImage = new VehicleModelImage();
            oVehicleModelImage.VehicleModelImageID = oReader.GetInt32("VehicleModelImageID");
            oVehicleModelImage.VehicleModelID = oReader.GetInt32("VehicleModelID");
            oVehicleModelImage.ImageTitle = oReader.GetString("ImageTitle");
            oVehicleModelImage.LargeImage = oReader.GetBytes("LargeImage");
            oVehicleModelImage.ImageType =(EnumImageType)oReader.GetInt32("ImageType");
            oVehicleModelImage.ImageTypeInInt =oReader.GetInt32("ImageType");
            return oVehicleModelImage;
        }

        private VehicleModelImage CreateObject(NullHandler oReader)
        {
            VehicleModelImage oVehicleModelImage = new VehicleModelImage();
            oVehicleModelImage = MapObject(oReader);
            return oVehicleModelImage;
        }

        private List<VehicleModelImage> CreateObjects(IDataReader oReader)
        {
            List<VehicleModelImage> oVehicleModelImage = new List<VehicleModelImage>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VehicleModelImage oItem = CreateObject(oHandler);
                oVehicleModelImage.Add(oItem);
            }
            return oVehicleModelImage;
        }

        #endregion

        #region Interface implementation
        public VehicleModelImageService() { }

        #region New System
        //public VehicleModelImage Save(VehicleModelImage oVehicleModelImage, Int64 nUserID)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        IDataReader reader;
        //        if (oVehicleModelImage.VehicleModelImageID <= 0)
        //        {
        //            reader = VehicleModelImageDA.InsertUpdate(tc, oVehicleModelImage, EnumDBOperation.Insert, nUserID);
        //        }
        //        else
        //        {
        //            reader = VehicleModelImageDA.InsertUpdate(tc, oVehicleModelImage, EnumDBOperation.Update, nUserID);
        //        }
        //        NullHandler oReader = new NullHandler(reader);
        //        if (reader.Read())
        //        {
        //            oVehicleModelImage = new VehicleModelImage();
        //            oVehicleModelImage = CreateObject(oReader);
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
        //        throw new ServiceException("Failed to Save VehicleModelImage. Because of " + e.Message, e);
        //        #endregion
        //    }
        //    return oVehicleModelImage;
        //}
        //public string Delete(int id, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        VehicleModelImage oVehicleModelImage = new VehicleModelImage();
        //        oVehicleModelImage.VehicleModelImageID = id;
        //        VehicleModelImageDA.Delete(tc, oVehicleModelImage, EnumDBOperation.Delete, nUserId);
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        return e.Message;
        //        //ExceptionLog.Write(e);
        //        //throw new ServiceException("Failed to Delete VehicleModelImage. Because of " + e.Message, e);
        //        #endregion
        //    }
        //    return "Data delete successfully";
        //}
        #endregion

        #region Old System
        public VehicleModelImage Save(VehicleModelImage oVehicleModelImage, Int64 nUserID)
        {
            VehicleModelThumbnail oVehicleModelThumbnail = new VehicleModelThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oVehicleModelImage.VehicleModelImageID <= 0)
                {
                    if (oVehicleModelImage.ImageType == EnumImageType.FrontImage)
                    {
                        VehicleModelImageDA.ResetFrontPage(tc, oVehicleModelImage.VehicleModelID);
                        VehicleModelThumbnailDA.ResetFrontPage(tc, oVehicleModelImage.VehicleModelID);
                    }
                    if (oVehicleModelImage.ImageType == EnumImageType.BackImage)
                    {
                        VehicleModelImageDA.ResetBackPage(tc, oVehicleModelImage.VehicleModelID);
                        VehicleModelThumbnailDA.ResetBackPage(tc, oVehicleModelImage.VehicleModelID);
                    }

                    if (oVehicleModelImage.ImageType == EnumImageType.MeasurmentSpecImage)
                    {
                        VehicleModelImageDA.ResetMeasurementSpecPage(tc, oVehicleModelImage.VehicleModelID);
                        VehicleModelThumbnailDA.ResetMeasurementSpecPage(tc, oVehicleModelImage.VehicleModelID);
                    }
                    oVehicleModelImage.VehicleModelImageID = VehicleModelImageDA.GetNewID(tc);
                    VehicleModelImageDA.Insert(tc, oVehicleModelImage, nUserID);

                    #region Insert Thumblin Image
                    oVehicleModelThumbnail = new VehicleModelThumbnail();
                    oVehicleModelThumbnail.VehicleModelThumbnailID = VehicleModelThumbnailDA.GetNewID(tc);
                    oVehicleModelThumbnail.VehicleModelID = oVehicleModelImage.VehicleModelID;
                    oVehicleModelThumbnail.ImageTitle = oVehicleModelImage.ImageTitle;
                    oVehicleModelThumbnail.ImageType= oVehicleModelImage.ImageType;
                    oVehicleModelThumbnail.ThumbnailImage = oVehicleModelImage.ThumbnailImage;
                    oVehicleModelThumbnail.VehicleModelImageID = oVehicleModelImage.VehicleModelImageID;
                    VehicleModelThumbnailDA.Insert(tc, oVehicleModelThumbnail, nUserID);
                    #endregion
                }
                else
                {
                    VehicleModelImageDA.Update(tc, oVehicleModelImage, nUserID);

                    #region Update Thumblin Image
                    oVehicleModelThumbnail = new VehicleModelThumbnail();
                    oVehicleModelThumbnail.VehicleModelThumbnailID = oVehicleModelImage.VehicleModelThumbnailID;
                    oVehicleModelThumbnail.VehicleModelID = oVehicleModelImage.VehicleModelID;
                    oVehicleModelThumbnail.ImageTitle = oVehicleModelImage.ImageTitle;
                    oVehicleModelThumbnail.ImageType = oVehicleModelImage.ImageType;
                    oVehicleModelThumbnail.ThumbnailImage = oVehicleModelImage.ThumbnailImage;
                    oVehicleModelThumbnail.VehicleModelImageID = oVehicleModelImage.VehicleModelImageID;
                    VehicleModelThumbnailDA.Update(tc, oVehicleModelThumbnail, nUserID);
                    #endregion
                }

                tc.End();
                BusinessObject.Factory.SetObjectState(oVehicleModelImage, ObjectState.Saved);
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
            return oVehicleModelImage;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VehicleModelImageDA.Delete(tc, id);
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


        public VehicleModelImage Get(int id, Int64 nUserId)
        {
            VehicleModelImage oAccountHead = new VehicleModelImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleModelImageDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get VehicleModelImage", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<VehicleModelImage> Gets(int nVehicleModelID, Int64 nUserID)
        {
            List<VehicleModelImage> oVehicleModelImage = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleModelImageDA.Gets(tc, nVehicleModelID);
                oVehicleModelImage = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleModelImage", e);
                #endregion
            }

            return oVehicleModelImage;
        }

        public VehicleModelImage GetFrontImage(int nVehicleModelID, Int64 nUserID)
        {

            VehicleModelImage oVehicleModelImage = new VehicleModelImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleModelImageDA.GetFrontImage(tc, nVehicleModelID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleModelImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VehicleModelImage", e);
                #endregion
            }

            return oVehicleModelImage;

        }

        public VehicleModelImage GetBackImage(int nVehicleModelID, Int64 nUserID)
        {

            VehicleModelImage oVehicleModelImage = new VehicleModelImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleModelImageDA.GetBackImage(tc, nVehicleModelID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleModelImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VehicleModelImage", e);
                #endregion
            }

            return oVehicleModelImage;

        }
        //
        public VehicleModelImage GetImageByType(int nVehicleModelID, int nType, Int64 nUserID)
        {

            VehicleModelImage oVehicleModelImage = new VehicleModelImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleModelImageDA.GetImageByType(tc, nVehicleModelID, nType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleModelImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VehicleModelImage", e);
                #endregion
            }

            return oVehicleModelImage;

        }
        //
        #endregion
    }
}
