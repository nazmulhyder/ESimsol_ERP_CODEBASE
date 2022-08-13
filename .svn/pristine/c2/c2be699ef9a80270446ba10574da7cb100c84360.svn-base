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
    public class FabricExpDeliveryService : MarshalByRefObject, IFabricExpDeliveryService
    {
        #region Private functions and declaration
        private FabricExpDelivery MapObject(NullHandler oReader)
        {
            FabricExpDelivery oFabricExpDelivery = new FabricExpDelivery();
            oFabricExpDelivery.FabricExpDeliveryID = oReader.GetInt32("FabricExpDeliveryID");
            oFabricExpDelivery.FSCDID = oReader.GetInt32("FSCDID");
            oFabricExpDelivery.Qty = oReader.GetDouble("Qty");
            oFabricExpDelivery.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oFabricExpDelivery.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oFabricExpDelivery.LastUpdateDateTime = oReader.GetDateTime("LastUpdateDateTime");
            oFabricExpDelivery.LastUpdateByName = oReader.GetString("LastUpdateByName");
            oFabricExpDelivery.ExeNo = oReader.GetString("ExeNo");
            oFabricExpDelivery.DispoQty = oReader.GetDouble("DispoQty");

            return oFabricExpDelivery;
        }
        private FabricExpDelivery CreateObject(NullHandler oReader)
        {
            FabricExpDelivery oFabricExpDelivery = new FabricExpDelivery();
            oFabricExpDelivery = MapObject(oReader);
            return oFabricExpDelivery;
        }

        private List<FabricExpDelivery> CreateObjects(IDataReader oReader)
        {
            List<FabricExpDelivery> oFabricExpDelivery = new List<FabricExpDelivery>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricExpDelivery oItem = CreateObject(oHandler);
                oFabricExpDelivery.Add(oItem);
            }
            return oFabricExpDelivery;
        }
        #endregion
        #region Interface implementation
        public FabricExpDelivery Save(FabricExpDelivery oFabricExpDelivery, Int64 nUserID)
        {
            string sRefChildIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricExpDelivery.FabricExpDeliveryID <= 0)
                {

                    reader = FabricExpDeliveryDA.InsertUpdate(tc, oFabricExpDelivery, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricExpDeliveryDA.InsertUpdate(tc, oFabricExpDelivery, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricExpDelivery = new FabricExpDelivery();
                    oFabricExpDelivery = CreateObject(oReader);
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
                    oFabricExpDelivery = new FabricExpDelivery();
                    oFabricExpDelivery.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricExpDelivery;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricExpDelivery oFabricExpDelivery = new FabricExpDelivery();
                oFabricExpDelivery.FabricExpDeliveryID = id;
                DBTableReferenceDA.HasReference(tc, "FabricExpDelivery", id);
                FabricExpDeliveryDA.Delete(tc, oFabricExpDelivery, EnumDBOperation.Delete, nUserId);
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
        public FabricExpDelivery Get(int id, Int64 nUserId)
        {
            FabricExpDelivery oFabricExpDelivery = new FabricExpDelivery();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricExpDeliveryDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricExpDelivery = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricExpDelivery", e);
                #endregion
            }
            return oFabricExpDelivery;
        }
        public List<FabricExpDelivery> Gets(Int64 nUserID)
        {
            List<FabricExpDelivery> oFabricExpDeliverys = new List<FabricExpDelivery>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricExpDeliveryDA.Gets(tc);
                oFabricExpDeliverys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricExpDelivery oFabricExpDelivery = new FabricExpDelivery();
                oFabricExpDelivery.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricExpDeliverys;
        }
        public List<FabricExpDelivery> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricExpDelivery> oFabricExpDeliverys = new List<FabricExpDelivery>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricExpDeliveryDA.Gets(tc, sSQL);
                oFabricExpDeliverys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricExpDelivery", e);
                #endregion
            }
            return oFabricExpDeliverys;
        }

        #endregion
    }
}