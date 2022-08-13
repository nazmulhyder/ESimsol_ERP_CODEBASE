using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class BuyerPercentService : MarshalByRefObject, IBuyerPercentService
    {
        #region Private functions and declaration
        private BuyerPercent MapObject(NullHandler oReader)
        {
            BuyerPercent oBuyerPercent = new BuyerPercent();
            oBuyerPercent.BuyerPercentID = oReader.GetInt32("BuyerPercentID");
            oBuyerPercent.BPosition = oReader.GetString("BPosition");
            oBuyerPercent.BPercent = oReader.GetDouble("BPercent");
            oBuyerPercent.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oBuyerPercent.LastUpdateDateTime = Convert.ToDateTime(oReader.GetDateTime("LastUpdateDateTime"));
            oBuyerPercent.LastUpdateByName = oReader.GetString("LastUpdateByName");
            return oBuyerPercent;
        }
        private BuyerPercent CreateObject(NullHandler oReader)
        {
            BuyerPercent oBuyerPercent = new BuyerPercent();
            oBuyerPercent = MapObject(oReader);
            return oBuyerPercent;
        }

        private List<BuyerPercent> CreateObjects(IDataReader oReader)
        {
            List<BuyerPercent> oBuyerPercent = new List<BuyerPercent>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BuyerPercent oItem = CreateObject(oHandler);
                oBuyerPercent.Add(oItem);
            }
            return oBuyerPercent;
        }
        #endregion
        #region Interface implementation
        public BuyerPercent Save(BuyerPercent oBuyerPercent, Int64 nUserID)
        {
            string sRefChildIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBuyerPercent.BuyerPercentID <= 0)
                {

                    reader = BuyerPercentDA.InsertUpdate(tc, oBuyerPercent, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = BuyerPercentDA.InsertUpdate(tc, oBuyerPercent, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBuyerPercent = new BuyerPercent();
                    oBuyerPercent = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oBuyerPercent = new BuyerPercent();
                    oBuyerPercent.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oBuyerPercent;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BuyerPercent oBuyerPercent = new BuyerPercent();
                oBuyerPercent.BuyerPercentID = id;
                DBTableReferenceDA.HasReference(tc, "BuyerPercent", id);
                BuyerPercentDA.Delete(tc, oBuyerPercent, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public BuyerPercent Get(int id, Int64 nUserId)
        {
            BuyerPercent oBuyerPercent = new BuyerPercent();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = BuyerPercentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBuyerPercent = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get BuyerPercent", e);
                #endregion
            }
            return oBuyerPercent;
        }
        public List<BuyerPercent> Gets(Int64 nUserID)
        {
            List<BuyerPercent> oBuyerPercents = new List<BuyerPercent>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BuyerPercentDA.Gets(tc);
                oBuyerPercents = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                BuyerPercent oBuyerPercent = new BuyerPercent();
                oBuyerPercent.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oBuyerPercents;
        }
        public List<BuyerPercent> Gets(string sSQL, Int64 nUserID)
        {
            List<BuyerPercent> oBuyerPercents = new List<BuyerPercent>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BuyerPercentDA.Gets(tc, sSQL);
                oBuyerPercents = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BuyerPercent", e);
                #endregion
            }
            return oBuyerPercents;
        }

        #endregion
    }
}