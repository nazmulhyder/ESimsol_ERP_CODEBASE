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

    public class KommFileThumbnailService : MarshalByRefObject, IKommFileThumbnailService
    {
        #region Private functions and declaration
        private KommFileThumbnail MapObject(NullHandler oReader)
        {
            KommFileThumbnail oKommFileThumbnail = new KommFileThumbnail();
            oKommFileThumbnail.KommFileThumbnailID = oReader.GetInt32("KommFileThumbnailID");
            oKommFileThumbnail.KommFileID = oReader.GetInt32("KommFileID");
            oKommFileThumbnail.ImageType = (EnumImageType)oReader.GetInt32("ImageType");
            oKommFileThumbnail.ImageTypeInInt = oReader.GetInt32("ImageType");
            oKommFileThumbnail.ImageTitle = oReader.GetString("ImageTitle");
            oKommFileThumbnail.ThumbnailImage = oReader.GetBytes("ThumbnailImage");
            oKommFileThumbnail.KommFileImageID = oReader.GetInt32("KommFileImageID");
            return oKommFileThumbnail;
        }

        private KommFileThumbnail CreateObject(NullHandler oReader)
        {
            KommFileThumbnail oKommFileThumbnail = new KommFileThumbnail();
            oKommFileThumbnail = MapObject(oReader);
            return oKommFileThumbnail;
        }

        private List<KommFileThumbnail> CreateObjects(IDataReader oReader)
        {
            List<KommFileThumbnail> oKommFileThumbnail = new List<KommFileThumbnail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KommFileThumbnail oItem = CreateObject(oHandler);
                oKommFileThumbnail.Add(oItem);
            }
            return oKommFileThumbnail;
        }

        #endregion

        #region Interface implementation
        public KommFileThumbnailService() { }

        public KommFileThumbnail Save(KommFileThumbnail oKommFileThumbnail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oKommFileThumbnail.KommFileThumbnailID <= 0)
                {
                    reader = KommFileThumbnailDA.InsertUpdate(tc, oKommFileThumbnail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = KommFileThumbnailDA.InsertUpdate(tc, oKommFileThumbnail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKommFileThumbnail = new KommFileThumbnail();
                    oKommFileThumbnail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save KommFileThumbnail. Because of " + e.Message, e);
                #endregion
            }
            return oKommFileThumbnail;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KommFileThumbnail oKommFileThumbnail = new KommFileThumbnail();
                oKommFileThumbnail.KommFileThumbnailID = id;
                KommFileThumbnailDA.Delete(tc, oKommFileThumbnail.KommFileThumbnailID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete KommFileThumbnail. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }


        #region Old System
        //public KommFileThumbnail Save(KommFileThumbnail oKommFileThumbnail, Int64 nUserID)
        //{
        //    TransactionContext tc = null;            
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        if (oKommFileThumbnail.KommFileThumbnailID<= 0)
        //        {
        //            oKommFileThumbnail.KommFileThumbnailID = KommFileThumbnailDA.GetNewID(tc);
        //            KommFileThumbnailDA.Insert(tc, oKommFileThumbnail, nUserID);                   
        //        }
        //        else
        //        {
        //            KommFileThumbnailDA.Update(tc, oKommFileThumbnail, nUserID);                   
        //        }

        //        tc.End();
        //        BusinessObject.Factory.SetObjectState(oKommFileThumbnail, ObjectState.Saved);
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
        //    return oKommFileThumbnail;
        //}
        //public string Delete(int id, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        KommFileThumbnailDA.Delete(tc, id);                
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


        public KommFileThumbnail Get(int id, Int64 nUserId)
        {
            KommFileThumbnail oAccountHead = new KommFileThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = KommFileThumbnailDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get KommFileThumbnail", e);
                #endregion
            }

            return oAccountHead;
        }
        
        public List<KommFileThumbnail> Gets(int nKommFileID, Int64 nUserID)
        {
            List<KommFileThumbnail> oKommFileThumbnail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = KommFileThumbnailDA.Gets(tc, nKommFileID);
                oKommFileThumbnail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KommFileThumbnail", e);
                #endregion
            }

            return oKommFileThumbnail;
        }

        public List<KommFileThumbnail> Gets(string sSql, Int64 nUserID)
        {
            List<KommFileThumbnail> oKommFileThumbnail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = KommFileThumbnailDA.Gets(tc, sSql);
                oKommFileThumbnail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KommFileThumbnail", e);
                #endregion
            }

            return oKommFileThumbnail;
        }
        
        public KommFileThumbnail GetFrontImage(int nKommFileID, Int64 nUserID)
        {

            KommFileThumbnail oKommFileThumbnail = new KommFileThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = KommFileThumbnailDA.GetFrontImage(tc, nKommFileID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKommFileThumbnail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KommFileThumbnail", e);
                #endregion
            }

            return oKommFileThumbnail;

        }

        public KommFileThumbnail GetMeasurementSpecImage(int nKommFileID, Int64 nUserID)
        {

            KommFileThumbnail oKommFileThumbnail = new KommFileThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = KommFileThumbnailDA.GetMeasurementSpecImage(tc, nKommFileID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKommFileThumbnail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KommFileThumbnail", e);
                #endregion
            }

            return oKommFileThumbnail;

        }

        

        public List<KommFileThumbnail> Gets_Report(int id, Int64 nUserID)
        {
            List<KommFileThumbnail> oKommFileThumbnail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KommFileThumbnailDA.Gets_Report(tc, id);
                oKommFileThumbnail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KommFile", e);
                #endregion
            }

            return oKommFileThumbnail;
        }

        #endregion
    }
}
