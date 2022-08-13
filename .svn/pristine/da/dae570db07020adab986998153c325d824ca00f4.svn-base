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
    public class DeliveryOrderNameService : MarshalByRefObject, IDeliveryOrderNameService
    {
        #region Private functions and declaration
        private DeliveryOrderName MapObject(NullHandler oReader)
        {
            DeliveryOrderName oDeliveryOrderName = new DeliveryOrderName();
            oDeliveryOrderName.DeliveryOrderNameID = oReader.GetInt32("DeliveryOrderNameID");
            oDeliveryOrderName.Name = oReader.GetString("Name");
            oDeliveryOrderName.Activity = oReader.GetBoolean("Activity");
            oDeliveryOrderName.Sequence = oReader.GetInt32("Sequence");
            oDeliveryOrderName.OrderType = oReader.GetInt32("OrderType");
            oDeliveryOrderName.IsFoc = oReader.GetBoolean("IsFoc");
            oDeliveryOrderName.IsGrey = oReader.GetBoolean("IsGrey");
            return oDeliveryOrderName;
        }
        private DeliveryOrderName CreateObject(NullHandler oReader)
        {
            DeliveryOrderName oDeliveryOrderName = new DeliveryOrderName();
            oDeliveryOrderName = MapObject(oReader);
            return oDeliveryOrderName;
        }
        private List<DeliveryOrderName> CreateObjects(IDataReader oReader)
        {
            List<DeliveryOrderName> oDeliveryOrderName = new List<DeliveryOrderName>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DeliveryOrderName oItem = CreateObject(oHandler);
                oDeliveryOrderName.Add(oItem);
            }
            return oDeliveryOrderName;
        }

        #endregion

        #region Interface implementation
        public DeliveryOrderName Save(DeliveryOrderName oDeliveryOrderName, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDeliveryOrderName.DeliveryOrderNameID <= 0)
                {
                    reader = DeliveryOrderNameDA.InsertUpdate(tc, oDeliveryOrderName, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DeliveryOrderNameDA.InsertUpdate(tc, oDeliveryOrderName, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDeliveryOrderName = new DeliveryOrderName();
                    oDeliveryOrderName = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDeliveryOrderName.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDeliveryOrderName;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DeliveryOrderName oDeliveryOrderName = new DeliveryOrderName();
                oDeliveryOrderName.DeliveryOrderNameID = id;
                //DBTableReferenceDA.HasReference(tc, "DeliveryOrderName", id);
                DeliveryOrderNameDA.Delete(tc, oDeliveryOrderName, EnumDBOperation.Delete, nUserId);
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
        public List<DeliveryOrderName> Gets(Int64 nUserID)
        {
            List<DeliveryOrderName> oDeliveryOrderNames = new List<DeliveryOrderName>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DeliveryOrderNameDA.Gets(tc);
                oDeliveryOrderNames = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDeliveryOrderNames = new List<DeliveryOrderName>();
                DeliveryOrderName oDeliveryOrderName = new DeliveryOrderName();
                oDeliveryOrderName.ErrorMessage = e.Message.Split('~')[0];
                oDeliveryOrderNames.Add(oDeliveryOrderName);
                #endregion
            }
            return oDeliveryOrderNames;
        }
        public DeliveryOrderName Get(int id, Int64 nUserId)
        {
            DeliveryOrderName oDeliveryOrderName = new DeliveryOrderName();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DeliveryOrderNameDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDeliveryOrderName = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDeliveryOrderName.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDeliveryOrderName;
        }
        public List<DeliveryOrderName> Gets(bool bActivity, Int64 nUserID)
        {
            List<DeliveryOrderName> oDeliveryOrderNames = new List<DeliveryOrderName>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DeliveryOrderNameDA.GetsActivity(tc, bActivity);
                oDeliveryOrderNames = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDeliveryOrderNames = new List<DeliveryOrderName>();
                DeliveryOrderName oDeliveryOrderName = new DeliveryOrderName();
                oDeliveryOrderName.ErrorMessage = e.Message.Split('~')[0];
                oDeliveryOrderNames.Add(oDeliveryOrderName);
                #endregion
            }
            return oDeliveryOrderNames;
        }
        #endregion
    }
}
