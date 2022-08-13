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
    public class StorePermissionService : MarshalByRefObject, IStorePermissionService
    {
        #region Private functions and declaration
        private StorePermission MapObject(NullHandler oReader)
        {
            StorePermission oStorePermission = new StorePermission();
            oStorePermission.StorePermissionID = oReader.GetInt32("StorePermissionID");
            oStorePermission.UserID = oReader.GetInt32("UserID");
            oStorePermission.ModuleName = (EnumModuleName)oReader.GetInt32("ModuleName");
            oStorePermission.ModuleNameInt = oReader.GetInt32("ModuleName");
            oStorePermission.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oStorePermission.StoreType = (EnumStoreType)oReader.GetInt32("StoreType");
            oStorePermission.StoreTypeInt = oReader.GetInt32("StoreType");
            oStorePermission.Remarks = oReader.GetString("Remarks");
            oStorePermission.WorkingUnitCode = oReader.GetString("WorkingUnitCode");
            oStorePermission.LocationName = oReader.GetString("LocationName");
            oStorePermission.OperationUnitName = oReader.GetString("OperationUnitName");
            return oStorePermission;
        }

        private StorePermission CreateObject(NullHandler oReader)
        {
            StorePermission oStorePermission = new StorePermission();
            oStorePermission = MapObject(oReader);
            return oStorePermission;
        }

        private List<StorePermission> CreateObjects(IDataReader oReader)
        {
            List<StorePermission> oStorePermission = new List<StorePermission>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                StorePermission oItem = CreateObject(oHandler);
                oStorePermission.Add(oItem);
            }
            return oStorePermission;
        }

        #endregion

        #region Interface implementation
        public StorePermissionService() { }

        public List<StorePermission> Save(StorePermission oStorePermission, int nUserID)
        {
            List<StorePermission> oStorePermissions = new List<StorePermission>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oStorePermission.StorePermissionID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.StorePermission, EnumRoleOperationType.Add);
                    foreach(WorkingUnit oItem in oStorePermission.WorkingUnits)
                    {
                        oStorePermission.WorkingUnitID = oItem.WorkingUnitID;
                        reader = StorePermissionDA.InsertUpdate(tc, oStorePermission, EnumDBOperation.Insert, nUserID);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oStorePermission = new StorePermission();
                            oStorePermission = CreateObject(oReader);
                            oStorePermissions.Add(oStorePermission);
                        }
                        reader.Close();
                    }
                    
                    //reader = StorePermissionDA.InsertUpdate(tc, oStorePermission, EnumDBOperation.Insert, nUserID);
                    
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.StorePermission, EnumRoleOperationType.Edit);
                    reader = StorePermissionDA.InsertUpdate(tc, oStorePermission, EnumDBOperation.Update, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oStorePermission = new StorePermission();
                        oStorePermission = CreateObject(oReader);
                        oStorePermissions.Add(oStorePermission);
                    }
                    reader.Close();
                }
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oStorePermission = new StorePermission();
                oStorePermission.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save StorePermission. Because of " + e.Message, e);
                #endregion
            }
            return oStorePermissions;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                StorePermission oStorePermission = new StorePermission();
                oStorePermission.StorePermissionID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.StorePermission, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "StorePermission", id);
                StorePermissionDA.Delete(tc, oStorePermission, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete StorePermission. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public StorePermission Get(int id, int nUserId)
        {
            StorePermission oAccountHead = new StorePermission();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = StorePermissionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get StorePermission", e);
                #endregion
            }

            return oAccountHead;
        }



        public List<StorePermission> Gets(int nUserID)
        {
            List<StorePermission> oStorePermission = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = StorePermissionDA.Gets(tc);
                oStorePermission = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get StorePermission", e);
                #endregion
            }

            return oStorePermission;
        }
        public List<StorePermission> Gets(string sSQL, int nUserID)
        {
            List<StorePermission> oStorePermission = new List<StorePermission>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from View_StorePermission where StorePermissionID in (1,2,80,272,347,370,60,45)";
                }
                reader = StorePermissionDA.Gets(tc, sSQL);
                oStorePermission = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get StorePermission", e);
                #endregion
            }

            return oStorePermission;
        }

        public List<StorePermission> GetsByUser(int nPermittedUserID, int nUserID)
        {
            List<StorePermission> oStorePermissions = new List<StorePermission>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = StorePermissionDA.GetsByUser(tc, nPermittedUserID);
                oStorePermissions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oStorePermissions = new List<StorePermission>();
                StorePermission oStorePermission = new StorePermission();
                oStorePermission.ErrorMessage = e.Message;
                oStorePermissions.Add(oStorePermission);
                #endregion
            }
            return oStorePermissions;
        }
        #endregion
    }
}
