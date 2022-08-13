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
    public class FabricProcessService : MarshalByRefObject, IFabricProcessService
    {
        #region Private functions and declaration
        private FabricProcess MapObject(NullHandler oReader)
        {
            FabricProcess oFabricProcess = new FabricProcess();
            oFabricProcess.FabricProcessID = oReader.GetInt32("FabricProcessID");
            oFabricProcess.ProcessType = (EnumFabricProcess)oReader.GetInt32("ProcessType");
            oFabricProcess.Name = oReader.GetString("Name");
            oFabricProcess.IsYarnDyed = oReader.GetBoolean("IsYarnDyed");
            return oFabricProcess;
        }
        private FabricProcess CreateObject(NullHandler oReader)
        {
            FabricProcess oFabricProcess = new FabricProcess();
            oFabricProcess = MapObject(oReader);
            return oFabricProcess;
        }
        private List<FabricProcess> CreateObjects(IDataReader oReader)
        {
            List<FabricProcess> oFabricProcess = new List<FabricProcess>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricProcess oItem = CreateObject(oHandler);
                oFabricProcess.Add(oItem);
            }
            return oFabricProcess;
        }

        #endregion

        #region Interface implementation
        public FabricProcess Save(FabricProcess oFabricProcess, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricProcess.FabricProcessID <= 0)
                {
                    reader = FabricProcessDA.InsertUpdate(tc, oFabricProcess, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricProcessDA.InsertUpdate(tc, oFabricProcess, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricProcess = new FabricProcess();
                    oFabricProcess = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricProcess = new FabricProcess();
                oFabricProcess.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricProcess;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricProcess oFabricProcess = new FabricProcess();
                oFabricProcess.FabricProcessID = id;
                DBTableReferenceDA.HasReference(tc, "FabricProcess", id);
                FabricProcessDA.Delete(tc, oFabricProcess, EnumDBOperation.Delete, nUserId);
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

        public List<FabricProcess> GetsByFabricNameType(string sName, string eFabricType, int nUserId)
        {
            List<FabricProcess> oFabricProcesss = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricProcessDA.GetsByFabricNameType(tc, sName, eFabricType);
                oFabricProcesss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Fabrics", e);
                #endregion
            }

            return oFabricProcesss;
        }

        public List<FabricProcess> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricProcessDA.Gets(tc, sSQL);
                oFabricProcesss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricProcesss = new List<FabricProcess>();
                FabricProcess oFabricProcess = new FabricProcess();
                oFabricProcess.ErrorMessage = e.Message.Split('~')[0];
                oFabricProcesss.Add(oFabricProcess);
                #endregion
            }
            return oFabricProcesss;
        }
        public List<FabricProcess> Gets(Int64 nUserID)
        {
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricProcessDA.Gets(tc);
                oFabricProcesss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricProcesss = new List<FabricProcess>();
                FabricProcess oFabricProcess = new FabricProcess();
                oFabricProcess.ErrorMessage = e.Message.Split('~')[0];
                oFabricProcesss.Add(oFabricProcess);
                #endregion
            }
            return oFabricProcesss;
        }
        public FabricProcess Get(int id, Int64 nUserId)
        {
            FabricProcess oFabricProcess = new FabricProcess();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricProcessDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricProcess = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricProcess = new FabricProcess();
                oFabricProcess.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricProcess;
        }

        #endregion
    }
}
