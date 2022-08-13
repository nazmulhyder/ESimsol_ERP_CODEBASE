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
    public class FNTreatmentSubProcessService : MarshalByRefObject, IFNTreatmentSubProcessService
    {
        #region Private functions and declaration

        private FNTreatmentSubProcess MapObject(NullHandler oReader)
        {
            FNTreatmentSubProcess oFNTreatmentSubProcess = new FNTreatmentSubProcess();
            oFNTreatmentSubProcess.FNTreatmentSubProcessID = oReader.GetInt32("FNTreatmentSubProcessID");
            oFNTreatmentSubProcess.FNTPID = oReader.GetInt32("FNTPID");
            oFNTreatmentSubProcess.SubProcessName = oReader.GetString("SubProcessName");
            oFNTreatmentSubProcess.FNProcess = oReader.GetString("FNProcess");
            oFNTreatmentSubProcess.Code = oReader.GetString("Code");

            return oFNTreatmentSubProcess;
        }

        private FNTreatmentSubProcess CreateObject(NullHandler oReader)
        {
            FNTreatmentSubProcess oFNTreatmentSubProcess = new FNTreatmentSubProcess();
            oFNTreatmentSubProcess = MapObject(oReader);
            return oFNTreatmentSubProcess;
        }

        private List<FNTreatmentSubProcess> CreateObjects(IDataReader oReader)
        {
            List<FNTreatmentSubProcess> oFNTreatmentSubProcess = new List<FNTreatmentSubProcess>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNTreatmentSubProcess oItem = CreateObject(oHandler);
                oFNTreatmentSubProcess.Add(oItem);
            }
            return oFNTreatmentSubProcess;
        }

        #endregion

        #region Interface implementation
        public FNTreatmentSubProcess Save(FNTreatmentSubProcess oFNTreatmentSubProcess, Int64 nUserID)
        {
            TransactionContext tc = null;
            oFNTreatmentSubProcess.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFNTreatmentSubProcess.FNTreatmentSubProcessID <= 0)
                {
                    reader = FNTreatmentSubProcessDA.InsertUpdate(tc, oFNTreatmentSubProcess, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FNTreatmentSubProcessDA.InsertUpdate(tc, oFNTreatmentSubProcess, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNTreatmentSubProcess = new FNTreatmentSubProcess();
                    oFNTreatmentSubProcess = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFNTreatmentSubProcess.ErrorMessage = e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save FNTreatmentSubProcess. Because of " + e.Message, e);
                #endregion
            }
            return oFNTreatmentSubProcess;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FNTreatmentSubProcess oFNTreatmentSubProcess = new FNTreatmentSubProcess();
                oFNTreatmentSubProcess.FNTreatmentSubProcessID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.FNTreatmentSubProcess, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FNTreatmentSubProcess", id);
                FNTreatmentSubProcessDA.Delete(tc, oFNTreatmentSubProcess, EnumDBOperation.Delete, nUserId);
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

        public FNTreatmentSubProcess Get(int id, Int64 nUserId)
        {
            FNTreatmentSubProcess oFNTreatmentSubProcess = new FNTreatmentSubProcess();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FNTreatmentSubProcessDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNTreatmentSubProcess = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FNTreatmentSubProcess", e);
                #endregion
            }
            return oFNTreatmentSubProcess;
        }

        public List<FNTreatmentSubProcess> Gets(Int64 nUserID)
        {
            List<FNTreatmentSubProcess> oFNTreatmentSubProcesss = new List<FNTreatmentSubProcess>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FNTreatmentSubProcessDA.Gets(tc);
                oFNTreatmentSubProcesss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FNTreatmentSubProcess oFNTreatmentSubProcess = new FNTreatmentSubProcess();
                oFNTreatmentSubProcess.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFNTreatmentSubProcesss;
        }

        public List<FNTreatmentSubProcess> Gets(string sSQL, Int64 nUserID)
        {
            List<FNTreatmentSubProcess> oFNTreatmentSubProcesss = new List<FNTreatmentSubProcess>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FNTreatmentSubProcessDA.Gets(tc, sSQL);
                oFNTreatmentSubProcesss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FNTreatmentSubProcess", e);
                #endregion
            }
            return oFNTreatmentSubProcesss;
        }

        #endregion
    }

}
