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
    public class DUStepWiseSetupService : MarshalByRefObject, IDUStepWiseSetupService
    {
        #region Private functions and declaration
        private DUStepWiseSetup MapObject(NullHandler oReader)
        {
            DUStepWiseSetup oDUStepWiseSetup = new DUStepWiseSetup();
            oDUStepWiseSetup.DUStepWiseSetupID = oReader.GetInt32("DUStepWiseSetupID");
            oDUStepWiseSetup.DyeingStepType = oReader.GetInt32("DyeingStepType");
            oDUStepWiseSetup.DUOrderSetupID = oReader.GetInt32("DUOrderSetupID");
            oDUStepWiseSetup.Note = oReader.GetString("Note");
            return oDUStepWiseSetup;
        }

        private DUStepWiseSetup CreateObject(NullHandler oReader)
        {
            DUStepWiseSetup oDUStepWiseSetup = new DUStepWiseSetup();
            oDUStepWiseSetup = MapObject(oReader);
            return oDUStepWiseSetup;
        }

        private List<DUStepWiseSetup> CreateObjects(IDataReader oReader)
        {
            List<DUStepWiseSetup> oDUStepWiseSetups = new List<DUStepWiseSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUStepWiseSetup oItem = CreateObject(oHandler);
                oDUStepWiseSetups.Add(oItem);
            }
            return oDUStepWiseSetups;
        }

        #endregion

        #region Interface implementation
        public DUStepWiseSetupService() { }
        public DUStepWiseSetup Save(DUStepWiseSetup oDUStepWiseSetup, Int64 nUserId)
        {

            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);
                #region DUStepWiseSetup
                IDataReader reader;
                if (oDUStepWiseSetup.DUStepWiseSetupID <= 0)
                {
                    reader = DUStepWiseSetupDA.InsertUpdate(tc, oDUStepWiseSetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = DUStepWiseSetupDA.InsertUpdate(tc, oDUStepWiseSetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUStepWiseSetup = new DUStepWiseSetup();
                    oDUStepWiseSetup = CreateObject(oReader);
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
                oDUStepWiseSetup = new DUStepWiseSetup();
                oDUStepWiseSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUStepWiseSetup;
        }
        public String Delete(DUStepWiseSetup oDUStepWiseSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DUStepWiseSetupDA.Delete(tc, oDUStepWiseSetup, EnumDBOperation.Delete, nUserID);
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
        public DUStepWiseSetup Get(int id, Int64 nUserId)
        {
            DUStepWiseSetup oDUStepWiseSetup = new DUStepWiseSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUStepWiseSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUStepWiseSetup = CreateObject(oReader);
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

            return oDUStepWiseSetup;
        }
        public DUStepWiseSetup GetBy(int nDyeingStepType, Int64 nUserId)
        {
            DUStepWiseSetup oDUStepWiseSetup = new DUStepWiseSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUStepWiseSetupDA.GetBy(tc, nDyeingStepType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUStepWiseSetup = CreateObject(oReader);
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

            return oDUStepWiseSetup;
        }
        public List<DUStepWiseSetup> Gets(Int64 nUserId)
        {
            List<DUStepWiseSetup> oDUStepWiseSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUStepWiseSetupDA.Gets(tc);
                oDUStepWiseSetups = CreateObjects(reader);
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

            return oDUStepWiseSetups;
        }
        public List<DUStepWiseSetup> Gets(string sSQL,Int64 nUserId)
        {
            List<DUStepWiseSetup> oDUStepWiseSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUStepWiseSetupDA.Gets(tc, sSQL);
                oDUStepWiseSetups = CreateObjects(reader);
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

            return oDUStepWiseSetups;
        }
     
        #endregion
    }
}