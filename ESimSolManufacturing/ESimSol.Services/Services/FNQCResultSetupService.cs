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
    public class FNQCResultSetupService : MarshalByRefObject, IFNQCResultSetupService
    {
        #region Private functions and declaration
        private FNQCResultSetup MapObject(NullHandler oReader)
        {
            FNQCResultSetup oFNQCResultSetup = new FNQCResultSetup();
            oFNQCResultSetup.FNQCResultSetupID = oReader.GetInt32("FNQCResultSetupID");
            oFNQCResultSetup.FNQCParameterID = oReader.GetInt32("FNQCParameterID");
            oFNQCResultSetup.FNTPID = oReader.GetInt32("FNTPID");
            oFNQCResultSetup.SubName = oReader.GetString("SubName");
            oFNQCResultSetup.TestMethod = oReader.GetString("TestMethod");
            oFNQCResultSetup.Value = oReader.GetString("Value");
            oFNQCResultSetup.Note = oReader.GetString("Note");
            oFNQCResultSetup.Name = oReader.GetString("Name");
            oFNQCResultSetup.Code = oReader.GetInt32("Code");
            oFNQCResultSetup.DBUserName = oReader.GetString("DBUserName");
            oFNQCResultSetup.LastUpdateByName = oReader.GetString("LastUpdateByName");
            oFNQCResultSetup.DBUserID = oReader.GetInt32("DBUserID");
            oFNQCResultSetup.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oFNQCResultSetup.SLNo = oReader.GetInt32("SLNo");
            oFNQCResultSetup.FnQCTestGroupID = oReader.GetInt32("FnQCTestGroupID");
            oFNQCResultSetup.FnQCTestGroupName = oReader.GetString("FnQCTestGroupName");

            return oFNQCResultSetup;
        }
        private FNQCResultSetup CreateObject(NullHandler oReader)
        {
            FNQCResultSetup oFNQCResultSetup = new FNQCResultSetup();
            oFNQCResultSetup = MapObject(oReader);
            return oFNQCResultSetup;
        }
        private List<FNQCResultSetup> CreateObjects(IDataReader oReader)
        {
            List<FNQCResultSetup> oFNQCResultSetup = new List<FNQCResultSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNQCResultSetup oItem = CreateObject(oHandler);
                oFNQCResultSetup.Add(oItem);
            }
            return oFNQCResultSetup;
        }

        #endregion

        #region Interface implementation
        public FNQCResultSetupService() { }
        public FNQCResultSetup Save(FNQCResultSetup oFNQCResultSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFNQCResultSetup.FNQCResultSetupID <= 0)
                {
                    reader = FNQCResultSetupDA.InsertUpdate(tc, oFNQCResultSetup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FNQCResultSetupDA.InsertUpdate(tc, oFNQCResultSetup, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNQCResultSetup = new FNQCResultSetup();
                    oFNQCResultSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save FNQCResultSetup. Because of " + e.Message, e);
                #endregion
            }
            return oFNQCResultSetup;
        }
        public List<FNQCResultSetup> SaveAll(List<FNQCResultSetup> oFNQCResultSetups, Int64 nUserID)
        {
            List<FNQCResultSetup> oTempFNQCResultSetups = new List<FNQCResultSetup>();
            FNQCResultSetup oTempFNQCResultSetup = new FNQCResultSetup();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                foreach (FNQCResultSetup oItem in oFNQCResultSetups)
                {
                    if (oItem.FNQCResultSetupID <= 0)
                    {
                        reader = FNQCResultSetupDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = FNQCResultSetupDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }

                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTempFNQCResultSetup = new FNQCResultSetup();
                        oTempFNQCResultSetup = CreateObject(oReader);
                        oTempFNQCResultSetups.Add(oTempFNQCResultSetup);
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save FNQCResultSetup. Because of " + e.Message, e);
                #endregion
            }
            return oTempFNQCResultSetups;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FNQCResultSetup oFNQCResultSetup = new FNQCResultSetup();
                oFNQCResultSetup.FNQCResultSetupID = id;
                FNQCResultSetupDA.Delete(tc, oFNQCResultSetup, EnumDBOperation.Delete, nUserId);
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
        public FNQCResultSetup Get(int id, Int64 nUserId)
        {
            FNQCResultSetup oFNQCResultSetup = new FNQCResultSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FNQCResultSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNQCResultSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FNQCResultSetup", e);
                #endregion
            }
            return oFNQCResultSetup;
        }
        public List<FNQCResultSetup> Gets(Int64 nUserID)
        {
            List<FNQCResultSetup> oFNQCResultSetups = new List<FNQCResultSetup>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNQCResultSetupDA.Gets(tc);
                oFNQCResultSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNQCResultSetup", e);
                #endregion
            }
            return oFNQCResultSetups;
        }
        public List<FNQCResultSetup> Gets(string sSQL,Int64 nUserID)
        {
            List<FNQCResultSetup> oFNQCResultSetups = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNQCResultSetupDA.Gets(tc,sSQL);
                oFNQCResultSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNQCResultSetup", e);
                #endregion
            }
            return oFNQCResultSetups;
        }
        #endregion
    }   
}

