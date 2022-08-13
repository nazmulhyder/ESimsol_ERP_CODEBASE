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
    public class CashFlowHeadService : MarshalByRefObject, ICashFlowHeadService
    {
        #region Private functions and declaration
        private CashFlowHead MapObject(NullHandler oReader)
        {
            CashFlowHead oCashFlowHead = new CashFlowHead();
            oCashFlowHead.CashFlowHeadID = oReader.GetInt32("CashFlowHeadID");
            oCashFlowHead.CashFlowHeadType = (EnumCashFlowHeadType)oReader.GetInt32("CashFlowHeadType");
            oCashFlowHead.CashFlowHeadTypeInt = oReader.GetInt32("CashFlowHeadType");
            oCashFlowHead.DisplayCaption = oReader.GetString("DisplayCaption");
            oCashFlowHead.IsDebit = oReader.GetBoolean("IsDebit");
            oCashFlowHead.Remarks = oReader.GetString("Remarks");
            oCashFlowHead.Sequence = oReader.GetInt32("Sequence");
            return oCashFlowHead;
        }

        private CashFlowHead CreateObject(NullHandler oReader)
        {
            CashFlowHead oCashFlowHead = new CashFlowHead();
            oCashFlowHead = MapObject(oReader);
            return oCashFlowHead;
        }

        private List<CashFlowHead> CreateObjects(IDataReader oReader)
        {
            List<CashFlowHead> oCashFlowHead = new List<CashFlowHead>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CashFlowHead oItem = CreateObject(oHandler);
                oCashFlowHead.Add(oItem);
            }
            return oCashFlowHead;
        }

        #endregion

        #region Interface implementation
        public CashFlowHeadService() { }

        public CashFlowHead Save(CashFlowHead oCashFlowHead, Int64 nUserID)
        {
           
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCashFlowHead.CashFlowHeadID <= 0)
                {                    
                    reader = CashFlowHeadDA.InsertUpdate(tc, oCashFlowHead, EnumDBOperation.Insert, nUserID);
                }
                else
                {                   
                    reader = CashFlowHeadDA.InsertUpdate(tc, oCashFlowHead, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCashFlowHead = new CashFlowHead();
                    oCashFlowHead = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oCashFlowHead = new CashFlowHead();
                oCashFlowHead.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oCashFlowHead;
        }

        public string UpdateScequence(CashFlowHead oCashFlowHead, Int64 nUserID)
        {
            string sFeedBackMessage = "Update Successfully";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (CashFlowHead oItem in oCashFlowHead.CashFlowHeads)
                {
                    CashFlowHeadDA.UpdateScequence(tc, oItem);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                sFeedBackMessage = e.Message.Split('!')[0];
                #endregion
            }
            return sFeedBackMessage;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CashFlowHead oCashFlowHead = new CashFlowHead();
                oCashFlowHead.CashFlowHeadID = id;
                CashFlowHeadDA.Delete(tc, oCashFlowHead, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete CashFlowHead. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public CashFlowHead Get(int id, Int64 nUserId)
        {
            CashFlowHead oAccountHead = new CashFlowHead();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CashFlowHeadDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get CashFlowHead", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<CashFlowHead> Gets(Int64 nUserID)
        {
            List<CashFlowHead> oCashFlowHead = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CashFlowHeadDA.Gets(tc);
                oCashFlowHead = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CashFlowHead", e);
                #endregion
            }

            return oCashFlowHead;
        }

        public List<CashFlowHead> Gets(string sSQL, Int64 nUserID)
        {
            List<CashFlowHead> oCashFlowHead = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CashFlowHeadDA.Gets(tc, sSQL);
                oCashFlowHead = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CashFlowHead", e);
                #endregion
            }

            return oCashFlowHead;
        }

        #endregion
    }   

    
}
