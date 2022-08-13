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
    public class DeliveryZoneService : MarshalByRefObject, IDeliveryZoneService
    {
        #region Private functions and declaration
        private DeliveryZone MapObject(NullHandler oReader)
        {
            DeliveryZone oDeliveryZone = new DeliveryZone();
            oDeliveryZone.DeliveryZoneID = oReader.GetInt32("DeliveryZoneID");
            oDeliveryZone.DeliveryZoneName = oReader.GetString("DeliveryZoneName");
            return oDeliveryZone;
        }
        private DeliveryZone CreateObject(NullHandler oReader)
        {
            DeliveryZone oDeliveryZone = new DeliveryZone();
            oDeliveryZone = MapObject(oReader);
            return oDeliveryZone;
        }
        private List<DeliveryZone> CreateObjects(IDataReader oReader)
        {
            List<DeliveryZone> oDeliveryZone = new List<DeliveryZone>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DeliveryZone oItem = CreateObject(oHandler);
                oDeliveryZone.Add(oItem);
            }
            return oDeliveryZone;
        }

        #endregion

        #region Interface implementation
        public DeliveryZone Save(DeliveryZone oDeliveryZone, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDeliveryZone.DeliveryZoneID <= 0)
                {
                    reader = DeliveryZoneDA.InsertUpdate(tc, oDeliveryZone, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DeliveryZoneDA.InsertUpdate(tc, oDeliveryZone, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDeliveryZone = new DeliveryZone();
                    oDeliveryZone = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDeliveryZone.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDeliveryZone;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DeliveryZone oDeliveryZone = new DeliveryZone();
                oDeliveryZone.DeliveryZoneID = id;                
                DeliveryZoneDA.Delete(tc, oDeliveryZone, EnumDBOperation.Delete, nUserId);
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
        public List<DeliveryZone> Gets(int nUserID)
        {
            List<DeliveryZone> oDeliveryZones = new List<DeliveryZone>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DeliveryZoneDA.Gets(tc);
                oDeliveryZones = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDeliveryZones = new List<DeliveryZone>();
                DeliveryZone oDeliveryZone = new DeliveryZone();
                oDeliveryZone.ErrorMessage = e.Message.Split('~')[0];
                oDeliveryZones.Add(oDeliveryZone);
                #endregion
            }
            return oDeliveryZones;
        }

        public List<DeliveryZone> Gets(string sSQL, int nUserID)
        {
            List<DeliveryZone> oDeliveryZones = new List<DeliveryZone>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DeliveryZoneDA.Gets(tc, sSQL);
                oDeliveryZones = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDeliveryZones = new List<DeliveryZone>();
                DeliveryZone oDeliveryZone = new DeliveryZone();
                oDeliveryZone.ErrorMessage = e.Message.Split('~')[0];
                oDeliveryZones.Add(oDeliveryZone);
                #endregion
            }
            return oDeliveryZones;
        }
        public DeliveryZone Get(int id, int nUserId)
        {
            DeliveryZone oDeliveryZone = new DeliveryZone();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DeliveryZoneDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDeliveryZone = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDeliveryZone.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDeliveryZone;
        }

        #endregion
    }
}
