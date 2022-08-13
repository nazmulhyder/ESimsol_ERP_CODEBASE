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

    public class SalesQuotationThumbnailService : MarshalByRefObject, ISalesQuotationThumbnailService
    {
        #region Private functions and declaration
        private SalesQuotationThumbnail MapObject(NullHandler oReader)
        {
            SalesQuotationThumbnail oSalesQuotationThumbnail = new SalesQuotationThumbnail();
            oSalesQuotationThumbnail.SalesQuotationThumbnailID = oReader.GetInt32("SalesQuotationThumbnailID");
            oSalesQuotationThumbnail.SalesQuotationID = oReader.GetInt32("SalesQuotationID");
            oSalesQuotationThumbnail.ImageType = (EnumImageType)oReader.GetInt32("ImageType");
            oSalesQuotationThumbnail.ImageTypeInInt = oReader.GetInt32("ImageType");
            oSalesQuotationThumbnail.ImageTitle = oReader.GetString("ImageTitle");
            oSalesQuotationThumbnail.ThumbnailImage = oReader.GetBytes("ThumbnailImage");
            oSalesQuotationThumbnail.SalesQuotationImageID = oReader.GetInt32("SalesQuotationImageID");
            return oSalesQuotationThumbnail;
        }

        private SalesQuotationThumbnail CreateObject(NullHandler oReader)
        {
            SalesQuotationThumbnail oSalesQuotationThumbnail = new SalesQuotationThumbnail();
            oSalesQuotationThumbnail = MapObject(oReader);
            return oSalesQuotationThumbnail;
        }

        private List<SalesQuotationThumbnail> CreateObjects(IDataReader oReader)
        {
            List<SalesQuotationThumbnail> oSalesQuotationThumbnail = new List<SalesQuotationThumbnail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalesQuotationThumbnail oItem = CreateObject(oHandler);
                oSalesQuotationThumbnail.Add(oItem);
            }
            return oSalesQuotationThumbnail;
        }

        #endregion

        #region Interface implementation
        public SalesQuotationThumbnailService() { }

        public SalesQuotationThumbnail Save(SalesQuotationThumbnail oSalesQuotationThumbnail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSalesQuotationThumbnail.SalesQuotationThumbnailID <= 0)
                {
                    reader = SalesQuotationThumbnailDA.InsertUpdate(tc, oSalesQuotationThumbnail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = SalesQuotationThumbnailDA.InsertUpdate(tc, oSalesQuotationThumbnail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesQuotationThumbnail = new SalesQuotationThumbnail();
                    oSalesQuotationThumbnail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save SalesQuotationThumbnail. Because of " + e.Message, e);
                #endregion
            }
            return oSalesQuotationThumbnail;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SalesQuotationThumbnail oSalesQuotationThumbnail = new SalesQuotationThumbnail();
                oSalesQuotationThumbnail.SalesQuotationThumbnailID = id;
                SalesQuotationThumbnailDA.Delete(tc, oSalesQuotationThumbnail.SalesQuotationThumbnailID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete SalesQuotationThumbnail. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }


        #region Old System
        //public SalesQuotationThumbnail Save(SalesQuotationThumbnail oSalesQuotationThumbnail, Int64 nUserID)
        //{
        //    TransactionContext tc = null;            
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        if (oSalesQuotationThumbnail.SalesQuotationThumbnailID<= 0)
        //        {
        //            oSalesQuotationThumbnail.SalesQuotationThumbnailID = SalesQuotationThumbnailDA.GetNewID(tc);
        //            SalesQuotationThumbnailDA.Insert(tc, oSalesQuotationThumbnail, nUserID);                   
        //        }
        //        else
        //        {
        //            SalesQuotationThumbnailDA.Update(tc, oSalesQuotationThumbnail, nUserID);                   
        //        }

        //        tc.End();
        //        BusinessObject.Factory.SetObjectState(oSalesQuotationThumbnail, ObjectState.Saved);
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
        //    return oSalesQuotationThumbnail;
        //}
        //public string Delete(int id, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        SalesQuotationThumbnailDA.Delete(tc, id);                
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


        public SalesQuotationThumbnail Get(int id, Int64 nUserId)
        {
            SalesQuotationThumbnail oAccountHead = new SalesQuotationThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalesQuotationThumbnailDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get SalesQuotationThumbnail", e);
                #endregion
            }

            return oAccountHead;
        }
        
        public List<SalesQuotationThumbnail> Gets(int nSalesQuotationID, Int64 nUserID)
        {
            List<SalesQuotationThumbnail> oSalesQuotationThumbnail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesQuotationThumbnailDA.Gets(tc, nSalesQuotationID);
                oSalesQuotationThumbnail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalesQuotationThumbnail", e);
                #endregion
            }

            return oSalesQuotationThumbnail;
        }

        public List<SalesQuotationThumbnail> Gets(string sSql, Int64 nUserID)
        {
            List<SalesQuotationThumbnail> oSalesQuotationThumbnail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesQuotationThumbnailDA.Gets(tc, sSql);
                oSalesQuotationThumbnail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalesQuotationThumbnail", e);
                #endregion
            }

            return oSalesQuotationThumbnail;
        }
        
        public SalesQuotationThumbnail GetFrontImage(int nSalesQuotationID, Int64 nUserID)
        {

            SalesQuotationThumbnail oSalesQuotationThumbnail = new SalesQuotationThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalesQuotationThumbnailDA.GetFrontImage(tc, nSalesQuotationID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesQuotationThumbnail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SalesQuotationThumbnail", e);
                #endregion
            }

            return oSalesQuotationThumbnail;

        }

        public SalesQuotationThumbnail GetMeasurementSpecImage(int nSalesQuotationID, Int64 nUserID)
        {

            SalesQuotationThumbnail oSalesQuotationThumbnail = new SalesQuotationThumbnail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalesQuotationThumbnailDA.GetMeasurementSpecImage(tc, nSalesQuotationID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesQuotationThumbnail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SalesQuotationThumbnail", e);
                #endregion
            }

            return oSalesQuotationThumbnail;

        }

        

        public List<SalesQuotationThumbnail> Gets_Report(int id, Int64 nUserID)
        {
            List<SalesQuotationThumbnail> oSalesQuotationThumbnail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SalesQuotationThumbnailDA.Gets_Report(tc, id);
                oSalesQuotationThumbnail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalesQuotation", e);
                #endregion
            }

            return oSalesQuotationThumbnail;
        }

        #endregion
    }
}
