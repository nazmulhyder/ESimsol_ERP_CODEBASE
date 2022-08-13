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
    public class MachineTypeService : MarshalByRefObject, IMachineTypeService
    {
        #region Private functions and declaration
        private MachineType MapObject(NullHandler oReader)
        {
            MachineType oMachineType = new MachineType();
            oMachineType.MachineTypeID = oReader.GetInt32("MachineTypeID");
            oMachineType.BUID = oReader.GetInt32("BUID");
            oMachineType.Name = oReader.GetString("Name");
            oMachineType.Note = oReader.GetString("Note");
            oMachineType.ModuleIDs = oReader.GetString("ModuleIDs");
            return oMachineType;
        }

        private MachineType CreateObject(NullHandler oReader)
        {
            MachineType oMachineType = new MachineType();
            oMachineType = MapObject(oReader);
            return oMachineType;
        }

        private List<MachineType> CreateObjects(IDataReader oReader)
        {
            List<MachineType> oMachineTypes = new List<MachineType>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MachineType oItem = CreateObject(oHandler);
                oMachineTypes.Add(oItem);
            }
            return oMachineTypes;
        }

        #endregion

        #region Interface implementation
        public MachineTypeService() { }

        public MachineType Save(MachineType oMachineType, Int64 nUserId)
        {

            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);
                #region MachineType
                IDataReader reader;
                if (oMachineType.MachineTypeID <= 0)
                {
                    reader = MachineTypeDA.InsertUpdate(tc, oMachineType, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = MachineTypeDA.InsertUpdate(tc, oMachineType, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMachineType = new MachineType();
                    oMachineType = CreateObject(oReader);
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
                oMachineType = new MachineType();
                oMachineType.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oMachineType;
        }
      
        public String Delete(MachineType oMachineType, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MachineTypeDA.Delete(tc, oMachineType, EnumDBOperation.Delete, nUserID);
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
        public MachineType Get(int id, Int64 nUserId)
        {
            MachineType oMachineType = new MachineType();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MachineTypeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMachineType = CreateObject(oReader);
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

            return oMachineType;
        }
        public MachineType GetBy(int nMachineType, Int64 nUserId)
        {
            MachineType oMachineType = new MachineType();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MachineTypeDA.GetBy(tc, nMachineType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMachineType = CreateObject(oReader);
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

            return oMachineType;
        }

        public List<MachineType> Gets(string sql,Int64 nUserId)
        {
            List<MachineType> oMachineTypes = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MachineTypeDA.Gets(tc, sql);
                oMachineTypes = CreateObjects(reader);
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

            return oMachineTypes;
        }
        public List<MachineType> Gets(Int64 nUserId)
        {
            List<MachineType> oMachineTypes = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MachineTypeDA.Gets(tc);
                oMachineTypes = CreateObjects(reader);
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

            return oMachineTypes;
        }
        public List<MachineType> GetsByModuleIDs(string ids, Int64 nUserId)
        {
            List<MachineType> oMachineTypes = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MachineTypeDA.GetsByModuleIDs(tc, ids);
                oMachineTypes = CreateObjects(reader);
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

            return oMachineTypes;
        }
        public MachineType Activate(MachineType oMachineType, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MachineTypeDA.Activate(tc, oMachineType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMachineType = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oMachineType = new MachineType();
                oMachineType.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oMachineType;
        }
    

        #endregion
    }
}