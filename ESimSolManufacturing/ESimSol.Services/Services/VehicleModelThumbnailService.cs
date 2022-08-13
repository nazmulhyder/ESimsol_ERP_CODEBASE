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

    public class VehicleModelThumbnailService : MarshalByRefObject, IVehicleModelThumbnailService
    {
        #region Private functions and declaration
        private VehicleModelThumbnail MapObject(NullHandler oReader)
        {
            VehicleModelThumbnail oVehicleModelThumbnail = new VehicleModelThumbnail();
            oVehicleModelThumbnail.VehicleModelThumbnailID = oReader.GetInt32("VehicleModelThumbnailID");
            oVehicleModelThumbnail.VehicleModelID = oReader.GetInt32("VehicleModelID");
            oVehicleModelThumbnail.ImageType = (EnumImageType)oReader.GetInt32("ImageType");
            oVehicleModelThumbnail.ImageTypeInInt = oReader.GetInt32("ImageType");
            oVehicleModelThumbnail.ImageTitle = oReader.GetString("ImageTitle");
            oVehicleModelThumbnail.ThumbnailImage = oReader.GetBytes("ThumbnailImage");
            oVehicleModelThumbnail.VehicleModelImageID = oReader.GetInt32("VehicleModelImageID");
            return oVehicleModelThumbnail;
        }

        private VehicleModelThumbnail CreateObject(NullHandler oReader)
        {
            VehicleModelThumbnail oVehicleModelThumbnail = new VehicleModelThumbnail();
            oVehicleModelThumbnail = MapObject(oReader);
            return oVehicleModelThumbnail;
        }

        private List<VehicleModelThumbnail> CreateObjects(IDataReader oReader)
        {
            List<VehicleModelThumbnail> oVehicleModelThumbnail = new List<VehicleModelThumbnail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VehicleModelThumbnail oItem = CreateObject(oHandler);
                oVehicleModelThumbnail.Add(oItem);
            }
            return oVehicleModelThumbnail;
        }

        #endregion

        #region Interface implementation
        public VehicleModelThumbnailService() { }

        public VehicleModelThumbnail Save(VehicleModelThumbnail oVehicleModelThumbnail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVehicleModelThumbnail.VehicleModelThumbnailID <= 0)
                {
                    reader = VehicleModelThumbnailDA.InsertUpdate(tc, oVehicleModelThumbnail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VehicleModelThumbnailDA.InsertUpdate(tc, oVehicleModelThumbnail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleModelThumbnail = new VehicleModelThumbnail();
                    oVehicleModelThumbnail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VehicleModelThumbnail. Because of " + e.Message, e);
                #endregion
            }
            return oVehicleModelThumbnail;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VehicleModelThumbnail oVehicleModelThumbnail = new VehicleModelThumbnail();
                oVehicleModelThumbnail.VehicleModelThumbnailID = id;
                VehicleModelThumbnailDA.Delete(tc, oVehicleModelThumbnail.VehicleModelThumbnailID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete VehicleModelThumbnail. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }


        #region Old System
        //public VehicleModelThumbnail Save(VehicleModelThumbnail oVehicleModelThumbnail, Int64 nUserID)
        //{
        //    TransactionContext tc = null;            
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        if (oVehicleModelThumbnail.VehicleModelThumbnailID<= 0)
        //        {
        //            oVehicleModelThumbnail.VehicleModelThumbnailID = VehicleModelThumbnailDA.GetNewID(tc);
        //            VehicleModelThumbnailDA.Insert(tc, oVehicleModelThumbnail, nUserID);                   
        //        }
        //        else
        //        {
        //            VehicleModelThumbnailDA.Update(tc, oVehicleModelThumbnail, nUserID);                   
        //        }

        //        tc.End();
        //        BusinessObject.Factory.SetObjectState(oVehicleModelThumbnail, ObjectState.Saved);
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();

        //        ExceptionLog.Write(e);
        //        throw new ServiceException("Failed to Save ImageCollection", e);
        //        #endregion
        //    }
        //    return oVehicleModelThumbnail;
        //}
        //public string Delete(int id, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        VehicleModelThumbnailDA.Delete(tc, id);                
        //        tc.End();
                
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();
        //        return e.Message;
        //        //throw new ServiceException(e.Message, e);
        //        #endregion
        //    }
        //    return "Data Delete Successfully";
        //}
        #endregion


        public VehicleModelThumbnail Get(int id, Int64 nUserId)
        {
            VehicleModelThumbnail oAccountHead = new VehicleModelThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleModelThumbnailDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get VehicleModelThumbnail", e);
                #endregion
            }

            return oAccountHead;
        }
        
        public List<VehicleModelThumbnail> Gets(int nVehicleModelID, Int64 nUserID)
        {
            List<VehicleModelThumbnail> oVehicleModelThumbnail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleModelThumbnailDA.Gets(tc, nVehicleModelID);
                oVehicleModelThumbnail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleModelThumbnail", e);
                #endregion
            }

            return oVehicleModelThumbnail;
        }

        public List<VehicleModelThumbnail> Gets(string sSql, Int64 nUserID)
        {
            List<VehicleModelThumbnail> oVehicleModelThumbnail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleModelThumbnailDA.Gets(tc, sSql);
                oVehicleModelThumbnail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleModelThumbnail", e);
                #endregion
            }

            return oVehicleModelThumbnail;
        }
        
        public VehicleModelThumbnail GetFrontImage(int nVehicleModelID, Int64 nUserID)
        {

            VehicleModelThumbnail oVehicleModelThumbnail = new VehicleModelThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleModelThumbnailDA.GetFrontImage(tc, nVehicleModelID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleModelThumbnail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VehicleModelThumbnail", e);
                #endregion
            }

            return oVehicleModelThumbnail;

        }

        public VehicleModelThumbnail GetMeasurementSpecImage(int nVehicleModelID, Int64 nUserID)
        {

            VehicleModelThumbnail oVehicleModelThumbnail = new VehicleModelThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleModelThumbnailDA.GetMeasurementSpecImage(tc, nVehicleModelID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleModelThumbnail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VehicleModelThumbnail", e);
                #endregion
            }

            return oVehicleModelThumbnail;

        }

        

        public List<VehicleModelThumbnail> Gets_Report(int id, Int64 nUserID)
        {
            List<VehicleModelThumbnail> oVehicleModelThumbnail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VehicleModelThumbnailDA.Gets_Report(tc, id);
                oVehicleModelThumbnail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleModel", e);
                #endregion
            }

            return oVehicleModelThumbnail;
        }

        #endregion
    }
}
