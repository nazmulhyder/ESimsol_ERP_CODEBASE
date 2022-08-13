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
    public class FNQCParameterService : MarshalByRefObject, IFNQCParameterService
    {
        #region Private functions and declaration
        private FNQCParameter MapObject(NullHandler oReader)
        {
            FNQCParameter oFNQCParameter = new FNQCParameter();
            oFNQCParameter.FNQCParameterID = oReader.GetInt32("FNQCParameterID");
            oFNQCParameter.Code = oReader.GetInt32("Code");
            oFNQCParameter.Name= oReader.GetString("Name");
            oFNQCParameter.DBUserName = oReader.GetString("DBUserName");
            oFNQCParameter.LastUpdateByName = oReader.GetString("LastUpdateByName");
            oFNQCParameter.DBUserID = oReader.GetInt32("DBUserID");
            oFNQCParameter.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oFNQCParameter.FnQCTestGroupID = oReader.GetInt32("FnQCTestGroupID");
            oFNQCParameter.FnQCTestGroupName = oReader.GetString("FnQCTestGroupName");

            return oFNQCParameter; 
        }
        private FNQCParameter CreateObject(NullHandler oReader)
        {
            FNQCParameter oFNQCParameter = new FNQCParameter();
            oFNQCParameter = MapObject(oReader);
            return oFNQCParameter;
        }
        private List<FNQCParameter> CreateObjects(IDataReader oReader)
        {
            List<FNQCParameter> oFNQCParameter = new List<FNQCParameter>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNQCParameter oItem = CreateObject(oHandler);
                oFNQCParameter.Add(oItem);
            }
            return oFNQCParameter;
        }

        #endregion

        #region Interface implementation
        public FNQCParameterService() { }
        public FNQCParameter Save(FNQCParameter oFNQCParameter, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFNQCParameter.FNQCParameterID <= 0)
                {
                    reader = FNQCParameterDA.InsertUpdate(tc, oFNQCParameter, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FNQCParameterDA.InsertUpdate(tc, oFNQCParameter, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNQCParameter = new FNQCParameter();
                    oFNQCParameter = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save FNQCParameter. Because of " + e.Message, e);
                #endregion
            }
            return oFNQCParameter;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FNQCParameter oFNQCParameter = new FNQCParameter();
                oFNQCParameter.FNQCParameterID = id;
                FNQCParameterDA.Delete(tc, oFNQCParameter, EnumDBOperation.Delete, nUserId);
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
        public FNQCParameter Get(int id, Int64 nUserId)
        {
            FNQCParameter oFNQCParameter = new FNQCParameter();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FNQCParameterDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNQCParameter = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FNQCParameter", e);
                #endregion
            }
            return oFNQCParameter;
        }
        public List<FNQCParameter> Gets(Int64 nUserID)
        {
            List<FNQCParameter> oFNQCParameters = new List<FNQCParameter>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNQCParameterDA.Gets(tc);
                oFNQCParameters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNQCParameter", e);
                #endregion
            }
            return oFNQCParameters;
        }
        public List<FNQCParameter> Gets(string sSQL,Int64 nUserID)
        {
            List<FNQCParameter> oFNQCParameters = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNQCParameterDA.Gets(tc,sSQL);
                oFNQCParameters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNQCParameter", e);
                #endregion
            }
            return oFNQCParameters;
        }
        #endregion
    }   
}
