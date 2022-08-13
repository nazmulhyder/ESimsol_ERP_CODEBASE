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
    public class DURequisitionSetupService : MarshalByRefObject, IDURequisitionSetupService
    {
        #region Private functions and declaration
        private DURequisitionSetup MapObject(NullHandler oReader)
        {
            DURequisitionSetup oDURequisitionSetup = new DURequisitionSetup();
            oDURequisitionSetup.DURequisitionSetupID = oReader.GetInt32("DURequisitionSetupID");
            oDURequisitionSetup.Name = oReader.GetString("Name");
            oDURequisitionSetup.ShortName = oReader.GetString("ShortName");
            oDURequisitionSetup.InOutType = (EnumInOutType)(oReader.GetInt32("InOutType"));
            oDURequisitionSetup.WorkingUnitID_Issue = oReader.GetInt32("WorkingUnitID_Issue");
            oDURequisitionSetup.WorkingUnitID_Receive = oReader.GetInt32("WorkingUnitID_Receive");
            oDURequisitionSetup.Activity = oReader.GetBoolean("Activity");
            return oDURequisitionSetup;
        }

        private DURequisitionSetup CreateObject(NullHandler oReader)
        {
            DURequisitionSetup oDURequisitionSetup = new DURequisitionSetup();
            oDURequisitionSetup = MapObject(oReader);
            return oDURequisitionSetup;
        }

        private List<DURequisitionSetup> CreateObjects(IDataReader oReader)
        {
            List<DURequisitionSetup> oDURequisitionSetups = new List<DURequisitionSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DURequisitionSetup oItem = CreateObject(oHandler);
                oDURequisitionSetups.Add(oItem);
            }
            return oDURequisitionSetups;
        }

        #endregion

        #region Interface implementation
        public DURequisitionSetupService() { }


        public DURequisitionSetup Save(DURequisitionSetup oDURequisitionSetup, Int64 nUserId)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                #region DURequisitionSetup
                IDataReader reader;
                if (oDURequisitionSetup.DURequisitionSetupID <= 0)
                {
                    reader = DURequisitionSetupDA.InsertUpdate(tc, oDURequisitionSetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = DURequisitionSetupDA.InsertUpdate(tc, oDURequisitionSetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDURequisitionSetup = new DURequisitionSetup();
                    oDURequisitionSetup = CreateObject(oReader);
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
                oDURequisitionSetup = new DURequisitionSetup();
                oDURequisitionSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDURequisitionSetup;
        }

        public String Delete(DURequisitionSetup oDURequisitionSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DURequisitionSetupDA.Delete(tc, oDURequisitionSetup, EnumDBOperation.Delete, nUserID);
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
        public DURequisitionSetup Get(int id, Int64 nUserId)
        {
            DURequisitionSetup oDURequisitionSetup = new DURequisitionSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DURequisitionSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDURequisitionSetup = CreateObject(oReader);
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

            return oDURequisitionSetup;
        }

        public List<DURequisitionSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<DURequisitionSetup> oDURequisitionSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DURequisitionSetupDA.Gets(sSQL, tc);
                oDURequisitionSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DURequisitionSetup", e);
                #endregion
            }
            return oDURequisitionSetup;
        }
        public DURequisitionSetup GetByType(int nRequisitionType, Int64 nUserId)
        {
            DURequisitionSetup oDURequisitionSetup = new DURequisitionSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DURequisitionSetupDA.GetByType(tc, nRequisitionType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDURequisitionSetup = CreateObject(oReader);
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

            return oDURequisitionSetup;
        }


        public List<DURequisitionSetup> Gets(Int64 nUserId)
        {
            List<DURequisitionSetup> oDURequisitionSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DURequisitionSetupDA.Gets(tc);
                oDURequisitionSetups = CreateObjects(reader);
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

            return oDURequisitionSetups;
        }
        public List<DURequisitionSetup> Gets(int nBUID, Int64 nUserId)
        {
            List<DURequisitionSetup> oDURequisitionSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DURequisitionSetupDA.Gets(tc, nBUID);
                oDURequisitionSetups = CreateObjects(reader);
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

            return oDURequisitionSetups;
        }

        public DURequisitionSetup Activate(DURequisitionSetup oDURequisitionSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DURequisitionSetupDA.Activate(tc, oDURequisitionSetup);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDURequisitionSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDURequisitionSetup = new DURequisitionSetup();
                oDURequisitionSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDURequisitionSetup;
        }


        #endregion
    }
}