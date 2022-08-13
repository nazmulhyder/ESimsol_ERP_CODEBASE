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
    public class FabricDispoService : MarshalByRefObject, IFabricDispoService
    {
        #region Private functions and declaration
        private FabricDispo MapObject(NullHandler oReader)
        {
            FabricDispo oFabricDispo = new FabricDispo();
            oFabricDispo.FabricDispoID = oReader.GetInt32("FabricDispoID");
            oFabricDispo.Code = oReader.GetString("Code");
            oFabricDispo.FabricOrderType = (EnumFabricRequestType)oReader.GetInt32("FabricOrderType");
            oFabricDispo.BusinessUnitType = (EnumBusinessUnitType)oReader.GetInt32("BusinessUnitType");
            oFabricDispo.IsReProduction = oReader.GetBoolean("IsReProduction");
            oFabricDispo.IsYD = oReader.GetBoolean("IsYD");
            oFabricDispo.CodeLength = oReader.GetInt32("CodeLength");
            return oFabricDispo;
        }
        private FabricDispo CreateObject(NullHandler oReader)
        {
            FabricDispo oFabricDispo = new FabricDispo();
            oFabricDispo = MapObject(oReader);
            return oFabricDispo;
        }
        private List<FabricDispo> CreateObjects(IDataReader oReader)
        {
            List<FabricDispo> oFabricDispo = new List<FabricDispo>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricDispo oItem = CreateObject(oHandler);
                oFabricDispo.Add(oItem);
            }
            return oFabricDispo;
        }
        #endregion

        #region Interface implementation
        public FabricDispoService() { }
        public FabricDispo Save(FabricDispo oFabricDispo, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricDispo.FabricDispoID <= 0)
                {
                    reader = FabricDispoDA.InsertUpdate(tc, oFabricDispo, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricDispoDA.InsertUpdate(tc, oFabricDispo, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricDispo = new FabricDispo();
                    oFabricDispo = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save FabricDispo. Because of " + e.Message, e);
                #endregion
            }
            return oFabricDispo;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricDispo oFabricDispo = new FabricDispo();
                oFabricDispo.FabricDispoID = id;
                FabricDispoDA.Delete(tc, oFabricDispo, EnumDBOperation.Delete, nUserId);
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

        public FabricDispo Get(int id, Int64 nUserId)
        {
            FabricDispo oFabricDispo = new FabricDispo();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricDispoDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricDispo = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricDispo", e);
                #endregion
            }
            return oFabricDispo;
        }
        public List<FabricDispo> Gets(Int64 nUserID)
        {
            List<FabricDispo> oFabricDispos = new List<FabricDispo>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricDispoDA.Gets(tc);
                oFabricDispos = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricDispo", e);
                #endregion
            }
            return oFabricDispos;
        }
        public List<FabricDispo> Gets(string sSQL,Int64 nUserID)
        {
            List<FabricDispo> oFabricDispos = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricDispoDA.Gets(tc,sSQL);
                oFabricDispos = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricDispo", e);
                #endregion
            }
            return oFabricDispos;
        }
        #endregion
    }   
}
