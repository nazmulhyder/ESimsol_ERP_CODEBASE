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
    public class DUOrderSetupService : MarshalByRefObject, IDUOrderSetupService
    {
        #region Private functions and declaration
        private DUOrderSetup MapObject(NullHandler oReader)
        {
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            oDUOrderSetup.DUOrderSetupID = oReader.GetInt32("DUOrderSetupID");
            oDUOrderSetup.OrderType = oReader.GetInt32("OrderType");
            oDUOrderSetup.BUID = oReader.GetInt32("BUID");
            oDUOrderSetup.CurrencyID = oReader.GetInt32("CurrencyID");
            oDUOrderSetup.Activity = oReader.GetBoolean("Activity");
            oDUOrderSetup.BUName = oReader.GetString("BUName");
            oDUOrderSetup.PrintNo = oReader.GetInt32("PrintNo");
            oDUOrderSetup.ComboNo = oReader.GetInt32("ComboNo");
            oDUOrderSetup.ComboNoDC = oReader.GetInt32("ComboNoDC");
            oDUOrderSetup.PrintName = oReader.GetString("PrintName");
            oDUOrderSetup.NoCode = oReader.GetString("NoCode");
            oDUOrderSetup.ShortName = oReader.GetString("ShortName");
            oDUOrderSetup.OrderName = oReader.GetString("OrderName");
            oDUOrderSetup.DONoCode = oReader.GetString("DONoCode");
            oDUOrderSetup.IsPIMendatory = oReader.GetBoolean("IsPIMendatory");
            oDUOrderSetup.IsRateMendatory = oReader.GetBoolean("IsRateMendatory");
            oDUOrderSetup.IsInvoiceMendatory = oReader.GetBoolean("IsInvoiceMendatory");
            oDUOrderSetup.IsApplyOutside = oReader.GetBoolean("IsApplyOutside");
            oDUOrderSetup.IsApplyDyeingStep = oReader.GetBoolean("IsApplyDyeingStep");
            oDUOrderSetup.IsSaveLabDip = oReader.GetBoolean("IsSaveLabDip");
            oDUOrderSetup.IsApplyFabric = oReader.GetBoolean("IsApplyFabric");
            oDUOrderSetup.IsInHouse = oReader.GetBoolean("IsInHouse");
            oDUOrderSetup.MUnitID = oReader.GetInt32("MUnitID");
            oDUOrderSetup.MUnitID_Alt = oReader.GetInt32("MUnitID_Alt");
            oDUOrderSetup.MUName = oReader.GetString("MUName");
            oDUOrderSetup.MUName_Alt = oReader.GetString("MUName_Alt");
            oDUOrderSetup.CurrencySY = oReader.GetString("CurrencySY");
            oDUOrderSetup.IsOpenRawLot = oReader.GetBoolean("IsOpenRawLot");
            if (String.IsNullOrEmpty(oDUOrderSetup.ShortName))
            { oDUOrderSetup.ShortName = oDUOrderSetup.OrderName; }
            oDUOrderSetup.DeliveryGrace = oReader.GetDouble("DeliveryGrace");
            oDUOrderSetup.DeliveryValidation = (EnumDeliveryValidation)oReader.GetInt32("DeliveryValidation");
            
            return oDUOrderSetup;
        }

        private DUOrderSetup CreateObject(NullHandler oReader)
        {
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            oDUOrderSetup = MapObject(oReader);
            return oDUOrderSetup;
        }

        private List<DUOrderSetup> CreateObjects(IDataReader oReader)
        {
            List<DUOrderSetup> oDUOrderSetups = new List<DUOrderSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUOrderSetup oItem = CreateObject(oHandler);
                oDUOrderSetups.Add(oItem);
            }
            return oDUOrderSetups;
        }

        #endregion

        #region Interface implementation
        public DUOrderSetupService() { }


        public DUOrderSetup Save(DUOrderSetup oDUOrderSetup, Int64 nUserId)
        {

            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);
                #region DUOrderSetup
                IDataReader reader;
                if (oDUOrderSetup.DUOrderSetupID <= 0)
                {
                    reader = DUOrderSetupDA.InsertUpdate(tc, oDUOrderSetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = DUOrderSetupDA.InsertUpdate(tc, oDUOrderSetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUOrderSetup = new DUOrderSetup();
                    oDUOrderSetup = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDUOrderSetup = new DUOrderSetup();
                oDUOrderSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUOrderSetup;
        }
      
        public String Delete(DUOrderSetup oDUOrderSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DUOrderSetupDA.Delete(tc, oDUOrderSetup, EnumDBOperation.Delete, nUserID);
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
        public DUOrderSetup Get(int id, Int64 nUserId)
        {
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUOrderSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUOrderSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oDUOrderSetup;
        }

        public List<DUOrderSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<DUOrderSetup> oDUOrderSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUOrderSetupDA.Gets(sSQL, tc);
                oDUOrderSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUOrderSetup", e);
                #endregion
            }
            return oDUOrderSetup;
        }
        public DUOrderSetup GetByType(int nOrderType, Int64 nUserId)
        {
            DUOrderSetup oDUOrderSetup = new DUOrderSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUOrderSetupDA.GetByType(tc, nOrderType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUOrderSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oDUOrderSetup;
        }
     

        public List<DUOrderSetup> Gets(Int64 nUserId)
        {
            List<DUOrderSetup> oDUOrderSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUOrderSetupDA.Gets(tc);
                oDUOrderSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oDUOrderSetups;
        }
        public List<DUOrderSetup> GetByOrderTypes(int nBUID,bool bIsInHouse, string sOrderType, Int64 nUserId)
        {
            List<DUOrderSetup> oDUOrderSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUOrderSetupDA.GetByOrderTypes(tc,nBUID,  bIsInHouse,  sOrderType);
                oDUOrderSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oDUOrderSetups;
        }
        public List<DUOrderSetup> GetsActive(  int nBUID,Int64 nUserId)
        {
            List<DUOrderSetup> oDUOrderSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUOrderSetupDA.GetsActive(tc,  nBUID);
                oDUOrderSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oDUOrderSetups;
        }

        public DUOrderSetup Activate(DUOrderSetup oDUOrderSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUOrderSetupDA.Activate(tc, oDUOrderSetup);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUOrderSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDUOrderSetup = new DUOrderSetup();
                oDUOrderSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUOrderSetup;
        }
    

        #endregion
    }
}