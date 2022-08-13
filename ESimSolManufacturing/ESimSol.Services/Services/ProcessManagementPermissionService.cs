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
    public class ProcessManagementPermissionService : MarshalByRefObject, IProcessManagementPermissionService
    {
        #region Private functions and declaration
        private ProcessManagementPermission MapObject(NullHandler oReader)
        {
            ProcessManagementPermission oProcessManagementPermission = new ProcessManagementPermission();
            oProcessManagementPermission.PMPID = oReader.GetInt32("PMPID");
            oProcessManagementPermission.UserID = oReader.GetInt32("UserID");
            oProcessManagementPermission.CompanyID = oReader.GetInt32("CompanyID");
            oProcessManagementPermission.LocationID = oReader.GetInt32("LocationID");
            oProcessManagementPermission.DepartmentID = oReader.GetInt32("DepartmentID");
            oProcessManagementPermission.ProcessManagementType = (EnumProcessManagementType)oReader.GetInt16("ProcessManagementType");
            oProcessManagementPermission.ProcessType = (EnumProcessType)oReader.GetInt16("ProcessType");
            oProcessManagementPermission.ProcessStatus = (EnumProcessStatus)oReader.GetInt16("ProcessStatus");
            oProcessManagementPermission.IsActive = oReader.GetBoolean("IsActive");
            oProcessManagementPermission.EmployeeNameCode = oReader.GetString("EmployeeNameCode");
            oProcessManagementPermission.LocationName = oReader.GetString("LocationName");
            oProcessManagementPermission.DepartmentName = oReader.GetString("DepartmentName");
            return oProcessManagementPermission;
        }

        private ProcessManagementPermission CreateObject(NullHandler oReader)
        {
            ProcessManagementPermission oProcessManagementPermission = MapObject(oReader);
            return oProcessManagementPermission;
        }

        private List<ProcessManagementPermission> CreateObjects(IDataReader oReader)
        {
            List<ProcessManagementPermission> oProcessManagementPermission = new List<ProcessManagementPermission>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProcessManagementPermission oItem = CreateObject(oHandler);
                oProcessManagementPermission.Add(oItem);
            }
            return oProcessManagementPermission;
        }

        #endregion

        #region Interface implementation
        public ProcessManagementPermissionService() { }

        public ProcessManagementPermission IUD(ProcessManagementPermission oProcessManagementPermission, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ProcessManagementPermissionDA.IUD(tc, oProcessManagementPermission, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oProcessManagementPermission = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oProcessManagementPermission.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oProcessManagementPermission;
        }

        public ProcessManagementPermission Activite(int nPMPID, bool Active, Int64 nUserId)
        {
            ProcessManagementPermission oProcessManagementPermission = new ProcessManagementPermission();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProcessManagementPermissionDA.Activity(nPMPID, Active, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProcessManagementPermission = CreateObject(oReader);
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
                oProcessManagementPermission.ErrorMessage = e.Message;
                #endregion
            }

            return oProcessManagementPermission;
        }

        public ProcessManagementPermission Get(int nPMPID, Int64 nUserId)
        {
            ProcessManagementPermission oProcessManagementPermission = new ProcessManagementPermission();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProcessManagementPermissionDA.Get(nPMPID,tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProcessManagementPermission = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get ProcessManagementPermission", e);
                oProcessManagementPermission.ErrorMessage = e.Message;
                #endregion
            }

            return oProcessManagementPermission;
        }
        public List<ProcessManagementPermission> Gets(int nUID, Int64 nUserID)
        {
            List<ProcessManagementPermission> oProcessManagementPermission = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProcessManagementPermissionDA.Gets( tc,nUID);
                oProcessManagementPermission = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProcessManagementPermission", e);
                #endregion
            }
            return oProcessManagementPermission;
        }

        public List<ProcessManagementPermission> Gets(string sSQL, Int64 nUserID)
        {
            List<ProcessManagementPermission> oProcessManagementPermission = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProcessManagementPermissionDA.Gets(sSQL, tc);
                oProcessManagementPermission = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProcessManagementPermission", e);
                #endregion
            }
            return oProcessManagementPermission;
        }


        #endregion
    }
}
