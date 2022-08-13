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
    public class FNQCResultService : MarshalByRefObject, IFNQCResultService
    {
        #region Private functions and declaration
        private FNQCResult MapObject(NullHandler oReader)
        {
            FNQCResult oFNQCResult = new FNQCResult();
            oFNQCResult.FNQCResultID = oReader.GetInt32("FNQCResultID");
            oFNQCResult.FNQCParameterID = oReader.GetInt32("FNQCParameterID");
            oFNQCResult.FNTPID = oReader.GetInt32("FNTPID");
            oFNQCResult.FNPBatchID = oReader.GetInt32("FNPBatchID");
            oFNQCResult.SubName = oReader.GetString("SubName");
            oFNQCResult.Value = oReader.GetString("Value");
            oFNQCResult.ValueResult = oReader.GetString("ValueResult");
            oFNQCResult.Note = oReader.GetString("Note");
            oFNQCResult.Name = oReader.GetString("Name");
            oFNQCResult.TestMethod = oReader.GetString("TestMethod");
            oFNQCResult.Code = oReader.GetInt32("Code");
            oFNQCResult.DBUserName = oReader.GetString("DBUserName");
            oFNQCResult.LastUpdateByName = oReader.GetString("LastUpdateByName");
            oFNQCResult.DBUserID = oReader.GetInt32("DBUserID");
            oFNQCResult.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oFNQCResult.SLNo = oReader.GetInt32("SLNo");
            oFNQCResult.FnQCTestGroupID = oReader.GetInt32("FnQCTestGroupID");
            oFNQCResult.FnQCTestGroupName = oReader.GetString("FnQCTestGroupName");

            return oFNQCResult;
        }
        private FNQCResult CreateObject(NullHandler oReader)
        {
            FNQCResult oFNQCResult = new FNQCResult();
            oFNQCResult = MapObject(oReader);
            return oFNQCResult;
        }
        private List<FNQCResult> CreateObjects(IDataReader oReader)
        {
            List<FNQCResult> oFNQCResult = new List<FNQCResult>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNQCResult oItem = CreateObject(oHandler);
                oFNQCResult.Add(oItem);
            }
            return oFNQCResult;
        }

        #endregion

        #region Interface implementation
        public FNQCResultService() { }
        public FNQCResult Save(FNQCResult oFNQCResult, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFNQCResult.FNQCResultID <= 0)
                {
                    reader = FNQCResultDA.InsertUpdate(tc, oFNQCResult, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FNQCResultDA.InsertUpdate(tc, oFNQCResult, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNQCResult = new FNQCResult();
                    oFNQCResult = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save FNQCResult. Because of " + e.Message, e);
                #endregion
            }
            return oFNQCResult;
        }
        public List<FNQCResult> SaveAll(List<FNQCResult> oFNQCResults, Int64 nUserID)
        {
            List<FNQCResult> oTempFNQCResults = new List<FNQCResult>();
            FNQCResult oTempFNQCResult = new FNQCResult();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                foreach (FNQCResult oItem in oFNQCResults)
                {
                    if (oItem.FNQCResultID <= 0)
                    {
                        reader = FNQCResultDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = FNQCResultDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }

                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTempFNQCResult = new FNQCResult();
                        oTempFNQCResult = CreateObject(oReader);
                        oTempFNQCResults.Add(oTempFNQCResult);
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
                throw new ServiceException("Failed to Save FNQCResult. Because of " + e.Message, e);
                #endregion
            }
            return oTempFNQCResults;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FNQCResult oFNQCResult = new FNQCResult();
                oFNQCResult.FNQCResultID = id;
                FNQCResultDA.Delete(tc, oFNQCResult, EnumDBOperation.Delete, nUserId);
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
        public FNQCResult Get(int id, Int64 nUserId)
        {
            FNQCResult oFNQCResult = new FNQCResult();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FNQCResultDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNQCResult = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FNQCResult", e);
                #endregion
            }
            return oFNQCResult;
        }
        public List<FNQCResult> Gets(Int64 nUserID)
        {
            List<FNQCResult> oFNQCResults = new List<FNQCResult>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNQCResultDA.Gets(tc);
                oFNQCResults = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNQCResult", e);
                #endregion
            }
            return oFNQCResults;
        }
        public List<FNQCResult> Gets(string sSQL,Int64 nUserID)
        {
            List<FNQCResult> oFNQCResults = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNQCResultDA.Gets(tc,sSQL);
                oFNQCResults = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNQCResult", e);
                #endregion
            }
            return oFNQCResults;
        }
        #endregion
    }   
}


