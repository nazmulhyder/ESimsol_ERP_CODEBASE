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
    public class FabricPOSetupService : MarshalByRefObject, IFabricPOSetupService
    {
        #region Private functions and declaration
        private FabricPOSetup MapObject(NullHandler oReader)
        {
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            oFabricPOSetup.FabricPOSetupID = oReader.GetInt32("FabricPOSetupID");
            oFabricPOSetup.Activity = oReader.GetBoolean("Activity");
            oFabricPOSetup.IsLDApply = oReader.GetBoolean("IsLDApply");
            oFabricPOSetup.PrintNo = oReader.GetInt32("PrintNo");
            oFabricPOSetup.POPrintName = oReader.GetString("POPrintName");
            oFabricPOSetup.NoCode = oReader.GetString("NoCode");
            oFabricPOSetup.OrderName = oReader.GetString("OrderName");
            oFabricPOSetup.FabricCode = oReader.GetString("FabricCode");
            oFabricPOSetup.CurrencyID = oReader.GetInt32("CurrencyID");
            oFabricPOSetup.CurrencyName = oReader.GetString("CurrencyName");
            oFabricPOSetup.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oFabricPOSetup.IsNeedCheckBy = oReader.GetBoolean("IsNeedCheckBy");
            oFabricPOSetup.IsNeedExpDelivery = oReader.GetBoolean("IsNeedExpDelivery");
            
            return oFabricPOSetup;
        }

        private FabricPOSetup CreateObject(NullHandler oReader)
        {
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            oFabricPOSetup = MapObject(oReader);
            return oFabricPOSetup;
        }

        private List<FabricPOSetup> CreateObjects(IDataReader oReader)
        {
            List<FabricPOSetup> oFabricPOSetups = new List<FabricPOSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricPOSetup oItem = CreateObject(oHandler);
                oFabricPOSetups.Add(oItem);
            }
            return oFabricPOSetups;
        }

        #endregion

        #region Interface implementation
        public FabricPOSetupService() { }


        public FabricPOSetup Save(FabricPOSetup oFabricPOSetup, Int64 nUserId)
        {

            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);
                #region FabricPOSetup
                IDataReader reader;
                if (oFabricPOSetup.FabricPOSetupID <= 0)
                {
                    reader = FabricPOSetupDA.InsertUpdate(tc, oFabricPOSetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = FabricPOSetupDA.InsertUpdate(tc, oFabricPOSetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricPOSetup = new FabricPOSetup();
                    oFabricPOSetup = CreateObject(oReader);
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
                oFabricPOSetup = new FabricPOSetup();
                oFabricPOSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricPOSetup;
        }
      
        public String Delete(FabricPOSetup oFabricPOSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricPOSetupDA.Delete(tc, oFabricPOSetup, EnumDBOperation.Delete, nUserID);
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
        public FabricPOSetup Get(int id, Int64 nUserId)
        {
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricPOSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricPOSetup = CreateObject(oReader);
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

            return oFabricPOSetup;
        }

        public List<FabricPOSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricPOSetup> oFabricPOSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricPOSetupDA.Gets(sSQL, tc);
                oFabricPOSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricPOSetup", e);
                #endregion
            }
            return oFabricPOSetup;
        }
     
        public List<FabricPOSetup> Gets(Int64 nUserId)
        {
            List<FabricPOSetup> oFabricPOSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricPOSetupDA.Gets(tc);
                oFabricPOSetups = CreateObjects(reader);
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

            return oFabricPOSetups;
        }
        public FabricPOSetup GetsActive( Int64 nUserId)
        {
            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricPOSetupDA.GetsActive(tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricPOSetup = CreateObject(oReader);
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

            return oFabricPOSetup;
        }
        public FabricPOSetup Activate(FabricPOSetup oFabricPOSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricPOSetupDA.Activate(tc, oFabricPOSetup);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricPOSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricPOSetup = new FabricPOSetup();
                oFabricPOSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricPOSetup;
        }
    

        #endregion
    }
}