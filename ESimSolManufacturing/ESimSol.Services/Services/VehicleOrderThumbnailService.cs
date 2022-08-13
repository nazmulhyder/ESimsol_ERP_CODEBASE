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

    public class VehicleOrderThumbnailService : MarshalByRefObject, IVehicleOrderThumbnailService
    {
        #region Private functions and declaration
        private VehicleOrderThumbnail MapObject(NullHandler oReader)
        {
            VehicleOrderThumbnail oVehicleOrderThumbnail = new VehicleOrderThumbnail();
            oVehicleOrderThumbnail.VehicleOrderThumbnailID = oReader.GetInt32("VehicleOrderThumbnailID");
            oVehicleOrderThumbnail.VehicleOrderID = oReader.GetInt32("VehicleOrderID");
            oVehicleOrderThumbnail.ImageType = (EnumImageType)oReader.GetInt32("ImageType");
            oVehicleOrderThumbnail.ImageTitle = oReader.GetString("ImageTitle");
            oVehicleOrderThumbnail.ThumbnailImage = oReader.GetBytes("ThumbnailImage");
            oVehicleOrderThumbnail.VehicleOrderImageID = oReader.GetInt32("VehicleOrderImageID");
            return oVehicleOrderThumbnail;
        }

        private VehicleOrderThumbnail CreateObject(NullHandler oReader)
        {
            VehicleOrderThumbnail oVehicleOrderThumbnail = new VehicleOrderThumbnail();
            oVehicleOrderThumbnail = MapObject(oReader);
            return oVehicleOrderThumbnail;
        }

        private List<VehicleOrderThumbnail> CreateObjects(IDataReader oReader)
        {
            List<VehicleOrderThumbnail> oVehicleOrderThumbnail = new List<VehicleOrderThumbnail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VehicleOrderThumbnail oItem = CreateObject(oHandler);
                oVehicleOrderThumbnail.Add(oItem);
            }
            return oVehicleOrderThumbnail;
        }

        #endregion

        #region Interface implementation
        public VehicleOrderThumbnailService() { }

        public VehicleOrderThumbnail Save(VehicleOrderThumbnail oVehicleOrderThumbnail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVehicleOrderThumbnail.VehicleOrderThumbnailID <= 0)
                {
                    reader = VehicleOrderThumbnailDA.InsertUpdate(tc, oVehicleOrderThumbnail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VehicleOrderThumbnailDA.InsertUpdate(tc, oVehicleOrderThumbnail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleOrderThumbnail = new VehicleOrderThumbnail();
                    oVehicleOrderThumbnail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VehicleOrderThumbnail. Because of " + e.Message, e);
                #endregion
            }
            return oVehicleOrderThumbnail;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VehicleOrderThumbnail oVehicleOrderThumbnail = new VehicleOrderThumbnail();
                oVehicleOrderThumbnail.VehicleOrderThumbnailID = id;
                VehicleOrderThumbnailDA.Delete(tc, oVehicleOrderThumbnail.VehicleOrderThumbnailID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete VehicleOrderThumbnail. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }


        #region Old System
        //public VehicleOrderThumbnail Save(VehicleOrderThumbnail oVehicleOrderThumbnail, Int64 nUserID)
        //{
        //    TransactionContext tc = null;            
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        if (oVehicleOrderThumbnail.VehicleOrderThumbnailID<= 0)
        //        {
        //            oVehicleOrderThumbnail.VehicleOrderThumbnailID = VehicleOrderThumbnailDA.GetNewID(tc);
        //            VehicleOrderThumbnailDA.Insert(tc, oVehicleOrderThumbnail, nUserID);                   
        //        }
        //        else
        //        {
        //            VehicleOrderThumbnailDA.Update(tc, oVehicleOrderThumbnail, nUserID);                   
        //        }

        //        tc.End();
        //        BusinessObject.Factory.SetObjectState(oVehicleOrderThumbnail, ObjectState.Saved);
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
        //    return oVehicleOrderThumbnail;
        //}
        //public string Delete(int id, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        VehicleOrderThumbnailDA.Delete(tc, id);                
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


        public VehicleOrderThumbnail Get(int id, Int64 nUserId)
        {
            VehicleOrderThumbnail oAccountHead = new VehicleOrderThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleOrderThumbnailDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get VehicleOrderThumbnail", e);
                #endregion
            }

            return oAccountHead;
        }
        
        public List<VehicleOrderThumbnail> Gets(int nVehicleOrderID, Int64 nUserID)
        {
            List<VehicleOrderThumbnail> oVehicleOrderThumbnail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleOrderThumbnailDA.Gets(tc, nVehicleOrderID);
                oVehicleOrderThumbnail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleOrderThumbnail", e);
                #endregion
            }

            return oVehicleOrderThumbnail;
        }

        public List<VehicleOrderThumbnail> Gets(string sSql, Int64 nUserID)
        {
            List<VehicleOrderThumbnail> oVehicleOrderThumbnail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleOrderThumbnailDA.Gets(tc, sSql);
                oVehicleOrderThumbnail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleOrderThumbnail", e);
                #endregion
            }

            return oVehicleOrderThumbnail;
        }
        
        public VehicleOrderThumbnail GetFrontImage(int nVehicleOrderID, Int64 nUserID)
        {

            VehicleOrderThumbnail oVehicleOrderThumbnail = new VehicleOrderThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleOrderThumbnailDA.GetFrontImage(tc, nVehicleOrderID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleOrderThumbnail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VehicleOrderThumbnail", e);
                #endregion
            }

            return oVehicleOrderThumbnail;

        }

        public VehicleOrderThumbnail GetMeasurementSpecImage(int nVehicleOrderID, Int64 nUserID)
        {

            VehicleOrderThumbnail oVehicleOrderThumbnail = new VehicleOrderThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VehicleOrderThumbnailDA.GetMeasurementSpecImage(tc, nVehicleOrderID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleOrderThumbnail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VehicleOrderThumbnail", e);
                #endregion
            }

            return oVehicleOrderThumbnail;

        }

        

        public List<VehicleOrderThumbnail> Gets_Report(int id, Int64 nUserID)
        {
            List<VehicleOrderThumbnail> oVehicleOrderThumbnail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VehicleOrderThumbnailDA.Gets_Report(tc, id);
                oVehicleOrderThumbnail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleOrder", e);
                #endregion
            }

            return oVehicleOrderThumbnail;
        }

        #endregion
    }
}
