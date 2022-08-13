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
    public class DBPermissionService : MarshalByRefObject, IDBPermissionService
    {
        #region Private functions and declaration
        private DBPermission MapObject(NullHandler oReader)
        {
            DBPermission oDBPermission = new DBPermission();
            oDBPermission.DBPermissionID = oReader.GetInt32("DBPermissionID");
            oDBPermission.UserID = oReader.GetInt32("UserID");
            oDBPermission.DashBoardType = (EnumDashBoardType)oReader.GetInt32("DashBoardType");
            oDBPermission.DashBoardTypeInt = oReader.GetInt32("DashBoardType");
            oDBPermission.Remarks = oReader.GetString("Remarks");
            return oDBPermission;
        }
        private DBPermission CreateObject(NullHandler oReader)
        {
            DBPermission oDBPermission = new DBPermission();
            oDBPermission = MapObject(oReader);
            return oDBPermission;
        }
        private List<DBPermission> CreateObjects(IDataReader oReader)
        {
            List<DBPermission> oDBPermission = new List<DBPermission>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DBPermission oItem = CreateObject(oHandler);
                oDBPermission.Add(oItem);
            }
            return oDBPermission;
        }

        #endregion

        #region Interface implementation
        public DBPermissionService() { }
        public DBPermission Save(DBPermission oDBPermission, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDBPermission.DBPermissionID <= 0)
                {
                    reader = DBPermissionDA.InsertUpdate(tc, oDBPermission, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DBPermissionDA.InsertUpdate(tc, oDBPermission, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDBPermission = new DBPermission();
                    oDBPermission = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDBPermission = new DBPermission();
                oDBPermission.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save DBPermission. Because of " + e.Message, e);
                #endregion
            }
            return oDBPermission;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DBPermission oDBPermission = new DBPermission();
                oDBPermission.DBPermissionID = id;
                DBPermissionDA.Delete(tc, oDBPermission, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete DBPermission. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }
        public DBPermission Get(int id, int nUserId)
        {
            DBPermission oAccountHead = new DBPermission();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DBPermissionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get DBPermission", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<DBPermission> Gets(int nUserID)
        {
            List<DBPermission> oDBPermission = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DBPermissionDA.Gets(tc);
                oDBPermission = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DBPermission", e);
                #endregion
            }

            return oDBPermission;
        }
        public List<DBPermission> Gets(string sSQL, int nUserID)
        {
            List<DBPermission> oDBPermission = new List<DBPermission>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from View_DBPermission where DBPermissionID in (1,2,80,272,347,370,60,45)";
                }
                reader = DBPermissionDA.Gets(tc, sSQL);
                oDBPermission = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DBPermission", e);
                #endregion
            }

            return oDBPermission;
        }
        public List<DBPermission> GetsByUser(int nPermittedUserID, int nUserID)
        {
            List<DBPermission> oDBPermissions = new List<DBPermission>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DBPermissionDA.GetsByUser(tc, nPermittedUserID);
                oDBPermissions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDBPermissions = new List<DBPermission>();
                DBPermission oDBPermission = new DBPermission();
                oDBPermission.ErrorMessage = e.Message;
                oDBPermissions.Add(oDBPermission);
                #endregion
            }
            return oDBPermissions;
        }
        #endregion
    }
}
