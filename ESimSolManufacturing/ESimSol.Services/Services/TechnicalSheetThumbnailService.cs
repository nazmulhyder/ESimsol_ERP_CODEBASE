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

    public class TechnicalSheetThumbnailService : MarshalByRefObject, ITechnicalSheetThumbnailService
    {
        #region Private functions and declaration
        private TechnicalSheetThumbnail MapObject(NullHandler oReader)
        {
            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            oTechnicalSheetThumbnail.TechnicalSheetThumbnailID = oReader.GetInt32("TechnicalSheetThumbnailID");
            oTechnicalSheetThumbnail.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oTechnicalSheetThumbnail.ImageType = (EnumImageType)oReader.GetInt32("ImageType");
            oTechnicalSheetThumbnail.ImageTitle = oReader.GetString("ImageTitle");
            oTechnicalSheetThumbnail.ThumbnailImage = oReader.GetBytes("ThumbnailImage");
            oTechnicalSheetThumbnail.TechnicalSheetImageID = oReader.GetInt32("TechnicalSheetImageID");
            return oTechnicalSheetThumbnail;
        }

        private TechnicalSheetThumbnail CreateObject(NullHandler oReader)
        {
            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            oTechnicalSheetThumbnail = MapObject(oReader);
            return oTechnicalSheetThumbnail;
        }

        private List<TechnicalSheetThumbnail> CreateObjects(IDataReader oReader)
        {
            List<TechnicalSheetThumbnail> oTechnicalSheetThumbnail = new List<TechnicalSheetThumbnail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TechnicalSheetThumbnail oItem = CreateObject(oHandler);
                oTechnicalSheetThumbnail.Add(oItem);
            }
            return oTechnicalSheetThumbnail;
        }

        #endregion

        #region Interface implementation
        public TechnicalSheetThumbnailService() { }

        public TechnicalSheetThumbnail Save(TechnicalSheetThumbnail oTechnicalSheetThumbnail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oTechnicalSheetThumbnail.TechnicalSheetThumbnailID <= 0)
                {
                    reader = TechnicalSheetThumbnailDA.InsertUpdate(tc, oTechnicalSheetThumbnail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = TechnicalSheetThumbnailDA.InsertUpdate(tc, oTechnicalSheetThumbnail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
                    oTechnicalSheetThumbnail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save TechnicalSheetThumbnail. Because of " + e.Message, e);
                #endregion
            }
            return oTechnicalSheetThumbnail;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
                oTechnicalSheetThumbnail.TechnicalSheetThumbnailID = id;
                TechnicalSheetThumbnailDA.Delete(tc, oTechnicalSheetThumbnail.TechnicalSheetThumbnailID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete TechnicalSheetThumbnail. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }


        #region Old System
        //public TechnicalSheetThumbnail Save(TechnicalSheetThumbnail oTechnicalSheetThumbnail, Int64 nUserID)
        //{
        //    TransactionContext tc = null;            
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        if (oTechnicalSheetThumbnail.TechnicalSheetThumbnailID<= 0)
        //        {
        //            oTechnicalSheetThumbnail.TechnicalSheetThumbnailID = TechnicalSheetThumbnailDA.GetNewID(tc);
        //            TechnicalSheetThumbnailDA.Insert(tc, oTechnicalSheetThumbnail, nUserID);                   
        //        }
        //        else
        //        {
        //            TechnicalSheetThumbnailDA.Update(tc, oTechnicalSheetThumbnail, nUserID);                   
        //        }

        //        tc.End();
        //        BusinessObject.Factory.SetObjectState(oTechnicalSheetThumbnail, ObjectState.Saved);
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
        //    return oTechnicalSheetThumbnail;
        //}
        //public string Delete(int id, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        TechnicalSheetThumbnailDA.Delete(tc, id);                
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


        public TechnicalSheetThumbnail Get(int id, Int64 nUserId)
        {
            TechnicalSheetThumbnail oAccountHead = new TechnicalSheetThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TechnicalSheetThumbnailDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get TechnicalSheetThumbnail", e);
                #endregion
            }

            return oAccountHead;
        }
        
        public List<TechnicalSheetThumbnail> Gets(int nTechnicalSheetID, Int64 nUserID)
        {
            List<TechnicalSheetThumbnail> oTechnicalSheetThumbnail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TechnicalSheetThumbnailDA.Gets(tc, nTechnicalSheetID);
                oTechnicalSheetThumbnail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TechnicalSheetThumbnail", e);
                #endregion
            }

            return oTechnicalSheetThumbnail;
        }

        public List<TechnicalSheetThumbnail> Gets(string sSql, Int64 nUserID)
        {
            List<TechnicalSheetThumbnail> oTechnicalSheetThumbnail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TechnicalSheetThumbnailDA.Gets(tc, sSql);
                oTechnicalSheetThumbnail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TechnicalSheetThumbnail", e);
                #endregion
            }

            return oTechnicalSheetThumbnail;
        }
        
        public TechnicalSheetThumbnail GetFrontImage(int nTechnicalSheetID, Int64 nUserID)
        {

            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TechnicalSheetThumbnailDA.GetFrontImage(tc, nTechnicalSheetID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTechnicalSheetThumbnail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get TechnicalSheetThumbnail", e);
                #endregion
            }

            return oTechnicalSheetThumbnail;

        }

        public TechnicalSheetThumbnail GetMeasurementSpecImage(int nTechnicalSheetID, Int64 nUserID)
        {

            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TechnicalSheetThumbnailDA.GetMeasurementSpecImage(tc, nTechnicalSheetID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTechnicalSheetThumbnail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get TechnicalSheetThumbnail", e);
                #endregion
            }

            return oTechnicalSheetThumbnail;

        }

        

        public List<TechnicalSheetThumbnail> Gets_Report(int id, Int64 nUserID)
        {
            List<TechnicalSheetThumbnail> oTechnicalSheetThumbnail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = TechnicalSheetThumbnailDA.Gets_Report(tc, id);
                oTechnicalSheetThumbnail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TechnicalSheet", e);
                #endregion
            }

            return oTechnicalSheetThumbnail;
        }

        #endregion
    }
}
