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
    public class VoucherHistoryService : MarshalByRefObject, IVoucherHistoryService
    {
        #region Private functions and declaration
        private VoucherHistory MapObject(NullHandler oReader)
        {
            VoucherHistory oVoucherHistory = new VoucherHistory();
            oVoucherHistory.VoucherHistoryID = oReader.GetInt32("VoucherHistoryID");
            oVoucherHistory.VoucherID = oReader.GetInt32("VoucherID");
            oVoucherHistory.UserID = oReader.GetInt32("UserID");
            oVoucherHistory.TransactionDate = oReader.GetDateTime("TransactionDate");
            oVoucherHistory.ActionType = (EnumRoleOperationType)oReader.GetInt16("ActionType");
            oVoucherHistory.UserName = oReader.GetString("UserName");
            oVoucherHistory.EmployeeNameCode = oReader.GetString("EmployeeNameCode");
            oVoucherHistory.VoucherTypeID = oReader.GetInt32("VoucherTypeID");
            oVoucherHistory.VoucherNo = oReader.GetString("VoucherNo");
            oVoucherHistory.Narration = oReader.GetString("Narration");
            oVoucherHistory.VoucherName = oReader.GetString("VoucherName");
            oVoucherHistory.VoucherDate = oReader.GetDateTime("VoucherDate");
            oVoucherHistory.PostingDate = oReader.GetDateTime("PostingDate");
            oVoucherHistory.AuthorizedByName = oReader.GetString("AuthorizedByName");
            oVoucherHistory.PreparedByName = oReader.GetString("PreparedByName");
            oVoucherHistory.VoucherAmount = oReader.GetDouble("VoucherAmount");
            oVoucherHistory.Remarks = oReader.GetString("Remarks");
            return oVoucherHistory;
        }

        private VoucherHistory CreateObject(NullHandler oReader)
        {
            VoucherHistory oVoucherHistory = new VoucherHistory();
            oVoucherHistory = MapObject(oReader);
            return oVoucherHistory;
        }

        private List<VoucherHistory> CreateObjects(IDataReader oReader)
        {
            List<VoucherHistory> oVoucherHistory = new List<VoucherHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VoucherHistory oItem = CreateObject(oHandler);
                oVoucherHistory.Add(oItem);
            }
            return oVoucherHistory;
        }

        #endregion

        #region Interface implementation
        public VoucherHistoryService() { }

        public VoucherHistory Save(VoucherHistory oVoucherHistory, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
               
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                if (oVoucherHistory.VoucherHistoryID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.VoucherHistory, EnumRoleOperationType.Add);
                    reader = VoucherHistoryDA.InsertUpdate(tc, oVoucherHistory, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.VoucherHistory, EnumRoleOperationType.Edit);
                    reader = VoucherHistoryDA.InsertUpdate(tc, oVoucherHistory, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucherHistory = new VoucherHistory();
                    oVoucherHistory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VoucherHistory. Because of " + e.Message, e);
                #endregion
            }
            return oVoucherHistory;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherHistory oVoucherHistory = new VoucherHistory();
                oVoucherHistory.VoucherHistoryID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.VoucherHistory, EnumRoleOperationType.Delete);
                VoucherHistoryDA.Delete(tc, oVoucherHistory, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete VoucherHistory. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public VoucherHistory Get(int id, int nUserId)
        {
            VoucherHistory oAccountHead = new VoucherHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherHistoryDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get VoucherHistory", e);
                #endregion
            }

            return oAccountHead;
        }

       

        public List<VoucherHistory> Gets(int nUserID)
        {
            List<VoucherHistory> oVoucherHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherHistoryDA.Gets(tc);
                oVoucherHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherHistory", e);
                #endregion
            }

            return oVoucherHistory;
        }

       
        public List<VoucherHistory> Gets(string sSQL,int nUserID)
        {
            List<VoucherHistory> oVoucherHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                
                reader = VoucherHistoryDA.Gets(tc, sSQL);
                oVoucherHistory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherHistory", e);
                #endregion
            }

            return oVoucherHistory;
        }

        public List<VoucherHistory> Gets(VoucherHistory oVoucherHistory, int nUserID)
        {
            List<VoucherHistory> oVoucherHistorys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;

                reader = VoucherHistoryDA.Gets(tc, oVoucherHistory);
                oVoucherHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherHistory", e);
                #endregion
            }

            return oVoucherHistorys;
        }

       
        #endregion
    }   
}