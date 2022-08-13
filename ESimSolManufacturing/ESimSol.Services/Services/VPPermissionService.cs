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
    public class VPPermissionService : MarshalByRefObject, IVPPermissionService
    {
        #region Private functions and declaration
        private VPPermission MapObject(NullHandler oReader)
        {
            VPPermission oVPPermission = new VPPermission();
            oVPPermission.VPPermissionID = oReader.GetInt32("VPPermissionID");
            oVPPermission.UserID = oReader.GetInt32("UserID");
            oVPPermission.IntegrationSetupID = oReader.GetInt32("IntegrationSetupID");
            oVPPermission.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oVPPermission.BUName = oReader.GetString("BUName");
            oVPPermission.BUSName = oReader.GetString("BUSName");
            oVPPermission.VoucherSetupIntegration = (EnumVoucherSetup)oReader.GetInt32("VoucherSetup");
            return oVPPermission;
        }

        private VPPermission CreateObject(NullHandler oReader)
        {
            VPPermission oVPPermission = new VPPermission();
            oVPPermission = MapObject(oReader);
            return oVPPermission;
        }

        private List<VPPermission> CreateObjects(IDataReader oReader)
        {
            List<VPPermission> oVPPermission = new List<VPPermission>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VPPermission oItem = CreateObject(oHandler);
                oVPPermission.Add(oItem);
            }
            return oVPPermission;
        }

        #endregion

        #region Interface implementation
        public VPPermissionService() { }

        public VPPermission Save(VPPermission oVPPermission, int nUserID)
        {
            TransactionContext tc = null;
            oVPPermission.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVPPermission.VPPermissionID <= 0)
                {
                    reader = VPPermissionDA.InsertUpdate(tc, oVPPermission, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VPPermissionDA.InsertUpdate(tc, oVPPermission, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVPPermission = new VPPermission();
                    oVPPermission = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oVPPermission.ErrorMessage = e.Message.Split('!')[0];

                #endregion
            }
            return oVPPermission;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VPPermission oVPPermission = new VPPermission();

                oVPPermission.VPPermissionID = id;
                VPPermissionDA.Delete(tc, oVPPermission, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                #endregion
            }
            return "Deleted";
        }

        public VPPermission Get(int id, int nUserId)
        {
            VPPermission oAccountHead = new VPPermission();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VPPermissionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get VPPermission", e);
                #endregion
            }

            return oAccountHead;
        }



        public List<VPPermission> Gets(int nUserID)
        {
            List<VPPermission> oVPPermission = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VPPermissionDA.Gets(tc);
                oVPPermission = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VPPermission", e);
                #endregion
            }

            return oVPPermission;
        }
        public List<VPPermission> Gets(string sSQL, int nUserID)
        {
            List<VPPermission> oVPPermission = new List<VPPermission>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from View_VPPermission where VPPermissionID in (1,2,80,272,347,370,60,45)";
                }
                reader = VPPermissionDA.Gets(tc, sSQL);
                oVPPermission = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VPPermission", e);
                #endregion
            }

            return oVPPermission;
        }

        public List<VPPermission> GetsByUser(int nPermittedUserID, int nUserID)
        {
            List<VPPermission> oVPPermissions = new List<VPPermission>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VPPermissionDA.GetsByUser(tc, nPermittedUserID);
                oVPPermissions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oVPPermissions = new List<VPPermission>();
                VPPermission oVPPermission = new VPPermission();
                oVPPermission.ErrorMessage = e.Message;
                oVPPermissions.Add(oVPPermission);
                #endregion
            }
            return oVPPermissions;
        }
        #endregion
    }
}
