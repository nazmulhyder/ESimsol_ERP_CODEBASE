using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class HeadDisplayConfigureService : MarshalByRefObject, IHeadDisplayConfigureService
    {
        #region Private functions and declaration
        private HeadDisplayConfigure MapObject(NullHandler oReader)
        {
            HeadDisplayConfigure oHeadDisplayConfigure = new HeadDisplayConfigure();
            oHeadDisplayConfigure.HeadDisplayConfigureID = oReader.GetInt32("HeadDisplayConfigureID");
            oHeadDisplayConfigure.VoucherTypeID= oReader.GetInt32("VoucherTypeID");
            oHeadDisplayConfigure.IsDebit = oReader.GetBoolean("IsDebit");
            oHeadDisplayConfigure.SubGroupID = oReader.GetInt32("SubGroupID");
            oHeadDisplayConfigure.AccountHeadCodeName = oReader.GetString("AccountHeadCodeName");
            oHeadDisplayConfigure.VoucherName = oReader.GetString("VoucherName");
            return oHeadDisplayConfigure;
        }

        private HeadDisplayConfigure CreateObject(NullHandler oReader)
        {
            HeadDisplayConfigure oHeadDisplayConfigure = new HeadDisplayConfigure();
            oHeadDisplayConfigure = MapObject(oReader);
            return oHeadDisplayConfigure;
        }

        private List<HeadDisplayConfigure> CreateObjects(IDataReader oReader)
        {
            List<HeadDisplayConfigure> oHeadDisplayConfigure = new List<HeadDisplayConfigure>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                HeadDisplayConfigure oItem = CreateObject(oHandler);
                oHeadDisplayConfigure.Add(oItem);
            }
            return oHeadDisplayConfigure;
        }

        #endregion

        #region Interface implementation
        public HeadDisplayConfigureService() { }

        public HeadDisplayConfigure Save(HeadDisplayConfigure oHeadDisplayConfigure, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                List<HeadDisplayConfigure> oHeadDisplayConfigures = new List<HeadDisplayConfigure>();
                oHeadDisplayConfigures = oHeadDisplayConfigure.HeadDisplayConfigures;
                int nVoucherTypeID = oHeadDisplayConfigure.VoucherTypeID;
                tc = TransactionContext.Begin(true);
                foreach (HeadDisplayConfigure oItem in oHeadDisplayConfigures)
                {
                    IDataReader reader;
                    reader = HeadDisplayConfigureDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oHeadDisplayConfigure = new HeadDisplayConfigure();
                        oHeadDisplayConfigure = CreateObject(oReader);
                    }
                    reader.Close();
                }
                IDataReader HDCreader = HeadDisplayConfigureDA.Gets(tc, nVoucherTypeID);
                oHeadDisplayConfigures = CreateObjects(HDCreader);
                HDCreader.Close();
                tc.End();
                oHeadDisplayConfigure.HeadDisplayConfigures = oHeadDisplayConfigures;
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save HeadDisplayConfigure. Because of " + e.Message, e);
                #endregion
            }
            return oHeadDisplayConfigure;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                HeadDisplayConfigure oHeadDisplayConfigure = new HeadDisplayConfigure();
                oHeadDisplayConfigure.HeadDisplayConfigureID = id;
                HeadDisplayConfigureDA.Delete(tc, oHeadDisplayConfigure, EnumDBOperation.Delete,nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete HeadDisplayConfigure. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public HeadDisplayConfigure Get(int id, int nUserId)
        {
            HeadDisplayConfigure oAccountHead = new HeadDisplayConfigure();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = HeadDisplayConfigureDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get HeadDisplayConfigure", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<HeadDisplayConfigure> Gets(int nUserId)
        {
            List<HeadDisplayConfigure> oHeadDisplayConfigure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HeadDisplayConfigureDA.Gets(tc);
                oHeadDisplayConfigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get HeadDisplayConfigure", e);
                #endregion
            }

            return oHeadDisplayConfigure;
        }

        public List<HeadDisplayConfigure> Gets(int nVoucherTypeID, int nUserId)
        {
            List<HeadDisplayConfigure> oHeadDisplayConfigure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HeadDisplayConfigureDA.Gets(tc,nVoucherTypeID);
                oHeadDisplayConfigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get HeadDisplayConfigure", e);
                #endregion
            }

            return oHeadDisplayConfigure;
        }
        #endregion
    }
}
