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
    public class ArchiveDataService : MarshalByRefObject, IArchiveDataService
    {
        private ArchiveData MapObject(NullHandler oReader)
        {
            ArchiveData oArchiveData = new ArchiveData();

            oArchiveData.ArchiveDataID = oReader.GetInt32("ArchiveDataID");
            oArchiveData.ArchiveNo = oReader.GetString("ArchiveNo");
            oArchiveData.ArchiveDate = oReader.GetDateTime("ArchiveDate");
            oArchiveData.ArchiveMonthID = (EnumMonth)oReader.GetInt32("ArchiveMonthID");
            oArchiveData.ArchiveYearID = oReader.GetInt32("ArchiveYearID");
            oArchiveData.EmpCount = oReader.GetInt32("EmpCount");
            oArchiveData.ArchiveStatus = (EnumArchiveStatus)oReader.GetInt16("ArchiveStatus");
            oArchiveData.Remarks = oReader.GetString("Remarks");
            oArchiveData.ApprovedByName = oReader.GetString("ApprovedByName");
            oArchiveData.ApprovedBy = oReader.GetInt32("ApprovedBy");
            return oArchiveData;
        }
        private ArchiveData CreateObject(NullHandler oReader)
        {
            ArchiveData oArchiveData = new ArchiveData();
            oArchiveData = MapObject(oReader);
            return oArchiveData;
        }

        private List<ArchiveData> CreateObjects(IDataReader oReader)
        {
            List<ArchiveData> oArchiveData = new List<ArchiveData>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ArchiveData oItem = CreateObject(oHandler);
                oArchiveData.Add(oItem);
            }
            return oArchiveData;
        }
        public ArchiveData Save(ArchiveData oArchiveData, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oArchiveData.ArchiveDataID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ArchiveData, EnumRoleOperationType.Add);
                    reader = ArchiveDataDA.InsertUpdate(tc, oArchiveData, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ArchiveData, EnumRoleOperationType.Edit);
                    reader = ArchiveDataDA.InsertUpdate(tc, oArchiveData, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oArchiveData = new ArchiveData();
                    oArchiveData = CreateObject(oReader);
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
                    oArchiveData = new ArchiveData();
                    oArchiveData.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oArchiveData;
        }
        public ArchiveData Approve(ArchiveData oArchiveData, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oArchiveData.ArchiveStatus = EnumArchiveStatus.Approved;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ArchiveData, EnumRoleOperationType.Approved);
                reader = ArchiveDataDA.InsertUpdate(tc, oArchiveData, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oArchiveData = new ArchiveData();
                    oArchiveData = CreateObject(oReader);
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
                    oArchiveData = new ArchiveData();
                    oArchiveData.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oArchiveData;
        }

        public ArchiveData Backup(ArchiveData oArchiveData, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oArchiveData.ArchiveStatus = EnumArchiveStatus.BackupData;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ArchiveData, EnumRoleOperationType.BackUP);
                reader = ArchiveDataDA.InsertUpdate(tc, oArchiveData, EnumDBOperation.Backup, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oArchiveData = new ArchiveData();
                    oArchiveData = CreateObject(oReader);
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
                    oArchiveData = new ArchiveData();
                    oArchiveData.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oArchiveData;
        }

        public ArchiveData Get(int id, Int64 nUserId)
        {
            ArchiveData oArchiveData = new ArchiveData();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ArchiveDataDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oArchiveData = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ArchiveData", e);
                #endregion
            }

            return oArchiveData;
        }
        public List<ArchiveData> Gets(string sSQL, Int64 nUserID)
        {
            List<ArchiveData> oArchiveData = new List<ArchiveData>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ArchiveDataDA.Gets(tc, sSQL);
                oArchiveData = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ArchiveData", e);
                #endregion
            }
            return oArchiveData;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ArchiveData oArchiveData = new ArchiveData();
                oArchiveData.ArchiveDataID = id;
                 AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ArchiveData, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ArchiveData", id);
                ArchiveDataDA.Delete(tc, oArchiveData, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        
    }
}
