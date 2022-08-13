using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class DymanicHeadSetupService : MarshalByRefObject, IDymanicHeadSetupService
    {
        #region Private functions and declaration
        private DymanicHeadSetup MapObject(NullHandler oReader)
        {
            DymanicHeadSetup oDymanicHeadSetup = new DymanicHeadSetup();
            oDymanicHeadSetup.DymanicHeadSetupID = oReader.GetInt32("DymanicHeadSetupID");
            oDymanicHeadSetup.Name = oReader.GetString("Name");
            oDymanicHeadSetup.ReferenceType = (EnumReferenceType)oReader.GetInt32("ReferenceType");
            oDymanicHeadSetup.MappingType = (EnumACMappingType)oReader.GetInt32("MappingType");
            oDymanicHeadSetup.ReferenceTypeInt = oReader.GetInt32("ReferenceType");
            oDymanicHeadSetup.MappingTypeInt = oReader.GetInt32("MappingType");
            oDymanicHeadSetup.MappingID = oReader.GetInt32("MappingID");
            oDymanicHeadSetup.Activity = oReader.GetBoolean("Activity");
            oDymanicHeadSetup.Note = oReader.GetString("Note");
            oDymanicHeadSetup.MappingName = oReader.GetString("MappingName");

            return oDymanicHeadSetup;
        }

        private DymanicHeadSetup CreateObject(NullHandler oReader)
        {
            DymanicHeadSetup oDymanicHeadSetup = new DymanicHeadSetup();
            oDymanicHeadSetup = MapObject(oReader);
            return oDymanicHeadSetup;
        }

        private List<DymanicHeadSetup> CreateObjects(IDataReader oReader)
        {
            List<DymanicHeadSetup> oDymanicHeadSetups = new List<DymanicHeadSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DymanicHeadSetup oItem = CreateObject(oHandler);
                oDymanicHeadSetups.Add(oItem);
            }
            return oDymanicHeadSetups;
        }

        #endregion

        #region Interface implementation
        public DymanicHeadSetupService() { }

        public DymanicHeadSetup Save(DymanicHeadSetup oDymanicHeadSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDymanicHeadSetup.DymanicHeadSetupID <= 0)
                {
                    reader = DymanicHeadSetupDA.InsertUpdate(tc, oDymanicHeadSetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = DymanicHeadSetupDA.InsertUpdate(tc, oDymanicHeadSetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDymanicHeadSetup = new DymanicHeadSetup();
                    oDymanicHeadSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDymanicHeadSetup.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return oDymanicHeadSetup;
        }
        public string Delete(DymanicHeadSetup oDymanicHeadSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {


                tc = TransactionContext.Begin(true);
            
               
                DymanicHeadSetupDA.Delete(tc, oDymanicHeadSetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public string Process(DymanicHeadSetup oDymanicHeadSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {


                tc = TransactionContext.Begin(true);


                DymanicHeadSetupDA.Process(tc, oDymanicHeadSetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return "Process done successfully";
        }

        public DymanicHeadSetup Get(int id, Int64 nUserId)
        {
            DymanicHeadSetup oDymanicHeadSetup = new DymanicHeadSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DymanicHeadSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDymanicHeadSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oDymanicHeadSetup;
        }

        public DymanicHeadSetup GetByRef(EnumReferenceType eEnumReferenceType, Int64 nUserId)
        {
            DymanicHeadSetup oDymanicHeadSetup = new DymanicHeadSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DymanicHeadSetupDA.GetByRef(tc, eEnumReferenceType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDymanicHeadSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oDymanicHeadSetup;
        }



        public List<DymanicHeadSetup> Gets(Int64 nUserId)
        {
            List<DymanicHeadSetup> oDymanicHeadSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DymanicHeadSetupDA.Gets(tc);
                oDymanicHeadSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oDymanicHeadSetups;
        }

        public List<DymanicHeadSetup> Gets(bool bActivity,Int64 nUserId)
        {
            List<DymanicHeadSetup> oDymanicHeadSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DymanicHeadSetupDA.Gets(tc, bActivity);
                oDymanicHeadSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oDymanicHeadSetups;
        }

        public string ActivateDymanicHeadSetup(DymanicHeadSetup oDymanicHeadSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {


                tc = TransactionContext.Begin(true);
                DymanicHeadSetupDA.ActivateDymanicHeadSetup(tc, oDymanicHeadSetup);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Activate PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return "Activate sucessfully";
        }
        #endregion
    }
}
