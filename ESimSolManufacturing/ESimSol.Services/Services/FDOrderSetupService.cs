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
    public class FDOrderSetupService : MarshalByRefObject, IFDOrderSetupService
    {
        #region Private functions and declaration
        private FDOrderSetup MapObject(NullHandler oReader)
        {
            FDOrderSetup oFDOrderSetup = new FDOrderSetup();
            oFDOrderSetup.FDOrderSetupID = oReader.GetInt32("FDOrderSetupID");
            oFDOrderSetup.FDOTypeInt = oReader.GetInt32("FDOType");
            oFDOrderSetup.FDOType = (EnumDOType)oReader.GetInt32("FDOType");
            oFDOrderSetup.BUID = oReader.GetInt32("BUID");
            oFDOrderSetup.CurrencyID = oReader.GetInt32("CurrencyID");
            oFDOrderSetup.Activity = oReader.GetBoolean("Activity");
            oFDOrderSetup.BUName = oReader.GetString("BUName");
            oFDOrderSetup.PrintNo = oReader.GetInt32("PrintNo");
            oFDOrderSetup.ComboNo = oReader.GetInt32("ComboNo");
            oFDOrderSetup.PrintName = oReader.GetString("PrintName");
            oFDOrderSetup.NoCode = oReader.GetString("NoCode");
            oFDOrderSetup.NoteFixed = oReader.GetString("NoteFixed");
            oFDOrderSetup.ShortName = oReader.GetString("ShortName");
            oFDOrderSetup.FDOName = oReader.GetString("FDOName");
            oFDOrderSetup.DONoCode = oReader.GetString("DONoCode");
            oFDOrderSetup.PrintFormat = oReader.GetInt32("PrintFormat");
            oFDOrderSetup.MUnitID = oReader.GetInt32("MUnitID");
            oFDOrderSetup.MUnitID_Alt = oReader.GetInt32("MUnitID_Alt");
            oFDOrderSetup.MUName = oReader.GetString("MUName");
            oFDOrderSetup.CurrencySY = oReader.GetString("CurrencySY");
            if (String.IsNullOrEmpty(oFDOrderSetup.ShortName))
            { oFDOrderSetup.ShortName = oFDOrderSetup.FDOName; }
            
            return oFDOrderSetup;
        }

        private FDOrderSetup CreateObject(NullHandler oReader)
        {
            FDOrderSetup oFDOrderSetup = new FDOrderSetup();
            oFDOrderSetup = MapObject(oReader);
            return oFDOrderSetup;
        }

        private List<FDOrderSetup> CreateObjects(IDataReader oReader)
        {
            List<FDOrderSetup> oFDOrderSetups = new List<FDOrderSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FDOrderSetup oItem = CreateObject(oHandler);
                oFDOrderSetups.Add(oItem);
            }
            return oFDOrderSetups;
        }

        #endregion

        #region Interface implementation
        public FDOrderSetupService() { }


        public FDOrderSetup Save(FDOrderSetup oFDOrderSetup, Int64 nUserId)
        {

            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);
                #region FDOrderSetup
                IDataReader reader;
                if (oFDOrderSetup.FDOrderSetupID <= 0)
                {
                    reader = FDOrderSetupDA.InsertUpdate(tc, oFDOrderSetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = FDOrderSetupDA.InsertUpdate(tc, oFDOrderSetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDOrderSetup = new FDOrderSetup();
                    oFDOrderSetup = CreateObject(oReader);
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
                oFDOrderSetup = new FDOrderSetup();
                oFDOrderSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFDOrderSetup;
        }
      
        public String Delete(FDOrderSetup oFDOrderSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FDOrderSetupDA.Delete(tc, oFDOrderSetup, EnumDBOperation.Delete, nUserID);
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
        public FDOrderSetup Get(int id, Int64 nUserId)
        {
            FDOrderSetup oFDOrderSetup = new FDOrderSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FDOrderSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDOrderSetup = CreateObject(oReader);
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

            return oFDOrderSetup;
        }

        public List<FDOrderSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<FDOrderSetup> oFDOrderSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FDOrderSetupDA.Gets(sSQL, tc);
                oFDOrderSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FDOrderSetup", e);
                #endregion
            }
            return oFDOrderSetup;
        }
        public FDOrderSetup GetByType(int nOrderType, Int64 nUserId)
        {
            FDOrderSetup oFDOrderSetup = new FDOrderSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FDOrderSetupDA.GetByType(tc, nOrderType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDOrderSetup = CreateObject(oReader);
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

            return oFDOrderSetup;
        }
     

        public List<FDOrderSetup> Gets(Int64 nUserId)
        {
            List<FDOrderSetup> oFDOrderSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FDOrderSetupDA.Gets(tc);
                oFDOrderSetups = CreateObjects(reader);
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

            return oFDOrderSetups;
        }
        public List<FDOrderSetup> GetByOrderTypes(int nBUID,bool bIsInHouse, string sOrderType, Int64 nUserId)
        {
            List<FDOrderSetup> oFDOrderSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FDOrderSetupDA.GetByOrderTypes(tc,nBUID,  bIsInHouse,  sOrderType);
                oFDOrderSetups = CreateObjects(reader);
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

            return oFDOrderSetups;
        }
        public List<FDOrderSetup> GetsActive(  int nBUID,Int64 nUserId)
        {
            List<FDOrderSetup> oFDOrderSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FDOrderSetupDA.GetsActive(tc,  nBUID);
                oFDOrderSetups = CreateObjects(reader);
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

            return oFDOrderSetups;
        }

        public FDOrderSetup Activate(FDOrderSetup oFDOrderSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FDOrderSetupDA.Activate(tc, oFDOrderSetup);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDOrderSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFDOrderSetup = new FDOrderSetup();
                oFDOrderSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFDOrderSetup;
        }
    

        #endregion
    }
}