using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class FabricBatchQCCheckService : MarshalByRefObject, IFabricBatchQCCheckService
    {
        #region Private functions and declaration
        private FabricBatchQCCheck MapObject(NullHandler oReader)
        {
            FabricBatchQCCheck oFabricBatchQCCheck = new FabricBatchQCCheck();
            oFabricBatchQCCheck.FabricBatchQCCheckID = oReader.GetInt32("FabricBatchQCCheckID");
            oFabricBatchQCCheck.FabricQCParNameID = oReader.GetInt32("FabricQCParNameID");
            oFabricBatchQCCheck.FabricBatchQCID = oReader.GetInt32("FabricBatchQCID");
            oFabricBatchQCCheck.Note = oReader.GetString("Note");
            oFabricBatchQCCheck.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oFabricBatchQCCheck.LastUpdateDateTime = Convert.ToDateTime(oReader.GetDateTime("LastUpdateDateTime"));
            oFabricBatchQCCheck.LastUpdateByName = oReader.GetString("LastUpdateByName");
            oFabricBatchQCCheck.Name = oReader.GetString("Name");
            return oFabricBatchQCCheck;
        }
        private FabricBatchQCCheck CreateObject(NullHandler oReader)
        {
            FabricBatchQCCheck oFabricBatchQCCheck = new FabricBatchQCCheck();
            oFabricBatchQCCheck = MapObject(oReader);
            return oFabricBatchQCCheck;
        }
        private List<FabricBatchQCCheck> CreateObjects(IDataReader oReader)
        {
            List<FabricBatchQCCheck> oFabricBatchQCCheck = new List<FabricBatchQCCheck>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBatchQCCheck oItem = CreateObject(oHandler);
                oFabricBatchQCCheck.Add(oItem);
            }
            return oFabricBatchQCCheck;
        }
        #endregion
        #region Interface implementation
        public FabricBatchQCCheck Save(FabricBatchQCCheck oFabricBatchQCCheck, Int64 nUserID)
        {
            string sRefChildIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricBatchQCCheck.FabricBatchQCCheckID <= 0)
                {

                    reader = FabricBatchQCCheckDA.InsertUpdate(tc, oFabricBatchQCCheck, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricBatchQCCheckDA.InsertUpdate(tc, oFabricBatchQCCheck, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchQCCheck = new FabricBatchQCCheck();
                    oFabricBatchQCCheck = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oFabricBatchQCCheck = new FabricBatchQCCheck();
                    oFabricBatchQCCheck.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricBatchQCCheck;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchQCCheck oFabricBatchQCCheck = new FabricBatchQCCheck();
                oFabricBatchQCCheck.FabricBatchQCCheckID = id;
                DBTableReferenceDA.HasReference(tc, "FabricBatchQCCheck", id);
                FabricBatchQCCheckDA.Delete(tc, oFabricBatchQCCheck, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public FabricBatchQCCheck Get(int id, Int64 nUserId)
        {
            FabricBatchQCCheck oFabricBatchQCCheck = new FabricBatchQCCheck();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricBatchQCCheckDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchQCCheck = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricBatchQCCheck", e);
                #endregion
            }
            return oFabricBatchQCCheck;
        }
        public List<FabricBatchQCCheck> Gets(Int64 nUserID)
        {
            List<FabricBatchQCCheck> oFabricBatchQCChecks = new List<FabricBatchQCCheck>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricBatchQCCheckDA.Gets(tc);
                oFabricBatchQCChecks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricBatchQCCheck oFabricBatchQCCheck = new FabricBatchQCCheck();
                oFabricBatchQCCheck.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatchQCChecks;
        }
        public List<FabricBatchQCCheck> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricBatchQCCheck> oFabricBatchQCChecks = new List<FabricBatchQCCheck>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricBatchQCCheckDA.Gets(tc, sSQL);
                oFabricBatchQCChecks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricBatchQCCheck", e);
                #endregion
            }
            return oFabricBatchQCChecks;
        }

        public List<FabricBatchQCCheck> SaveList(List<FabricBatchQCCheck> oFabricBatchQCChecks, Int64 nUserID)
        {
            FabricBatchQCCheck oFabricBatchQCCheck = new FabricBatchQCCheck();
            List<FabricBatchQCCheck> oReturnFabricBatchQCChecks = new List<FabricBatchQCCheck>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (FabricBatchQCCheck oTempFabricBatchQCCheck in oFabricBatchQCChecks)
                {
                    #region FabricBatchQCCheck Part
                    IDataReader reader;
                    if (oTempFabricBatchQCCheck.FabricBatchQCCheckID <= 0)
                    {

                        reader = FabricBatchQCCheckDA.InsertUpdate(tc, oTempFabricBatchQCCheck, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {

                        reader = FabricBatchQCCheckDA.InsertUpdate(tc, oTempFabricBatchQCCheck, EnumDBOperation.Update, nUserID);

                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricBatchQCCheck = new FabricBatchQCCheck();
                        oFabricBatchQCCheck = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion
                    oReturnFabricBatchQCChecks.Add(oFabricBatchQCCheck);
                }
                tc.End();

            }
            catch (Exception e)
            {
                if (tc != null)
                {
                    tc.HandleError();
                    oFabricBatchQCCheck = new FabricBatchQCCheck();
                    oFabricBatchQCCheck.ErrorMessage = e.Message;
                    oReturnFabricBatchQCChecks = new List<FabricBatchQCCheck>();
                    oReturnFabricBatchQCChecks.Add(oFabricBatchQCCheck);
                }
            }
            return oReturnFabricBatchQCChecks;
        }

        #endregion
    }
}
