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
    public class FabricMachineTypeService : MarshalByRefObject, IFabricMachineTypeService
    {
        #region Private functions and declaration

        private FabricMachineType MapObject(NullHandler oReader)
        {
            FabricMachineType oFabricMachineType = new FabricMachineType();
            oFabricMachineType.FabricMachineTypeID = oReader.GetInt32("FabricMachineTypeID");
            oFabricMachineType.Name = oReader.GetString("Name");
            oFabricMachineType.Brand = oReader.GetString("Brand");
            oFabricMachineType.ParentID = oReader.GetInt32("ParentID");
            oFabricMachineType.Note = oReader.GetString("Note");
            oFabricMachineType.ParentName = oReader.GetString("ParentName");
            return oFabricMachineType;
        }

        private FabricMachineType CreateObject(NullHandler oReader)
        {
            FabricMachineType oFabricMachineType = new FabricMachineType();
            oFabricMachineType = MapObject(oReader);
            return oFabricMachineType;
        }

        private List<FabricMachineType> CreateObjects(IDataReader oReader)
        {
            List<FabricMachineType> oFabricMachineType = new List<FabricMachineType>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricMachineType oItem = CreateObject(oHandler);
                oFabricMachineType.Add(oItem);
            }
            return oFabricMachineType;
        }

        #endregion

        #region Interface implementation
        public FabricMachineType Save(FabricMachineType oFabricMachineType, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricMachineType.FabricMachineTypeID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricMachineType", EnumRoleOperationType.Add);
                    reader = FabricMachineTypeDA.InsertUpdate(tc, oFabricMachineType, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricMachineType", EnumRoleOperationType.Edit);
                    reader = FabricMachineTypeDA.InsertUpdate(tc, oFabricMachineType, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricMachineType = new FabricMachineType();
                    oFabricMachineType = CreateObject(oReader);
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
                    oFabricMachineType = new FabricMachineType();
                    oFabricMachineType.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricMachineType;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricMachineType oFabricMachineType = new FabricMachineType();
                oFabricMachineType.FabricMachineTypeID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "FabricMachineType", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FabricMachineType", id);
                FabricMachineTypeDA.Delete(tc, oFabricMachineType, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public FabricMachineType Get(int id, Int64 nUserId)
        {
            FabricMachineType oFabricMachineType = new FabricMachineType();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricMachineTypeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricMachineType = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricMachineType", e);
                #endregion
            }
            return oFabricMachineType;
        }

        public List<FabricMachineType> Gets(Int64 nUserID)
        {
            List<FabricMachineType> oFabricMachineTypes = new List<FabricMachineType>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricMachineTypeDA.Gets(tc);
                oFabricMachineTypes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricMachineType oFabricMachineType = new FabricMachineType();
                oFabricMachineType.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricMachineTypes;
        }

        public List<FabricMachineType> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricMachineType> oFabricMachineTypes = new List<FabricMachineType>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricMachineTypeDA.Gets(tc, sSQL);
                oFabricMachineTypes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricMachineType", e);
                #endregion
            }
            return oFabricMachineTypes;
        }

        #endregion
    }

}
