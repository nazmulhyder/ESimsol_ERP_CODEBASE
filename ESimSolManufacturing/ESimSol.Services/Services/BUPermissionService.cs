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
    public class BUPermissionService : MarshalByRefObject, IBUPermissionService
    {
        #region Private functions and declaration
        private BUPermission MapObject(NullHandler oReader)
        {
            BUPermission oBUPermission = new BUPermission();
            oBUPermission.BUPermissionID = oReader.GetInt32("BUPermissionID");
            oBUPermission.UserID = oReader.GetInt32("UserID");            
            oBUPermission.BUID = oReader.GetInt32("BUID");            
            oBUPermission.Remarks = oReader.GetString("Remarks");
            oBUPermission.BUCode = oReader.GetString("BUCode");
            oBUPermission.BUName = oReader.GetString("BUName");
            oBUPermission.BUWiseShiftID = oReader.GetInt32("BUWiseShiftID"); 
            oBUPermission.ShiftID = oReader.GetInt32("ShiftID"); 
            return oBUPermission;
        }

        private BUPermission CreateObject(NullHandler oReader)
        {
            BUPermission oBUPermission = new BUPermission();
            oBUPermission = MapObject(oReader);
            return oBUPermission;
        }

        private List<BUPermission> CreateObjects(IDataReader oReader)
        {
            List<BUPermission> oBUPermission = new List<BUPermission>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BUPermission oItem = CreateObject(oHandler);
                oBUPermission.Add(oItem);
            }
            return oBUPermission;
        }

        #endregion

        #region Interface implementation
        public BUPermissionService() { }

        public BUPermission Save(BUPermission oBUPermission, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBUPermission.BUPermissionID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.BUPermission, EnumRoleOperationType.Add);
                    reader = BUPermissionDA.InsertUpdate(tc, oBUPermission, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.BUPermission, EnumRoleOperationType.Edit);
                    reader = BUPermissionDA.InsertUpdate(tc, oBUPermission, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBUPermission = new BUPermission();
                    oBUPermission = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oBUPermission = new BUPermission();
                oBUPermission.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save BUPermission. Because of " + e.Message, e);
                #endregion
            }
            return oBUPermission;
        }

        public BUPermission SaveBUWiseShift(BUPermission oBUPermission, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBUPermission.BUWiseShiftID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.BUPermission, EnumRoleOperationType.Add);
                    reader = BUPermissionDA.InsertUpdateBUWiseShift(tc, oBUPermission, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.BUPermission, EnumRoleOperationType.Edit);
                    reader = BUPermissionDA.InsertUpdateBUWiseShift(tc, oBUPermission, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBUPermission = new BUPermission();
                    oBUPermission = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oBUPermission = new BUPermission();
                oBUPermission.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save BUPermission. Because of " + e.Message, e);
                #endregion
            }
            return oBUPermission;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BUPermission oBUPermission = new BUPermission();
                oBUPermission.BUPermissionID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.BUPermission, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "BUPermission", id);
                BUPermissionDA.Delete(tc, oBUPermission, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete BUPermission. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public string DeleteBUWiseShift(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BUPermission oBUPermission = new BUPermission();
                oBUPermission.BUWiseShiftID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.BUPermission, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "BUPermission", id);
                BUPermissionDA.DeleteBUWiseShift(tc, oBUPermission, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete BUPermission. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public BUPermission Get(int id, int nUserId)
        {
            BUPermission oAccountHead = new BUPermission();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BUPermissionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get BUPermission", e);
                #endregion
            }

            return oAccountHead;
        }



        public List<BUPermission> Gets(int nUserID)
        {
            List<BUPermission> oBUPermission = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BUPermissionDA.Gets(tc);
                oBUPermission = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BUPermission", e);
                #endregion
            }

            return oBUPermission;
        }
        public List<BUPermission> Gets(string sSQL, int nUserID)
        {
            List<BUPermission> oBUPermission = new List<BUPermission>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from View_BUPermission where BUPermissionID in (1,2,80,272,347,370,60,45)";
                }
                reader = BUPermissionDA.Gets(tc, sSQL);
                oBUPermission = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BUPermission", e);
                #endregion
            }

            return oBUPermission;
        }

        public List<BUPermission> GetsByUser(int nPermittedUserID, int nUserID)
        {
            List<BUPermission> oBUPermissions = new List<BUPermission>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BUPermissionDA.GetsByUser(tc, nPermittedUserID);
                oBUPermissions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oBUPermissions = new List<BUPermission>();
                BUPermission oBUPermission = new BUPermission();
                oBUPermission.ErrorMessage = e.Message;
                oBUPermissions.Add(oBUPermission);
                #endregion
            }
            return oBUPermissions;
        }
        #endregion
    }
}
