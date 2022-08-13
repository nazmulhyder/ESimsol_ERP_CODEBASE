using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{

    public class DevelopmentTypeService : MarshalByRefObject, IDevelopmentTypeService
    {
        #region Private functions and declaration
        private DevelopmentType MapObject(NullHandler oReader)
        {
            DevelopmentType oDevelopmentType = new DevelopmentType();
            oDevelopmentType.DevelopmentTypeID = oReader.GetInt32("DevelopmentTypeID");
            oDevelopmentType.Name = oReader.GetString("Name");
            oDevelopmentType.Note = oReader.GetString("Note");
            return oDevelopmentType;
        }

        private DevelopmentType CreateObject(NullHandler oReader)
        {
            DevelopmentType oDevelopmentType = new DevelopmentType();
            oDevelopmentType = MapObject(oReader);
            return oDevelopmentType;
        }

        private List<DevelopmentType> CreateObjects(IDataReader oReader)
        {
            List<DevelopmentType> oDevelopmentType = new List<DevelopmentType>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DevelopmentType oItem = CreateObject(oHandler);
                oDevelopmentType.Add(oItem);
            }
            return oDevelopmentType;
        }

        #endregion

        #region Interface implementation
        public DevelopmentTypeService() { }

        public DevelopmentType Save(DevelopmentType oDevelopmentType, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDevelopmentType.DevelopmentTypeID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DevelopmentType, EnumRoleOperationType.Add);
                    reader = DevelopmentTypeDA.InsertUpdate(tc, oDevelopmentType, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DevelopmentType, EnumRoleOperationType.Edit);
                    reader = DevelopmentTypeDA.InsertUpdate(tc, oDevelopmentType, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDevelopmentType = new DevelopmentType();
                    oDevelopmentType = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDevelopmentType = new DevelopmentType();
                oDevelopmentType.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oDevelopmentType;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DevelopmentType oDevelopmentType = new DevelopmentType();
                oDevelopmentType.DevelopmentTypeID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.DevelopmentType, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "DevelopmentType", id);
                DevelopmentTypeDA.Delete(tc, oDevelopmentType, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete DevelopmentType. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public DevelopmentType Get(int id, Int64 nUserId)
        {
            DevelopmentType oAccountHead = new DevelopmentType();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DevelopmentTypeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DevelopmentType", e);
                #endregion
            }

            return oAccountHead;
        }


        public List<DevelopmentType> Gets(Int64 nUserID)
        {
            List<DevelopmentType> oDevelopmentType = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentTypeDA.Gets(tc);
                oDevelopmentType = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentType", e);
                #endregion
            }

            return oDevelopmentType;
        }

        public List<DevelopmentType> Gets(string sSQL, Int64 nUserID)
        {
            List<DevelopmentType> oDevelopmentType = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentTypeDA.Gets(tc, sSQL);
                oDevelopmentType = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentType", e);
                #endregion
            }

            return oDevelopmentType;
        }

        #endregion
    }   

}
