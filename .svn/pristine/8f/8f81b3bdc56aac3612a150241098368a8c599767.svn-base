using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class DeliverySetupService : MarshalByRefObject, IDeliverySetupService
    {
        #region Private functions and declaration
        private DeliverySetup MapObject(NullHandler oReader)
        {
            DeliverySetup oDeliverySetup = new DeliverySetup();
            oDeliverySetup.DeliverySetupID = oReader.GetInt32("DeliverySetupID");
            oDeliverySetup.PrintHeader = oReader.GetString("PrintHeader");
            oDeliverySetup.OrderPrintNo = (EnumExcellColumn)oReader.GetInt32("OrderPrintNo");
            oDeliverySetup.ChallanPrintNo = (EnumExcellColumn)oReader.GetInt32("ChallanPrintNo");
            oDeliverySetup.BUID = oReader.GetInt32("BUID");
            oDeliverySetup.DCPrefix = oReader.GetString("DCPrefix");
            oDeliverySetup.GPPrefix = oReader.GetString("GPPrefix");
            oDeliverySetup.ImagePad = oReader.GetBytes("ImagePad");
            oDeliverySetup.ImagePadName = oReader.GetString("ImagePadName");
            oDeliverySetup.PrintFormatType = (EnumPrintFormatType)oReader.GetInt32("PrintFormatType");
            oDeliverySetup.OverDCQty = oReader.GetDouble("OverDCQty");
            oDeliverySetup.OverDeliverPercentage = oReader.GetDouble("OverDeliverPercentage");
            return oDeliverySetup;
        }

        private DeliverySetup CreateObject(NullHandler oReader)
        {
            DeliverySetup oDeliverySetup = new DeliverySetup();
            oDeliverySetup = MapObject(oReader);
            return oDeliverySetup;
        }

        private List<DeliverySetup> CreateObjects(IDataReader oReader)
        {
            List<DeliverySetup> oDeliverySetup = new List<DeliverySetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DeliverySetup oItem = CreateObject(oHandler);
                oDeliverySetup.Add(oItem);
            }
            return oDeliverySetup;
        }

        #endregion

        #region Interface implementation
        public DeliverySetupService() { }
        public DeliverySetup Save(DeliverySetup oDeliverySetup, Int64 nUserId)
        {
            DeliverySetup oTempDeliverySetup = new DeliverySetup();
            oTempDeliverySetup.ImagePad = oDeliverySetup.ImagePad;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oDeliverySetup.ImagePad = null;
                if (oDeliverySetup.DeliverySetupID <= 0)
                {
                    reader = DeliverySetupDA.InsertUpdate(tc, oDeliverySetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = DeliverySetupDA.InsertUpdate(tc, oDeliverySetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDeliverySetup = new DeliverySetup();
                    oDeliverySetup = CreateObject(oReader);
                }
                reader.Close();

                oTempDeliverySetup.DeliverySetupID = oDeliverySetup.DeliverySetupID;
                if (oTempDeliverySetup.ImagePad != null)
                {
                    DeliverySetupDA.UpdateImagePad(tc, oTempDeliverySetup, nUserId);
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDeliverySetup.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return oDeliverySetup;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DeliverySetup oDeliverySetup = new DeliverySetup();
                oDeliverySetup.DeliverySetupID = id;
                DeliverySetupDA.Delete(tc, oDeliverySetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }
        public DeliverySetup Get(int id, Int64 nUserId)
        {
            DeliverySetup oDeliverySetup = new DeliverySetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DeliverySetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDeliverySetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oDeliverySetup;
        }
        public DeliverySetup GetByBU(int buid, Int64 nUserId)
        {
            DeliverySetup oDeliverySetup = new DeliverySetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DeliverySetupDA.GetByBU(tc, buid);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDeliverySetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }
            return oDeliverySetup;
        }
    
        public List<DeliverySetup> Gets(Int64 nUserId)
        {
            List<DeliverySetup> oDeliverySetup = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DeliverySetupDA.Gets(tc);
                oDeliverySetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Delivery Setup", e);
                #endregion
            }

            return oDeliverySetup;
        }

        public List<DeliverySetup> Gets(string sSQL, Int64 nUserId)
        {
            List<DeliverySetup> oDeliverySetup = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DeliverySetupDA.Gets(tc, sSQL);
                oDeliverySetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Delivery Setup", e);
                #endregion
            }

            return oDeliverySetup;
        }

        #endregion
    }
}

