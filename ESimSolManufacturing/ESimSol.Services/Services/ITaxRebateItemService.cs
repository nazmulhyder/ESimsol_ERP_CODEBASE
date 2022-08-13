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
    public class ITaxRebateItemService : MarshalByRefObject, IITaxRebateItemService
    {
        #region Private functions and declaration
        private ITaxRebateItem MapObject(NullHandler oReader)
        {
            ITaxRebateItem oITaxRebateItem = new ITaxRebateItem();

            oITaxRebateItem.ITaxRebateItemID = oReader.GetInt32("ITaxRebateItemID");
            oITaxRebateItem.ITaxRebateType = (EnumITaxRebateType)oReader.GetInt16("ITaxRebateType");
            oITaxRebateItem.Description = oReader.GetString("Description");
            oITaxRebateItem.IsActive = oReader.GetBoolean("IsActive");
            return oITaxRebateItem;

        }

        private ITaxRebateItem CreateObject(NullHandler oReader)
        {
            ITaxRebateItem oITaxRebateItem = MapObject(oReader);
            return oITaxRebateItem;
        }

        private List<ITaxRebateItem> CreateObjects(IDataReader oReader)
        {
            List<ITaxRebateItem> oITaxRebateItems = new List<ITaxRebateItem>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITaxRebateItem oItem = CreateObject(oHandler);
                oITaxRebateItems.Add(oItem);
            }
            return oITaxRebateItems;
        }

        #endregion

        #region Interface implementation
        public ITaxRebateItemService() { }

        public ITaxRebateItem IUD(ITaxRebateItem oITaxRebateItem, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ITaxRebateItemDA.IUD(tc, oITaxRebateItem, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oITaxRebateItem = CreateObject(oReader);
                }
                reader.Close();

                tc.End();

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oITaxRebateItem = new ITaxRebateItem();
                    oITaxRebateItem.ErrorMessage = Global.DeleteMessage;
                }
                    
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxRebateItem = new ITaxRebateItem();
                oITaxRebateItem.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oITaxRebateItem;
        }


        public ITaxRebateItem Get(int nITaxRebateItemID, Int64 nUserId)
        {
            ITaxRebateItem oITaxRebateItem = new ITaxRebateItem();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxRebateItemDA.Get(nITaxRebateItemID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRebateItem = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxRebateItem = new ITaxRebateItem();
                oITaxRebateItem.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxRebateItem;
        }

        public ITaxRebateItem Get(string sSQL, Int64 nUserId)
        {
            ITaxRebateItem oITaxRebateItem = new ITaxRebateItem();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxRebateItemDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRebateItem = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxRebateItem = new ITaxRebateItem();
                oITaxRebateItem.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxRebateItem;
        }

        public List<ITaxRebateItem> Gets(Int64 nUserID)
        {
            List<ITaxRebateItem> oITaxRebateItems = new List<ITaxRebateItem>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxRebateItemDA.Gets(tc);
                oITaxRebateItems = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ITaxRebateItem oITaxRebateItem = new ITaxRebateItem();
                oITaxRebateItem.ErrorMessage = e.Message;
                oITaxRebateItems.Add(oITaxRebateItem);
                #endregion
            }
            return oITaxRebateItems;
        }

        public List<ITaxRebateItem> Gets(string sSQL, Int64 nUserID)
        {
            List<ITaxRebateItem> oITaxRebateItems = new List<ITaxRebateItem>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxRebateItemDA.Gets(sSQL, tc);
                oITaxRebateItems = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ITaxRebateItem oITaxRebateItem = new ITaxRebateItem();
                oITaxRebateItem.ErrorMessage = e.Message;
                oITaxRebateItems.Add(oITaxRebateItem);
                #endregion
            }
            return oITaxRebateItems;
        }

        #endregion

        #region Activity
        public ITaxRebateItem Activite(int nITaxRebateItemID, bool Active, Int64 nUserId)
        {
            ITaxRebateItem oITaxRebateItem = new ITaxRebateItem();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxRebateItemDA.Activity(nITaxRebateItemID, Active, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRebateItem = CreateObject(oReader);
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
                oITaxRebateItem.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxRebateItem;
        }


        #endregion

    }
}
