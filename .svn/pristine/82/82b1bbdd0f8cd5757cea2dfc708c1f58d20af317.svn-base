using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class PNWiseAccountHeadService : MarshalByRefObject, IPNWiseAccountHeadService
    {
        #region Private functions and declaration
        private PNWiseAccountHead MapObject(NullHandler oReader)
        {
            PNWiseAccountHead oPNWiseAccountHead = new PNWiseAccountHead();
            oPNWiseAccountHead.PNWiseAccountHeadID = oReader.GetInt32("PNWiseAccountHeadID");
            oPNWiseAccountHead.AccountHeadNature = oReader.GetInt32("AccountHeadNature");
            oPNWiseAccountHead.ProductNature = oReader.GetInt32("ProductNature");
            oPNWiseAccountHead.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oPNWiseAccountHead.AccountHeadName = oReader.GetString("AccountHeadName");
            oPNWiseAccountHead.AccountCode = oReader.GetString("AccountCode");
            return oPNWiseAccountHead;
        }

        private PNWiseAccountHead CreateObject(NullHandler oReader)
        {
            PNWiseAccountHead oPNWiseAccountHead = new PNWiseAccountHead();
            oPNWiseAccountHead = MapObject(oReader);
            return oPNWiseAccountHead;
        }

        private List<PNWiseAccountHead> CreateObjects(IDataReader oReader)
        {
            List<PNWiseAccountHead> oPNWiseAccountHead = new List<PNWiseAccountHead>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PNWiseAccountHead oItem = CreateObject(oHandler);
                oPNWiseAccountHead.Add(oItem);
            }
            return oPNWiseAccountHead;
        }

        #endregion

        #region Interface implementation
        public PNWiseAccountHeadService() { }

        public PNWiseAccountHead Save(PNWiseAccountHead oPNWiseAccountHead, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPNWiseAccountHead.PNWiseAccountHeadID <= 0)
                {
                    reader = PNWiseAccountHeadDA.InsertUpdate(tc, oPNWiseAccountHead, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = PNWiseAccountHeadDA.InsertUpdate(tc, oPNWiseAccountHead, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPNWiseAccountHead = new PNWiseAccountHead();
                    oPNWiseAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save PNWiseAccountHead. Because of " + e.Message, e);
                #endregion
            }
            return oPNWiseAccountHead;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PNWiseAccountHead oPNWiseAccountHead = new PNWiseAccountHead();
                oPNWiseAccountHead.PNWiseAccountHeadID = id;
                PNWiseAccountHeadDA.Delete(tc, oPNWiseAccountHead, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public PNWiseAccountHead Get(int id, Int64 nUserId)
        {
            PNWiseAccountHead oPNWiseAccountHead = new PNWiseAccountHead();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = PNWiseAccountHeadDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPNWiseAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PNWiseAccountHead", e);
                #endregion
            }
            return oPNWiseAccountHead;
        }

        public List<PNWiseAccountHead> GetByCategory(bool bCategory, Int64 nUserID)
        {
            List<PNWiseAccountHead> oPNWiseAccountHeads = new List<PNWiseAccountHead>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PNWiseAccountHeadDA.GetByCategory(tc, bCategory);
                oPNWiseAccountHeads = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PNWiseAccountHeads", e);
                #endregion
            }
            return oPNWiseAccountHeads;
        }

        public List<PNWiseAccountHead> Gets(Int64 nUserID)
        {
            List<PNWiseAccountHead> oPNWiseAccountHeads = new List<PNWiseAccountHead>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PNWiseAccountHeadDA.Gets(tc);
                oPNWiseAccountHeads = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PNWiseAccountHead", e);
                #endregion
            }
            return oPNWiseAccountHeads;
        }
        public List<PNWiseAccountHead> Gets(string sSQL, Int64 nUserID)
        {
            List<PNWiseAccountHead> oPNWiseAccountHeads = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PNWiseAccountHeadDA.Gets(tc, sSQL);
                oPNWiseAccountHeads = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PNWiseAccountHead", e);
                #endregion
            }
            return oPNWiseAccountHeads;
        }


        public List<PNWiseAccountHead> GetByNegoPNWiseAccountHead(int nPNWiseAccountHeadID, Int64 nUserID)
        {
            List<PNWiseAccountHead> oPNWiseAccountHeads = new List<PNWiseAccountHead>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PNWiseAccountHeadDA.GetByNegoPNWiseAccountHead(tc, nPNWiseAccountHeadID);
                oPNWiseAccountHeads = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PNWiseAccountHeads", e);
                #endregion
            }
            return oPNWiseAccountHeads;
        }
        #endregion
    }
}