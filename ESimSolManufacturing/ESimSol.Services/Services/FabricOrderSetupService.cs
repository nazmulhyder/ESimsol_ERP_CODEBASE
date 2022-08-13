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
    public class FabricOrderSetupService : MarshalByRefObject, IFabricOrderSetupService
    {
        #region Private functions and declaration
        private FabricOrderSetup MapObject(NullHandler oReader)
        {
            FabricOrderSetup oFabricOrderSetup = new FabricOrderSetup();
            oFabricOrderSetup.FabricOrderSetupID = oReader.GetInt32("FabricOrderSetupID");
            oFabricOrderSetup.FabricOrderType = (EnumFabricRequestType)oReader.GetInt32("FabricOrderType");
            oFabricOrderSetup.BUID = oReader.GetInt32("BUID");
            oFabricOrderSetup.CurrencyID = oReader.GetInt32("CurrencyID");
            oFabricOrderSetup.Activity = oReader.GetBoolean("Activity");
            oFabricOrderSetup.BUName = oReader.GetString("BUName");
            oFabricOrderSetup.POPrintName = oReader.GetString("POPrintName");
            oFabricOrderSetup.PrintNo = (EnumExcellColumn)oReader.GetInt32("PrintNo");
            oFabricOrderSetup.ComboNo = oReader.GetInt32("ComboNo");
            oFabricOrderSetup.CodeNo = oReader.GetString("CodeNo");
            oFabricOrderSetup.CodeName = oReader.GetString("CodeName");
            oFabricOrderSetup.ShortName = oReader.GetString("ShortName");
            oFabricOrderSetup.OrderName = oReader.GetString("OrderName");
            oFabricOrderSetup.IsApplyPO = oReader.GetBoolean("IsApplyPO");
            oFabricOrderSetup.IsLocal = oReader.GetBoolean("IsLocal");
            oFabricOrderSetup.IsRateApply = oReader.GetBoolean("IsRateApply");
            oFabricOrderSetup.MUnitID = oReader.GetInt32("MUnitID");
            oFabricOrderSetup.MUnitID_Alt = oReader.GetInt32("MUnitID_Alt");
            oFabricOrderSetup.MUName = oReader.GetString("MUName");
            oFabricOrderSetup.CodeNo_Lab = oReader.GetString("CodeNo_Lab");
            oFabricOrderSetup.CodeName_Lab = oReader.GetString("CodeName_Lab");
            oFabricOrderSetup.CurrencySymbol = oReader.GetString("CurrencySymbol");
            if (String.IsNullOrEmpty(oFabricOrderSetup.ShortName))
            { oFabricOrderSetup.ShortName = oFabricOrderSetup.OrderName; }
            
            return oFabricOrderSetup;
        }

        private FabricOrderSetup CreateObject(NullHandler oReader)
        {
            FabricOrderSetup oFabricOrderSetup = new FabricOrderSetup();
            oFabricOrderSetup = MapObject(oReader);
            return oFabricOrderSetup;
        }

        private List<FabricOrderSetup> CreateObjects(IDataReader oReader)
        {
            List<FabricOrderSetup> oFabricOrderSetups = new List<FabricOrderSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricOrderSetup oItem = CreateObject(oHandler);
                oFabricOrderSetups.Add(oItem);
            }
            return oFabricOrderSetups;
        }

        #endregion

        #region Interface implementation
        public FabricOrderSetupService() { }


        public FabricOrderSetup Save(FabricOrderSetup oFabricOrderSetup, Int64 nUserId)
        {

            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);
                #region FabricOrderSetup
                IDataReader reader;
                if (oFabricOrderSetup.FabricOrderSetupID <= 0)
                {
                    reader = FabricOrderSetupDA.InsertUpdate(tc, oFabricOrderSetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = FabricOrderSetupDA.InsertUpdate(tc, oFabricOrderSetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricOrderSetup = new FabricOrderSetup();
                    oFabricOrderSetup = CreateObject(oReader);
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
                oFabricOrderSetup = new FabricOrderSetup();
                oFabricOrderSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricOrderSetup;
        }
      
        public String Delete(FabricOrderSetup oFabricOrderSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricOrderSetupDA.Delete(tc, oFabricOrderSetup, EnumDBOperation.Delete, nUserID);
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
        public FabricOrderSetup Get(int id, Int64 nUserId)
        {
            FabricOrderSetup oFabricOrderSetup = new FabricOrderSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricOrderSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricOrderSetup = CreateObject(oReader);
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

            return oFabricOrderSetup;
        }

        public List<FabricOrderSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricOrderSetup> oFabricOrderSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricOrderSetupDA.Gets(sSQL, tc);
                oFabricOrderSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricOrderSetup", e);
                #endregion
            }
            return oFabricOrderSetup;
        }
        public FabricOrderSetup GetByType(int nOrderType, Int64 nUserId)
        {
            FabricOrderSetup oFabricOrderSetup = new FabricOrderSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricOrderSetupDA.GetByType(tc, nOrderType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricOrderSetup = CreateObject(oReader);
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

            return oFabricOrderSetup;
        }

        public List<FabricOrderSetup> Gets(Int64 nUserId)
        {
            List<FabricOrderSetup> oFabricOrderSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricOrderSetupDA.Gets(tc);
                oFabricOrderSetups = CreateObjects(reader);
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

            return oFabricOrderSetups;
        }
        public List<FabricOrderSetup> GetByOrderTypes(int nBUID, bool bIsApplyPO,  Int64 nUserId)
        {
            List<FabricOrderSetup> oFabricOrderSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricOrderSetupDA.GetByOrderTypes(tc, nBUID, bIsApplyPO);
                oFabricOrderSetups = CreateObjects(reader);
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

            return oFabricOrderSetups;
        }
        public List<FabricOrderSetup> GetsActive(  int nBUID,Int64 nUserId)
        {
            List<FabricOrderSetup> oFabricOrderSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricOrderSetupDA.GetsActive(tc,  nBUID);
                oFabricOrderSetups = CreateObjects(reader);
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

            return oFabricOrderSetups;
        }

        public FabricOrderSetup Activate(FabricOrderSetup oFabricOrderSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricOrderSetupDA.Activate(tc, oFabricOrderSetup);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricOrderSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricOrderSetup = new FabricOrderSetup();
                oFabricOrderSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricOrderSetup;
        }
    

        #endregion
    }
}