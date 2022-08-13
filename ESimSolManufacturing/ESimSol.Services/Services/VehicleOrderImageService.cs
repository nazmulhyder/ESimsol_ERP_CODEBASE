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
    public class VehicleOrderImageService : MarshalByRefObject, IVehicleOrderImageService
    {
        #region Private functions and declaration
        private VehicleOrderImage MapObject(NullHandler oReader)
        {
            VehicleOrderImage oVehicleOrderImage = new VehicleOrderImage();
            oVehicleOrderImage.VehicleOrderImageID = oReader.GetInt32("VehicleOrderImageID");
            oVehicleOrderImage.VehicleOrderID = oReader.GetInt32("VehicleOrderID");
            oVehicleOrderImage.ImageTitle = oReader.GetString("ImageTitle");
            oVehicleOrderImage.LargeImage = oReader.GetBytes("LargeImage");
            oVehicleOrderImage.ImageType =(EnumImageType)oReader.GetInt32("ImageType");
            oVehicleOrderImage.ImageTypeInInt =oReader.GetInt32("ImageType");
            return oVehicleOrderImage;
        }

        private VehicleOrderImage CreateObject(NullHandler oReader)
        {
            VehicleOrderImage oVehicleOrderImage = new VehicleOrderImage();
            oVehicleOrderImage = MapObject(oReader);
            return oVehicleOrderImage;
        }

        private List<VehicleOrderImage> CreateObjects(IDataReader oReader)
        {
            List<VehicleOrderImage> oVehicleOrderImage = new List<VehicleOrderImage>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VehicleOrderImage oItem = CreateObject(oHandler);
                oVehicleOrderImage.Add(oItem);
            }
            return oVehicleOrderImage;
        }

        #endregion

        #region Interface implementation
        public VehicleOrderImageService() { }

        #region New System
        //public VehicleOrderImage Save(VehicleOrderImage oVehicleOrderImage, Int64 nUserID)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        IDataReader reader;
        //        if (oVehicleOrderImage.VehicleOrderImageID <= 0)
        //        {
        //            reader = VehicleOrderImageDA.InsertUpdate(tc, oVehicleOrderImage, EnumDBOperation.Insert, nUserID);
        //        }
        //        else
        //        {
        //            reader = VehicleOrderImageDA.InsertUpdate(tc, oVehicleOrderImage, EnumDBOperation.Update, nUserID);
        //        }
        //        NullHandler oReader = new NullHandler(reader);
        //        if (reader.Read())
        //        {
        //            oVehicleOrderImage = new VehicleOrderImage();
        //            oVehicleOrderImage = CreateObject(oReader);
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
        //        throw new ServiceException("Failed to Save VehicleOrderImage. Because of " + e.Message, e);
        //        #endregion
        //    }
        //    return oVehicleOrderImage;
        //}
        //public string Delete(int id, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        VehicleOrderImage oVehicleOrderImage = new VehicleOrderImage();
        //        oVehicleOrderImage.VehicleOrderImageID = id;
        //        VehicleOrderImageDA.Delete(tc, oVehicleOrderImage, EnumDBOperation.Delete, nUserId);
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        return e.Message;
        //        //ExceptionLog.Write(e);
        //        //throw new ServiceException("Failed to Delete VehicleOrderImage. Because of " + e.Message, e);
        //        #endregion
        //    }
        //    return "Data delete successfully";
        //}
        #endregion

        #region Old System
        public VehicleOrderImage Save(VehicleOrderImage oVehicleOrderImage, Int64 nUserID)
        {
            VehicleOrderThumbnail oVehicleOrderThumbnail = new VehicleOrderThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oVehicleOrderImage.VehicleOrderImageID <= 0)
                {
                    if (oVehicleOrderImage.ImageType == EnumImageType.FrontImage)
                    {
                        VehicleOrderImageDA.ResetFrontPage(tc, oVehicleOrderImage.VehicleOrderID);
                        VehicleOrderThumbnailDA.ResetFrontPage(tc, oVehicleOrderImage.VehicleOrderID);
                    }
                    if (oVehicleOrderImage.ImageType == EnumImageType.BackImage)
                    {
                        VehicleOrderImageDA.ResetBackPage(tc, oVehicleOrderImage.VehicleOrderID);
                        VehicleOrderThumbnailDA.ResetBackPage(tc, oVehicleOrderImage.VehicleOrderID);
                    }

                    if (oVehicleOrderImage.ImageType == EnumImageType.MeasurmentSpecImage)
                    {
                        VehicleOrderImageDA.ResetMeasurementSpecPage(tc, oVehicleOrderImage.VehicleOrderID);
                        VehicleOrderThumbnailDA.ResetMeasurementSpecPage(tc, oVehicleOrderImage.VehicleOrderID);
                    }
                    oVehicleOrderImage.VehicleOrderImageID = VehicleOrderImageDA.GetNewID(tc);
                    VehicleOrderImageDA.Insert(tc, oVehicleOrderImage, nUserID);

                    #region Insert Thumblin Image
                    oVehicleOrderThumbnail = new VehicleOrderThumbnail();
                    oVehicleOrderThumbnail.VehicleOrderThumbnailID = VehicleOrderThumbnailDA.GetNewID(tc);
                    oVehicleOrderThumbnail.VehicleOrderID = oVehicleOrderImage.VehicleOrderID;
                    oVehicleOrderThumbnail.ImageTitle = oVehicleOrderImage.ImageTitle;
                    oVehicleOrderThumbnail.ImageType= oVehicleOrderImage.ImageType;
                    oVehicleOrderThumbnail.ThumbnailImage = oVehicleOrderImage.ThumbnailImage;
                    oVehicleOrderThumbnail.VehicleOrderImageID = oVehicleOrderImage.VehicleOrderImageID;
                    VehicleOrderThumbnailDA.Insert(tc, oVehicleOrderThumbnail, nUserID);
                    #endregion
                }
                else
                {
                    VehicleOrderImageDA.Update(tc, oVehicleOrderImage, nUserID);

                    #region Update Thumblin Image
                    oVehicleOrderThumbnail = new VehicleOrderThumbnail();
                    oVehicleOrderThumbnail.VehicleOrderThumbnailID = oVehicleOrderImage.VehicleOrderThumbnailID;
                    oVehicleOrderThumbnail.VehicleOrderID = oVehicleOrderImage.VehicleOrderID;
                    oVehicleOrderThumbnail.ImageTitle = oVehicleOrderImage.ImageTitle;
                    oVehicleOrderThumbnail.ImageType = oVehicleOrderImage.ImageType;
                    oVehicleOrderThumbnail.ThumbnailImage = oVehicleOrderImage.ThumbnailImage;
                    oVehicleOrderThumbnail.VehicleOrderImageID = oVehicleOrderImage.VehicleOrderImageID;
                    VehicleOrderThumbnailDA.Update(tc, oVehicleOrderThumbnail, nUserID);
                    #endregion
                }

                tc.End();
                BusinessObject.Factory.SetObjectState(oVehicleOrderImage, ObjectState.Saved);
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
            return oVehicleOrderImage;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VehicleOrderImageDA.Delete(tc, id);
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


        public VehicleOrderImage Get(int id, Int64 nUserId)
        {
            VehicleOrderImage oAccountHead = new VehicleOrderImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleOrderImageDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get VehicleOrderImage", e);
                #endregion
            }

            return oAccountHead;
        }


        public VehicleOrderImage GetImageByType(int nVehicleOrderID,int nType, Int64 nUserID)
        {

            VehicleOrderImage oVehicleOrderImage = new VehicleOrderImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleOrderImageDA.GetImageByType(tc, nVehicleOrderID, nType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleOrderImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VehicleOrderImage", e);
                #endregion
            }

            return oVehicleOrderImage;

        }
        public List<VehicleOrderImage> Gets(int nVehicleOrderID, Int64 nUserID)
        {
            List<VehicleOrderImage> oVehicleOrderImage = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleOrderImageDA.Gets(tc, nVehicleOrderID);
                oVehicleOrderImage = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleOrderImage", e);
                #endregion
            }

            return oVehicleOrderImage;
        }

        public VehicleOrderImage GetFrontImage(int nVehicleOrderID, Int64 nUserID)
        {

            VehicleOrderImage oVehicleOrderImage = new VehicleOrderImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleOrderImageDA.GetFrontImage(tc, nVehicleOrderID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleOrderImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VehicleOrderImage", e);
                #endregion
            }

            return oVehicleOrderImage;

        }

        public VehicleOrderImage GetBackImage(int nVehicleOrderID, Int64 nUserID)
        {

            VehicleOrderImage oVehicleOrderImage = new VehicleOrderImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleOrderImageDA.GetBackImage(tc, nVehicleOrderID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleOrderImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VehicleOrderImage", e);
                #endregion
            }

            return oVehicleOrderImage;

        }
        //
        #endregion
    }
}
