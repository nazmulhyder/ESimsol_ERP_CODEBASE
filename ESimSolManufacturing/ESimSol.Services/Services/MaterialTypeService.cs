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

    public class MaterialTypeService : MarshalByRefObject, IMaterialTypeService
    {
        #region Private functions and declaration
        private MaterialType MapObject(NullHandler oReader)
        {
            MaterialType oMaterialType = new MaterialType();
            oMaterialType.MaterialTypeID = oReader.GetInt32("MaterialTypeID");
            oMaterialType.Name = oReader.GetString("Name");
            oMaterialType.Note = oReader.GetString("Note");
            return oMaterialType;
        }

        private MaterialType CreateObject(NullHandler oReader)
        {
            MaterialType oMaterialType = new MaterialType();
            oMaterialType = MapObject(oReader);
            return oMaterialType;
        }

        private List<MaterialType> CreateObjects(IDataReader oReader)
        {
            List<MaterialType> oMaterialType = new List<MaterialType>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MaterialType oItem = CreateObject(oHandler);
                oMaterialType.Add(oItem);
            }
            return oMaterialType;
        }

        #endregion

        #region Interface implementation
        public MaterialTypeService() { }

        public MaterialType Save(MaterialType oMaterialType, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMaterialType.MaterialTypeID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.MaterialType, EnumRoleOperationType.Add);
                    reader = MaterialTypeDA.InsertUpdate(tc, oMaterialType, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.MaterialType, EnumRoleOperationType.Edit);
                    reader = MaterialTypeDA.InsertUpdate(tc, oMaterialType, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMaterialType = new MaterialType();
                    oMaterialType = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oMaterialType = new MaterialType();
                oMaterialType.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oMaterialType;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MaterialType oMaterialType = new MaterialType();
                oMaterialType.MaterialTypeID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.MaterialType, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "MaterialType", id);
                MaterialTypeDA.Delete(tc, oMaterialType, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete MaterialType. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public MaterialType Get(int id, Int64 nUserId)
        {
            MaterialType oAccountHead = new MaterialType();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MaterialTypeDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get MaterialType", e);
                #endregion
            }

            return oAccountHead;
        }


        public List<MaterialType> Gets(Int64 nUserID)
        {
            List<MaterialType> oMaterialType = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MaterialTypeDA.Gets(tc);
                oMaterialType = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MaterialType", e);
                #endregion
            }

            return oMaterialType;
        }

        public List<MaterialType> Gets(string sSQL, Int64 nUserID)
        {
            List<MaterialType> oMaterialType = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MaterialTypeDA.Gets(tc, sSQL);
                oMaterialType = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MaterialType", e);
                #endregion
            }

            return oMaterialType;
        }

        #endregion
    }   

}
