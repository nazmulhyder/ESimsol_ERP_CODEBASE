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
    public class FabricSeekingDateService : MarshalByRefObject, IFabricSeekingDateService
    {
        #region Private functions and declaration
        private FabricSeekingDate MapObject(NullHandler oReader)
        {
            FabricSeekingDate oFabricSeekingDate = new FabricSeekingDate();
            oFabricSeekingDate.FabricID = oReader.GetInt32("FabricID");
            oFabricSeekingDate.FabricRequestTypeInt = oReader.GetInt32("FabricRequestType");
            oFabricSeekingDate.FabricRequestType = (EnumFabricRequestType)oReader.GetInt32("FabricRequestType");
            oFabricSeekingDate.SeekingDate = oReader.GetDateTime("SeekingDate");
            oFabricSeekingDate.NoOfSets = oReader.GetInt32("NoOfSets");
            return oFabricSeekingDate;
        }
        private FabricSeekingDate CreateObject(NullHandler oReader)
        {
            FabricSeekingDate oFabricSeekingDate = new FabricSeekingDate();
            oFabricSeekingDate = MapObject(oReader);
            return oFabricSeekingDate;
        }
        private List<FabricSeekingDate> CreateObjects(IDataReader oReader)
        {
            List<FabricSeekingDate> oFabricSeekingDate = new List<FabricSeekingDate>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricSeekingDate oItem = CreateObject(oHandler);
                oFabricSeekingDate.Add(oItem);
            }
            return oFabricSeekingDate;
        }
        #endregion

        #region Interface implementation
        public FabricSeekingDate Save(FabricSeekingDate oFabricSeekingDate, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
               
               reader = FabricSeekingDateDA.InsertUpdate(tc, oFabricSeekingDate, EnumDBOperation.Insert, nUserID);
               
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSeekingDate = new FabricSeekingDate();
                    oFabricSeekingDate = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricSeekingDate = new FabricSeekingDate();
                oFabricSeekingDate.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricSeekingDate;
        }
        public string Delete(FabricSeekingDate oFabricSeekingDate, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricSeekingDateDA.Delete(tc, oFabricSeekingDate, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public FabricSeekingDate Get(int id, Int64 nUserId)
        {
            FabricSeekingDate oAccountHead = new FabricSeekingDate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricSeekingDateDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricSeekingDate", e);
                #endregion
            }

            return oAccountHead;
        }
      
        public List<FabricSeekingDate> Gets(int nFabricID, Int64 nUserID)
        {
            List<FabricSeekingDate> oFabricSeekingDate = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricSeekingDateDA.Gets(tc, nFabricID);
                oFabricSeekingDate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricSeekingDate", e);
                #endregion
            }
            return oFabricSeekingDate;
        }
           
        public List<FabricSeekingDate> Gets (string sSQL, Int64 nUserID)
        {
            List<FabricSeekingDate> oFabricSeekingDate = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricSeekingDateDA.Gets(tc, sSQL);
                oFabricSeekingDate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricSeekingDate", e);
                #endregion
            }
            return oFabricSeekingDate;
        }
        
        #endregion
    }
}
